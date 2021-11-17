namespace Delobytes.AspNetCore.Application;

public interface ICommandResult<TResult>
{
    public CommandStatus Status { get; }
    public TResult Data { get; }
    public string[] ErrorMessages { get; }
}
