﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Delobytes.AspNetCore.Middleware;

/// <summary>
/// Прослойка перехвата исключения в HTTP-контексте. Выводит в дополнение к коду детали проблемы. 
/// </summary>
public class HttpExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionHandlerOptions _options;

    private static JsonSerializerOptions _defaultJsonOptions = new JsonSerializerOptions();

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="next">Следующий делегат.</param>
    /// <param name="options">Настройи обработки исключения.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public HttpExceptionHandler(RequestDelegate next,
        HttpExceptionHandlerOptions options)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _options = options ?? throw new ArgumentNullException(nameof(options));
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
            context.Response.StatusCode = _options.RequestCancelledStatusCode;
            logger.LogInformation("Client cancelled the request.");
        }
        catch (Exception ex)
        {
            if (_options.LogException)
            {
                logger.LogError(ex, "Unhandled exception.");
            }

            if (context.Response.HasStarted)
            {
                logger.LogError("Response already started");
                throw;
            }

            if (_options.ReplaceStatusCode)
            {
                context.Response.StatusCode = _options.ExceptionStatusCode;
            }

            if (_options.ReturnProblemDetails && context.Request.AcceptsJson())
            {
                await WriteProblemDetails(context, ex);
            }
            else
            {
                context.Response.ContentType = "text/plain";
                string? message = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode) switch
                {
                    { Length: > 0 } reasonPhrase => reasonPhrase,
                    _ => "An error occurred"
                };
                await context.Response.WriteAsync(message + "\r\n");
                await context.Response.WriteAsync($"Request ID: { Activity.Current?.Id ?? context.TraceIdentifier }");
            }
        }
    }

    private async Task WriteProblemDetails(HttpContext context, Exception ex)
    {
        if (context.Response.Body.CanWrite)
        {
            ProblemDetails problemDetails = new ProblemDetails
            {
                Status = _options.ExceptionStatusCode,
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


        
