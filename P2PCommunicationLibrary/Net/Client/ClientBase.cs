﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Net
{
    abstract class ClientBase : IClient
    {
        public event PeerClosedConnectionEventHandler ConnectionClosedEvent;
        public event MessageReceivedEventHandler MessageReceivedEvent;

        #region Private members
        protected readonly object SocketMonitor = new object();
        protected readonly object SendMonitor = new object();
        protected readonly object ReadMonitor = new object();
        #endregion

        #region Properties

        protected MessageManager MessageManager { get; private set; }
        protected IPEndPoint ConnectionIpEndPoint { get; private set; }


        public bool IsListeningMessages { get; private set; }

        public IPEndPoint LocalEndPoint { get; protected set; }
        public IPEndPoint RemoteEndPoint { get; protected set; }                       

        public Socket ClientSocket { get; protected set; }
        #endregion

        protected ClientBase(MessageManager messageManager)
        {
            MessageManager = messageManager;
        }

        protected ClientBase(IPEndPoint connectionIpEndPoint, MessageManager messageManager)
            :this(messageManager)
        {
            ConnectionIpEndPoint = connectionIpEndPoint;            
        }
        
        public abstract void Send(BinaryMessageBase message);

        public abstract BinaryMessageBase Read();

        public void ListenOneMessage()
        {
            if (IsListeningMessages)
                return;

            lock (SocketMonitor)
            {
                BinaryMessageBase receivedMessage = Read();

                if (MessageReceivedEvent != null)
                    MessageReceivedEvent(this, new MessageEventArgs(receivedMessage));
            }
        }

        public void ListenMessages()
        {
            lock (SocketMonitor)
            {
                IsListeningMessages = true;

                do
                {
                    BinaryMessageBase receivedMessage = Read();

                    if (MessageReceivedEvent != null)
                        MessageReceivedEvent(this, new MessageEventArgs(receivedMessage));

                } while (IsListeningMessages);
            }
        }

        public void StopListeningMessages()
        {
            if (IsListeningMessages)
            {                      
                IsListeningMessages = false;               
            }
        }

        public void Close()
        {
            lock (SocketMonitor)
            {
                try
                {
                    StopListeningMessages();
                    ClientSocket.Close();
                }
                catch (SocketException se)
                {
                    Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
                }

                if (ConnectionClosedEvent != null)
                    ConnectionClosedEvent.Invoke(this, new EventArgs());
            }
        }
    }
}
