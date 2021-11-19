using System;

namespace Delobytes.AspNetCore.Application.Actions;

/// <summary>
/// Объект удачного завершения действия.
/// </summary>
public class Success : ResponseObject, ISuccess
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="correlationId">Идентификатор корреляции.</param>
    protected Success(Guid correlationId) : base(correlationId) { }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="correlationId">Идентификатор корреляции.</param>
    /// <param name="aggregateVersion">Версия агрегата.</param>
    public Success(Guid correlationId, string aggregateVersion = null) : base(correlationId)
    {
        AggregateVersion = aggregateVersion;
    }

    /// <summary>
    /// Признак удачного завершения действия.
    /// </summary>
    public override bool IsSuccess => true;
    /// <summary>
    /// Версия агрегата (временная метка или еТаг).
    /// </summary>
    public string AggregateVersion { get; protected set; }
}
