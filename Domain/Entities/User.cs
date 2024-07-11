using Domain.Common;

namespace Domain.Entities;

public class User : BaseAuditableEntity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public List<Product> Products { get; set; } = new();
}
