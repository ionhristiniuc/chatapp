﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Net
{
    class ServerTcp : IServer
    {
        public event ClientConnectedEventHandler NewClientEvent;

        #region Private Members
        private Socket _listener;
        private MessageManager _messageManager;
        private bool _binded = false;

        private int _backlog = 32;
        private readonly object _syncRoot = new object();
        #endregion

        #region Properties
        public IPAddress Address { get; private set; }
        public int Port { get; private set; }
        public bool IsListening { get; private set; }        
        #endregion

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
                    if(!_binded)
                        Bind();
                    // fire up the Server
                    _listener.Listen(_backlog);

                    // set listening bit
                    IsListening = true;
                }

                // Enter the listening loop.
                do
                {
                    Trace.Write("Looking for someone to talk to... ");

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

        public void Bind()
        {
            try
            {
                lock (_syncRoot)
                {
                    _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _listener.Bind(new IPEndPoint(Address, Port));
                    Port = ((IPEndPoint) _listener.LocalEndPoint).Port;
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
            try
            {
                lock (_syncRoot)
                {
                    if (!_binded)
                        Bind();

                    _listener.Listen(_backlog);

                    Socket newClient = _listener.Accept();
                    return new ClientTcp(newClient, _messageManager);
                }
            }
            catch (SocketException se)
            {
                Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
            }

            return null;
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
        #endregion

        #region Private Methods
        private void ProcessClient(IClient client)
        {
            if (NewClientEvent != null)
                NewClientEvent(this, client);
        }

        #endregion        

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
    }
}