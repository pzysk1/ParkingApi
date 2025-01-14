namespace ParkingApi.Models.DbContexts
{
    using Microsoft.EntityFrameworkCore;
    using ParkingApi.Models.Entities;

    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options) { }

        public DbSet<Parking> Parking { get; set; }

        public DbSet<Cars> Cars { get; set; }

        public DbSet<ParkingCars> ParkingCars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParkingCars>()
                .HasOne(pc => pc.Parking)
                .WithMany()
                .HasForeignKey(pc => pc.ParkingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ParkingCars>()
                .HasOne(pc => pc.Cars)
                .WithMany()
                .HasForeignKey(pc => pc.CarId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
