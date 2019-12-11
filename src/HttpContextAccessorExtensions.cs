using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;

namespace Delobytes.AspNetCore
{
    /// <summary>
    /// <see cref="IHttpContextAccessor"/> extension methods.
    /// </summary>
    public static class HttpContextAccessorExtensions
    {
        /// <summary>
        /// Retrieves Correlation ID from the HTTP context.
        /// </summary>
        /// <param name="correlationIdHeaderName">Correlation ID header name.</param>
        /// <returns><c>Guid</c> value or <c>Guid.Empty</c> if the name is not found or cannot be parsed.</returns>
        public static Guid GetCorrelationId(this IHttpContextAccessor context, string correlationIdHeaderName)
        {
            if (context.HttpContext.Request.Headers.TryGetValue(correlationIdHeaderName, out StringValues stringValues))
            {
                if (Guid.TryParse(stringValues[0], out Guid corrId))
                {
                    return corrId;
                }
            }

            return Guid.Empty;
        }
    }
}
