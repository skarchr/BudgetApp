using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public static class TransactionDrilldown
    {

        public static Highchart CreateChart(List<Transaction> transactions)
        {

            return new Highchart
            {
                Title = new Title
                {
                    Text = "Expenses"
                },
                Series = new List<Series>
                {
                    CreateMainCategorySeries(transactions)
                },
                Drilldown = new Drilldown { Series = CreateDrilldownSeries(transactions) }
            };
        }

        private static List<Series> CreateDrilldownSeries(List<Transaction> transactions)
        {
            var series = new List<Series>();

            var mainIndex = 0;
            foreach (var mainCategory in Categories.Grouped.Keys.Where(s => s != "Income"))
            {
                var data = new List<Data>();
                var index = 0;
                foreach (var category in Categories.Grouped[mainCategory].OrderBy(s => s.ToString()))
                {
                    data.Add(new Data
                    {
                        Color = HighchartUtilities.Colors[mainIndex],
                        Name = category.ToString(),
                        X = index,
                        Y = transactions.Where(s => s.Category == category).Sum(s => s.Amount)
                    });
                    index++;
                }
                series.Add(new Series { Id = mainCategory.ToLower(), Name = mainCategory, Type = "column", Data = data });
                mainIndex++;
            }
            return series;
        }

        private static Series CreateMainCategorySeries(List<Transaction> transactions)
        {
            var data = new List<Data>();
            var index = 0;
            foreach (var mainCategory in Categories.Grouped.Keys.Where(s => s != "Income"))
            {
                data.Add(new Data
                {
                    Color = HighchartUtilities.Colors[index],
                    Drilldown = mainCategory.ToLower(),
                    Name = mainCategory,
                    X = index,
                    Y = transactions.Where(s => s.Category != null && Categories.GetMainCategory(s.Category.Value) == mainCategory).Sum(s => s.Amount)
                });
                index++;
            }

            return new Series
            {
                Name = "Expenses",
                Type = "column",
                Id = "mainCategories",
                Data = data
            };
        }

    }
}