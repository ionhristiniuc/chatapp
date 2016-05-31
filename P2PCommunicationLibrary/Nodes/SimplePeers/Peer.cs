﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers
{
    class Peer
    {
        private IClient _superPeerClient;
        
        private IEncryptor _encryptor;

        public MessageManager MessageManager { get; private set; }
        public PeerAddress PeerAddress{ get; private set; }
        public bool IsRunning { get; private set; }
        public IPEndPoint SuperPeerEndPoint { get; private set; }

        public IEncryptor Encryptor
        {
            get { return _encryptor; }
            set
            {
                if (!IsRunning)
                {
                    _encryptor = value;
                }
            }   
        }    
        
        public Peer(IPEndPoint superPeerEndPoint)
        {                                  
            InitMessageManager();
            SuperPeerEndPoint = superPeerEndPoint;
        }

        public Peer(IPEndPoint superPeerEndPoint, IEncryptor encryptor)
        {
            Encryptor = encryptor;
            InitMessageManager();
            SuperPeerEndPoint = superPeerEndPoint;
        }

        private void InitMessageManager()
        {
            if (Encryptor != null)
                MessageManager = new MessageManager(Encryptor);
            else
                MessageManager = new MessageManager();
        }

        /// <summary>
        /// Connectiong to Super Peer
        /// </summary>
        public void Run(ClientType clientType)
        {
            IsRunning = true;           

            try
            {              
                InitSuperPeerConnection();
                InitPeerType(clientType);
                PeerAddress = GetPeerAddress();

                //Read confirmation message
                _superPeerClient.Read();
                RunSendPingMessageTask();
            }
            catch (SocketException se)
            {
                Trace.WriteLine("Error connectiong to the Server");
                Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
                Close();
                throw;
            }
        }        

        private void InitSuperPeerConnection()
        {
            _superPeerClient = new ClientTcp(SuperPeerEndPoint, MessageManager);
            _superPeerClient.Send(new ConnectionMessage());
        }

        private void InitPeerType(ClientType clientType)
        {           
            if (clientType == ClientType.Client)
                _superPeerClient.Send(new RequestMessage(MessageType.InitConnectionAsClient));
            else if (clientType == ClientType.Server)
            {             
                _superPeerClient.Send(new RequestMessage(MessageType.InitConnectionAsServer));                               
            }           
        }

        private PeerAddress GetPeerAddress()
        {                      
            var requestMessage = new RequestMessage(MessageType.ClientPeerAddress);
            _superPeerClient.Send(requestMessage);

            PeerAddress peerAddress = ((PeerAddressMessage)_superPeerClient.Read()).PeerAddress;
            peerAddress.PrivateEndPoint = new IPEndPoint(LocalIpAddress(), _superPeerClient.LocalEndPoint.Port);
            _superPeerClient.Send(new PeerAddressMessage(peerAddress, MessageType.ClientPeerAddress));
           
            return peerAddress;
        }

        private void RunSendPingMessageTask()
        {
            PeriodicTask periodicTask = new PeriodicTask(
                SendPingMessageToSuperPeer,
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(30),
                CancellationToken.None);

            periodicTask.DoPeriodicWorkAsync();
        }

        private void SendPingMessageToSuperPeer()
        {            
            _superPeerClient.Send(new RequestMessage(MessageType.Ping));
        }

        public void Close()
        {
            if (!IsRunning)
                return;

            IsRunning = false;
               
            var closeConnectionMessage = new RequestMessage(MessageType.CloseConnection);
            _superPeerClient.Send(closeConnectionMessage);            

            try
            {
                Thread.Sleep(5000);
                _superPeerClient.Close();
            }
            catch (SocketException)
            {        
            }          
                                  
        }                   

        private static IPAddress LocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public void SendToSuperPeer(BinaryMessageBase message)
        {
            _superPeerClient.Send(message);
        }

        public BinaryMessageBase ReadFromSuperPeer()
        {
            return _superPeerClient.Read();
        }       

        public void AddMethodToMessageReceivedEvent(MessageReceivedEventHandler messageReceivedEventHandler)
        {
            _superPeerClient.MessageReceivedEvent += messageReceivedEventHandler;
        }

        public void RemoveMethodFromMessageReceivedEvent(MessageReceivedEventHandler messageReceivedEventHandler)
        {
            _superPeerClient.MessageReceivedEvent -= messageReceivedEventHandler;
        }

        public void StartListenMessagesFromSuperPeer()
        {
            _superPeerClient.ListenMessages();
        }

        public void StopListenMessagesFromSuperPeer()
        {
            _superPeerClient.ListenMessages();
        }

        public void Send(IClient client, byte[] byteArray)
        {
            client.Send(new ByteArrayMessage(byteArray, 0, byteArray.Length));
        }

        public byte[] Read(IClient client)
        {
            return ((ByteArrayMessage)client.Read()).ByteArray;
        }
    }
}
