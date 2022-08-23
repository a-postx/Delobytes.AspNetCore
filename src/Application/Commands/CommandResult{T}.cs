namespace Delobytes.AspNetCore.Application.Commands;

/// <summary>
/// Результат выполнения команды, возвращающей объект данных.
/// </summary>
public class CommandResult<TResult> : CommandResult, ICommandResult<TResult>
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    protected CommandResult() { }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="status">Статус завершения команды.</param>
    /// <param name="data">Объект данных.</param>
    /// <param name="errorMessages">Массив сообщений об ошибках.</param>
    public CommandResult(CommandStatus status, TResult data, string[]? errorMessages = null) : base (status, errorMessages)
    {
        Data = data;
    }

    /// <summary>
    /// Объект данных.
    /// </summary>
    public TResult? Data { get; protected set; }
}
