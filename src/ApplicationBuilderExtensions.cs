using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Revolex.AspNetCore.Middleware;

namespace Revolex.AspNetCore
{
    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Use <see cref="HttpException"/> as an alternative method of returning error result.
        /// </summary>
        /// <param name="application">Application builder.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseHttpException(this IApplicationBuilder application)
        {
            return UseHttpException(application, null);
        }

        /// <summary>
        /// Use <see cref="HttpException"/> as an alternative method of returning error result.
        /// </summary>
        /// <param name="application">Application builder.</param>
        /// <param name="configureOptions">Middleware options.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseHttpException(this IApplicationBuilder application, Action<HttpExceptionMiddlewareOptions> configureOptions)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            HttpExceptionMiddlewareOptions options = new HttpExceptionMiddlewareOptions();
            configureOptions?.Invoke(options);

            return application.UseMiddleware<HttpExceptionMiddleware>(options);
        }

        /// <summary>
        /// Executes the specified action if the specified <paramref name="condition"/> is <c>true</c> which can be
        /// used to conditionally add to the request execution pipeline.
        /// </summary>
        /// <param name="application">Application builder.</param>
        /// <param name="condition">If set to <c>true</c> the action is executed.</param>
        /// <param name="action">Action used to add to the request execution pipeline.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseIf(
            this IApplicationBuilder application,
            bool condition,
            Func<IApplicationBuilder, IApplicationBuilder> action)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (condition)
            {
                application = action(application);
            }

            return application;
        }

        /// <summary>
        /// Executes the specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise executes the <paramref name="elseAction"/>. This can be used to conditionally add to
        /// the request execution pipeline.
        /// </summary>
        /// <param name="application">Application builder.</param>
        /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">Action used to add to the request execution pipeline if the condition is
        /// <c>true</c>.</param>
        /// <param name="elseAction">Action used to add to the request execution pipeline if the condition is
        /// <c>false</c>.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseIfElse(
            this IApplicationBuilder application,
            bool condition,
            Func<IApplicationBuilder, IApplicationBuilder> ifAction,
            Func<IApplicationBuilder, IApplicationBuilder> elseAction)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (ifAction == null)
            {
                throw new ArgumentNullException(nameof(ifAction));
            }

            if (elseAction == null)
            {
                throw new ArgumentNullException(nameof(elseAction));
            }

            application = condition ? ifAction(application) : elseAction(application);

            return application;
        }

        /// <summary>
        /// Executes the specified action using <see cref="HttpContext"/> to determine if the specified
        /// <paramref name="condition"/> is <c>true</c> which can be used to conditionally add to the request execution
        /// pipeline.
        /// </summary>
        /// <param name="application">Application builder.</param>
        /// <param name="condition">If set to <c>true</c> the action is executed.</param>
        /// <param name="action">Action used to add to the request execution pipeline.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseIf(
            this IApplicationBuilder application,
            Func<HttpContext, bool> condition,
            Func<IApplicationBuilder, IApplicationBuilder> action)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            IApplicationBuilder builder = application.New();

            action(builder);

            return application.Use(next =>
            {
                builder.Run(next);

                RequestDelegate branch = builder.Build();

                return context =>
                {
                    if (condition(context))
                    {
                        return branch(context);
                    }

                    return next(context);
                };
            });
        }

        /// <summary>
        /// Executes the specified <paramref name="ifAction"/> using <see cref="HttpContext"/> to determine if the
        /// specified <paramref name="condition"/> is <c>true</c>, otherwise executes the
        /// <paramref name="elseAction"/>. This can be used to conditionally add to the request execution pipeline.
        /// </summary>
        /// <param name="application">Application builder.</param>
        /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">Action used to add to the request execution pipeline if the condition is
        /// <c>true</c>.</param>
        /// <param name="elseAction">Action used to add to the request execution pipeline if the condition is
        /// <c>false</c>.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseIfElse(
            this IApplicationBuilder application,
            Func<HttpContext, bool> condition,
            Func<IApplicationBuilder, IApplicationBuilder> ifAction,
            Func<IApplicationBuilder, IApplicationBuilder> elseAction)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
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

            IApplicationBuilder ifBuilder = application.New();
            IApplicationBuilder elseBuilder = application.New();

            ifAction(ifBuilder);
            elseAction(elseBuilder);

            return application.Use(next =>
            {
                ifBuilder.Run(next);
                elseBuilder.Run(next);

                RequestDelegate ifBranch = ifBuilder.Build();
                RequestDelegate elseBranch = elseBuilder.Build();

                return context =>
                {
                    if (condition(context))
                    {
                        return ifBranch(context);
                    }
                    else
                    {
                        return elseBranch(context);
                    }
                };
            });
        }

        /// <summary>
        /// Returns 500 Internal Server Error response when an unhandled exception occurs.
        /// </summary>
        /// <param name="application">Application builder.</param>
        /// <returns>The same application builder.</returns>
        public static IApplicationBuilder UseInternalServerErrorOnException(this IApplicationBuilder application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return application.UseMiddleware<InternalServerErrorExceptionMiddleware>();
        }
    }
}