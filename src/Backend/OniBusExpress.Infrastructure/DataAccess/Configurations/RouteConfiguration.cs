using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OniBusExpress.Domain.Entities;

namespace OniBusExpress.Infrastructure.DataAccess.Configurations
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("Routes");

            builder.HasKey(route => route.Id);

            builder.Property(route => route.Origin)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(route => route.Destination)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(route => route.EstimatedDuration)
                .IsRequired();

            builder.HasMany(route => route.Trips)
                .WithOne(trip => trip.Route)
                .HasForeignKey(trip => trip.RouteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
