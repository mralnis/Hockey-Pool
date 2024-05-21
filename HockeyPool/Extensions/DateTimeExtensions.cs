namespace HockeyPool.Extensions
{
    public static class DateTimeExtensions
    {
        public static string DayOfWeekLv(this DateTime date)
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
    }
}
