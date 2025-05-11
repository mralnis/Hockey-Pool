namespace HockeyPool.Extensions;

public static class DateTimeExtensions
{
    public static string DayOfWeekLvShort(this DateTime date)
    {
        return date.DayOfWeek switch
        {
            DayOfWeek.Monday => "P",
            DayOfWeek.Tuesday => "O",
            DayOfWeek.Wednesday => "T",
            DayOfWeek.Thursday => "C",
            DayOfWeek.Friday => "Pk",
            DayOfWeek.Saturday => "S",
            DayOfWeek.Sunday => "Sv",
            _ => "",
        };
    }

    public static string DayOfWeekLv(this DateTime date)
    {
        return date.DayOfWeek switch
        {
            DayOfWeek.Monday => "Pirmdiena",
            DayOfWeek.Tuesday => "Otrdiena",
            DayOfWeek.Wednesday => "Trešdiena",
            DayOfWeek.Thursday => "Ceturtdiena",
            DayOfWeek.Friday => "Piektdiena",
            DayOfWeek.Saturday => "Sestdiena",
            DayOfWeek.Sunday => "Svētdiena",
            _ => "",
        };
    }
}
