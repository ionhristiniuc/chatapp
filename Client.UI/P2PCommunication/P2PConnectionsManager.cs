using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.UI.NSClient;
using Client.UI.NSClient.Configuration;
using DTO.Entities;
using DTO.NSEntities.Messages;
using DTO.NSEntities.Messages.P2PConnectivity;
using DTO.P2PEntities.Messages;
using Microsoft.Practices.ObjectBuilder2;
using P2PCommunicationLibrary;

namespace Client.UI.P2PCommunication
{
    public class P2PConnectionsManager
    {
        public event MessageReceivedEvent MessageReceivedEvent;
        public event ConnectedToFriendEvent ConnectedToFriendEvent;
        private IList<P2PConnection> _connections = new List<P2PConnection>();
        private UserModel _user;
        private readonly NSConnection _nsConnection;
        private readonly IPEndPoint _superPeerEndPoint;

        public P2PConnectionsManager(UserModel user, NSConnection nsConnection)
        {
            _user = user;
            _nsConnection = nsConnection;
            _nsConnection.ObjectReceivedEvent += NsConnectionOnObjectReceivedEvent;
            _superPeerEndPoint = new IPEndPoint(IPAddress.Parse(SuperPeerConfig.Url), SuperPeerConfig.Port);
        }        

        public bool IsConnected(string userId)
        {
            return _connections.Any(c => c.UserId == userId);
        }

        public bool SendMessage(string userId, string message)
        {
            var conn = _connections.FirstOrDefault(c => c.UserId == userId);

            if (conn == null)
                return false;

            return conn.SendMessage(new TextMessage() {Text = message});
        }

        public bool StartConnectTo(string userId)
        {
            var conn = new ClientPeerConnection(_superPeerEndPoint, MessageReceivedEvent) { UserId = userId };
            conn.Run();
            _connections.Add(conn);
            return _nsConnection.SendConnectToFriendRequest(userId, conn.GetPeerAddress());
        }

        private void NsConnectionOnObjectReceivedEvent(NSBaseMessage nsBaseMessage)
        {
            if (nsBaseMessage is ConnectToFriendResponse)
            {
                var resp = (ConnectToFriendResponse) nsBaseMessage;
                var address = resp.Address.ToPeerAddress();
                var conn = (ClientPeerConnection) _connections.First(c => c.UserId == resp.UserId);
                Task.Factory.StartNew(() => conn.Connect(address))
                    .ContinueWith(t => ConnectedToFriendEvent?.Invoke(resp.UserId))
                    .ContinueWith(t => conn.ReadMessages());
            }
            else if (nsBaseMessage is AllowFriendToConnectRequest)
            {
                var req = (AllowFriendToConnectRequest)nsBaseMessage;
                var address = req.Address.ToPeerAddress();                
                var conn = new ServerPeerConnection(_superPeerEndPoint, MessageReceivedEvent) {UserId = req.UserId};
                conn.Run();
                _connections.Add(conn);
                Task.Factory.StartNew(() => conn.AllowConnection(address))
                    .ContinueWith(t => conn.ReadMessages());
                //MessageBox.Show("P2PConMng - Sending allow friend to connect resp to " + req.UserId);
                _nsConnection.SendAllowFriendToConnectResponse(req.UserId, conn.GetPeerAddress());                    
                //Task.Factory.StartNew(() => conn.ReadMessages());                
            }
        }

        public void Stop()
        {
            _connections.ForEach(c => c.Stop());
        }
    }

    public delegate void MessageReceivedEvent(string userId, string message);
    public delegate void ConnectedToFriendEvent(string userId);
}
