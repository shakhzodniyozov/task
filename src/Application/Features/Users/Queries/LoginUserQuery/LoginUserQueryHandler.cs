using Application.Common.Interfaces;
using Application.Common.Responses;
using MediatR;

namespace Application.Features.Users.Queries.LoginUserQuery;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, BaseResponse<string>>
{
    private readonly IAuthenticationService _authenticationService;

    public LoginUserQueryHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<BaseResponse<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _authenticationService.Login(request.Email, request.Password);

        return result.IsSuccess ? new SuccessResponse<string>(result.Value)
            : new ErrorResponse<string>(result.Reasons.FirstOrDefault()?.Message);
    }
}