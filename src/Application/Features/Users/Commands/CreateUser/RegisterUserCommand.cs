using Application.Common.Responses;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public class RegisterUserCommand : IRequest<BaseResponse<Guid>>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}