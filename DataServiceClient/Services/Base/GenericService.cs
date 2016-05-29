using System;
using System.Net.Http;
using System.Threading.Tasks;
using DataServiceClient.Configuration;
using DataServiceClient.Services.Utils;
using DTO.Containers;

namespace DataServiceClient.Services.Base
{
    public class GenericService<T> : IGenericService<T>
        where T : new()
    {
        protected string AccessToken { get; set; }
        protected string ServicePath { get; set; }
        protected ISerializer Serializer { get; set; }

        public GenericService(string accessToken, string servicePath)
        {
            AccessToken = accessToken;
            ServicePath = servicePath;
            Serializer = new JsonSerializer();
        }

        public async Task<T> Get(string id)
        {
            using (var client = new HttpClient())
            {
                PrepareHeaders(client);
                var path = $"{ConfigManager.ServiceUrl}/{ServicePath}/{id}";
                var result = await client.GetStringAsync(path);
                return Serializer.Deserialize<T>(result);
            }
        }

        public Items<T> GetList(int page = 0, int perPage = 10)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Add(T entity)
        {
            using (var client = new HttpClient())
            {
                PrepareHeaders(client);
                var path = $"{ConfigManager.ServiceUrl}{ServicePath}";
                var response = await client.PostAsJsonAsync(path, entity);
                return response.IsSuccessStatusCode;    // TODO should change to return resource id
            }
        }

        public void Update(T entity, string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Remove(string id)
        {
            using (var client = new HttpClient())
            {
                PrepareHeaders(client);
                var path = $"{ConfigManager.ServiceUrl}{ServicePath}/{id}";
                var result = await client.DeleteAsync(path);
                return result.IsSuccessStatusCode;
            }
        }

        protected void PrepareHeaders(HttpClient client)
        {
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
            // currently no authentication
        }

        protected async Task<Items<T>> GetItems(string path)
        {
            using (var client = new HttpClient())
            {
                PrepareHeaders(client);
                var result = await client.GetStringAsync(path);
                return Serializer.Deserialize<Items<T>>(result);
            }
        } 
    }
}