using CommunicationLibrary.Messages.MessageHierarchy;

namespace CommunicationLibrary.Messages.MessageFactory
{
    interface IDecodingFactory
    {
        BinaryMessageBase GetDecoding(byte[] encoding);
    }
}
