namespace FurnitureERP.Application.Common.Services;

public static class WorkingDaysCalculator
{
    // Posune datum o zadaný počet pracovních dnů a přeskočí víkendy.
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

    // Vrátí nejbližší pondělí jako datum plánovaného dovozu materiálu.
    public static DateTime GetNextDeliveryMonday(DateTime from)
    {
        var date = from.Date;
        if (date.DayOfWeek == DayOfWeek.Monday)
            return date;

        int daysUntilMonday = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;
        return date.AddDays(daysUntilMonday);
    }
}
