using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces;
using Application.Common.Services;
using Domain.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Common.Services;

public class AuthService : IAuthService
{
    public AuthService(IApplicationDbContext dbContext,
                        IConfiguration configuration,
                        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _dbContext;

    public async Task<Result<string>> Login(string email, string password)
    {
        var user = await _dbContext.Users.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

        if (user is null)
        {
            return new Error("User with provided Email was not found.");
        }

        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
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

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChanges();
            
            return Result.Ok(user.Id);
        }

        return Result.Fail("User with provided Email already exists.");
    }

    public async Task<bool> UserExists(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

        return user is not null;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
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

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

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
        return Guid.Parse(_httpContextAccessor.HttpContext!.User.Claims.Single(x => x.Type == "userId").Value);
    }
}
