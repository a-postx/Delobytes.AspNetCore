namespace Delobytes.AspNetCore.Application.Commands;

/// <summary>
/// Результат выполнения команды.
/// </summary>
public class CommandResult : ICommandResult
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    protected CommandResult() { }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="status">Статус завершения команды.</param>
    /// <param name="errorMessages">Массив сообщений об ошибках.</param>
    public CommandResult(CommandStatus status, string[] errorMessages = null)
    {
        Status = status;
        ErrorMessages = errorMessages;
    }

    /// <summary>
    /// Статус завершения команды.
    /// </summary>
    public CommandStatus Status { get; protected set; }
    /// <summary>
    /// Массив сообщений об ошибках.
    /// </summary>
    public string[] ErrorMessages { get; protected set; }
}
