using System;
using CommunicationLibrary.Messages.MessageHierarchy;
using CommunicationLibrary.Util.Serialization;
using DTO.NSEntities;
using DTO.NSEntities.Messages;
using DTO.NSEntities.Messages.Connectivity;
using DTO.NSEntities.Messages.Contacts;
using DTO.NSEntities.Messages.KeepAlive;

namespace CommunicationLibrary.NS
{
    public class CommunicationUtils
    {
        private readonly ISerializer _serializer;

        public CommunicationUtils()
        {
            _serializer = new JsonSerializer();
        }

        public NSBaseMessage GetMessageObject(BinaryMessageBase message)
        {
            var txtMess = message as TextMessage;
            if (txtMess != null)
            {
                return DeserializeMessage(txtMess.Text);
            }
            else
                return null;
        }

        private NSBaseMessage DeserializeMessage(string str)
        {
            var nsBase = _serializer.Deserialize<NSBaseMessage>(str);

            switch (nsBase.MessageType)
            {
                case NSMessageTypeEnum.ConnectRequest:
                    return _serializer.Deserialize<ConnectRequest>(str);
                case NSMessageTypeEnum.ConnectResponse:
                    return _serializer.Deserialize<ConnectResponse>(str);                
                case NSMessageTypeEnum.DisconnectRequest:
                    return _serializer.Deserialize<DisconnectRequest>(str);
                case NSMessageTypeEnum.DisconnectResponse:
                    return _serializer.Deserialize<DisconnectResponse>(str);
                case NSMessageTypeEnum.KeepAliveRequest:
                    return _serializer.Deserialize<KeepAliveRequest>(str);
                case NSMessageTypeEnum.KeepAliveResponse:
                    return _serializer.Deserialize<KeepAliveResponse>(str);
                case NSMessageTypeEnum.GetOnlineContactsRequest:
                    return _serializer.Deserialize<GetOnlineContactsRequest>(str);
                case NSMessageTypeEnum.GetOnlineContactsResponse:
                    return _serializer.Deserialize<GetOnlineContactsResponse>(str);
                case NSMessageTypeEnum.NotifyClientOnlineRequest:
                    return _serializer.Deserialize<NotifyClientOnlineRequest>(str);
                case NSMessageTypeEnum.NotifyClientOnlineResponse:
                    return _serializer.Deserialize<NotifyClientOnlineResponse>(str);
                case NSMessageTypeEnum.NotifyClientOfflineRequest:
                    return _serializer.Deserialize<NotifyClientOfflineRequest>(str);
                case NSMessageTypeEnum.NotifyClientOfflineResponse:
                    return _serializer.Deserialize<NotifyClientOfflineResponse>(str);
                default:
                    throw new ArgumentException("Unexpected message type " + nsBase.MessageType);
            }
        }

        public BinaryMessageBase GetBinaryMessage(object obj)
        {
            return new TextMessage(_serializer.Serialize(obj));
        }
    }
}
