using FluentResults;
using MediatR;

namespace Application.Features.Users.Commands.Create;

public class RegisterUserCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}