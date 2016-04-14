using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public class Balance
    {
        public static Highchart CreateChart(List<Transaction> transactions)
        {
            var series = new List<Series>();

            if (transactions.Count != 0)
                series = CreateSeries(transactions);

            return new Highchart
            {
                Title = new Title
                {
                    Text = "Balance"
                },
                Series = series
            };
        }

        private static List<Series> CreateSeries(List<Transaction> transactions)
        {
            var incomeData = CreateIncomeData(transactions.Where(s => s.CategoryType == "Income").OrderBy(s => s.Date).ToList());

            var incomeList = new Series
            {
                Color = "rgb(72, 221, 184)",
                Name = "Income",
                Type = "column",
                Data = incomeData,
                DataGrouping = new DataGrouping
                {
                    Approximation = "sum",
                    Enabled = true,
                    Forced = true
                },
                Marker = new Marker
                {
                    Enabled = true,
                    Radius = 1
                }
            };
            var expensesData = CreateIncomeData(transactions.Where(s => s.CategoryType == "Expenses").OrderBy(s => s.Date).ToList());

            var expensesList = new Series
            {
                Color = "rgb(232, 124, 124)",
                Name = "Expenses",
                Type = "column",
                Data = expensesData,
                DataGrouping = new DataGrouping
                {
                    Approximation = "sum",
                    Enabled = true,
                    Forced = true
                }
            };
            return new List<Series>
            {
                incomeList,expensesList
            };

        }

        private static List<Data> CreateIncomeData(List<Transaction> transactions)
        {
            var dict = new Dictionary<DateTime, List<Transaction>>();

            var result = new List<Data>();

            foreach (var transaction in transactions)
            {
                var day = transaction.Date;

                if (!dict.ContainsKey(day))
                {
                    dict.Add(day, new List<Transaction>());
                }

                dict[day].Add(transaction);
            }

            foreach (var day in dict)
            {
                result.Add(new Data
                {
                    X = HighchartUtilities.ConvertToMilliseconds(day.Key),
                    Y = Math.Round(day.Value.Sum(s => s.Amount),0)
                });
            }
            return result;
        }
    }
}