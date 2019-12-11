using System;
using Delobytes.AspNetCore.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Delobytes.AspNetCore
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the HTTP exception handling middleware.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The same services collection.</returns>
        public static IServiceCollection AddHttpException(this IServiceCollection services) =>
            services.AddSingleton<HttpExceptionMiddleware>();

        /// <summary>
        /// Adds the server timing middleware.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>The same services collection.</returns>
        public static IServiceCollection AddServerTiming(this IServiceCollection services) =>
            services.AddSingleton<ServerTimingMiddleware>();

        /// <summary>
        /// Execute specified action if the specified <paramref name="condition"/> is <c>true</c>. Can be
        /// used to conditionally configure the MVC services.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <param name="condition">If set to <c>true</c> the action is executed.</param>
        /// <param name="action">Action used to configure the MVC services.</param>
        /// <returns>The same services collection.</returns>
        public static IServiceCollection AddIf(
            this IServiceCollection services,
            bool condition,
            Func<IServiceCollection, IServiceCollection> action)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (condition)
            {
                services = action(services);
            }

            return services;
        }

        /// <summary>
        /// Execute specified <paramref name="ifAction"/> if the specified <paramref name="condition"/> is
        /// <c>true</c>, otherwise execute the <paramref name="elseAction"/>. Can be used to conditionally
        /// configure the MVC services.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <param name="condition">If set to <c>true</c> the <paramref name="ifAction"/> is executed, otherwise the
        /// <paramref name="elseAction"/> is executed.</param>
        /// <param name="ifAction">Action used to configure the MVC services if the condition is <c>true</c>.</param>
        /// <param name="elseAction">Action used to configure the MVC services if the condition is <c>false</c>.</param>
        /// <returns>The same services collection.</returns>
        public static IServiceCollection AddIfElse(
            this IServiceCollection services,
            bool condition,
            Func<IServiceCollection, IServiceCollection> ifAction,
            Func<IServiceCollection, IServiceCollection> elseAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (ifAction == null)
            {
                throw new ArgumentNullException(nameof(ifAction));
            }

            if (elseAction == null)
            {
                throw new ArgumentNullException(nameof(elseAction));
            }

            services = condition ? ifAction(services) : elseAction(services);

            return services;
        }

        /// <summary>
        /// Register <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container.
        /// </summary>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <param name="services">Services collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>The same services collection.</returns>
        public static IServiceCollection ConfigureSingleton<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TOptions : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return services.Configure<TOptions>(configuration)
                .AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
        }

        /// <summary>
        /// Register <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container
        /// and run data annotation validation.
        /// </summary>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <param name="services">Services collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>The same services collection.</returns>
        public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TOptions : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddOptions<TOptions>().Bind(configuration).ValidateDataAnnotations();

            return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
        }

        /// <summary>
        /// Register <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container,
        /// run data annotation validation and custom validation using the default failure message.
        /// </summary>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <param name="services">Services collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="validation">Validation function.</param>
        /// <returns>The same services collection.</returns>
        public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            Func<TOptions, bool> validation)
            where TOptions : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (validation == null)
            {
                throw new ArgumentNullException(nameof(validation));
            }

            services.AddOptions<TOptions>().Bind(configuration).ValidateDataAnnotations().Validate(validation);

            return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
        }

        /// <summary>
        /// Register <see cref="IOptions{TOptions}"/> and <typeparamref name="TOptions"/> to the services container,
        /// run data annotation validation and custom validation.
        /// </summary>
        /// <typeparam name="TOptions">Options type.</typeparam>
        /// <param name="services">Services collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <param name="validation">Validation function.</param>
        /// <param name="failureMessage">Failure message to use when validation fails.</param>
        /// <returns>The same services collection.</returns>
        public static IServiceCollection ConfigureAndValidateSingleton<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            Func<TOptions, bool> validation,
            string failureMessage)
            where TOptions : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (validation == null)
            {
                throw new ArgumentNullException(nameof(validation));
            }

            if (failureMessage == null)
            {
                throw new ArgumentNullException(nameof(failureMessage));
            }

            services.AddOptions<TOptions>().Bind(configuration).ValidateDataAnnotations().Validate(validation, failureMessage);

            return services.AddSingleton(x => x.GetRequiredService<IOptions<TOptions>>().Value);
        }
    }
}
