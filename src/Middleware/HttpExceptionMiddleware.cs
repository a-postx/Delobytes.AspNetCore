using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Delobytes.AspNetCore.Middleware;

/// <summary>
/// Catches exception and log it with additional info.
/// </summary>
public class HttpExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionMiddlewareOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpExceptionMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware.</param>
    /// <param name="options">The options.</param>
    public HttpExceptionMiddleware(RequestDelegate next, HttpExceptionMiddlewareOptions options)
    {
        _next = next;
        _options = options;
    }

    /// <inheritdoc/>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context).ConfigureAwait(false);
        }
        catch (HttpException ex)
        {
            ILoggerFactory factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
            ILogger<HttpExceptionMiddleware> logger = factory.CreateLogger<HttpExceptionMiddleware>();
            logger.LogInformation(ex, "Executing HttpExceptionMiddleware, setting HTTP status code {0}.", ex.StatusCode);
            context.Response.StatusCode = ex.StatusCode;

            if (_options.IncludeReasonPhraseInResponse)
            {
                IHttpResponseFeature? responseFeature = context.Features.Get<IHttpResponseFeature>();

                if (responseFeature is not null)
                {
                    responseFeature.ReasonPhrase = ex.Message;
                }
            }
        }
    }
}
