namespace OniBusExpress.Domain.Extensions
{
    public static class CpfExtensions
    {
        public static string OnlyDigits(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return new string(value.Where(char.IsDigit).ToArray());
        }

        public static bool IsValidCpf(this string value)
        {
            var cpf = value.OnlyDigits();

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            var firstCheckDigit = CalculateCheckDigit(cpf, 9);
            if (firstCheckDigit != cpf[9] - '0')
                return false;

            var secondCheckDigit = CalculateCheckDigit(cpf, 10);
            if (secondCheckDigit != cpf[10] - '0')
                return false;

            return true;
        }

        private static int CalculateCheckDigit(string cpf, int length)
        {
            var sum = 0;
            var weight = length + 1;

            for (var i = 0; i < length; i++)
                sum += (cpf[i] - '0') * weight--;

            var remainder = sum % 11;

            return remainder < 2 ? 0 : 11 - remainder;
        }
    }
}
