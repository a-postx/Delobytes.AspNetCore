using System;
using System.Net;

namespace Delobytes.AspNetCore
{
    /// <summary>
    /// Describes exception that occurred during processing of HTTP requests.
    /// </summary>
    /// <seealso cref="Exception" />
    public class HttpException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" /> class.
        /// </summary>
        /// <param name="httpStatusCode">HTTP status code.</param>
        public HttpException(int httpStatusCode)
        {
            StatusCode = httpStatusCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" /> class.
        /// </summary>
        /// <param name="httpStatusCode">HTTP status code.</param>
        public HttpException(HttpStatusCode httpStatusCode)
        {
            StatusCode = (int) httpStatusCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" /> class.
        /// </summary>
        /// <param name="httpStatusCode">HTTP status code.</param>
        /// <param name="message">Exception message.</param>
        public HttpException(int httpStatusCode, string message) : base(message)
        {
            StatusCode = httpStatusCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" /> class.
        /// </summary>
        /// <param name="httpStatusCode">HTTP status code.</param>
        /// <param name="message">Exception message.</param>
        public HttpException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            StatusCode = (int) httpStatusCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" /> class.
        /// </summary>
        /// <param name="httpStatusCode">HTTP status code.</param>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public HttpException(int httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = httpStatusCode;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HttpException" /> class.
        /// </summary>
        /// <param name="httpStatusCode">HTTP status code.</param>
        /// <param name="message">Exception message.</param>
        /// <param name="inner">Inner exception.</param>
        public HttpException(HttpStatusCode httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = (int) httpStatusCode;
        }

        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        /// <value>
        /// HTTP status code.
        /// </value>
        public int StatusCode { get; }
    }
}
