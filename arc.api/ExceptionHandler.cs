using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;

namespace arc.api
{
    /// <summary>
    /// Middleware for handling exceptions.
    /// </summary>
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandler"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger instance.</param>
        public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the exception handler asynchronously.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Microsoft.AspNetCore.Antiforgery.AntiforgeryValidationException ex)
            {
                TryLogError($"CSRF validation failed: {ex}");
                await HandleExceptionAsync(httpContext, ex, StatusCodes.Status400BadRequest, "CSRF validation failed");
            }
            catch (UnauthorizedAccessException ex)
            {
                TryLogError($"Unauthorized access: {ex}");
                await HandleExceptionAsync(httpContext, ex, StatusCodes.Status401Unauthorized, "Unauthorized access");
            }
            catch (JsonSerializationException ex)
            {
                TryLogError($"JSON serialization error: {ex}");
                await HandleExceptionAsync(httpContext, ex, StatusCodes.Status400BadRequest, "Invalid data format");
            }
            catch (Exception ex)
            {
                TryLogError($"An error occurred: {ex}");
                await HandleExceptionAsync(httpContext, ex, StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        private void TryLogError(string message)
        {
            try
            {
                _logger.LogError(message);
            }
            catch
            {
                // Logging must not prevent the error response from being sent.
            }
        }

        /// <summary>
        /// Handles the exception asynchronously.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="message">The error message.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            message = "A problem happened while handling your request.";
            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }

    /// <summary>
    /// Represents the details of an error.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
