﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions
{
    public class GraphBuilder
    {
        public static string TransactionGraph(List<Transaction> transactions)
        {

            var fix = SumCategory(transactions, "Fixed");
            var food = SumCategory(transactions, "Food");
            var personal = SumCategory(transactions, "Personal");
            var shelter = SumCategory(transactions, "Shelter");
            var trans = SumCategory(transactions, "Transport");


            return String.Format("[[\"Fixed\",{0}],[\"Food\",{1}],[\"Personal\",{2}],[\"Shelter\",{3}],[\"Transport\",{4}]]", fix, food, personal, shelter, trans);
        }

        private static string SumCategory(IEnumerable<Transaction> transactions, string mainCategory)
        {
            return transactions.Where(s => s.Category != null && Categories.GetMainCategory(s.Category.Value) == mainCategory).Sum(s => s.Amount).ToString(new CultureInfo("en-US"));
        }

        public static Highchart TransactionDrilldownGraph(List<Transaction> transactions)
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
                Drilldown = new Drilldown{Series = CreateDrilldownSeries(transactions) }
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
                series.Add(new Series {Id = mainCategory.ToLower(), Name = mainCategory, Type = "column", Data = data});
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