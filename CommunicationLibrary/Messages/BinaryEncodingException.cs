using System;

namespace CommunicationLibrary.Messages
{
    public class BinaryEncodingException : Exception
    {
        public BinaryEncodingException()
            : base()
        {
        }

        public BinaryEncodingException(string message)
            : base(message)
        {
        }

        public BinaryEncodingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
