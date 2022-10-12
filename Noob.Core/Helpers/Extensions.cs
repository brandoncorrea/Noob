namespace Noob.Core.Helpers;

public static class Extensions
{
    public static bool IsAfterNow(this DateTime date) => date > DateTime.Now;
    public static bool LessThanOneDayAgo(this DateTime date) => date.AddDays(1).IsAfterNow();
    public static bool LessThanDaysAgo(this DateTime date, int days) => date.AddDays(days).IsAfterNow();
    public static string Join(this IEnumerable<string> items, string separator) => string.Join(separator, items);
}
