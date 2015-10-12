using System;
using System.Collections.Generic;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public static class GraphBuilder
    {
        public static Highchart TransactionDrilldownGraph(List<Transaction> transactions, string currency = "Amount", string graphType = "column", bool sorted = false)
        {
            return TransactionDrilldown.CreateExpensesChart(transactions, currency, graphType, sorted);
        }

        public static Highchart DailyExpensesGraph(List<Transaction> transactions, string currency = "Amount")
        {
            return DailyExpenses.CreateChart(transactions, currency);
        }

        public static long ConvertDateToMilliSeconds(DateTime date)
        {
            return (date.Ticks - ((new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).Ticks)) / 10000;
        }

        public static Highchart OverviewGraph(List<RangeViewer> rangeviewers, ApplicationUser user)
        {
            return Overview.CreateChart(rangeviewers, user);
        }

        public static Highchart IncomeDrilldownGraph(List<Transaction> transactions, string currency = "Amount", string graphType = "column", bool sorted = false)
        {
            return TransactionDrilldown.CreateIncomeChart(transactions, currency, graphType, sorted);
        }

        public static Highchart DrilldownGraph(List<Transaction> transactions, string currency, string graphType = "column", bool sorted = false)
        {
            return TransactionDrilldown.CreateChart(transactions, currency, graphType, sorted);
        }

        public static Highchart PrognosisGraph(List<Transaction> transactions, string currency, bool income = false)
        {
            return Prognosis.CreateChart(transactions, currency, income);
        }

        public static Highchart SpcGraph(List<Transaction> transactions, string currency, Range range)
        {
            return Spc.CreateChart(transactions, currency, range);
        }

        public static Highchart BurnRateGraph(List<Transaction> transactions, double? expensesGoal, string currency)
        {
            return BurnRate.CreateChart(transactions, DateTime.Now, expensesGoal, currency);
        }
    }
}