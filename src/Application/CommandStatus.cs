namespace Delobytes.AspNetCore.Application;

/// <summary>
/// Статус завершения команды.
/// </summary>
public enum CommandStatus
{
    Unknown = 0,
    Ok = 1,
    NotFound = 2,
    ArgumentInvalid = 3,
    BadRequest = 4,
    ModelInvalid = 5,
    UnprocessableEntity = 6,
    ConcurrencyIssue = 7,
    EntityAlreadyExist = 8,
    Forbidden = 9
}
