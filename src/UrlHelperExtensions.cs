using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Delobytes.AspNetCore
{
    /// <summary>
    /// <see cref="IUrlHelper"/> extension methods.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generate fully qualified URL to an action method by using the specified action name, controller name and
        /// route values.
        /// </summary>
        /// <param name="urlHelper">URL helper.</param>
        /// <param name="actionName">Action method name.</param>
        /// <param name="controllerName">Controller name.</param>
        /// <param name="routeValues">Route values.</param>
        /// <returns>Absolute URL.</returns>
        public static string AbsoluteAction(
            this IUrlHelper urlHelper,
            string actionName,
            string controllerName,
            object routeValues = null)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            return urlHelper.Action(actionName, controllerName, routeValues, urlHelper.ActionContext.HttpContext.Request.Scheme);
        }

        /// <summary>
        /// Generate fully qualified URL to a specified content by using the content path. Converts
        /// virtual (relative) path to absolute path.
        /// </summary>
        /// <param name="urlHelper">URL helper.</param>
        /// <param name="contentPath">Content path.</param>
        /// <returns>Absolute URL.</returns>
        public static string AbsoluteContent(this IUrlHelper urlHelper, string contentPath)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            HttpRequest request = urlHelper.ActionContext.HttpContext.Request;
            return new Uri(new Uri(request.Scheme + "://" + request.Host.Value), urlHelper.Content(contentPath)).ToString();
        }

        /// <summary>
        /// Generate fully qualified URL to a specified route by using the route name and route values.
        /// </summary>
        /// <param name="urlHelper">URL helper.</param>
        /// <param name="routeName">Route name.</param>
        /// <param name="routeValues">Route values.</param>
        /// <returns>Absolute URL.</returns>
        public static string AbsoluteRouteUrl(this IUrlHelper urlHelper, string routeName, object routeValues = null)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            //routeValues object property names must correspond to controller http route values
            return urlHelper.RouteUrl(routeName, routeValues, urlHelper.ActionContext.HttpContext.Request.Scheme);
        }
    }
}