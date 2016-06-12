using System;
using System.Linq;
using CommunicationLibrary.Messages;
using CommunicationLibrary.Net.Client;
using CommunicationLibrary.Nodes;
using NotificationService.Repository;
using NotificationService.Repository.Entities;
using CommunicationLibrary.Messages.MessageHierarchy;
using CommunicationLibrary.NS;
using DataServiceClient.Services;
using DTO.NSEntities;
using DTO.NSEntities.Messages;
using DTO.NSEntities.Messages.Connectivity;
using DTO.NSEntities.Messages.Contacts;
using DTO.NSEntities.Messages.KeepAlive;
using DTO.NSEntities.Messages.P2PConnectivity;

namespace NotificationService
{
    class ClientConnection
    {
        private readonly IClient _client;
        private readonly IUsersService _usersService;
        public ClientInfo Info { get; }
        private CommunicationUtils _communicationHelper = new CommunicationUtils();        

        public ClientConnection(IClient client, IUsersService usersService)
        {
            _client = client;
            _usersService = usersService;
            Info = new ClientInfo(client);

            _client.ConnectionClosedEvent += RepositoryCleaner.ClientOnConnectionClosedEvent;

            Console.WriteLine("Client " + _client.RemoteEndPoint + " " + _client.LocalEndPoint + " Connected");
        }

        public async void BeginProcessClientConnection()
        {
            if (!InitClientConnection())
                return;

            //if (!InitPeerType())
            //    return;

            //if (!InitClientAddress())
            //    return;

            Info.PeerAddress(new PeerAddress(_client.RemoteEndPoint, _client.RemoteEndPoint));

            ClientsConnectionsManager.AddClient(this);
            await ClientsConnectionsManager.NotifyContactsClientOnline(Info.Username);

            try
            {
                ReadMessages();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred {0}", e);
            }
            finally
            {
                _client.StopListeningMessages();
                RepositoryCleaner.CleanRepositories(this);
                //SendObject(new DisconnectResponse()); 
                await ClientsConnectionsManager.NotifyContactsClientOffline(Info.Username);
            }
        }

        private void ReadMessages()
        {
            var obj = ReadObject();
            
            while (obj != null && !(obj is DisconnectRequest))
            {                
                Console.WriteLine("Received message: {0}", obj.GetType());                
                ProcessMessage(obj as NSBaseMessage);

                obj = ReadObject();
            }            
        }

        private async void ProcessMessage(NSBaseMessage obj)
        {
            if (obj is ConnectRequest)
            {
                
            }
            else if (obj is KeepAliveRequest)
            {
                SendObject(new KeepAliveResponse());
            }            
            else if (obj is GetOnlineContactsRequest)
            {
                var allContacts = await _usersService.GetFriends(Info.Username);
                var onlineContacts = allContacts.Data
                    .Where(c => ClientsConnectionsManager.IsOnline(c.Id))
                    .Select(c => c.Id);
                var resp = new GetOnlineContactsResponse() {Contacts = onlineContacts};
                SendObject(resp);
            } 
            else if (obj is ConnectToFriendRequest)
            {
                // should check if is a friend
                var req = (ConnectToFriendRequest) obj;
                var contact = ClientsConnectionsManager.GetClientConnection(req.UserId);
                if (contact == null)
                {
                    Console.WriteLine("Client {0} is offline", req.UserId);
                    return; // should send an error message
                }

                //var address = new P2PCommunicationLibrary.PeerAddress(
                //    Info.PeerAddress().PrivateEndPoint, Info.PeerAddress().PublicEndPoint);
                contact.SendObject(new AllowFriendToConnectRequest() {Address = req.Address, UserId = Info.Username});
            }  
            else if (obj is AllowFriendToConnectResponse)
            {
                var resp = (AllowFriendToConnectResponse) obj;
                var contact = ClientsConnectionsManager.GetClientConnection(resp.UserId);
                if (contact == null)
                {
                    Console.WriteLine("Client {0} is offline", resp.UserId);
                    return; // should send an error message
                }

                //var address = new P2PCommunicationLibrary.PeerAddress(
                //    Info.PeerAddress().PrivateEndPoint, Info.PeerAddress().PublicEndPoint);

                contact.SendObject(new ConnectToFriendResponse() {Address = resp.PeerAddress,
                    UserId = Info.Username});
            }         
            else
            {
                Console.WriteLine("Unexpected message received {0}", obj.MessageType);
            }
        }

        private bool InitClientConnection()
        {
            bool clientConnected = true;

            var message = ReadObject();

            Console.WriteLine("Init client connection: " + message);

            if (message == null)
            {
                _client.Close();
                clientConnected = false;
            }
            else if (!(message is ConnectRequest))
            {
                _client.Close();
                clientConnected = false;
            }

            var req = message as ConnectRequest;
            InitClientInfo(req);

            return clientConnected;
        }

        public void NotifyClientOnline(string clientId)
        {
            SendObject(new NotifyClientOnlineRequest() {Contact = clientId});
        }

        public void NotifyClientOffline(string clientId)
        {
            if (_client.ClientSocket.IsConnected())
                SendObject(new NotifyClientOfflineRequest() { Contact = clientId });
        }

        private bool InitClientAddress()
        {
            var requestMessage = _client.Read() as RequestMessage;

            if (requestMessage != null && requestMessage.RequestedMessageType == MessageType.ClientPeerAddress)
            {
                PeerAddress peerAddress = new PeerAddress { PublicEndPoint = _client.RemoteEndPoint };

                _client.Send(new PeerAddressMessage(peerAddress, MessageType.ClientPeerAddress));
                peerAddress.PrivateEndPoint = ((PeerAddressMessage)_client.Read()).PeerAddress.PrivateEndPoint;

                Info.PeerAddress(peerAddress);
                return true;
            }
            else
            {
                _client.Close();
                return false;
            }
        }

        private void InitClientInfo(ConnectRequest req)
        {
            Info.ConnectionDateTime(DateTime.Now);
            Info.LastPingMesssageDateTime = DateTime.Now;
            Info.Username = req.Username;            
            Info.Client.ClientId = req.Username;            
        }

        private object ReadObject()
        {
            var message = _client.Read();
            var objMess = _communicationHelper.GetMessageObject(message);
            return objMess;
        }

        private void SendObject(object obj)
        {
            _client.Send(_communicationHelper.GetBinaryMessage(obj));
        }
    }
}
