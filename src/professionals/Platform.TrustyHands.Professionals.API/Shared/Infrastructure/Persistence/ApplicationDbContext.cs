using Microsoft.EntityFrameworkCore;
using Platform.TrustyHands.Professionals.API.Shared.Models;

namespace Platform.TrustyHands.Professionals.API.Shared.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Professional> Professionals => Set<Professional>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Professional>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Specialty).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}
