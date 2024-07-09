using Domain.Entities;
using FluentResults;

namespace Application.Common.Services;

public interface IAuthService
{
    Task<Result<Guid>> Register(User user, string password);
    Task<Result<string>> Login(string email, string password);
    Guid GetUserId();
}
