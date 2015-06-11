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

        public static Highchart CreateChart(List<Transaction> transactions, string currency, string graphType)
        {

            return new Highchart
            {
                Currency = currency,
                Title = new Title
                {
                    Text = "Expenses"
                },
                Series = new List<Series>
                {
                    CreateMainCategorySeries(transactions, graphType)
                },
                Drilldown = new Drilldown { Series = CreateDrilldownSeries(transactions, graphType) }
            };
        }

        private static List<Series> CreateDrilldownSeries(List<Transaction> transactions, string graphType)
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
                        Color = HighchartUtilities.Colors[CategoryExt.GetCategoryColor(mainCategory)],
                        Name = category.ToString(),
                        X = index,
                        Y = transactions.Where(s => s.Category == category).Sum(s => s.Amount)
                    });
                    index++;
                }
                series.Add(new Series { Id = mainCategory.ToLower(), Name = mainCategory, Type = graphType, Data = data });
                mainIndex++;
            }
            return series;
        }

        private static Series CreateMainCategorySeries(List<Transaction> transactions, string graphType)
        {
            var data = new List<Data>();
            var index = 0;
            foreach (var mainCategory in Categories.Grouped.Keys.Where(s => s != "Income"))
            {

                data.Add(new Data
                {
                    Color = HighchartUtilities.Colors[CategoryExt.GetCategoryColor(mainCategory)],
                    Drilldown = mainCategory.ToLower(),
                    Name = mainCategory,
                    X = index,
                    Y = transactions.Where(s => s.Category != null && CategoryExt.GetMainCategory(s.Category.Value) == mainCategory).Sum(s => s.Amount)
                });
                index++;
            }

            return new Series
            {
                Name = "Expenses",
                Type = graphType,
                Id = "mainCategories",
                Data = data
            };
        }

    }
}