using System;
using CommunicationLibrary.Net.Client;
using CommunicationLibrary.Nodes;

namespace NotificationService.Repository.Entities
{
    class ClientInfo
    {
        public IClient Client { get; set; }
        public DateTime LastPingMesssageDateTime { get; set; }
        public string Username { get; set; }

        //public Token Token { get; private set; }
        private DateTime _connectionDateTime;
        private PeerAddress _peerAddress;        

        public ClientInfo(IClient client)
        {
            Client = client;
            //Token = Token.GenerateNew();
        }

        public DateTime ConnectionDateTime()
        {
            return _connectionDateTime;
        }

        public PeerAddress PeerAddress()
        {
            return _peerAddress;
        }        

        public ClientInfo ConnectionDateTime(DateTime connDateTime)
        {
            _connectionDateTime = connDateTime;
            return this;
        }

        public ClientInfo PeerAddress(PeerAddress peerAddress)
        {
            _peerAddress = peerAddress;
            return this;
        }        
    }
}