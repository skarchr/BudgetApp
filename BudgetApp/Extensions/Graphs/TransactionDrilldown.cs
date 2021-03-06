﻿using System.Collections.Generic;
using System.Linq;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public static class TransactionDrilldown
    {

        public static Highchart CreateChart(List<Transaction> transactions, string currency, string graphType, bool sorted)
        {

            var drilldowns = new Drilldown
            {
                Series = CreateDrilldownSeries(transactions, graphType, sorted, false)
            };

            drilldowns.Series.Add(CreateSubMainCategorySeries(transactions, graphType, sorted, false));
            drilldowns.Series.AddRange(CreateDrilldownSeries(transactions, graphType, sorted, true));

            return new Highchart
            {
                Currency = currency,
                Type = graphType,
                Title = new Title
                {
                    Text = "Transactions"
                },
                Series = new List<Series>
                {
                    CreateTransactionCategorySeries(transactions, graphType, sorted, false)
                },
                Drilldown = drilldowns
            };
        }

        private static Series CreateTransactionCategorySeries(List<Transaction> transactions, string graphType, bool sorted, bool income)
        {
            var data = new List<Data>
            {
                new Data
                {
                    Color = "#E87C7C",
                    Drilldown = "expenses",
                    Name = "Expenses",
                    X = 0,
                    Y =
                        transactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income)
                            .Sum(s => s.Amount)
                },
                new Data
                {
                    Color = "#48ddb8",
                    Drilldown = "income",
                    Name = "Income",
                    X = 1,
                    Y =
                        transactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) == Categories.Income)
                            .Sum(s => s.Amount)
                }
            };

            if (sorted)
                data = SortDataListByY(data);

            return new Series
            {
                Id = "transactions",
                Name = "Transactions",
                Type = graphType,
                Data = data
            };
        }


        public static Highchart CreateExpensesChart(List<Transaction> transactions, string currency, string graphType, bool sorted)
        {

            return new Highchart
            {
                Currency = currency,
                Type = graphType,
                Title = new Title
                {
                    Text = "Expenses"
                },
                Series = new List<Series>
                {
                    CreateSubMainCategorySeries(transactions, graphType, sorted, false)
                },
                Drilldown = new Drilldown { Series = CreateDrilldownSeries(transactions, graphType, sorted, false) }
            };
        }

        private static List<Series> CreateDrilldownSeries(List<Transaction> transactions, string graphType, bool sorted, bool income)
        {
            var series = new List<Series>();

            var categories = income
                ? Categories.Grouped.Keys.Where(s => s == "Income")
                : Categories.Grouped.Keys.Where(s => s != "Income");

            foreach (var mainCategory in categories)
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

                series.Add(new Series { Id = mainCategory.ToLower(), Name = mainCategory, Type = graphType, Data = sorted ? SortDataListByY(data) : data });
            }
            return series;
        }

        private static Series CreateSubMainCategorySeries(List<Transaction> transactions, string graphType, bool sorted, bool income)
        {
            var data = new List<Data>();
            var index = 0;
            var categories = income
                ? Categories.Grouped.Keys.Where(s => s == "Income")
                : Categories.Grouped.Keys.Where(s => s != "Income");

            foreach (var mainCategory in categories)
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
                Name = income ? "Income":"Expenses",
                Type = graphType,
                Id = "expenses",
                Data = sorted ? SortDataListByY(data) : data
            };
        }


        private static List<Data> SortDataListByY(List<Data> data)
        {
            var sortedList = data.OrderByDescending(s => s.Y).Select(s => s.Name);

            var result = new List<Data>();

            var index = 0;
            foreach (var item in sortedList)
            {                
                var temp = data.First(s => s.Name == item);

                temp.X = index;

                result.Add(temp);

                index++;
            }
            return result;
        }

        public static Highchart CreateIncomeChart(List<Transaction> transactions, string currency, string graphType, bool sorted)
        {
            return new Highchart
            {
                Currency = currency,
                Type = graphType,
                Title = new Title
                {
                    Text = "Income"
                },
                Series = new List<Series>
                {
                    CreateSubMainCategorySeries(transactions, graphType, sorted, true)
                },
                Drilldown = new Drilldown { Series = CreateDrilldownSeries(transactions, graphType, sorted, true) }
            };
        }
    }
}