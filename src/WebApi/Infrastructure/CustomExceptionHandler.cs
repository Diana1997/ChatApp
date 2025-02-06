using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application._Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace WebApi.Infrastructure
{
    public class CustomExceptionHandler
    {
        private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

        public CustomExceptionHandler()
        {
            _exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, Task>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            };
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionType = exception.GetType();

            if (_exceptionHandlers.TryGetValue(exceptionType, out var handler))
            {
                await handler.Invoke(httpContext, exception);
                return true;
            }

            await HandleUnknownException(httpContext, exception);
            return true;
        }

        private async Task HandleValidationException(HttpContext httpContext, Exception ex)
        {
            var exception = (ValidationException)ex;

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/json"; 

            var errorResponse = new ErrorResponse(StatusCodes.Status400BadRequest, errors: exception.Errors);
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
        
        private async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            httpContext.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse(StatusCodes.Status401Unauthorized);
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }

        private async Task HandleForbiddenAccessException(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            httpContext.Response.ContentType = "application/json"; 

            var errorResponse = new ErrorResponse(StatusCodes.Status403Forbidden);
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
        
        private async Task HandleUnknownException(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/json"; 

            var errorResponse = new ErrorResponse(StatusCodes.Status500InternalServerError, message: "An unexpected error occurred.");
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
