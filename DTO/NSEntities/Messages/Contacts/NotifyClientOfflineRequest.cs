using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities.Messages.Contacts
{
    public class NotifyClientOfflineRequest : NSBaseMessage
    {
        public NotifyClientOfflineRequest()
            : base(NSMessageTypeEnum.NotifyClientOfflineRequest)
        {
        }

        public string Contact { get; set; }
    }
}
