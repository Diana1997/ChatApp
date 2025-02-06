using System;
using System.Threading.Tasks;
using Application._Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace WebApi.Infrastructure
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Pass the request to the next middleware
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case ValidationException validationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        error = validationException.Message
                    }));
                    break;

                case UnauthorizedAccessException exception:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        error = "Unauthorized access"
                    }));
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        error = "An unexpected error occurred."
                    }));
                    break;
            }
        }
    }
}