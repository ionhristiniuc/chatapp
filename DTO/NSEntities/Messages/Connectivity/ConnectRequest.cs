using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities.Messages.Connectivity
{
    public class ConnectRequest : NSBaseMessage
    {
        public string Username { get; set; }

        public ConnectRequest() 
            : base(NSMessageTypeEnum.ConnectRequest)
        {
        }
    }
}
