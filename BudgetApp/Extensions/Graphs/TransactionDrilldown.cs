﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public static class TransactionDrilldown
    {

        public static Highchart CreateChart(List<Transaction> transactions, string currency, string graphType, bool sorted)
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
                    CreateMainCategorySeries(transactions, graphType, sorted)
                },
                Drilldown = new Drilldown { Series = CreateDrilldownSeries(transactions, graphType, sorted) }
            };
        }

        private static List<Series> CreateDrilldownSeries(List<Transaction> transactions, string graphType, bool sorted)
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

                series.Add(new Series { Id = mainCategory.ToLower(), Name = mainCategory, Type = graphType, Data = sorted ? SortDataListByY(data) : data });
                mainIndex++;
            }
            return series;
        }

        private static Series CreateMainCategorySeries(List<Transaction> transactions, string graphType, bool sorted)
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
    }
}