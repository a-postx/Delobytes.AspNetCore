namespace Delobytes.AspNetCore.Application;

/// <summary>
/// Объект неудачного завершения действия. 
/// </summary>
public interface IFailure
{
    /// <summary>
    /// Признак удачного завершения действия.
    /// </summary>
    bool IsSuccess { get; }
    /// <summary>
    /// Массив сообщений об ошибках.
    /// </summary>
    string[] ErrorMessages { get; }
    
}