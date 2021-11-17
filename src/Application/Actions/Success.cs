using System;

namespace Delobytes.AspNetCore.Application.Actions;

public class Success : ApiResponse, ISuccess
{
    protected Success(Guid correlationId) : base(correlationId) { }

    public Success(Guid correlationId, string aggregateVersion = null) : base(correlationId)
    {
        AggregateVersion = aggregateVersion;
    }

    public override bool IsSuccess => true;
    /// <summary>
    /// TimeStamp or ETag
    /// </summary>
    public string AggregateVersion { get; protected set; }
}
