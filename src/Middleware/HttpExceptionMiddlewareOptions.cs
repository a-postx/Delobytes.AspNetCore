namespace Delobytes.AspNetCore.Middleware
{
    /// <summary>
    /// Options to control <see cref="HttpExceptionMiddleware"/>.
    /// </summary>
    public class HttpExceptionMiddlewareOptions
    {
        /// <summary>
        /// Should ReasonPhrase be included in HttpResponseMessage.
        /// </summary>
        public bool IncludeReasonPhraseInResponse { get; set; } = false;
    }
}