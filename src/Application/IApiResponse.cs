using System;

namespace Delobytes.AspNetCore.Application;

public interface IApiResponse
{
    Guid CorrelationId { get; }
    bool IsSuccess { get; }
}
