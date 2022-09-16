namespace Delobytes.AspNetCore.Application;

/// <summary>
/// Статус завершения команды.
/// </summary>
public enum CommandStatus
{
    /// <summary>
    /// Неизвестно.
    /// </summary>
    Unknown = 0,
    /// <summary>
    /// Команда завершена успешно.
    /// </summary>
    Ok = 1,
    /// <summary>
    /// Сущность не найдена.
    /// </summary>
    NotFound = 2,
    /// <summary>
    /// Неверные аргументы запроса.
    /// </summary>
    ArgumentInvalid = 3,
    /// <summary>
    /// Неверный запрос.
    /// </summary>
    BadRequest = 4,
    /// <summary>
    /// Неверная модель.
    /// </summary>
    ModelInvalid = 5,
    /// <summary>
    /// Сущность не может быть обработана.
    /// </summary>
    UnprocessableEntity = 6,
    /// <summary>
    /// Ошибка параллельного доступа.
    /// </summary>
    ConcurrencyIssue = 7,
    /// <summary>
    /// Сущность уже существует.
    /// </summary>
    EntityAlreadyExist = 8,
    /// <summary>
    /// Доступ запрещён.
    /// </summary>
    Forbidden = 9
}
