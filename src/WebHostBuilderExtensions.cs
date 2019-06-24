using System;
using Microsoft.AspNetCore.Hosting;

namespace Delobytes.AspNetCore
{
    /// <summary>
    /// <see cref="IWebHostBuilder"/> extension methods.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Execute specified action if the specified <paramref name="condition"/> is <c>true</c>. Can be
        /// used to conditionally add to the host builder.
        /// </summary>
        /// <param name="hostBuilder">Host builder.</param>
        /// <param name="condition">If set to <c>true</c> the action is executed.</param>
        /// <param name="action">Action used to add to the host builder.</param>
        /// <returns>The same host builder.</returns>
        public static IWebHostBuilder UseIf(
            this IWebHostBuilder hostBuilder,
            bool condition,
            Func<IWebHostBuilder, IWebHostBuilder> action)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (condition)
            {
                hostBuilder = action(hostBuilder);
            }

            return hostBuilder;
        }

        /// <summary>
        /// Execute specified action if the specified <paramref name="condition"/> is <c>true</c>. Can be
        /// used to conditionally add to the host builder.
        /// </summary>
        /// <param name="hostBuilder">Host builder.</param>
        /// <param name="condition">If <c>true</c> is returned the action is executed.</param>
        /// <param name="action">Action used to add to the host builder.</param>
        /// <returns>The same host builder.</returns>
        public static IWebHostBuilder UseIf(
            this IWebHostBuilder hostBuilder,
            Func<IWebHostBuilder, bool> condition,
            Func<IWebHostBuilder, IWebHostBuilder> action)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (condition(hostBuilder))
            {
                hostBuilder = action(hostBuilder);
            }

            return hostBuilder;
        }

        /// <summary>
        /// Execute specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise execute the <paramref name="elseAction"/>. Can be used to conditionally add to
        /// the host builder.
        /// </summary>
        /// <param name="hostBuilder">Host builder.</param>
        /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">Action used to add to the host builder if the condition is <c>true</c>.</param>
        /// <param name="elseAction">Action used to add to the host builder if the condition is <c>false</c>.</param>
        /// <returns>The same host builder.</returns>
        public static IWebHostBuilder UseIfElse(
            this IWebHostBuilder hostBuilder,
            bool condition,
            Func<IWebHostBuilder, IWebHostBuilder> ifAction,
            Func<IWebHostBuilder, IWebHostBuilder> elseAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            if (ifAction == null)
            {
                throw new ArgumentNullException(nameof(ifAction));
            }

            if (elseAction == null)
            {
                throw new ArgumentNullException(nameof(elseAction));
            }

            hostBuilder = condition ? ifAction(hostBuilder) : elseAction(hostBuilder);

            return hostBuilder;
        }

        /// <summary>
        /// Execute specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. Can be used to conditionally add to
        /// the host builder.
        /// </summary>
        /// <param name="hostBuilder">Host builder.</param>
        /// <param name="condition">If <c>true</c> is returned the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">Action used to add to the host builder if the condition is <c>true</c>.</param>
        /// <param name="elseAction">Action used to add to the host builder if the condition is <c>false</c>.</param>
        /// <returns>The same host builder.</returns>
        public static IWebHostBuilder UseIfElse(
            this IWebHostBuilder hostBuilder,
            Func<IWebHostBuilder, bool> condition,
            Func<IWebHostBuilder, IWebHostBuilder> ifAction,
            Func<IWebHostBuilder, IWebHostBuilder> elseAction)
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (ifAction == null)
            {
                throw new ArgumentNullException(nameof(ifAction));
            }

            if (elseAction == null)
            {
                throw new ArgumentNullException(nameof(elseAction));
            }

            hostBuilder = condition(hostBuilder) ? ifAction(hostBuilder) : elseAction(hostBuilder);

            return hostBuilder;
        }
    }
}
