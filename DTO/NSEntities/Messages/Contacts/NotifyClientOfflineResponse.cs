using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities.Messages.Contacts
{
    public class NotifyClientOfflineResponse : NSBaseMessage
    {
        public NotifyClientOfflineResponse()
            : base(NSMessageTypeEnum.NotifyClientOfflineResponse)
        {
        }
    }
}
