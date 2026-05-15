using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JTExpress.Api;

public partial class JtexpressDbDbContext : DbContext
{
    public JtexpressDbDbContext()
    {
    }

    public JtexpressDbDbContext(DbContextOptions<JtexpressDbDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Rate> Rates { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<TrackingEvent> TrackingEvents { get; set; }

    public virtual DbSet<TrackingResult> TrackingResults { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rate>(entity =>
        {
            entity.Property(e => e.FirstKg).HasPrecision(10, 2);
            entity.Property(e => e.SucceedingKg).HasPrecision(10, 2);
        });

        modelBuilder.Entity<TrackingEvent>(entity =>
        {
            entity.HasIndex(e => e.TrackingResultEntityId, "IX_TrackingEvents_TrackingResultEntityId");

            entity.HasOne(d => d.TrackingResultEntity).WithMany(p => p.TrackingEvents).HasForeignKey(d => d.TrackingResultEntityId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
