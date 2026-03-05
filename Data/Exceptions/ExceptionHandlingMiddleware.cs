using System.Net;
using System.Text.Json;

namespace TaskManagement.Data.Exceptions
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                await WriteProblemDetailsAsync(context, ex);
            }
        }

        private static Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
        {
            var (statusCode, title) = ex switch
            {
                ArgumentException => ((int)HttpStatusCode.BadRequest, "Bad Request"),
                KeyNotFoundException => ((int)HttpStatusCode.NotFound, "Not Found"),
                UnauthorizedAccessException => ((int)HttpStatusCode.Forbidden, "Forbidden"),
                _ => ((int)HttpStatusCode.InternalServerError, "Server Error")
            };

            var response = new ErrorResponse
            {
                StatusCode = statusCode,
                Title = title,
                Message = ex.Message,
                TraceId = context.TraceIdentifier
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(json);
        }

        private sealed class ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Title { get; set; } = default!;
            public string Message { get; set; } = default!;
            public string TraceId { get; set; } = default!;
        }
    }
}
