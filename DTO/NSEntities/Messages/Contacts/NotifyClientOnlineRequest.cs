using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities.Messages.Contacts
{
    public class NotifyClientOnlineRequest : NSBaseMessage
    {
        public NotifyClientOnlineRequest()
            : base(NSMessageTypeEnum.NotifyClientOnlineRequest)
        {
        }

        public string Contact { get; set; }
    }
}
