namespace Delobytes.AspNetCore.Application.Commands;

public class CommandResult : ICommandResult
{
    protected CommandResult() { }

    public CommandResult(CommandStatus status, string[] errorMessages = null)
    {
        Status = status;
        ErrorMessages = errorMessages;
    }

    public CommandStatus Status { get; protected set; }
    public string[] ErrorMessages { get; protected set; }
}
