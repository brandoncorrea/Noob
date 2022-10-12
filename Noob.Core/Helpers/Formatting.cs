namespace Noob.Core.Helpers;

public static class Formatting
{
    public static string TimeFromNow(DateTime date)
    {
        var timeDifference = date.Subtract(DateTime.Now);
        var dayTerm = timeDifference.Days == 1 ? "day" : "days";
        var hourTerm = timeDifference.Hours == 1 ? "hour" : "hours";
        var minuteTerm = timeDifference.Minutes == 1 ? "minute" : "minutes";

        if (timeDifference.Days > 0 && timeDifference.Hours > 0 && timeDifference.Minutes > 0)
            return $"{timeDifference.Days} {dayTerm}, {timeDifference.Hours} {hourTerm}, and {timeDifference.Minutes} {minuteTerm}";
        if (timeDifference.Days > 0 && timeDifference.Hours > 0)
            return $"{timeDifference.Days} {dayTerm} and {timeDifference.Hours} {hourTerm}";
        if (timeDifference.Days > 0 && timeDifference.Minutes > 0)
            return $"{timeDifference.Days} {dayTerm} and {timeDifference.Minutes} {minuteTerm}";
        if (timeDifference.Hours > 0 && timeDifference.Minutes > 0)
            return $"{timeDifference.Hours} {hourTerm} and {timeDifference.Minutes} {minuteTerm}";
        if (timeDifference.Days > 0)
            return $"{timeDifference.Days} {dayTerm}";
        if (timeDifference.Hours > 0)
            return $"{timeDifference.Hours} {hourTerm}";
        return $"{timeDifference.Minutes} {minuteTerm}";
    }

    public static string AsPlural(this string unitText, int count) =>
        count == 1 ? $"{count} {unitText}" : $"{count} {unitText}s";
    public static string NibletTerm(int niblets) => AsPlural("Niblet", niblets);
    public static string BrownieTerm(int browniePoints) => AsPlural("Brownie Point", browniePoints);

    public static Dictionary<int, string> ItemSlotNames = new Dictionary<int, string>
    {
        { 1, "Main Hand" },
        { 2, "Off-Hand" },
        { 3, "Head" },
        { 4, "Torso" },
        { 5, "Legs" },
        { 6, "Hands" },
        { 7, "Feet" },
        { 8, "Back" },
    };

    public static string ItemSlotName(int slotId) => ItemSlotNames.GetValueOrDefault(slotId);

    public static bool IsNotNullOrWhitespace(string s) =>
        !string.IsNullOrWhiteSpace(s);
    public static IEnumerable<string> TakePopulated(this IEnumerable<string> lines) =>
        lines.Where(IsNotNullOrWhitespace);
    public static IEnumerable<string> TakePopulated(params string[] lines) =>
        lines.TakePopulated();
}
