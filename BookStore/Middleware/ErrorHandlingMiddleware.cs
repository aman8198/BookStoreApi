using BookStore.CustomExceptions;
using Newtonsoft.Json;

namespace BookStore.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning($"Resource not found: {ex.Message}");

                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = 404,
                    Message = $"Resource not found: {ex.Message}"
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex}");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = 500,
                    Message = "Internal Server Error"
                };

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
