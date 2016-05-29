using System.Net;
using CommunicationLibrary.Net.Client;
using CommunicationLibrary.Nodes;

namespace CommunicationLibrary.Net.Server
{
    public interface IServer : IPeer
    {
        event ClientConnectedEventHandler NewClientEvent;

        IPAddress Address { get; }
        int Port { get; }

        bool IsListening { get; }
        void Listen();
        void StopListening();
        void Bind();
        IClient AcceptClient();
    }
}
