using FluentAssertions;
using OniBusExpress.Domain.Extensions;

namespace OniBusExpress.Tests.Domain.Extensions
{
    public class CpfExtensionsTest
    {
        [Theory]
        [InlineData("11144477735")]
        [InlineData("111.444.777-35")]
        public void Valid_Cpf_Returns_True(string cpf)
        {
            cpf.IsValidCpf().Should().BeTrue();
        }

        [Theory]
        [InlineData("11144477736")]
        [InlineData("11111111111")]
        [InlineData("123")]
        [InlineData("")]
        public void Invalid_Cpf_Returns_False(string cpf)
        {
            cpf.IsValidCpf().Should().BeFalse();
        }
    }
}
