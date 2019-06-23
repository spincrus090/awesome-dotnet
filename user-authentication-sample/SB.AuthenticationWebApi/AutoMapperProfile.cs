using AutoMapper;
using SB.Model.DTO;
using SB.Model.Entity.Authentication;

namespace SB.AuthenticationWebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
