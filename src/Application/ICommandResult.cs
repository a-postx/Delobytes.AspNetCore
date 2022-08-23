namespace Delobytes.AspNetCore.Application;

/// <summary>
/// Результат выполнения команды.
/// </summary>
public interface ICommandResult
{
    /// <summary>
    /// Статус завершения команды.
    /// </summary>
    public CommandStatus Status { get; }
    /// <summary>
    /// Массив сообщений об ошибках.
    /// </summary>
    public string[]? ErrorMessages { get; }
}
