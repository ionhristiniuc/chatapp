using System.Collections.Generic;
using System.Threading.Tasks;
using DataServiceClient.Services.Base;
using DTO;
using DTO.Containers;
using DTO.Entities;

namespace DataServiceClient.Services
{
    public interface IUsersService : IGenericService<UserModel>
    {
        //Task<UserModel> GetCurrent();    
        Task<Items<UserModel>> GetFriends(string userId);
    }
}