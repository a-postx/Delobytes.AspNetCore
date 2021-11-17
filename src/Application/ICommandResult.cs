namespace Delobytes.AspNetCore.Application;

public interface ICommandResult
{
    public CommandStatus Status { get; }
    public string[] ErrorMessages { get; }
}
