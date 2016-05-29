namespace CommunicationLibrary.Messages.MessageHierarchy
{
    public abstract class BinaryMessageBase
    {        
        public abstract MessageType TypeOfMessage { get; protected set; }
        public abstract byte[] GetEncoding();
    }
}
