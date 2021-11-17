using System;

namespace Delobytes.AspNetCore.Application.Actions;

public abstract class ApiResponse : IApiResponse
{
    protected ApiResponse(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; private set; }
    public abstract bool IsSuccess { get; }
}
