using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users.Queries.GetUserByIdQuery;

public class GetUserByIdMappings : Profile
{
    public GetUserByIdMappings()
    {
        CreateMap<User, GetUserByIdDto>();
    }
}