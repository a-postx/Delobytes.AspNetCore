using System;

namespace Delobytes.AspNetCore.Application.Actions;

/// <summary>
/// Объект неудачного завершения действия. 
/// </summary>
public class Failure : ResponseObject, IFailure
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="correlationId">Идентификатор корреляции.</param>
    protected Failure(Guid correlationId) : base(correlationId) { }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="correlationId">Идентификатор корреляции.</param>
    /// <param name="errorMessages">Массив сообщений об ошибках.</param>
    public Failure(Guid correlationId, string[]? errorMessages = null) : base(correlationId)
    {
        ErrorMessages = errorMessages;
    }

    /// <summary>
    /// Признак удачного завершения действия.
    /// </summary>
    public override bool IsSuccess => false;
    /// <summary>
    /// Массив сообщений об ошибках.
    /// </summary>
    public string[]? ErrorMessages { get; protected set; }
}
