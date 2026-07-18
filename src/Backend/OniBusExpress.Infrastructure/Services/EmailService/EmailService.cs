using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using OniBusExpress.Domain.Services.EmailService;

namespace OniBusExpress.Infrastructure.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendReservationConfirmation(
            string toEmail,
            string passengerName,
            string reservationCode,
            string origin,
            string destination,
            DateTime departureTime,
            int seatNumber)
        {
            var host = _configuration["SMTP_HOST"] ?? throw new InvalidOperationException("SMTP_HOST não configurado");
            var port = int.Parse(_configuration["SMTP_PORT"] ?? "587");
            var user = _configuration["SMTP_USER"] ?? throw new InvalidOperationException("SMTP_USER não configurado");
            var pass = _configuration["SMTP_PASS"] ?? throw new InvalidOperationException("SMTP_PASS não configurado");
            var from = _configuration["EMAIL_FROM"] ?? user;

            using var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };

            using var message = new MailMessage(from, toEmail)
            {
                Subject = $"Reserva confirmada - {reservationCode}",
                Body = BuildBody(passengerName, reservationCode, origin, destination, departureTime, seatNumber),
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);
        }

        private static string BuildBody(
            string passengerName,
            string reservationCode,
            string origin,
            string destination,
            DateTime departureTime,
            int seatNumber)
        {
            return $"""
                <p>Olá, {passengerName}!</p>
                <p>Sua reserva foi confirmada com sucesso.</p>
                <ul>
                    <li><strong>Código da reserva:</strong> {reservationCode}</li>
                    <li><strong>Trajeto:</strong> {origin} → {destination}</li>
                    <li><strong>Data e hora de partida:</strong> {departureTime:dd/MM/yyyy HH:mm}</li>
                    <li><strong>Assento:</strong> {seatNumber}</li>
                </ul>
                <p>Guarde o código da reserva para consultas futuras.</p>
                """;
        }
    }
}
