using OniBusExpress.Domain.Enums;

namespace OniBusExpress.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string ReservationCode { get; set; } = null!;
        public Guid TripId { get; set; }
        public Trip Trip { get; set; } = null!;
        public Guid PassengerId { get; set; }
        public Passenger Passenger { get; set; } = null!;
        public int SeatNumber { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
