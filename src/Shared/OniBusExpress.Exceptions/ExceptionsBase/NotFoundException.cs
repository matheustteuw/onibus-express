using System.Net;

namespace OniBusExpress.Exceptions.ExceptionsBase
{
    public class NotFoundException : OniBusExpressException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
    }
}
