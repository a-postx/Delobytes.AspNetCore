using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Delobytes.AspNetCore
{
    /// <summary>
    /// <see cref="IDistributedCache"/> extension methods.
    /// </summary>
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Gets the <see cref="string"/> value with the specified key from the cache asynchronously or returns
        /// <c>null</c> if the key was not found.
        /// </summary>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="encoding">The encoding of the <see cref="string"/> value or <c>null</c> to use UTF-8.</param>
        /// <returns>The <see cref="string"/> value or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache"/> or <paramref name="key"/> is <c>null</c>.</exception>
        public static async Task<string> GetStringAsync(this IDistributedCache cache, string key, Encoding encoding = null, CancellationToken cancellationToken = default)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] bytes = await cache.GetAsync(key, cancellationToken).ConfigureAwait(false);

            if (bytes == null)
            {
                return null;
            }

            return encoding.GetString(bytes);
        }

        /// <summary>
        /// Gets the value of type <typeparamref name="T"/> with the specified key from the cache asynchronously by
        /// deserializing it from JSON format or returns <c>null</c> if the key was not found.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="encoding">The encoding of the JSON or <c>null</c> to use UTF-8.</param>
        /// <returns>The value of type <typeparamref name="T"/> or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache"/> or <paramref name="key"/> is <c>null</c>.</exception>
        public static async Task<T> GetAsJsonAsync<T>(
            this IDistributedCache cache,
            string key,
            Encoding encoding = null,
            CancellationToken cancellationToken = default) where T : class
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            string json = await GetStringAsync(cache, key, encoding, cancellationToken).ConfigureAwait(false);

            if (json == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Sets the <see cref="string"/> value with the specified key in the cache asynchronously.
        /// </summary>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="value">The cache item value.</param>
        /// <param name="encoding">The <see cref="string"/> values encoding or <c>null</c> for UTF-8.</param>
        /// <param name="options">The cache options or <c>null</c> to use the default cache options.</param>
        /// <returns>A task representing this action.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache"/> or <paramref name="key"/> is <c>null</c>.</exception>
        public static Task SetAsync(
            this IDistributedCache cache,
            string key,
            string value,
            Encoding encoding = null,
            DistributedCacheEntryOptions options = null,
            CancellationToken cancellationToken = default)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (options == null)
            {
                options = new DistributedCacheEntryOptions();
            }

            byte[] bytes = encoding.GetBytes(value);
            return cache.SetAsync(key, bytes, options, cancellationToken);
        }

        /// <summary>
        /// Sets the value of type <typeparamref name="T"/> with the specified key in the cache asynchronously by
        /// serializing it to JSON format.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="cache">The distributed cache.</param>
        /// <param name="key">The cache item key.</param>
        /// <param name="value">The cache item value.</param>
        /// <param name="encoding">The encoding to use for the JSON or <c>null</c> to use UTF-8.</param>
        /// <param name="options">The cache options or <c>null</c> to use the default cache options.</param>
        /// <returns>The value of type <typeparamref name="T"/> or <c>null</c> if the key was not found.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="cache"/> or <paramref name="key"/> is <c>null</c>.</exception>
        public static Task SetAsJsonAsync<T>(
            this IDistributedCache cache,
            string key,
            T value,
            Encoding encoding = null,
            DistributedCacheEntryOptions options = null,
            CancellationToken cancellationToken = default) where T : class
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            if (options == null)
            {
                options = new DistributedCacheEntryOptions();
            }

            string json = JsonConvert.SerializeObject(value, Formatting.None);
            return SetAsync(cache, key, json, encoding, options, cancellationToken);
        }
    }
}
