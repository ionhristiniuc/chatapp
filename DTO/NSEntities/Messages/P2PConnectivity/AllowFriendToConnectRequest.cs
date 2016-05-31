﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P2PCommunicationLibrary;

namespace DTO.NSEntities.Messages.P2PConnectivity
{
    public class AllowFriendToConnectRequest : NSBaseMessage
    {
        public PeerAddress Address { get; set; }

        public AllowFriendToConnectRequest()
            : base(NSMessageTypeEnum.AllowFriendToConnectRequest)
        {
        }
    }
}
