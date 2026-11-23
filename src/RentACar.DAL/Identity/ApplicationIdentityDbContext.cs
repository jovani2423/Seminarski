
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentACar.DAL.Entities;

namespace RentACar.DAL.Identity
{
    public class ApplicationIdentityDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationIdentityDbContext(
            DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<CarCategory> CarCategories { get; set; }

        public DbSet<Branch> Branches { get; set; }

        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Car>()
                .HasIndex(c => c.RegistrationNumber)
                .IsUnique();

            builder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasPrecision(10, 2);

            builder.Entity<Reservation>()
                .HasOne(r => r.Car)
                .WithMany()
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(10, 2);

            builder.Entity<Car>()
                .HasOne(c => c.CarCategory)
                .WithMany(category => category.Cars)
                .HasForeignKey(c => c.CarCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Car>()
                .HasOne(c => c.Branch)
                .WithMany(branch => branch.Cars)
                .HasForeignKey(c => c.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MaintenanceRecord>()
                .HasOne(m => m.Car)
                .WithMany(c => c.MaintenanceRecords)
                .HasForeignKey(m => m.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MaintenanceRecord>()
                .Property(m => m.Cost)
                .HasPrecision(10, 2);
        }
    }
}
