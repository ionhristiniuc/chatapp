using AutoMapper;
using Chat.DataService.DB;
using DTO.Entities;

namespace Chat.DataService.Core.Mapping
{
    public class UserMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<User, UserModel>()
                .ForMember(u => u.Friends, c => c.Ignore());

            //CreateMap<UserModel, User>();
        }
    }
}