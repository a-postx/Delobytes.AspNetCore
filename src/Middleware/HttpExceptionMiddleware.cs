using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Revolex.AspNetCore.Middleware
{
    internal class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpExceptionMiddlewareOptions _options;

        public HttpExceptionMiddleware(RequestDelegate next, HttpExceptionMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
            catch (HttpException httpException)
            {
                ILoggerFactory factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                ILogger<HttpExceptionMiddleware> logger = factory.CreateLogger<HttpExceptionMiddleware>();
                logger.LogInformation(
                    httpException,
                    "Executing HttpExceptionMiddleware, setting HTTP status code {0}.",
                    httpException.StatusCode);

                context.Response.StatusCode = httpException.StatusCode;

                if (_options.IncludeReasonPhraseInResponse)
                {
                    IHttpResponseFeature responseFeature = context.Features.Get<IHttpResponseFeature>();
                    responseFeature.ReasonPhrase = httpException.Message;
                }
            }
        }
    }
}