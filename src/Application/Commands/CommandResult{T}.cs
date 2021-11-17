namespace Delobytes.AspNetCore.Application.Commands;

public class CommandResult<TResult> : CommandResult, ICommandResult<TResult>
{
    protected CommandResult() { }

    public CommandResult(CommandStatus status, TResult data, string[] errorMessages = null) : base (status, errorMessages)
    {
        Data = data;
    }

    public TResult Data { get; protected set; }
}
