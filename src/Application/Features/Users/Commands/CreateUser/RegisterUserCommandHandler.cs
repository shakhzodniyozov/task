using Application.Common.Interfaces;
using Application.Common.Responses;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Users.Commands.CreateUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, BaseResponse<Guid>>
{
    private readonly IMapper _mapper;
    private readonly IAuthenticationService _authenticationService;

    public RegisterUserCommandHandler(IMapper mapper, IAuthenticationService authenticationService)
    {
        _mapper = mapper;
        _authenticationService = authenticationService;
    }

    public async Task<BaseResponse<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        var registerResult = await _authenticationService.Register(user, request.Password);

        return registerResult.Value == Guid.Empty ? new ErrorResponse<Guid>("Something went wrong while registering user.") 
            : new SuccessResponse<Guid>(registerResult.Value);
    }
}