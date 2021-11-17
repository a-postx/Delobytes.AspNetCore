using System;

namespace Delobytes.AspNetCore.Application.Actions;

public class Success<TPayload> : Success, ISuccess<TPayload>
{
    protected Success(Guid correlationId) : base(correlationId) { }

    public Success(Guid correlationId, string aggregateVersion, TPayload data) : base(correlationId, aggregateVersion)
    {
        Data = data;
    }

    public TPayload Data { get; protected set; }
}
