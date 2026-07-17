namespace OniBusExpress.Communication.Responses
{
    public class ResponseTripDetailsJson
    {
        public Guid Id { get; set; }
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public decimal BasePrice { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public IList<int> OccupiedSeats { get; set; } = [];
    }
}
