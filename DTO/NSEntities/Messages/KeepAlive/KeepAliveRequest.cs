using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities.Messages.KeepAlive
{
    public class KeepAliveRequest : NSBaseMessage
    {
        public KeepAliveRequest() 
            : base(NSMessageTypeEnum.KeepAliveRequest)
        {
        }
    }
}
