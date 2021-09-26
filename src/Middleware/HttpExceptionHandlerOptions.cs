namespace Delobytes.AspNetCore.Middleware
{
    /// <summary>
    /// Настройки <see cref="HttpExceptionHandler "/>.
    /// </summary>
    public class HttpExceptionHandlerOptions
    {
        /// <summary>
        /// Включать ли детали исключения в вывод.
        /// </summary>
        public bool IncludeStackTrace { get; set; } = false;
    }
}