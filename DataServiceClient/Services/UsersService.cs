using System.Collections.Generic;
using System.Threading.Tasks;
using DataServiceClient.Configuration;
using DataServiceClient.Services.Base;
using DTO;
using DTO.Containers;
using DTO.Entities;

namespace DataServiceClient.Services
{
    public class UsersService : GenericService<UserModel>, IUsersService
    {
        public UsersService(string accessToken)
            : base(accessToken, "users")
        {

        }

        public async Task<Items<UserModel>> GetFriends(string userId)
        {
            var path = $"{ConfigManager.ServiceUrl}/{ServicePath}/{userId}/friends";
            return await base.GetItems(path);
        }
    }
}