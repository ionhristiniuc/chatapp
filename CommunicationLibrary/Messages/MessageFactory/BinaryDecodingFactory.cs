﻿using CommunicationLibrary.Messages.MessageHierarchy;

namespace CommunicationLibrary.Messages.MessageFactory
{
    class BinaryDecodingFactory:IDecodingFactory
    {
        public BinaryMessageBase GetDecoding(byte[] encoding)
        {
            switch ((MessageType)encoding[0])
            {
                case MessageType.Connection:
                    return new ConnectionMessage();
                case MessageType.Request:
                    return  new RequestMessage(encoding);
                case MessageType.Confirmation:
                    return new ConfirmationMessage(encoding);
                case MessageType.TextMessage:
                    return new TextMessage(encoding);
                case MessageType.PeerAddress:
                    return new PeerAddressMessage(encoding);
                case MessageType.ClientPeerAddress:
                    return new PeerAddressMessage(encoding);
                case MessageType.ConnectAsClient:
                    return new PeerAddressMessage(encoding);
                case MessageType.ConnectAsServer:
                    return new PeerAddressMessage(encoding);
                case MessageType.IntegerMessage:
                    return new IntegerMessage(encoding);
                case MessageType.ConnectionPort:
                    return new IntegerMessage(encoding);
                case MessageType.BinaryArrayMessage:
                    return new ByteArrayMessage(encoding);
                default:
                    return null;
            }
        }
    }
}
