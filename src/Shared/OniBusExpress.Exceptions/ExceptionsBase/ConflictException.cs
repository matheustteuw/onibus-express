using System.Net;

namespace OniBusExpress.Exceptions.ExceptionsBase
{
    public class ConflictException : OniBusExpressException
    {
        public ConflictException(string message) : base(message)
        {
        }

        public override IList<string> GetErrorMessages() => [Message];

        public override HttpStatusCode GetStatusCode() => HttpStatusCode.Conflict;
    }
}
