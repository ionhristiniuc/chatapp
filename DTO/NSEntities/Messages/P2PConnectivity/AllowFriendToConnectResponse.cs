using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities.Messages.P2PConnectivity
{
    public class AllowFriendToConnectResponse : NSBaseMessage
    {
        public bool Allowed { get; set; }

        public AllowFriendToConnectResponse()
            : base(NSMessageTypeEnum.AllowFriendToConnectResponse)
        {
        }
    }
}
