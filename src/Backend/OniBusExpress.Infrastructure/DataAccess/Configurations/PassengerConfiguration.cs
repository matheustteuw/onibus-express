using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OniBusExpress.Domain.Entities;

namespace OniBusExpress.Infrastructure.DataAccess.Configurations
{
    public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
    {
        public void Configure(EntityTypeBuilder<Passenger> builder)
        {
            builder.ToTable("Passengers");

            builder.HasKey(passenger => passenger.Id);

            builder.Property(passenger => passenger.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(passenger => passenger.Cpf)
                .IsRequired()
                .HasMaxLength(11)
                .IsFixedLength();

            builder.Property(passenger => passenger.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(passenger => passenger.BirthDate)
                .IsRequired();
        }
    }
}
