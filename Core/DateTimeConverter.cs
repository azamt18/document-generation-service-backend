namespace Core;

public static class DateTimeConverter
{
    public static string ConvertToDate(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }

    public static string ConvertToDateTime(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static DateTime GetDateTimeDayEnd(this DateTime dateTime)
    {
        return dateTime.Date
            .AddDays(1)
            .AddMilliseconds(-1);
    }
}