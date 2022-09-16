using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Delobytes.AspNetCore.Middleware;

/// <summary>
/// Прослойка перехвата исключения в HTTP-контексте. Выводит в дополнение к коду детали проблемы. 
/// </summary>
public class HttpExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionHandlerOptions _options;
    private readonly ILogger<HttpExceptionHandler> _logger;

    private static JsonSerializerOptions _defaultJsonOptions = new JsonSerializerOptions();

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="next">Следующий делегат.</param>
    /// <param name="options">Настройи обработки исключения.</param>
    /// <param name="logger">Логер.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public HttpExceptionHandler(RequestDelegate next,
        HttpExceptionHandlerOptions options,
        ILogger<HttpExceptionHandler> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task InvokeAsync(HttpContext context, ILogger<HttpExceptionHandler> logger)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = _options.StatusCode;
            _logger.LogInformation("Client cancelled the request.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception.");

            await WriteProblemDetails(context, ex);
        }
    }

    private async Task WriteProblemDetails(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        if (context.Response.Body.CanWrite)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = ex.Message,
                Type = null,
                Detail = _options.IncludeStackTraceInResponse ? ex.StackTrace : null,
                Instance = context.Request.HttpContext.Request.Path,
            };

            context.Response.ContentType = "application/problem+json";
            await JsonSerializer.SerializeAsync(context.Response.Body, problemDetails, _defaultJsonOptions, context.RequestAborted);
        }
    }
}
