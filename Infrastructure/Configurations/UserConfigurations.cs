using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasMany(x => x.Products)
            .WithOne(x => x.CreatedUser)
            .HasForeignKey(x => x.CreatedUserId);
        
        builder.HasData(new User
        {
            Id = Guid.NewGuid(),
            Name = "Test user",
            Email = "test@test.com",
            CreatedAt = DateTime.UtcNow
        });
    }
}