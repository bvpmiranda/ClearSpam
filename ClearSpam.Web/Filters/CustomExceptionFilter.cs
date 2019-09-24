using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ClearSpam.Application.Exceptions;

namespace ClearSpam.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            switch (exception)
            {
                case ValidationException validationException:
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Result = new JsonResult(
                        validationException.Failures);

                    return;

                case ArgumentNullException _:
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Result = new JsonResult(exception.Message);

                    return;
            }

            var code = HttpStatusCode.InternalServerError;

            var multipleExceptions = false;
            if (exception is AggregateException aggregateException)
            {
                exception = exception.InnerException;
                multipleExceptions = aggregateException.InnerExceptions.Count > 1;
            }

            switch (exception)
            {
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
            }

            var returnException = multipleExceptions ? context.Exception : exception;
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new
            {
                error = new[] { returnException.Message },
                stackTrace = returnException.StackTrace
            });
        }
    }
}
