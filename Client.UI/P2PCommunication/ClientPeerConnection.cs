using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public ClientPeerConnection(IPEndPoint superPeerEndPoint, MessageReceivedEvent onMessageReceivedEvent,
            ConnectionClosedEvent onConnectionClosedEvent)
            : base(new JsonSerializer())
        {
            MessageReceivedEvent = onMessageReceivedEvent;
            ConnectionClosedEvent = onConnectionClosedEvent;
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

            try
            {
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
            catch (Exception e)
            {
                //MessageBox.Show("An error occurred while reading messages for user " + UserId
                //                + " : " + e);
                Trace.TraceWarning("An error occurred while reading messages for user " + UserId
                                   + " : " + e);
            }
            finally
            {
                ConnectionClosedEvent?.Invoke(UserId);
            }
        }

        public override PeerAddressContract GetPeerAddress()
        {
            return new PeerAddressContract(Peer.GetPeerAddress());
        }
    }
}
