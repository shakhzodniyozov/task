using Application.Common.Services;
using AutoMapper;
using Domain.Entities;
using FluentResults;
using MediatR;

namespace Application.Features.Users.Commands.Create;

public class RegisterUserCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IMapper mapper;
    private readonly IAuthService authService;

    public RegisterUserCommandHandler(IMapper mapper, IAuthService authService)
    {
        this.mapper = mapper;
        this.authService = authService;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<User>(request);
        var registerResult = await authService.Register(user, request.Password);

        return registerResult.Value == Guid.Empty ? Result.Fail("Somesthig went wrong while registering user.") : Result.Ok(registerResult.Value);
    }
}