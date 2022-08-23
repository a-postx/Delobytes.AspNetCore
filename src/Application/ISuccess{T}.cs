namespace Delobytes.AspNetCore.Application;

/// <summary>
/// Объект удачного завершения действия.
/// </summary>
/// <typeparam name="TPayload">Тип объекта данных.</typeparam>
public interface ISuccess<TPayload>
{
    /// <summary>
    /// Объект данных.
    /// </summary>
    TPayload? Data { get; }
}