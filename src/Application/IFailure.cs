namespace Delobytes.AspNetCore.Application;

public interface IFailure
{
    string[] ErrorMessages { get; }
    bool IsSuccess { get; }
}