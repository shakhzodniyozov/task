using Application.Features.Users.Commands.Update.UpdateUser;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Users.Commands.UpdateUser.UpdateUser;

public class UpdateUserMappings : Profile
{
    public UpdateUserMappings()
    {
        CreateMap<UpdateUserCommand, User>();
    }
}