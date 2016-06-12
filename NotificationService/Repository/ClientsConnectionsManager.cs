using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunicationLibrary.Net.Client;
using DataServiceClient.Services;
using NotificationService.Repository.Entities;

namespace NotificationService.Repository
{
    static class ClientsConnectionsManager
    {
        private static readonly Dictionary<string, ClientConnection> Clients;
        private static readonly object ClientsMonitor = new object();
        private static readonly IUsersService _usersService = new UsersService(null);

        static ClientsConnectionsManager()
        {
            Clients = new Dictionary<string, ClientConnection>();
        }

        #region Clients
        public static IList<ClientConnection> GetClients()
        {
            lock (ClientsMonitor)
            {
                return new List<ClientConnection>(Clients.Values);
            }
        }

        public static void RemoveClient(ClientConnection client)
        {
            lock (ClientsMonitor)
            {
                Clients.Remove(client.Info.Username);
            }
        }

        public static void AddClient(ClientConnection client)
        {
            lock (ClientsMonitor)
            {
                Clients.Add(client.Info.Username, client);
            }
        }

        //public static void AddClient(IClient client, string username)
        //{
        //    AddClient(new ClientInfo(client) {Username = username});
        //}

        public static ClientConnection GetClientConnection(string username)
        {
            lock (ClientsMonitor)
            {
                if (Clients.ContainsKey(username))
                    return Clients[username];
                else
                    return null;
            }
        }
        #endregion

        public static void RemoveClient(string clientId)
        {
            lock (ClientsMonitor)
            {
                Clients.Remove(clientId);
            }
        }

        public static bool IsOnline(string clientId)
        {
            return Clients.ContainsKey(clientId);
        }

        public static async 
        Task
NotifyContactsClientOnline(string clientId)
        {
            var onlineFriends = await OnlineFriendsConnections(clientId);

            foreach (var friendConnection in onlineFriends)
            {
                try
                {                    
                    friendConnection.NotifyClientOnline(clientId);
                }
                catch (Exception e)
                {
                    Console.WriteLine("NotifyContactsClientOnline - Failed to notify client {0}", e);
                }   
            }
        }

        public static async Task NotifyContactsClientOffline(string clientId)
        {
            var onlineFriends = await OnlineFriendsConnections(clientId);

            foreach (var friendConnection in onlineFriends)
            {
                try
                {
                    friendConnection.NotifyClientOffline(clientId);
                }
                catch (Exception e)
                {
                    Console.WriteLine("NotifyContactsClientOffline - Failed to notify client {0}", e);
                }
            }
        }

        private static async Task<IEnumerable<ClientConnection>> OnlineFriendsConnections(string clientId)
        {
            var friends = await _usersService.GetFriends(clientId);
            var onlineFriends = Clients
                .Where(c => friends.Data.Any(f => f.Id == c.Key))
                .Select(c => c.Value);

            return onlineFriends;
        }       
    }
}