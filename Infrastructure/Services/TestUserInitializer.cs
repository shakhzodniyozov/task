using Application.Common.Interfaces;
using Application.Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class TestUserInitializer
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public TestUserInitializer(IApplicationDbContext dbContext, IAuthService authService, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _authService = authService;
        _configuration = configuration;
    }

    public async Task SetPasswordToTestUser()
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == "test@test.com");

        if (user is not null && user.PasswordHash is null)
        {
            _authService.CreatePasswordHash(_configuration.GetSection("SeederOptions:InitialUserPassword").Value!,
                out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dbContext.SaveChanges();
        }
    }
}