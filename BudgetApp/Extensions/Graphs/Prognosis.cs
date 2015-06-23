using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public static class Prognosis
    {
        public static Highchart CreateChart(List<Transaction> transactions, string currency, bool income = false)
        {
            var categories = new List<string>
            {
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "May",
                "Jun",
                "Jul",
                "Aug",
                "Sep",
                "Okt",
                "Nov",
                "Des"
            };

            var series = CreateSeries(transactions, categories);

            var predicted = CreatePredictedSeries(transactions);

            if(predicted != null)
                series.Add(predicted);

            return new Highchart
            {
                Categories = categories,
                Currency = currency,
                Series = series,
                Title = new Title
                {
                    Text = income ? "Income" : "Expenses"
                }
            };
        }

        private static Series CreatePredictedSeries(List<Transaction> transactions)
        {
            var trans = transactions.Where(s => s.Date.Year == DateTime.Now.Year && CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income).ToList();

            if(trans.Count == 0)
                return null;

            var startDate = trans.OrderBy(s => s.Date).First().Date;
            var lastDate = trans.OrderBy(s => s.Date).Last().Date;

            var days = lastDate == startDate ? 1.0 : (lastDate - startDate).Days;

            var daily = trans.Sum(s => s.Amount) / days;

            var annual = daily*365;

            var monthly = Math.Round(annual/12, 1);

            var totAmount = 0.0;

            var data = new List<Data>();

            for (var i = 1; i <= 12; i++)
            {
                totAmount += monthly;

                data.Add(new Data
                {
                    X = i-1,
                    Y = totAmount,
                    DataLabels = new DataLabels
                    {
                        Enabled = false
                    }
                });
            }
            return new Series
            {
                Data = data,
                Name = "Predicted (" + DateTime.Now.Year + ")",
                Color = "#ECECEC",
                Type = "line"
            };
        }
 

        private static List<Series> CreateSeries(List<Transaction> transactions, List<string> categories)
        {
            var dict = new Dictionary<int, List<Transaction>>();
            var series = new List<Series>();
            foreach (
                var transaction in
                    transactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income))
            {
                var year = transaction.Date.Year;

                if (!dict.ContainsKey(year))
                {
                    dict.Add(year, new List<Transaction>());
                }

                dict[year].Add(transaction);
            }
            var mainIndex = 0;
            foreach (var year in dict.OrderBy(s => s.Key))
            {
                var data = new List<Data>();

                var index = 0;
                var totAmount = 0.0;

                for (var i = 1; i <= 12; i++)
                {
                    totAmount += year.Value.Where(s => s.Date.Month == i).Sum(s => s.Amount);

                    if (DateTime.Now >= new DateTime(year.Key, i, 1))
                    {
                        data.Add(new Data
                        {
                            Name = categories[i - 1],
                            X = index,
                            Y = totAmount,
                            DataLabels = new DataLabels
                            {
                                Enabled = false
                            }
                        });
                    }
                    index++;
                }

                series.Add(new Series
                {
                    Color = HighchartUtilities.Colors[mainIndex%HighchartUtilities.Colors.Count],
                    Id = year.Key.ToString().ToLower(),
                    Name = year.Key.ToString(),
                    Data = data,
                    Type = "line"
                });
                mainIndex++;
            }
            return series;
        }
    }
}