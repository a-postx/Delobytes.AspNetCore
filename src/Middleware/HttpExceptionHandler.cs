using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Delobytes.AspNetCore.Middleware
{
    /// <summary>
    /// Прослойка перехвата исключения в HTTP-контексте. Выводит в дополнение к коду детали проблемы. 
    /// </summary>
    public class HttpExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly HttpExceptionHandlerOptions _options;

        public HttpExceptionHandler(RequestDelegate next, HttpExceptionHandlerOptions options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task InvokeAsync(HttpContext context, ILogger<HttpExceptionHandler> logger)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException ex)
            {
                if (!context.RequestAborted.IsCancellationRequested)
                {
                    await WriteProblemDetails(context, ex);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (context.Response.Body.CanWrite)
                {
                    await WriteProblemDetails(context, ex);
                }
            }
        }

        private async Task WriteProblemDetails(HttpContext context, Exception ex)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = ex.Message,
                Type = null,
                Detail = _options.IncludeStackTrace ? ex.StackTrace : null,
                Instance = context.Request.HttpContext.Request.Path,
            };

            context.Response.ContentType = "application/problem+json";
            await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails, null, context.RequestAborted);
        }
    }
}
