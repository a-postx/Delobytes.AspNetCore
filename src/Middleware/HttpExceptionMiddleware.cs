using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Delobytes.AspNetCore.Middleware;

internal class HttpExceptionMiddleware : IMiddleware
{
    private readonly RequestDelegate next;
    private readonly HttpExceptionMiddlewareOptions options;

    public HttpExceptionMiddleware(RequestDelegate next, HttpExceptionMiddlewareOptions options)
    {
        this.next = next;
        this.options = options;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await this.next.Invoke(context).ConfigureAwait(false);
        }
        catch (HttpException ex)
        {
            ILoggerFactory factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
            ILogger<HttpExceptionMiddleware> logger = factory.CreateLogger<HttpExceptionMiddleware>();
            logger.LogInformation(ex, "Executing HttpExceptionMiddleware, setting HTTP status code {0}.", ex.StatusCode);
            context.Response.StatusCode = ex.StatusCode;

            if (options.IncludeReasonPhraseInResponse)
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
