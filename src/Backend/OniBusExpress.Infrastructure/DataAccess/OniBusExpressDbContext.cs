using Microsoft.EntityFrameworkCore;
using OniBusExpress.Domain.Entities;

namespace OniBusExpress.Infrastructure.DataAccess
{
    public class OniBusExpressDbContext : DbContext
    {
        public OniBusExpressDbContext(DbContextOptions<OniBusExpressDbContext> options)
            : base(options)
        {
        }

        public DbSet<Route> Routes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OniBusExpressDbContext).Assembly);
        }
    }
}
