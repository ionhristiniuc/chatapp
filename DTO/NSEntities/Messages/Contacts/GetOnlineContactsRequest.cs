namespace DTO.NSEntities.Messages.Contacts
{
    public class GetOnlineContactsRequest : NSBaseMessage
    {
        public GetOnlineContactsRequest()
            : base(NSMessageTypeEnum.GetOnlineContactsRequest)
        {
        }
    }
}
