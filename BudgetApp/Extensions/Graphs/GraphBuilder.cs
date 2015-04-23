using System;
using System.Collections.Generic;
using System.Linq;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public static class GraphBuilder
    {
        public static Highchart TransactionDrilldownGraph(List<Transaction> transactions)
        {
            return TransactionDrilldown.CreateChart(transactions);
        }


        public static Highchart DailyExpensesGraph(List<Transaction> transactions)
        {
            return DailyExpenses.CreateChart(transactions);
        }


        public static long ConvertDateToMilliSeconds(DateTime date)
        {
            return (date.ToUniversalTime().Ticks - ((new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks)) / 10000;
        }
    }
}