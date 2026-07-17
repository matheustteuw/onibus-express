namespace OniBusExpress.Communication.Responses
{
    public class ResponseReservationJson
    {
        public string ReservationCode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int SeatNumber { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public decimal BasePrice { get; set; }
    }
}
