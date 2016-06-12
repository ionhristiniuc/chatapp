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
using P2PCommunicationLibrary.SimplePeers.ServerPeer;

namespace Client.UI.P2PCommunication
{
    public class ServerPeerConnection : P2PConnection
    {
        public ServerPeer Peer { get; set; }

        public ServerPeerConnection(IPEndPoint superPeerEndPoint, MessageReceivedEvent onMessageReceivedEvent,
            ConnectionClosedEvent onConnectionClosedEvent)
            : base(new JsonSerializer())
        {
            MessageReceivedEvent = onMessageReceivedEvent;
            ConnectionClosedEvent = onConnectionClosedEvent;
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
                //MessageBox.Show("ServerPeerConnection.ReadMessages - An error occurred while reading messages for user "
                //                + UserId + " : " + e);
                Trace.TraceWarning("ClientPeerConnection.ReadMessages - An error occurred while reading messages for user "
                                   + UserId + " : " + e);
            }
            finally
            {
                ConnectionClosedEvent?.Invoke(UserId);
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
