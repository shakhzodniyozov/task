using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users.Queries.LoginUserQuery;

public class LoginUserMappings : Profile
{
    public LoginUserMappings()
    {
        CreateMap<User, UserDto>();
    }
}