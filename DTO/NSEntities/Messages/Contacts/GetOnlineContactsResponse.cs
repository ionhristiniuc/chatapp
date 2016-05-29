using System.Collections.Generic;

namespace DTO.NSEntities.Messages.Contacts
{
    public class GetOnlineContactsResponse : NSBaseMessage
    {
        public IEnumerable<string> Contacts { get; set; }

        public GetOnlineContactsResponse()
            : base(NSMessageTypeEnum.GetOnlineContactsResponse)
        {
        }
    }
}
