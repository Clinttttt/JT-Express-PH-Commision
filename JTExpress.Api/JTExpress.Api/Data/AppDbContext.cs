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

        modelBuilder.Entity<ServiceEntity>().HasData(
            new ServiceEntity { Id = 1, Name = "Express Delivery", Description = "Next-day delivery within Metro Manila. Perfect for urgent shipments and time-sensitive packages.", Icon = "EXP", PriceLabel = "PHP 89+" },
            new ServiceEntity { Id = 2, Name = "Standard Delivery", Description = "2-5 business days to any province nationwide. Reliable and cost-effective for regular shipments.", Icon = "STD", PriceLabel = "PHP 60+" },
            new ServiceEntity { Id = 3, Name = "Cash on Delivery", Description = "Buyer pays only upon receiving the parcel. Secure payment option for online sellers.", Icon = "COD", PriceLabel = "Free" },
            new ServiceEntity { Id = 4, Name = "Bulky Cargo", Description = "Specialized handling for oversized and heavy items. Professional care for valuable goods.", Icon = "BLK", PriceLabel = "Custom" },
            new ServiceEntity { Id = 5,  Name = "Door-to-Door", Description = "Picked up and delivered to your exact address. Convenient service for busy customers.", Icon = "D2D", PriceLabel = "PHP 79+" });

        modelBuilder.Entity<RateEntity>().HasData(
            new RateEntity { Id = 1, Zone = "Metro Manila", FirstKg = 89m, SucceedingKg = 19m },
            new RateEntity { Id = 2, Zone = "Luzon", FirstKg = 120m, SucceedingKg = 29m },
            new RateEntity { Id = 3, Zone = "Visayas", FirstKg = 150m, SucceedingKg = 39m },
            new RateEntity { Id = 4, Zone = "Mindanao", FirstKg = 150m, SucceedingKg = 39m },
            new RateEntity { Id = 5, Zone = "Island Provinces", FirstKg = 180m, SucceedingKg = 49m });

        modelBuilder.Entity<TrackingResultEntity>().HasData(
            new TrackingResultEntity { Id = 1, TrackingNumber = "JT123456789PH", Status = "Delivered", Sender = "Juan dela Cruz", Recipient = "Maria Santos", EstimatedDelivery = "May 13, 2025", CurrentLocation = "Makati City" },
            new TrackingResultEntity { Id = 2, TrackingNumber = "JT987654321PH", Status = "In Transit", Sender = "Pedro Reyes", Recipient = "Ana Lim", EstimatedDelivery = "May 16, 2025", CurrentLocation = "Manila Sorting Center" },
            new TrackingResultEntity { Id = 3, TrackingNumber = "JT555000111PH", Status = "Out for Delivery", Sender = "Carlo Mendoza", Recipient = "Rose Villanueva", EstimatedDelivery = "May 15, 2025", CurrentLocation = "Davao Hub" });

        modelBuilder.Entity<TrackingEventEntity>().HasData(
            new TrackingEventEntity { Id = 1, TrackingResultEntityId = 1, Date = "May 13 03:00 PM", Status = "Delivered", Location = "Makati City" },
            new TrackingEventEntity { Id = 2, TrackingResultEntityId = 1, Date = "May 13 09:00 AM", Status = "Out for Delivery", Location = "Makati Hub" },
            new TrackingEventEntity { Id = 3, TrackingResultEntityId = 1, Date = "May 12 11:00 PM", Status = "Arrived at Hub", Location = "Makati Hub" },
            new TrackingEventEntity { Id = 4, TrackingResultEntityId = 1, Date = "May 12 06:00 AM", Status = "Parcel Picked Up", Location = "Quezon City" },
            new TrackingEventEntity { Id = 5, TrackingResultEntityId = 2, Date = "May 14 08:00 AM", Status = "In Transit", Location = "Manila Sorting Center" },
            new TrackingEventEntity { Id = 6, TrackingResultEntityId = 2, Date = "May 13 04:00 PM", Status = "Parcel Picked Up", Location = "Cebu City" },
            new TrackingEventEntity { Id = 7, TrackingResultEntityId = 3, Date = "May 15 07:00 AM", Status = "Out for Delivery", Location = "Davao Hub" },
            new TrackingEventEntity { Id = 8, TrackingResultEntityId = 3, Date = "May 14 10:00 PM", Status = "Arrived at Hub", Location = "Davao Hub" },
            new TrackingEventEntity { Id = 9, TrackingResultEntityId = 3, Date = "May 14 02:00 PM", Status = "In Transit", Location = "Manila Hub" });

        modelBuilder.Entity<BranchEntity>().HasData(
            new BranchEntity { Id = 1, Name = "J&T Express — Makati", Address = "123 Ayala Ave, Makati City", Region = "Metro Manila", Phone = "(02) 8123-4001", Hours = "Mon–Sat 8AM–6PM", Latitude = 14.5547, Longitude = 121.0244 },
            new BranchEntity { Id = 2, Name = "J&T Express — Quezon City", Address = "45 Quezon Ave, QC", Region = "Metro Manila", Phone = "(02) 8123-4002", Hours = "Mon–Sat 8AM–6PM", Latitude = 14.6760, Longitude = 121.0437 },
            new BranchEntity { Id = 3, Name = "J&T Express — Cebu", Address = "78 Colon St, Cebu City", Region = "Visayas", Phone = "(032) 412-3001", Hours = "Mon–Sat 8AM–6PM", Latitude = 10.2969, Longitude = 123.9016 },
            new BranchEntity { Id = 4, Name = "J&T Express — Davao", Address = "22 Ilustre St, Davao City", Region = "Mindanao", Phone = "(082) 227-3001", Hours = "Mon–Sat 8AM–6PM", Latitude = 7.0731, Longitude = 125.6128 },
            new BranchEntity { Id = 5, Name = "J&T Express — Pampanga", Address = "100 MacArthur Hwy, San Fernando", Region = "Luzon", Phone = "(045) 961-3001", Hours = "Mon–Sat 8AM–6PM", Latitude = 15.0794, Longitude = 120.6200 },
            new BranchEntity { Id = 6, Name = "J&T Express — Iloilo", Address = "55 Iznart St, Iloilo City", Region = "Visayas", Phone = "(033) 335-3001", Hours = "Mon–Sat 8AM–6PM", Latitude = 10.6969, Longitude = 122.5640 });
    }
}

public sealed class ServiceEntity
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Icon { get; init; }
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
    public required string PasswordHash { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
