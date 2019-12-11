using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;

namespace Delobytes.AspNetCore
{
    /// <summary>
    /// <see cref="IHeaderDictionary"/> extension methods.
    /// </summary>
    public static class HeadersDictionaryExtensions
    {
        /// <summary>
        /// Retrieves Correlation ID from the headers dictionary.
        /// </summary>
        /// <param name="correlationIdHeaderName">Correlation ID header name.</param>
        /// <returns><c>Guid</c> value or <c>Guid.Empty</c> if the name is not found or cannot be parsed.</returns>
        public static Guid GetCorrelationId(this IHeaderDictionary headers, string correlationIdHeaderName)
        {
            if (headers.TryGetValue(correlationIdHeaderName, out StringValues stringValues))
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
