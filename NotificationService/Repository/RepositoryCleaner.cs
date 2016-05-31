using System;
using System.Collections.Generic;
using System.Net.Sockets;
using CommunicationLibrary.Net.Client;
using NotificationService.Repository.Entities;

namespace NotificationService.Repository
{
    static class RepositoryCleaner
    {
        public static void CheckClientsConnection()
        {
            Console.WriteLine("check...");

            //DisplayRepositoryState();

            //CheckClientsLastPingMessageDateTime();    // TODO implement keep alive functionality
            //CheckSocketConnection(); // TODO should notify also clients
        }

        //private static void DisplayRepositoryState()
        //{
        //    Console.WriteLine("Clients :" + ClientsConnectionsManager.GetClients().Count
        //                      + "\nConnections :" + ConnectionsRepository.GetConnections().Count
        //                      + "\nPeerClients :" + ConnectionsRepository.GetClients().Count
        //                      + "\nPeerServers :" + ConnectionsRepository.GetServers().Count);
        //}

        private static void CheckSocketConnection()
        {
            foreach (var client in ClientsConnectionsManager.GetClients())
            {
                if (!client.Info.Client.ClientSocket.IsConnected())
                    CleanRepositories(client);
            }
        }

        public static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
        }

        private static void CheckClientsLastPingMessageDateTime()
        {
            foreach (var client in ClientsConnectionsManager.GetClients())
            {
                DateTime lastPingMessageDateTime = client.Info.LastPingMesssageDateTime;

                if ((DateTime.Now - lastPingMessageDateTime).TotalSeconds > 60)
                    CleanRepositories(client);
            }
        }

        public static void ClientOnConnectionClosedEvent(IClient client, EventArgs enentArgs)
        {
            CleanRepositories(client.ClientId);
        }

        private static void CleanRepositories(string clientId)
        {
            ClientsConnectionsManager.RemoveClient(clientId);
        }

        public static void CleanRepositories(ClientConnection client)
        {
            //DisplayRepositoryState();

            CleanClientRepository(client);
           // CleanConnectionRepository(client);

            //DisplayRepositoryState();
        }

        private static void CleanConnectionRepository(IClient closedClient)
        {
            //CleanClientFromConnectionsRepository(closedClient);
            //CleanServerFromConnectionsRepository(closedClient);
            //CleanConnectionsFromConnectionsRepository(closedClient);
        }       

        //private static void CleanClientFromConnectionsRepository(IClient closedClient)
        //{
        //    List<SuperPeerClient> clientList = ConnectionsRepository.GetClients();

        //    foreach (SuperPeerClient client in clientList.Where(client => client.GetSuperPeerClient() == closedClient))
        //    {
        //        ConnectionsRepository.RemoveClient(client);
        //    }
        //}

        private static void CleanClientRepository(ClientConnection closedClient)
        {
            ClientsConnectionsManager.RemoveClient(closedClient);
        }
    }
}