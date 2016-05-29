using CommunicationLibrary.Net.Client;

namespace CommunicationLibrary.Net.Server
{
    public delegate void ClientConnectedEventHandler(IServer sender, IClient newClient);
}
