namespace Delobytes.AspNetCore.Application;

/// <summary>
/// Результат выполнения команды, возвращающей объект данных.
/// </summary>
public interface ICommandResult<TResult>
{
    /// <summary>
    /// Статус завершения команды.
    /// </summary>
    public CommandStatus Status { get; }
    /// <summary>
    /// Объект данных.
    /// </summary>
    public TResult Data { get; }
    /// <summary>
    /// Массив сообщений об ошибках.
    /// </summary>
    public string[] ErrorMessages { get; }
}
