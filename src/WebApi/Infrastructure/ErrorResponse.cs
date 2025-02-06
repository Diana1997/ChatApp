using System.Collections.Generic;

namespace WebApi.Infrastructure
{
    public class ErrorResponse
    {
        public ErrorResponse(int statusCode, string message = null, IDictionary<string, string[]> errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public IDictionary<string, string[]> Errors { get; }
    }
}