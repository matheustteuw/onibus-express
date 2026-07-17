using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OniBusExpress.Domain.Entities;
using OniBusExpress.Domain.Enums;

namespace OniBusExpress.Infrastructure.DataAccess.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");

            builder.HasKey(reservation => reservation.Id);

            builder.Property(reservation => reservation.ReservationCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(reservation => reservation.SeatNumber)
                .IsRequired();

            builder.Property(reservation => reservation.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(reservation => reservation.CreatedAt)
                .IsRequired();
    
            builder.HasIndex(reservation => reservation.ReservationCode)
                .IsUnique();
          
            builder.HasIndex(reservation => new { reservation.TripId, reservation.SeatNumber })
                .IsUnique()
                .HasFilter($"[Status] = {(int)ReservationStatus.Active}");
        }
    }
}
