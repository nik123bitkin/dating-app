using System;
using System.Net;
using System.Threading.Tasks;
using AppCore.Exceptions;
using Microsoft.AspNetCore.Http;

namespace date_app.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if ((exception is AlreadyExistsException) || (exception is NotFoundException) || (exception is ForbiddenActionException))
                {
                    await httpContext.Response.WriteAsync(exception.Message);
                }
                else
                {
                    await httpContext.Response.WriteAsync("Internal server error.");
                }
            }
        }
    }
}
