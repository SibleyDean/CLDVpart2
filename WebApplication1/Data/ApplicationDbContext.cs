using Microsoft.EntityFrameworkCore;
using eventEasefour.Models;

namespace eventEasefour.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // ✅ DbSets - Main Tables
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Bookings> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Venues Table Configuration
            modelBuilder.Entity<Venue>(entity =>
            {
                entity.ToTable("Venues");
                entity.HasKey(v => v.VenueId);

                entity.Property(v => v.VenueName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(v => v.Location)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(v => v.Capacity)
                    .IsRequired();

                entity.Property(v => v.ImageUrl)
                    .HasColumnType("nvarchar(max)")
                    .IsRequired(false); // ✅ Nullable Image URL

                // ✅ Relationships
                entity.HasMany(v => v.Events)
                      .WithOne(e => e.Venue)
                      .HasForeignKey(e => e.VenueId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.Bookings)
                      .WithOne(b => b.Venue)
                      .HasForeignKey(b => b.VenueId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ✅ Events Table Configuration
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events");
                entity.HasKey(e => e.EventId);

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EventDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnType("nvarchar(max)");

                // ✅ Foreign Key Relationship with Venue
                entity.HasOne(e => e.Venue)
                      .WithMany(v => v.Events)
                      .HasForeignKey(e => e.VenueId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ✅ Bookings Table Configuration (If Exists)
            modelBuilder.Entity<Bookings>(entity =>
            {
                entity.ToTable("Bookings");
                entity.HasKey(b => b.BookingId);

                entity.Property(b => b.BookingDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                // ✅ Foreign Key Relationships
                entity.HasOne(b => b.Event)
                      .WithMany(e => e.Bookings)
                      .HasForeignKey(b => b.EventId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(b => b.Venue)
                      .WithMany(v => v.Bookings)
                      .HasForeignKey(b => b.VenueId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
