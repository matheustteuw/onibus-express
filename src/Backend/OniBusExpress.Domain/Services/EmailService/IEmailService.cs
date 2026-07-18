namespace OniBusExpress.Domain.Services.EmailService
{
    public interface IEmailService
    {
        Task SendReservationConfirmation(
            string toEmail,
            string passengerName,
            string reservationCode,
            string origin,
            string destination,
            DateTime departureTime,
            int seatNumber);
    }
}
