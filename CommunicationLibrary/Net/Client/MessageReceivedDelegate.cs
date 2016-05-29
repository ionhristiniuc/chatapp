using System;
using CommunicationLibrary.Messages.MessageHierarchy;

namespace CommunicationLibrary.Net.Client
{
    public delegate void MessageReceivedEventHandler(IClient sender, MessageEventArgs messageArgs);

    public class MessageEventArgs : EventArgs
    {
        public BinaryMessageBase Message { get; private set; }

        public MessageEventArgs(BinaryMessageBase message)
        {
            Message = message;
        }
    }
}
