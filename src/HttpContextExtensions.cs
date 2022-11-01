using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Delobytes.AspNetCore;

/// <summary>
/// <see cref="HttpContext"/> extension methods.
/// </summary>
public static class HttpContextExtensions
{
    private const string NoCache = "no-cache";
    private const string NoCacheMaxAge = "no-cache,max-age=";
    private const string NoStore = "no-store";
    private const string NoStoreNoCache = "no-store,no-cache";
    private const string PublicMaxAge = "public,max-age=";
    private const string PrivateMaxAge = "private,max-age=";

    private static readonly MediaTypeHeaderValue _jsonMediaType = new MediaTypeHeaderValue("application/json");

    /// <summary>
    /// Adds Cache-Control and Pragma HTTP headers by applying specified cache profile to the HTTP context.
    /// </summary>
    /// <param name="context">HTTP context.</param>
    /// <param name="cacheProfile">Cache profile.</param>
    /// <returns>The same HTTP context.</returns>
    /// <exception cref="System.ArgumentNullException">context or cacheProfile.</exception>
    public static HttpContext ApplyCacheProfile(this HttpContext context, CacheProfile cacheProfile)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cacheProfile);

        IHeaderDictionary headers = context.Response.Headers;

        if (!string.IsNullOrEmpty(cacheProfile.VaryByHeader))
        {
            headers[HeaderNames.Vary] = cacheProfile.VaryByHeader;
        }

        if (cacheProfile.NoStore == true)
        {
            // Cache-control: no-store, no-cache is valid.
            if (cacheProfile.Location == ResponseCacheLocation.None)
            {
                headers[HeaderNames.CacheControl] = NoStoreNoCache;
                headers[HeaderNames.Pragma] = NoCache;
            }
            else
            {
                headers[HeaderNames.CacheControl] = NoStore;
            }
        }
        else
        {
            string cacheControlValue;
            string duration = cacheProfile.Duration.GetValueOrDefault().ToString(CultureInfo.InvariantCulture);
            switch (cacheProfile.Location)
            {
                case ResponseCacheLocation.Any:
                    cacheControlValue = PublicMaxAge + duration;
                    break;
                case ResponseCacheLocation.Client:
                    cacheControlValue = PrivateMaxAge + duration;
                    break;
                case ResponseCacheLocation.None:
                    cacheControlValue = NoCacheMaxAge + duration;
                    headers[HeaderNames.Pragma] = NoCache;
                    break;
                default:
                    NotImplementedException exception = new NotImplementedException($"Unknown {nameof(ResponseCacheLocation)}: {cacheProfile.Location}");
                    Debug.Fail(exception.ToString());
                    throw exception;
            }

            headers[HeaderNames.CacheControl] = cacheControlValue;
        }

        return context;
    }

    /// <summary>
    /// Determines if the request accepts responses formatted as JSON via the <c>Accepts</c> header.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequest"/>.</param>
    /// <returns><c>true</c> if the <c>Accept</c> header contains a media type compatible with "application/json".</returns>
    public static bool AcceptsJson(this HttpRequest httpRequest) => Accepts(httpRequest, _jsonMediaType);

    /// <summary>
    /// Determines if the request accepts responses formatted as the specified media type via the <c>Accepts</c> header.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequest"/>.</param>
    /// <param name="mediaType">The media type.</param>
    /// <returns><c>true</c> if the <c>Accept</c> header contains a compatible media type.</returns>
    public static bool Accepts(this HttpRequest httpRequest, string mediaType) =>
        Accepts(httpRequest, new MediaTypeHeaderValue(mediaType));

    /// <summary>
    /// Determines if the request accepts responses formatted as the specified media type via the <c>Accepts</c> header.
    /// </summary>
    /// <param name="httpRequest">The <see cref="HttpRequest"/>.</param>
    /// <param name="mediaType">The <see cref="MediaTypeHeaderValue"/>.</param>
    /// <returns><c>true</c> if the <c>Accept</c> header contains a compatible media type.</returns>
    public static bool Accepts(this HttpRequest httpRequest, MediaTypeHeaderValue mediaType)
    {
        if (httpRequest.GetTypedHeaders().Accept is { Count: > 0 } acceptHeader)
        {
            for (var i = 0; i < acceptHeader.Count; i++)
            {
                var acceptHeaderValue = acceptHeader[i];

                if (mediaType.IsSubsetOf(acceptHeaderValue))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
