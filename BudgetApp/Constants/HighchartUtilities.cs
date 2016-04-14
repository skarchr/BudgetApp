using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetApp.Constants
{
    public static class HighchartUtilities
    {
        public static readonly List<string> Colors = new List<string>
        {
            "#69D2E7",
            "#A7DBD8",
            "#48DDb8",
            "#E0E4CC",
            "#48DDb8",
            "#F38630",
            "#A9DEF9",
            "#009900",
            "rgb(232, 124, 124)"
        };


        public static double ConvertToMilliseconds(DateTime date)
        {
            return date.Subtract(new DateTime(1970,1,1,0,0,0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}