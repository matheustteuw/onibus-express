namespace OniBusExpress.Communication.Requests
{
    public class RequestRegisterReservationJson
    {
        public Guid TripId { get; set; }
        public int SeatNumber { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; }
    }
}
