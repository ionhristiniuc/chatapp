using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Chat.DataService.Core;
using Chat.DataService.Repositories;
using DTO.Containers;
using DTO.Entities;

namespace Chat.DataService.Controllers
{
    [RoutePrefix("users")]
    public class UsersController : ApiController
    {
        private readonly IUsersRepository _usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [Route("{userId}/friends", Name = "GetFriends")]
        public async Task<IHttpActionResult> GetFriends(string userId)
        {
            var friends = await _usersRepository.GetUserFriends(userId);
            var result = ObjectMapper.ToItems(friends, 0, 100, friends.Count());
            return Ok(result);
        }

        [Route("{id}", Name = "GetUser")]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            try
            {
                var user = await _usersRepository.GetUser(id);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception e)
            {
                //Trace.WriteLine(e.ToString());
                return InternalServerError();
            }
        }
    }
}
