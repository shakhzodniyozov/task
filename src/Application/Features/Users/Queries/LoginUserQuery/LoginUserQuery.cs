using FluentResults;
using MediatR;

namespace Application.Features.Users.Queries.LoginUserQuery;

public class LoginUserQuery : IRequest<Result<string>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}