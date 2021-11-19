using System;

namespace Delobytes.AspNetCore.Application.Actions;

/// <summary>
/// Объект, уходящий на слой контроллеров, который записывается в тело ответа.
/// </summary>
public abstract class ResponseObject : IResponseObject
{
    /// <summary>
    /// Создаёт новый экземпляр <see cref="ResponseObject"/> class.
    /// </summary>
    protected ResponseObject(Guid correlationId)
    {
        CorrelationId = correlationId;
    }

    /// <summary>
    /// Корелляционный идентификатор.
    /// </summary>
    public Guid CorrelationId { get; private set; }
    /// <summary>
    /// Признак удачного завершения действия.
    /// </summary>
    public abstract bool IsSuccess { get; }
}
