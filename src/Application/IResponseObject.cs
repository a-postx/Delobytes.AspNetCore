using System;

namespace Delobytes.AspNetCore.Application;

/// <summary>
/// Объект, уходящий на слой контроллеров, который записывается в тело ответа.
/// </summary>
public interface IResponseObject
{
    /// <summary>
    /// Корелляционный идентификатор.
    /// </summary>
    Guid CorrelationId { get; }
    /// <summary>
    /// Признак удачного завершения действия.
    /// </summary>
    bool IsSuccess { get; }
}
