using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BudgetApp.Extensions
{
    public class DateHelper
    {
        public static string GetMonthText(DateTime date, bool shortMonths = false)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            return shortMonths ? months[date.Month - 1].Substring(0, 3) : months[date.Month - 1];
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