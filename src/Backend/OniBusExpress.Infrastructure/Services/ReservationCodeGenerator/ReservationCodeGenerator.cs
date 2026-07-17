using System.Security.Cryptography;
using System.Text;
using OniBusExpress.Domain.Services.ReservationCodeGenerator;

namespace OniBusExpress.Infrastructure.Services.ReservationCodeGenerator
{
    public class ReservationCodeGenerator : IReservationCodeGenerator
    {
        private const string Letters = "ABCDEFGHJKLMNPQRSTUVWXYZ"; 
        private const string Digits = "0123456789";
        private const int LettersLength = 3;
        private const int DigitsLength = 5;

        public string Generate()
        {
            var builder = new StringBuilder(LettersLength + 1 + DigitsLength);

            for (var i = 0; i < LettersLength; i++)
                builder.Append(Letters[RandomNumberGenerator.GetInt32(Letters.Length)]);

            builder.Append('-');

            for (var i = 0; i < DigitsLength; i++)
                builder.Append(Digits[RandomNumberGenerator.GetInt32(Digits.Length)]);

            return builder.ToString();
        }
    }
}
