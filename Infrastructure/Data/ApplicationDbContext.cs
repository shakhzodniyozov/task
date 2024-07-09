using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfigurations());
        builder.ApplyConfiguration(new ProductConfigurations());
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var entries = ChangeTracker
            .Entries<BaseAuditableEntity>()
            .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                ((BaseAuditableEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
            else if (entry.State == EntityState.Modified)
                ((BaseAuditableEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChanges(CancellationToken cancellationToken) => await this.SaveChangesAsync(cancellationToken);
}