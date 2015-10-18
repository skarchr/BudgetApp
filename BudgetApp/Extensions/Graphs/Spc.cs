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

        public static Highchart CreateChart(List<Transaction> transactions, string currency, ChartRange range = ChartRange.Daily)
        {

            var categories = new List<string>();

            var serie = new List<Series>();
            var plotlinesY = new List<PlotLine>();
            var plotlinesX = new List<PlotLine>();
            int? max = null;

            transactions = transactions.Where(s => (CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income)).ToList();

            if (transactions.Count > 0)
            {
                serie.Add(CreateSeries(transactions, range, out categories));

                if (serie.First().Data.Count > 0)
                {
                    var median = serie[0].Data.Select(s => s.Y.Value).ToList().Median();
                    var stdDev = serie[0].Data.Select(s => s.Y.Value).ToList().StandardDeviation();

                    max = FindMaxValue(serie.First().Data, median, stdDev);

                    plotlinesX = CreatePlotLineX(serie[0].Data);

                    plotlinesX.Add(new PlotLine
                    {
                        Label = new Label
                        {
                            Text = string.Format("Median: {0,10:# ##0.00}, Mean: {1,10:# ##0.00}", median, serie[0].Data.Average(s => s.Y)),
                            Style = new Style
                            {
                                FontSize = "10px",
                                FontWeight = "bold"
                            },
                            VerticalAlign = "bottom",
                            Align = "right",
                            Y = -5
                        },
                        Value = 24,
                        Width = 2
                    });

                    plotlinesY.Add(

                        new PlotLine
                        {
                            Color = "rgb(72, 221, 184)",
                            DashStyle = "dash",
                            Width = 2,                            
                            Value = median
                        });

                    plotlinesY.Add(

                        new PlotLine
                        {
                            Color = "#b94a48",
                            DashStyle = "dash",
                            Width = 1,
                            Value = median + (3 * stdDev)
                        });

                    plotlinesY.Add(

                        new PlotLine
                        {
                            Color = "#b94a48",
                            DashStyle = "dash",
                            Width = 1,
                            Value = median - (3 * stdDev)
                        });
                }
            }

            return new Highchart
            {
                Categories = categories,
                Currency = currency,
                Series = serie,
                Title = new Title
                {
                    Text = "Statistical process control"
                },
                XAxis = new List<Axis>
                {
                    new Axis
                    {
                        Id = "x-line",
                        Categories = categories,
                        PlotLines = plotlinesX
                    }
                },
                YAxis = new List<Axis>
                {
                    new Axis
                    {
                        Id = "y-line",
                        PlotLines = plotlinesY,
                        Max = max
                    }
                }
            };
        }

        private static Series CreateSeries(List<Transaction> transactions, ChartRange range, out List<string> categories)
        {
            var data = range == ChartRange.Monthly
                ? CreateMonthData(transactions, out categories)
                : range == ChartRange.Weekly
                    ? CreateWeekData(transactions, out categories)
                    : CreateDayData(transactions, out categories);

            return new Series
            {
                Color = "rgb(0, 148, 244)",
                Type = "line",
                Data = data,
                Name = "Expenses",
                Id = "spc_expenses"
            };

        }

        private static List<Data> CreateDayData(List<Transaction> transactions, out List<string> categories)
        {
            categories = new List<string>();
            var data = new List<Data>();

            var endDate = transactions.OrderByDescending(s => s.Date).First().Date;
            var startDate = endDate.AddDays(-24);

            if (transactions.Count == 0)
                return data;

            var index = 0;
            while (startDate <= endDate && startDate != DateTime.Now)
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

        private static List<Data> CreateWeekData(List<Transaction> transactions, out List<string> categories)
        {
            categories = new List<string>();
            var data = new List<Data>();

            var endDate = transactions.OrderByDescending(s => s.Date).First().Date;
            var startDate = endDate.AddDays(-175);

            transactions = transactions.Where(s => s.Date >= startDate).ToList();

            var dateRange = new List<DateTime>();

            var initDate = endDate;

            if (DateHelper.GetWeekNumber(endDate) == DateHelper.GetWeekNumber(DateTime.Now) && endDate.Year == DateTime.Now.Year)
                initDate = initDate.AddDays(-7);
            
            var dateIndex = 0;

            while (dateIndex <= 24)
            {
                dateRange.Add(initDate);

                initDate = initDate.AddDays(-7);
                dateIndex++;
            }

            var index = 0;
            foreach (var date in dateRange.OrderBy(s => s))
            {
                var week = DateHelper.GetWeekNumber(date);

                if (week == 53)
                {
                    categories.Add("1");

                    data.Add(new Data
                    {
                        X = index,
                        Y = transactions.Where(s => DateHelper.GetWeekNumber(s.Date) == week || DateHelper.GetWeekNumber(s.Date) == 1).Sum(s => s.Amount),
                        DataLabels = new DataLabels { Enabled = false },
                        Year = date.Year
                    });

                }
                else
                {
                    categories.Add(week.ToString());

                    data.Add(new Data
                    {
                        X = index,
                        Y = transactions.Where(s => DateHelper.GetWeekNumber(s.Date) == week).Sum(s => s.Amount),
                        DataLabels = new DataLabels { Enabled = false },
                        Year = date.Year
                    });
                }
                
                index++;
            }

            return data;
        }

        private static List<Data> CreateMonthData(List<Transaction> transactions, out List<string> categories)
        {
            categories = new List<string>();
            var data = new List<Data>();

            var endDate = transactions.OrderByDescending(s => s.Date).First().Date;
            var startDate = new DateTime(endDate.AddMonths(-25).Year, endDate.AddMonths(-25).Month, 1);

            transactions = transactions.Where(s => s.Date >= startDate).ToList();

            var dateRange = new List<DateTime>();

            var initDate = endDate;

            if (endDate.Month == DateTime.Now.Month && endDate.Year == DateTime.Now.Year)
                initDate = initDate.AddMonths(-1);

            var dateIndex = 0;

            while (dateIndex <= 24)
            {
                dateRange.Add(initDate);

                initDate = initDate.AddMonths(-1);
                dateIndex++;
            }

            var index = 0;
            foreach (var date in dateRange.OrderBy(s => s))
            {
                var month = date.Month;
                
                categories.Add(DateHelper.GetMonthText(month,true));

                data.Add(new Data
                {
                    X = index,
                    Y = transactions.Where(s => s.Date.Month == month && s.Date.Year == date.Year).Sum(s => s.Amount),
                    DataLabels = new DataLabels {Enabled = false},
                    Year = date.Year
                });
                

                index++;
            }

            return data;
        }

        private static List<PlotLine> CreatePlotLineX(List<Data> data)
        {
            var result = new List<PlotLine>();
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
                result.Add(new PlotLine
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