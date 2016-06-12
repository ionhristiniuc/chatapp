using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunicationLibrary.Util.Serialization;
using DTO.NSEntities;
using DTO.P2PEntities.Messages;
using P2PCommunicationLibrary;
using P2PCommunicationLibrary.SimplePeers.ServerPeer;

namespace Client.UI.P2PCommunication
{
    public class ServerPeerConnection : P2PConnection
    {
        public ServerPeer Peer { get; set; }

        public ServerPeerConnection(IPEndPoint superPeerEndPoint, MessageReceivedEvent onMessageReceivedEvent)
            : base(new JsonSerializer())
        {
            MessageReceivedEvent = onMessageReceivedEvent;
            Peer = new ServerPeer(superPeerEndPoint, 0);   
        }        

        public override bool SendMessage(P2PMessageBase message)
        {
            var bytes = GetBytes(message);
            Peer.Send(bytes);
            return true;
        }

        public override void ReadMessages()
        {
            IsListening = true;

            //MessageBox.Show(UserId + " - Reading messages ...");

            while (IsListening)
            {
                var data = Peer.Read();
                var message = GetMessage(data);

                if (message is TextMessage)
                {
                    MessageReceivedEvent?.Invoke(UserId, (message as TextMessage).Text); // maybe need async?
                }
            }
        }

        public override void Stop()
        {
            IsListening = false;
            Peer.Close();
        }

        public override void Run()
        {
            Peer.Run();
        }

        public override PeerAddressContract GetPeerAddress()
        {
            return new PeerAddressContract(Peer.GetPeerAddress());
        }

        public void AllowConnection(PeerAddress address)
        {
            Peer.AllowConnection(address);
        }
    }
}
