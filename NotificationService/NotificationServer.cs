using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunicationLibrary.Messages;
using CommunicationLibrary.Messages.MessageEncryption;
using CommunicationLibrary.Net.Client;
using CommunicationLibrary.Net.Server;
using CommunicationLibrary.Nodes;
using DataServiceClient.Services;
using NotificationService.Repository;

namespace NotificationService
{
    class NotificationServer    
    {
        #region Properties
        public IPAddress Address { get; private set; }
        public int Port { get; private set; }

        public bool IsRunning { get; private set; }
        #endregion

        private IServer _server;
        private MessageManager _messageManager;
        private IEncryptor _encryptor;
        private readonly IUsersService _usersService = new UsersService(null);

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

        internal MessageManager GetMessageManager()
        {
            return _messageManager;
        }

        public NotificationServer(IPAddress address, int port)
        {
            Address = address;
            Port = port;
        }

        public void Run()
        {
            IsRunning = true;

            _messageManager = new MessageManager(Encryptor);

            try
            {
                _server = new ServerTcp(Address, Port, _messageManager);
                _server.NewClientEvent += ClientConnected_EventHandler;
                RunConnectionCheckTask();
                _server.Listen();
            }
            catch (SocketException e)
            {
                StopRunning();
                throw;
            }
        }

        private static void RunConnectionCheckTask()
        {
            PeriodicTask periodicTask = new PeriodicTask(
                RepositoryCleaner.CheckClientsConnection,
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(30),
                CancellationToken.None);

            periodicTask.DoPeriodicWorkAsync();
        }

        public void StopRunning()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _server.StopListening();
            }
        }

        /// <summary>
        /// This event occurs when a Client connects to the server
        /// </summary>       
        private void ClientConnected_EventHandler(IServer sender, IClient client)
        {
            Task.Factory.StartNew(() => new ClientConnection(client, _usersService).BeginProcessClientConnection());
        }
    }
}
