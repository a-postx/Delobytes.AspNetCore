namespace Delobytes.AspNetCore.Application;

/// <summary>
/// Объект удачного завершения действия.
/// </summary>
public interface ISuccess
{
    /// <summary>
    /// Версия агрегата (временная метка или еТаг).
    /// </summary>
    public string AggregateVersion { get; }
}
