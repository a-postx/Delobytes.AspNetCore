using System;

namespace Delobytes.AspNetCore.Application.Actions;

public class Failure : ApiResponse, IFailure
{
    protected Failure(Guid correlationId) : base(correlationId) { }

    public Failure(Guid correlationId, string[] errorMessages = null) : base(correlationId)
    {
        ErrorMessages = errorMessages;
    }

    public override bool IsSuccess => false;
    public string[] ErrorMessages { get; protected set; }
}
