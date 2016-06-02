using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2PCommunicationLibrary.SimplePeers.ServerPeer;

namespace Client.UI.P2PCommunication
{
    public class ServerPeerConnection : P2PConnection
    {
        public ServerPeer Peer { get; set; }

        public override bool SendMessage(string message)
        {
            //Peer.Send();  // Maybe should send objects
            return true;
        }

        public override void Stop()
        {
            Peer.Close();
        }
    }
}
