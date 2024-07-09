using Application.Common.Services;
using FluentResults;
using MediatR;

namespace Application;

public class LoginUserQuery : IRequest<Result<string>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Result<string>>
{
    private readonly IAuthService authService;

    public LoginUserQueryHandler(IAuthService authService)
    {
        this.authService = authService;
    }

    public async Task<Result<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        return await authService.Login(request.Email, request.Password);
    }
}