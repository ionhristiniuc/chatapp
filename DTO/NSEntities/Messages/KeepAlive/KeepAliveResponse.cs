using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities.Messages.KeepAlive
{
    public class KeepAliveResponse : NSBaseMessage
    {
        public KeepAliveResponse()
            : base(NSMessageTypeEnum.KeepAliveResponse)
        {
        }
    }
}
