using Microsoft.EntityFrameworkCore;

namespace JTExpress.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ServiceEntity> Services => Set<ServiceEntity>();
    public DbSet<RateEntity> Rates => Set<RateEntity>();
    public DbSet<TrackingResultEntity> TrackingResults => Set<TrackingResultEntity>();
    public DbSet<TrackingEventEntity> TrackingEvents => Set<TrackingEventEntity>();
    public DbSet<BranchEntity> Branches => Set<BranchEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RateEntity>(entity =>
        {
            entity.Property(rate => rate.FirstKg).HasPrecision(10, 2);
            entity.Property(rate => rate.SucceedingKg).HasPrecision(10, 2);
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
        });
    }
}

public sealed class ServiceEntity
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string PriceLabel { get; init; }
}

public sealed class RateEntity
{
    public int Id { get; init; }
    public required string Zone { get; set; }
    public decimal FirstKg { get; set; }
    public decimal SucceedingKg { get; set; }
}

public sealed class TrackingResultEntity
{
    public int Id { get; init; }
    public required string TrackingNumber { get; set; }
    public required string Status { get; set; }
    public required string Sender { get; set; }
    public required string Recipient { get; set; }
    public required string EstimatedDelivery { get; set; }
    public required string CurrentLocation { get; set; }
    public List<TrackingEventEntity> Timeline { get; set; } = [];
}

public sealed class TrackingEventEntity
{
    public int Id { get; init; }
    public int TrackingResultEntityId { get; set; }
    public required string Date { get; set; }
    public required string Status { get; set; }
    public required string Location { get; set; }
}

public sealed class BranchEntity
{
    public int Id { get; init; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Region { get; set; }
    public required string Phone { get; set; }
    public required string Hours { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public sealed class UserEntity
{
    public int Id { get; init; }
    public required string Username { get; init; }
    public required string PasswordHash { get; set; }
    public required string RestorationKey { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
