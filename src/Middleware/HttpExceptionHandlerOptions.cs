namespace Delobytes.AspNetCore.Middleware;

/// <summary>
/// Настройки <see cref="HttpExceptionHandler "/>.
/// </summary>
public class HttpExceptionHandlerOptions
{
    /// <summary>
    /// Нестандартный код 499 'Клиент закрыл запрос', который используется в NGINX.
    /// </summary>
    public const int ClientClosedRequest = 499;

    /// <summary>
    /// Включать ли детали исключения в вывод деталей проблемы.
    /// </summary>
    public bool IncludeStackTraceInResponse { get; set; } = false;

    /// <summary>
    /// Статус код для установки ответа при отмене запроса клиентом. По-умолчанию, нестандартный 499.
    /// См. https://stackoverflow.com/questions/46234679/what-is-the-correct-http-status-code-for-a-cancelled-request.
    /// </summary>
    public int RequestCancelledStatusCode { get; set; } = ClientClosedRequest;

    /// <summary>
    /// Логировать детали исключения.
    /// </summary>
    public bool LogException { get; set; } = true;

    /// <summary>
    /// Возвращать в теле ответа объект с деталями проблемы.
    /// </summary>
    public bool ReturnProblemDetails { get; set; } = true;

    /// <summary>
    /// Заменять код ответа на единый (при обработке исключения).
    /// </summary>
    public bool ReplaceStatusCode { get; set; } = true;

    /// <summary>
    /// Код ответа при замене на единый.
    /// </summary>
    public int ExceptionStatusCode { get; set; } = 500;
}