using Application.Features.Users.Queries;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Commands.Update.UpdateUser;

public class UpdateUserCommand : IRequest<Result<UserDto>>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}