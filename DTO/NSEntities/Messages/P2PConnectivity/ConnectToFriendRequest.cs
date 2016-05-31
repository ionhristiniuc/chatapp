using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities.Messages.P2PConnectivity
{
    public class ConnectToFriendRequest : NSBaseMessage
    {
        public string UserId { get; set; }

        public ConnectToFriendRequest()
            : base(NSMessageTypeEnum.ConnectToFriendRequest)
        {
        }
    }
}
