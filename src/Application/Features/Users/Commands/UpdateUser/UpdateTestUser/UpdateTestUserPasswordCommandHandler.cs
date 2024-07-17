using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Features.Users.Commands.Update.UpdateTestUser;

public class UpdateTestUserPasswordCommandHandler : IRequestHandler<UpdateTestUserPasswordCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public UpdateTestUserPasswordCommandHandler(IApplicationDbContext dbContext, IAuthService authService, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _authService = authService;
        _configuration = configuration;
    }


    public async Task Handle(UpdateTestUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == "test@test.com", cancellationToken);

        if (user is not null && user.PasswordHash is null)
        {
            _authService.CreatePasswordHash(_configuration.GetSection("SeederOptions:InitialUserPassword").Value!,
                out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dbContext.SaveChanges(cancellationToken);
        }
    }
}