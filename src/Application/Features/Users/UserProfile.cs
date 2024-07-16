using Application.Features.Users.Commands.Create;
using Application.Features.Users.Commands.Update;
using Application.Features.Users.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserCommand, User>();
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserCommand, User>();
    }
}