using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;

namespace BudgetApp.Extensions
{
    public static class DateHelper
    {
        public static readonly List<string> ShortMonths = new List<string> { "Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Des" };

        public static readonly List<string> FullMonths = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public static string GetMonthText(DateTime date, bool shortMonths = false)
        {
            return string.Format("{0}", (shortMonths ? ShortMonths[date.Month - 1] : FullMonths[date.Month - 1]));
        }

        public static bool IsCurrentMonth(DateTime date)
        {
            return date.Month == DateTime.Now.Month;
        }

        public static bool IsWithinDays(DateTime date, int range)
        {
            var min = DateTime.Now.AddDays(-range);

            return date >= min && date < date.AddDays(1);

        }

        public static int GetWeekNumber(DateTime dtPassed)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(dtPassed, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }


        public static DateTime GetWeekStartDate(int year, int weekOfYear)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            var firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        public static DateTime GetWeekEndDate(int year, int weekOfyear)
        {
            return GetWeekStartDate(year, weekOfyear).AddDays(6);
        }
        
    }
}