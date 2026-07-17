using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OniBusExpress.Domain.Entities;

namespace OniBusExpress.Infrastructure.DataAccess.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.ToTable("Trips");

            builder.HasKey(trip => trip.Id);

            builder.Property(trip => trip.DepartureTime)
                .IsRequired();

            builder.Property(trip => trip.BasePrice)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(trip => trip.TotalSeats)
                .IsRequired();

            builder.HasMany(trip => trip.Reservations)
                .WithOne(reservation => reservation.Trip)
                .HasForeignKey(reservation => reservation.TripId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
