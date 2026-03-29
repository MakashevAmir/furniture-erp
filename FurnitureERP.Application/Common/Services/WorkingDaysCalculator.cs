namespace FurnitureERP.Application.Common.Services;

/// <summary>
/// Pomocná třída pro výpočty v pracovních dnech (pondělí–pátek).
/// Dodávka materiálů probíhá každé pondělí.
/// </summary>
public static class WorkingDaysCalculator
{
    /// <summary>
    /// Přidá zadaný počet pracovních dnů k datu (přeskočí soboty a neděle).
    /// </summary>
    public static DateTime AddWorkingDays(DateTime start, int workingDays)
    {
        var date = start.Date;
        var remaining = workingDays;
        while (remaining > 0)
        {
            date = date.AddDays(1);
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                remaining--;
        }
        return date;
    }

    /// <summary>
    /// Vrátí datum zahájení výroby při nedostatku materiálů.
    /// Dodávky probíhají každé pondělí – výroba začne v pondělí.
    /// Pokud je dnes pondělí, vrátí dnešní datum (dodávka přijde dnes).
    /// Jinak vrátí nejbližší příští pondělí.
    /// </summary>
    public static DateTime GetNextDeliveryMonday(DateTime from)
    {
        var date = from.Date;
        if (date.DayOfWeek == DayOfWeek.Monday)
            return date;

        int daysUntilMonday = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;
        return date.AddDays(daysUntilMonday);
    }
}
