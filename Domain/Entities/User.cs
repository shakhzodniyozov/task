using Domain.Common;

namespace Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public List<Product> Products { get; set; } = new();
}
