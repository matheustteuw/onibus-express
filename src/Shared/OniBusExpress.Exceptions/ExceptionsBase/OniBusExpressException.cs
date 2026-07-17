using System.Net;

namespace OniBusExpress.Exceptions.ExceptionsBase
{
    public abstract class OniBusExpressException : SystemException
    {
        protected OniBusExpressException(string message) : base(message)
        {
        }

        public abstract IList<string> GetErrorMessages();
        public abstract HttpStatusCode GetStatusCode();
    }
}
