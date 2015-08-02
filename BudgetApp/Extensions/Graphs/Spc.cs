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
        private static int? FindMaxValue(List<Data> data, double median, double stdDev)
        {
            var fourStdDev = (int?) Math.Ceiling(median + (4*stdDev));
            var tranMax = (int?)Math.Ceiling(data.Max(s => s.Y.Value));

            return fourStdDev > tranMax ? fourStdDev : tranMax;

        }

        public static Highchart CreateChart(List<Transaction> transactions, string currency, Range range = Range.Annual)
        {

            var categories = new List<string>();

            var serie = new List<Series>();
            var plotlinesY = new List<PlotLines>();
            var plotlinesX = new List<PlotLines>();
            int? max = null;

            if (transactions.Count > 0)
            {
                serie.Add(CreateSeries(transactions, range, out categories));

                if (serie.First().Data.Count > 0)
                {
                    var median = serie[0].Data.Select(s => s.Y.Value).ToList().Median();
                    var stdDev = serie[0].Data.Select(s => s.Y.Value).ToList().StandardDeviation();

                    max = FindMaxValue(serie.First().Data, median, stdDev);

                    plotlinesX = CreatePlotLineX(serie[0].Data);

                    plotlinesY.Add(

                        new PlotLines
                        {
                            Color = "rgb(72, 221, 184)",
                            DashStyle = "dash",
                            Width = 2,
                            Label = new Label
                            {
                                Text = "Median",
                                Style = new Style
                                {
                                    Color = "rgb(72, 221, 184)"
                                },
                                Y = 14,
                                X = -5,
                                Align = "right"
                            },
                            Value = median
                        });

                    plotlinesY.Add(

                        new PlotLines
                        {
                            Color = "#b94a48",
                            DashStyle = "dash",
                            Width = 1,
                            Label = new Label
                            {
                                Text = "UCL",
                                Style = new Style
                                {
                                    Color = "#b94a48"
                                },
                                Y = 14,
                                X = -5,
                                Align = "right"
                            },
                            Value = median + (3 * stdDev)
                        });

                    plotlinesY.Add(

                        new PlotLines
                        {
                            Color = "#b94a48",
                            DashStyle = "dash",
                            Width = 1,
                            Label = new Label
                            {
                                Text = "LCL",
                                Style = new Style
                                {
                                    Color = "#b94a48"
                                },
                                Y = -6,
                                X = -5,
                                Align = "right"
                            },
                            Value = median - (3 * stdDev)
                        });
                }
            }

            return new Highchart
            {
                Max = max,
                PlotLinesY = plotlinesY,
                PlotLinesX = plotlinesX,
                Categories = categories,
                Currency = currency,
                Series = serie,
                Title = new Title
                {
                    Text = "SPC (expenses)"
                }
            };
        }

        private static Series CreateSeries(List<Transaction> transactions, Range range, out List<string> categories)
        {
            transactions = transactions.Where(s => (CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income)).ToList();

            var endDate = transactions.OrderByDescending(s => s.Date).First().Date;

            var addedDays = -25 * ( range != null ? range == Range.Week ? 7 : range == Range.Month ? 30 : 1 : 1 );

            transactions = transactions.Where(s => s.Date >= endDate.AddDays(addedDays)).OrderBy(s => s.Date).ToList();

            return new Series
            {
                Color = "rgb(0, 148, 244)",
                Type = "line",
                Data = range != Range.Annual ?
                        range == Range.Week ? CreateWeekData(transactions, range, out categories) : CreateMonthData(transactions, out categories) :
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

        private static List<Data> CreateWeekData(List<Transaction> transactions, Range range, out List<string> categories)
        {
            categories = new List<string>();
            var data = new List<Data>();

            var dict = new Dictionary<int, List<Transaction>>();

            foreach (var expense in transactions)
            {
                var ran = range == Range.Week ? DateHelper.GetWeekNumber(expense.Date) : expense.Date.Month;

                if (!dict.ContainsKey(ran))
                {
                    dict.Add(ran, new List<Transaction>());
                }

                dict[ran].Add(expense);
            }

            var index = 0;
            foreach (var ran in dict)
            {
                categories.Add(range == Range.Week ? ran.Key.ToString() : DateHelper.GetMonthText(ran.Key, true));

                data.Add(new Data
                {
                    X = index,
                    Y = ran.Value.Sum(s => s.Amount),
                    DataLabels = new DataLabels{Enabled = false},
                    Year = ran.Value.First().Date.Year
                });
                index++;
            }

            return data;
        }

        private static List<Data> CreateMonthData(List<Transaction> transactions, out List<string> categories)
        {
            categories = new List<string>();
            var data = new List<Data>();
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

            var index = 0;
            foreach (var year in dict)
            {
                foreach (var month in year.Value)
                {
                    categories.Add(DateHelper.GetMonthText(month.Key, true));

                    data.Add(new Data
                    {
                        X = index,
                        Y = month.Value.Sum(s => s.Amount),
                        DataLabels = new DataLabels { Enabled = false },
                        Year = month.Value.First().Date.Year
                    });
                    index++;
                }
            }

            return data;
        }

        private static List<PlotLines> CreatePlotLineX(List<Data> data)
        {
            var result = new List<PlotLines>();
            var list = new Dictionary<int, int>();

            for (var i = 0; i < data.Count; i++)
            {
                if (data.Count != i + 1)
                {
                    if (data[i].Year < data[i + 1].Year)
                    {
                        if (!list.ContainsKey(data[i + 1].Year))
                            list.Add(data[i + 1].Year, i + 1);
                    }
                }
            }

            foreach (var i in list)
            {
                result.Add(new PlotLines
                {
                    Color = "#c0c0c0",
                    DashStyle = "",
                    Width = 1,
                    Id = i.Key.ToString(),
                    Label = new Label
                    {
                        Text = i.Key.ToString(),
                        Style = new Style
                        {
                            Color = "#c0c0c0"
                        },
                        Y = 15
                    },
                    Value = (i.Value - 0.5)
                });
            }
            return result;
        }
    }
}