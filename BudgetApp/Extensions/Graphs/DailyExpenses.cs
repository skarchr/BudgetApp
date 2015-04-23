using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public static class DailyExpenses
    {
        public static Highchart CreateChart(List<Transaction> transactions)
        {
            return new Highchart
            {
                Title = new Title
                {
                    Text = "Daily expenses"
                },
                Series = new List<Series>
                {
                    new Series
                    {
                        Name = "Expenses",
                        Id = "expenses",
                        Data = CreateDataList(transactions)
                    }
                }
            };
        }

        private static List<Data> CreateDataList(List<Transaction> transactions)
        {

            var listUniqueDates = transactions.Select(s => s.Date).Distinct().OrderBy(s => s);

            var array = new List<Data>();

            foreach (var date in listUniqueDates)
            {
                var sum = transactions.Where(s => s.Date == date && Categories.GetMainCategory(s.Category.Value) != "Income").Sum(s => s.Amount);

                if (sum > 0.0)
                {
                    array.Add(new Data
                    {
                        X = GraphBuilder.ConvertDateToMilliSeconds(date),
                        Y = sum,
                        Color = HighchartUtilities.Colors[3],
                        DataLabels = new DataLabels{ Enabled = false }
                    });
                }                    
            }
            return array;

        } 

    }
}