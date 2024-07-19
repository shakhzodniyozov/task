using Domain.Common;

namespace Domain.Entities;

public class Product : BaseAuditableEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
    public User CreatedUser { get; set; } = null!;
    public Guid CreatedUserId { get; set; }
    public Guid UpdateUserId { get; set; }
}
