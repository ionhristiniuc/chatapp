using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DTO;
using DTO.Entities;

namespace Chat.DataService.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UserModel>> GetUserFriends(string userId);
        Task<UserModel> GetUser(string id);
    }
}