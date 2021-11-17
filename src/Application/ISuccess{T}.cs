namespace Delobytes.AspNetCore.Application;

public interface ISuccess<TPayload>
{
    TPayload Data { get; }
}