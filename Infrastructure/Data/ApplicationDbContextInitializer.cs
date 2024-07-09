using Application.Common.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class InitializerExtensions
{
    public static async Task InitializeDbContext(this WebApplication webApplication)
    {
        var scope = webApplication.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
        await initializer.InitializeDbContext();
    }
}

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IAuthService _authService;

    public ApplicationDbContextInitializer(ApplicationDbContext dbContext, IAuthService authService)
    {
        _dbContext = dbContext;
        _authService = authService;
    }

    public async Task InitializeDbContext()
    {
        var userId = Guid.NewGuid();
        
        if (!_dbContext.Set<User>().Any())
        {
            var user = new User
            {
                Id = userId,
                Name = "Test User",
                Email = "test@test.com",
                CreatedAt = DateTime.UtcNow
            };

            await _authService.Register(user, "password");
        }

        if(!_dbContext.Set<Product>().Any())
        {
            await _dbContext.AddRangeAsync(new Product[]
            {
                new()
                {
                    Name = "Product 1",
                    Price = 123,
                    CreatedUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Description = "Some description"
                },
                new()
                {
                    Name = "Product 2",
                    Price = 321,
                    CreatedUserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Description = "Some description"
                }
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}
