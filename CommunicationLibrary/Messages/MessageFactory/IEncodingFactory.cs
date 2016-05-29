using CommunicationLibrary.Messages.MessageHierarchy;

namespace CommunicationLibrary.Messages.MessageFactory
{
    interface IEncodingFactory
    {
        byte[] GetEncoding(BinaryMessageBase message);
    }
}
