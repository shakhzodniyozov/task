using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Services;
using Arch.EntityFrameworkCore.UnitOfWork;
using Domain.Entities;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    public AuthService(IUnitOfWork uow, IConfiguration configuration, TokenValidationParameters tokenValidationParameters, IHttpContextAccessor httpContextAccessor)
    {
        userRepo = uow.GetRepository<User>();
        this.uow = uow;
        this.configuration = configuration;
        this.tokenValidationParameters = tokenValidationParameters;
        this.httpContextAccessor = httpContextAccessor;
    }

    private readonly IRepository<User> userRepo;
    private readonly IUnitOfWork uow;
    private readonly IConfiguration configuration;
    private readonly TokenValidationParameters tokenValidationParameters;
    private readonly IHttpContextAccessor httpContextAccessor;

    public async Task<Result<string>> Login(string email, string password)
    {
        var user = await userRepo.GetFirstOrDefaultAsync(predicate: x => x.Email.ToLower() == email.ToLower());

        if (user is null)
        {
            return new Error("User with provided Email was not found.");
        }
        else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            return new Error("Wrong password.");
        }

        var accessToken = CreateToken(user);
        return Result.Ok(accessToken);
    }

    public async Task<Result<Guid>> Register(User user, string password)
    {
        if (!await UserExists(user.Email))
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.CreatedAt = DateTime.UtcNow;

            await userRepo.InsertAsync(user);
            await uow.SaveChangesAsync();
            
            return Result.Ok(user.Id);
        }

        return Result.Fail("User with provided Email already exists.");
    }

    public async Task<bool> UserExists(string email)
    {
        var user = await userRepo.GetFirstOrDefaultAsync(predicate: x => x.Email.ToLower() == email.ToLower());

        return user is not null;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
            {
                new("userId", user.Id.ToString()),
                new("email", user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return accessToken;
    }

    public Guid GetUserId()
    {
        return Guid.Parse(httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == "userId").Value);
    }
}
