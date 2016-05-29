using CommunicationLibrary.Messages.MessageHierarchy;

namespace CommunicationLibrary.Messages.MessageFactory
{
    class BinaryEncodingFactory:IEncodingFactory
    {
        public byte[] GetEncoding(BinaryMessageBase message)
        {
            return message.GetEncoding();
        }
    }
}
