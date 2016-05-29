using System.Collections.Generic;

namespace DTO.Entities
{
    public class UserModel
    {
        public string Id { get; set; }
        public IEnumerable<UserModel> Friends { get; set; }      
    }
}