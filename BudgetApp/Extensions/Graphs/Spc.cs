using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;
using BudgetApp.Extensions.Statistics;

namespace BudgetApp.Extensions.Graphs
{
    public class Spc
    {
        public static Highchart CreateChart(List<Transaction> transactions, string currency, Range? range)
        {

            var categories = new List<string>();

            var serie = new List<Series>();
            var plotlinesY = new List<PlotLines>();

            if (transactions.Count > 0)
            {
                serie.Add(CreateSeries(transactions, range, out categories));

                if (serie.First().Data.Count > 0)
                {
                    var median = Statistics.Statistics.Median(serie[0].Data.Select(s => s.Y.Value).ToList());
                    var stdDev = Statistics.Statistics.StandardDeviation(serie[0].Data.Select(s => s.Y.Value).ToList());

                    plotlinesY.Add(

                        new PlotLines
                        {
                            Color = "green",
                            DashStyle = "dash",
                            Width = 1,
                            Label = new Label
                            {
                                Text = "Median",
                                Style = new Style
                                {                                    
                                    Color = "green"
                                },
                                Y = 4,
                                X = -46
                            },
                            Value = median
                        });

                    plotlinesY.Add(

                        new PlotLines
                        {
                            Color = "blue",
                            DashStyle = "dash",
                            Width = 1,
                            Label = new Label
                            {
                                Text = "UCL",
                                Style = new Style
                                {
                                    Color = "blue"
                                },
                                Y = 4,
                                X = -28
                            },
                            Value = median + (3 * stdDev)
                        });

                    plotlinesY.Add(

                        new PlotLines
                        {
                            Color = "blue",
                            DashStyle = "dash",
                            Width = 1,
                            Label = new Label
                            {
                                Text = "LCL",
                                Style = new Style
                                {
                                    Color = "blue"
                                },
                                Y = 15
                            },
                            Value = median - (3 * stdDev)
                        });
                }
            }

            return new Highchart
            {
                PlotLinesY = plotlinesY,
                Categories = categories,
                Currency = currency,
                Series = serie,
                Title = new Title
                {
                    Text = "SPC"
                }
            };
        }

        private static Series CreateSeries(List<Transaction> transactions, Range? range, out List<string> categories)
        {
            categories = new List<string>();

            transactions = transactions.Where(s => (CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income)).ToList();

            var endDate = transactions.OrderByDescending(s => s.Date).First().Date;

            var addedDays = -25 * ( range != null ? range == Range.Week ? 7 : range == Range.Month ? 30 : 1 : 1 );

            transactions = transactions.Where(s => s.Date >= endDate.AddDays(addedDays)).OrderBy(s => s.Date).ToList();

            return new Series
            {
                Color = "#b94a48",
                Type = "line",
                Data = range == Range.Month ? 
                    new List<Data>() : 
                    range == Range.Week ? 
                        CreateWeekData(transactions, transactions.First().Date, endDate, out categories) : 
                        CreateDayData(transactions, transactions.First().Date, endDate, out categories),
                Name = "SPC",
                Id = "spc_expenses"
            };

        }

        private static List<Data> CreateDayData(List<Transaction> transactions, DateTime startDate, DateTime endDate, out List<string> categories)
        {
            categories = new List<string>();
            var data = new List<Data>();

            var index = 0;
            while (startDate <= endDate)
            {
                categories.Add(startDate.ToString("d.MMM"));

                data.Add(new Data
                {
                    X = index,
                    Y = transactions.Where(s => s.Date == startDate).Sum(s => s.Amount),
                    DataLabels = new DataLabels { Enabled = false }
                });

                startDate = startDate.AddDays(1);
                index++;
            }
            return data;
        }

        private static List<Data> CreateWeekData(List<Transaction> transactions, DateTime startDate, DateTime endDate, out List<string> categories)
        {
            categories = new List<string>();
            var data = new List<Data>();

            var dict = new Dictionary<int, List<Transaction>>();

            foreach (var expense in transactions)
            {
                var week = DateHelper.GetWeekNumber(expense.Date);

                if (!dict.ContainsKey(week))
                {
                    dict.Add(week, new List<Transaction>());
                }

                dict[week].Add(expense);
            }

            var index = 0;
            foreach (var week in dict)
            {
                categories.Add(week.Key.ToString());

                data.Add(new Data
                {
                    X = index,
                    Y = week.Value.Sum(s => s.Amount),
                    DataLabels = new DataLabels{Enabled = false}
                });
                index++;
            }

            return data;
        }
    }
}