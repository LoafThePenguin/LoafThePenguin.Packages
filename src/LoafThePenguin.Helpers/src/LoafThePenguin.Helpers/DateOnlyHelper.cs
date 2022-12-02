namespace LoafThePenguin.Helpers;

/// <summary>
/// Содержит методы для работы с <see cref="DateOnly"/>
/// </summary>
public static class DateOnlyHelper
{
    /// <summary>
    /// Возвращает текущую дату.
    /// </summary>
    public static DateOnly DateToday => DateOnly.FromDateTime(DateTime.Now);
}
