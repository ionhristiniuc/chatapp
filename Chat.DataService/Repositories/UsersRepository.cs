using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.DataService.DB;
using DTO;
using System.Data.Entity;
using AutoMapper;
using DTO.Entities;

namespace Chat.DataService.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public async Task<IEnumerable<UserModel>> GetUserFriends(string userId)
        {
            using (var dbModel = new ChatEntities())
            {
                var friends = await dbModel.Users
                    .Include(u => u.Friends)
                    .Include(u => u.FriendFor)
                    .Where(u => u.Friends.Any(f => f.Id == userId) || u.FriendFor.Any(f => f.Id == userId))
                    .ToListAsync();

                var models = Mapper.Map<IEnumerable<UserModel>>(friends);
                return models;
            }
        }

        public async Task<UserModel> GetUser(string id)
        {
            using (var dbModel = new ChatEntities())
            {
                var user = await dbModel.Users
                    .Include(u => u.Friends)
                    .Include(u => u.FriendFor)
                    .FirstOrDefaultAsync(u => u.Id == id);

                var model = Mapper.Map<UserModel>(user);

                model.Friends = Mapper.Map<IEnumerable<UserModel>>(user.Friends)
                        .Concat(Mapper.Map<IEnumerable<UserModel>>(user.FriendFor)); 
                               
                return model;
            }
        }
    }
}