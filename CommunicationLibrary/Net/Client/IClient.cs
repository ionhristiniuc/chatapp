using System.Net;
using System.Net.Sockets;
using CommunicationLibrary.Messages.MessageHierarchy;
using CommunicationLibrary.Nodes;

namespace CommunicationLibrary.Net.Client
{
    public interface IClient : IPeer
    {
        string ClientId { get; set; }

        event PeerClosedConnectionEventHandler ConnectionClosedEvent;
        event MessageReceivedEventHandler MessageReceivedEvent;

        IPEndPoint LocalEndPoint { get; }
        IPEndPoint RemoteEndPoint { get; }

        void Send(BinaryMessageBase message);
        BinaryMessageBase Read();

        Socket ClientSocket { get; }

        void ListenMessages();
        void ListenOneMessage();
        bool IsListeningMessages { get; }
        void StopListeningMessages();
    }
}
