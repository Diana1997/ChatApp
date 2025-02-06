using System.Threading.Tasks;
using Application._Common.Interfaces;
using Application._Common.Models;
using Domain;
using Microsoft.AspNetCore.Http;

namespace WebApi.Attributes
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEventLogger _eventLogger;

        public RequestLoggingMiddleware(RequestDelegate next, IEventLogger eventLogger)
        {
            _next = next;
            _eventLogger = eventLogger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;

            await _eventLogger.LogEvent(
                new EventLogModel
                {
                    ActionType = ActionType.ApiCall,
                    Details = $"Request made to {requestPath}",
                    RequestMethod = requestMethod,
                    RequestPath = requestPath,
                });

            await _next(context);

            var responseStatus = context.Response.StatusCode.ToString();
            
            await _eventLogger.LogEvent(
                new EventLogModel
                {
                    ActionType = ActionType.ApiResponse,
                    Details = $"Response sent from {requestPath}",
                    RequestMethod = requestMethod,
                    ResponseStatus = responseStatus
                });
        }
    }

}