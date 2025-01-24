using Domain.Users;
using UseCases.Users.Response;
using AutoMapper;

namespace WebApi.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponse>();

            CreateMap<UserResponse, User>();
        }
    }
}

