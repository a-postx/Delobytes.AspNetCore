using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Revolex.AspNetCore.Middleware
{
    internal class InternalServerErrorExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context).ConfigureAwait(false);
            }
            catch
            {
                ILoggerFactory factory = context.RequestServices.GetRequiredService<ILoggerFactory>();
                ILogger<InternalServerErrorExceptionMiddleware> logger = factory.CreateLogger<InternalServerErrorExceptionMiddleware>();
                logger.LogInformation(
                    "Executing InternalServerErrorOnExceptionMiddleware, setting HTTP status code {0}.",
                    StatusCodes.Status500InternalServerError);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }
}
