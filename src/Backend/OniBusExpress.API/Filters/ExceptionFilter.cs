using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OniBusExpress.Communication.Responses;
using OniBusExpress.Exceptions;
using OniBusExpress.Exceptions.ExceptionsBase;
using System.Net;

namespace OniBusExpress.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is OniBusExpressException onibusExpressException)
                HandleProjectException(onibusExpressException, context);
            else
            {
                ThrowUnknowException(context);
            }
        }

        private static void HandleProjectException(OniBusExpressException onibusExpressException, ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)onibusExpressException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(onibusExpressException.GetErrorMessages()));
        }

        private static void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
        }
    }
}
