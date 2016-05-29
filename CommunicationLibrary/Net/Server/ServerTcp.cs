using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using CommunicationLibrary.Messages;
using CommunicationLibrary.Net.Client;

namespace CommunicationLibrary.Net.Server
{
    public class ServerTcp : IServer
    {
        public event ClientConnectedEventHandler NewClientEvent;

        #region Properties
        public IPAddress Address { get; }
        public int Port { get; set; }
        public bool IsListening { get; private set; }
        #endregion

        private MessageManager _messageManager;
        private Socket _listener;
        private bool _binded = false;

        private int _backlog = 32;
        private readonly object _syncRoot = new object();

        #region Constructors
        public ServerTcp(IPAddress address, int port, MessageManager messageManager)
        {
            Port = port;
            Address = address;
            _messageManager = messageManager;
        }
        #endregion

        #region Public Methods
        public void Listen()
        {
            try
            {
                lock (_syncRoot)
                {
                    if (!_binded)
                        Bind();
                    // fire up the Server
                    _listener.Listen(_backlog);

                    // set listening bit
                    IsListening = true;
                }

                // Enter the listening loop.
                do
                {
                    // Wait for connection
                    Socket newClient = _listener.Accept();
                    Trace.WriteLine("Connected to new Client");

                    // queue a request to take care of the Client
                    ProcessClient(new ClientTcp(newClient, _messageManager));
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessClient), newClient);
                }
                while (IsListening);
            }
            catch (SocketException se)
            {
                Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
            }
            finally
            {
                // shut it down
                StopListening();
            }

        }

        public void StopListening()
        {
            if (IsListening)
            {
                lock (_syncRoot)
                {
                    // set listening bit
                    IsListening = false;
                }
            }
        }

        public void Bind()
        {
            try
            {
                lock (_syncRoot)
                {
                    _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _listener.Bind(new IPEndPoint(Address, Port));
                    Port = ((IPEndPoint)_listener.LocalEndPoint).Port;
                    _binded = true;
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
            }
            finally
            {
                StopListening();
            }
        }

        public IClient AcceptClient()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            lock (_syncRoot)
            {
                try
                {
                    IsListening = false;
                    _listener.Close();
                }
                catch (SocketException se)
                {
                    Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
                }
            }
        }
        #endregion

        #region Private Methods
        private void ProcessClient(IClient client)
        {
            NewClientEvent?.Invoke(this, client);
        }

        #endregion        
    }
}
