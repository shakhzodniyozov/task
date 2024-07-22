using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users.Queries.GetAllUsersQuery;

public class GetAllUserMappings : Profile
{
    public GetAllUserMappings()
    {
        CreateMap<User, UserDto>();
    }
}