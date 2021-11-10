namespace Delobytes.AspNetCore.Middleware;

/// <summary>
/// Настройки <see cref="HttpExceptionHandler "/>.
/// </summary>
public class HttpExceptionHandlerOptions
{
    /// <summary>
    /// Включать ли детали исключения в вывод деталей проблемы.
    /// </summary>
    public bool IncludeStackTraceInResponse { get; set; } = false;
}