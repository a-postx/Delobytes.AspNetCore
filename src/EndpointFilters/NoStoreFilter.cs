using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;

namespace Delobytes.AspNetCore.EndpointFilters;

/// <summary>
/// Фильтр добавляет заголовок no-store в контекст ответа.
/// </summary>
public class NoStoreFilter : IEndpointFilter
{
    /// <inheritdoc/>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        object? result = await next(context);

        context.HttpContext.Response.Headers[HeaderNames.CacheControl] = "no-store";

        return result;
    }
}
