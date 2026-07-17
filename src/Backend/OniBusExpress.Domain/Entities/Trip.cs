namespace OniBusExpress.Domain.Entities
{
    public class Trip
    {
        public Guid Id { get; set; }
        public Guid RouteId { get; set; }
        public Route Route { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public decimal BasePrice { get; set; }
        public int TotalSeats { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
