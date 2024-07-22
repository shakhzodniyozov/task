using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Users.Commands.UpdateUser.UpdateTestUser;

public class UpdateTestUserPasswordCommandHandler : IRequestHandler<UpdateTestUserPasswordCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IAuthenticationService _authenticationService;
    private readonly IConfiguration _configuration;

    public UpdateTestUserPasswordCommandHandler(IApplicationDbContext dbContext, IAuthenticationService authenticationService, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _authenticationService = authenticationService;
        _configuration = configuration;
    }


    public async Task Handle(UpdateTestUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == "test@test.com", cancellationToken);

        if (user is not null && user.PasswordHash is null)
        {
            _authenticationService.CreatePasswordHash(_configuration.GetSection("SeederOptions:InitialUserPassword").Value!,
                out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dbContext.SaveChanges(cancellationToken);
        }
    }
}