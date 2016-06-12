using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommunicationLibrary.Util.Serialization;
using DTO.NSEntities;
using DTO.P2PEntities.Messages;
using P2PCommunicationLibrary;
using P2PCommunicationLibrary.SimplePeers.ClientPeer;

namespace Client.UI.P2PCommunication
{
    public class ClientPeerConnection : P2PConnection
    {        
        public ClientPeer Peer { get; set; }        

        public ClientPeerConnection(IPEndPoint superPeerEndPoint, MessageReceivedEvent onMessageReceivedEvent)
            : base(new JsonSerializer())
        {
            MessageReceivedEvent = onMessageReceivedEvent;
            Peer = new ClientPeer(superPeerEndPoint);
        }

        public override bool SendMessage(P2PMessageBase message)
        {
            var bytes = GetBytes(message);
            Peer.Send(bytes);
            return true;
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

        public void Connect(PeerAddress address)
        {
            Peer.Connect(address);
        }

        public override void ReadMessages()
        {
            IsListening = true;            

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

        public override PeerAddressContract GetPeerAddress()
        {
            return new PeerAddressContract(Peer.GetPeerAddress());
        }
    }
}
