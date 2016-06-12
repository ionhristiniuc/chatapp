using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2PCommunicationLibrary;

namespace DTO.NSEntities.Messages.P2PConnectivity
{
    public class ConnectToFriendResponse : NSBaseMessage
    {
        public PeerAddressContract Address { get; set; }
        public string UserId { get; set; }

        public ConnectToFriendResponse()
            : base(NSMessageTypeEnum.ConnectToFriendResponse)
        {
        }
    }
}
