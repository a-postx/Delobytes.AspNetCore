using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Revolex.AspNetCore
{
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

        /// <summary>
        /// Adds Cache-Control and Pragma HTTP headers by applying specified cache profile to the HTTP context.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <param name="cacheProfile">Cache profile.</param>
        /// <returns>The same HTTP context.</returns>
        /// <exception cref="System.ArgumentNullException">context or cacheProfile.</exception>
        public static HttpContext ApplyCacheProfile(this HttpContext context, CacheProfile cacheProfile)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (cacheProfile == null)
            {
                throw new ArgumentNullException(nameof(cacheProfile));
            }

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
        /// Gets <see cref="IUrlHelper"/> instance. Use <see cref="IUrlHelperFactory"/> and
        /// <see cref="IActionContextAccessor"/>.
        /// </summary>
        /// <param name="httpContext">HTTP context.</param>
        /// <returns><see cref="IUrlHelper"/> instance for the current request.</returns>
        public static IUrlHelper GetUrlHelper(this HttpContext httpContext)
        {
            IServiceProvider services = httpContext.RequestServices;
            ActionContext actionContext = services.GetRequiredService<IActionContextAccessor>().ActionContext;
            IUrlHelper urlHelper = services.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(actionContext);

            return urlHelper;
        }
    }
}
