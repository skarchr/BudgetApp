using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Ranger
{
    public static class RangeHelper
    {
        public static List<RangeViewer> GetWeek(List<Transaction> transactions, string currency)
        {
            var model = new List<RangeViewer>();
            var dict = new Dictionary<int, Dictionary<int, List<Transaction>>>();

            foreach (var expense in transactions)
            {
                var year = expense.Date.Year;

                if (!dict.ContainsKey(year))
                {
                    dict.Add(year, new Dictionary<int, List<Transaction>>());
                }

                var week = DateHelper.GetWeekNumber(expense.Date);

                if (!dict[year].ContainsKey(week))
                {
                    dict[year].Add(week, new List<Transaction>());
                }

                dict[year][week].Add(expense);
            }

            foreach (var year in dict)
            {
                foreach (var week in year.Value)
                {
                    model.Add(new RangeViewer
                    {
                        Year = year.Key,
                        Range = Range.Week,
                        Title = week.Key.ToString(),
                        StartDate = DateHelper.GetWeekStartDate(year.Key, week.Key),
                        EndDate = DateHelper.GetWeekEndDate(year.Key, week.Key),
                        Transactions = week.Value.OrderByDescending(s => s.Date).ToList(),
                        Graph = GraphBuilder.TransactionDrilldownGraph(week.Value, currency).ToJson()
                    });
                }
            }


            return model.OrderByDescending(s => s.StartDate).ToList();
        }

        public static List<RangeViewer> GetMonth(List<Transaction> transactions, string currency)
        {
            var model = new List<RangeViewer>();

            var dict = new Dictionary<int, Dictionary<int, List<Transaction>>>();

            foreach (var transaction in transactions)
            {
                var year = transaction.Date.Year;

                if (!dict.ContainsKey(year))
                {
                    dict.Add(year, new Dictionary<int, List<Transaction>>());
                }

                var month = transaction.Date.Month;

                if (!dict[year].ContainsKey(month))
                {
                    dict[year].Add(month, new List<Transaction>());
                }

                dict[year][month].Add(transaction);
            }

            foreach (var year in dict)
            {
                foreach (var month in year.Value)
                {
                    model.Add(new RangeViewer
                    {
                        Year = year.Key,
                        Range = Range.Month,
                        Title = month.Key.ToString(),
                        StartDate = new DateTime(year.Key, month.Key, 1),
                        EndDate = new DateTime(year.Key, month.Key, DateTime.DaysInMonth(year.Key, month.Key)),
                        Transactions = month.Value.OrderByDescending(s => s.Date).ToList(),
                        Graph = GraphBuilder.TransactionDrilldownGraph(month.Value, currency).ToJson()
                    });
                }
            }
            return model.OrderByDescending(s => s.StartDate).ToList();
        }

        public static List<RangeViewer> GetAnnual(List<Transaction> transactions, string currency)
        {
            var model = new List<RangeViewer>();

            var dict = new Dictionary<int, List<Transaction>>();

            foreach (var transaction in transactions)
            {
                var year = transaction.Date.Year;

                if (!dict.ContainsKey(year))
                {
                    dict.Add(year, new List<Transaction>());
                }

                dict[year].Add(transaction);
            }

            foreach (var year in dict)
            {

                model.Add(new RangeViewer
                {
                    Year = year.Key,
                    Range = Range.Annual,
                    Title = year.Key.ToString(),
                    StartDate = new DateTime(year.Key, 1, 1),
                    EndDate = new DateTime(year.Key, 12, 31),
                    Transactions = year.Value.OrderByDescending(s => s.Date).ToList(),
                    Graph = GraphBuilder.TransactionDrilldownGraph(year.Value, currency).ToJson()
                });

            }
            return model.OrderByDescending(s => s.StartDate).ToList();
        }

    }
}