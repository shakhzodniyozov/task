using Application.Common.Interfaces;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Queries.LoginUserQuery;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<string>>
{
    private readonly IAuthService _authService;

    public LoginUserQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        return await _authService.Login(request.Email, request.Password);
    }
}