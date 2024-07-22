using Application.Common.Responses;
using MediatR;

namespace Application.Features.Users.Queries.LoginUserQuery;

public class LoginUserQuery : IRequest<BaseResponse<string>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}