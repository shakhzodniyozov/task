using Application.Features.Users.Commands.Create;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users.Commands.CreateUser;

public class RegisterUserMappings : Profile
{
    public RegisterUserMappings()
    {
        CreateMap<RegisterUserCommand, User>();
    }
}