using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities.Messages.Connectivity
{
    public class ConnectResponse : NSBaseMessage
    {
        public ConnectResponse() 
            : base(NSMessageTypeEnum.ConnectResponse)
        {
        }
    }
}
