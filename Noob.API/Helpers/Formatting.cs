using System;
using Noob.API.Models;

namespace Noob.API.Helpers
{
    public static class Formatting
    {
        public static string TimeFromNow(DateTime date)
        {
            var timeDifference = date.Subtract(DateTime.Now);
            //var dayTerm = timeDifference.Days == 1 ? "day" : "days";
            var hourTerm = timeDifference.Hours == 1 ? "hour" : "hours";
            var minuteTerm = timeDifference.Minutes == 1 ? "minute" : "minutes";

            if (timeDifference.Hours > 0 && timeDifference.Minutes > 0)
                return $"{timeDifference.Hours} {hourTerm} and {timeDifference.Minutes} {minuteTerm}";
            if (timeDifference.Hours > 0)
                return $"{timeDifference.Hours} {hourTerm}";
            return $"{timeDifference.Minutes} {minuteTerm}";
        }
    }
}

