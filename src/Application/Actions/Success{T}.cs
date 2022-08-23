using System;

namespace Delobytes.AspNetCore.Application.Actions;

/// <summary>
/// Объект удачного завершения действия.
/// </summary>
/// <typeparam name="TPayload">Тип объекта данных.</typeparam>
public class Success<TPayload> : Success, ISuccess<TPayload>
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
    /// <param name="data">Объект данных.</param>
    public Success(Guid correlationId, string? aggregateVersion, TPayload? data) : base(correlationId, aggregateVersion)
    {
        Data = data;
    }

    /// <summary>
    /// Объект данных.
    /// </summary>
    public TPayload? Data { get; protected set; }
}
