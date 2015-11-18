using System;
using System.Collections.Generic;
using System.Linq;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public class BurnRate
    {
        public static Highchart CreateChart(List<Transaction> transactions, DateTime currentDate, double? expensesGoal, string currency)
        {
            var series = new List<Series>();

            if (expensesGoal != null && transactions.Count != 0)
            {
                series.AddRange(CreateSeries1(transactions, currentDate, expensesGoal.Value, currency));
            }

            var daysLeft = DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day;

            double plotlineValue;

            if (daysLeft > 5)
            {
                plotlineValue =
                    GraphBuilder.ConvertDateToMilliSeconds(new DateTime(currentDate.Year, currentDate.Month,
                        (int) Math.Floor(daysLeft/2.0) + currentDate.Day));
            }
            else
            {
                plotlineValue =
                    GraphBuilder.ConvertDateToMilliSeconds(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day - 5));
            }

            return new Highchart
            {
                Currency = currency,
                Title = new Title
                {
                    Text = string.Format("Burn rate ({0})", expensesGoal == null ? "Expenses goal" : expensesGoal.Value.ToString())
                },
                Series = series,
                XAxis = new List<Axis>
                {
                    new Axis
                    {
                        PlotLines = new List<PlotLine>
                        {
                            new PlotLine
                            {
                                Value = plotlineValue,
                                Width = 2,
                                Color = null,
                                Label = new Label
                                {
                                    Text = string.Format("{0} day{1} left", daysLeft, daysLeft == 1 ? "":"s"),
                                    Align = "center",
                                    VerticalAlign = "bottom",
                                    Y = -5,
                                    Style = new Style
                                    {
                                        FontSize = "14px",
                                        FontWeight = "bold",
                                        Color = "#313131"
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private static List<Series> CreateSeries1(List<Transaction> transactions, DateTime currentDate, double goal, string currency)
        {
            var series = new List<Series>();

            double left;

            var filteredTransactions =
               transactions.Where(s => s.Date.Year == currentDate.Year && s.Date.Month == currentDate.Month && CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income).ToList();

            series.Add(CreateActualSeries(filteredTransactions, currentDate, currency, goal,  out left));

            series.Add(CreateLeftoverSeries(currentDate, currency, left));

            series.Add(new Series
            {
                Type = "line",
                Name = string.Format("{0} left", currency),
                Color = "#313131",
                NegativeColor = "#ff2a3e",
                Data = new List<Data>
                {
                    new Data
                    {
                        X = GraphBuilder.ConvertDateToMilliSeconds(new DateTime(currentDate.Year, currentDate.Month, currentDate.Day)),
                        Y = left,
                    }
                }
            });

            return series;
        }

        private static Series CreateLeftoverSeries(DateTime currentDate, string currency, double left)
        {
            var data = new List<Data>();
            var startDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
            var maxDays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

            while (startDate <= new DateTime(currentDate.Year, currentDate.Month, maxDays))
            {
                data.Add(new Data
                {
                    X = GraphBuilder.ConvertDateToMilliSeconds(startDate),
                    Y = left,
                    DataLabels = new DataLabels
                    {
                        Enabled = false
                    }
                });

                startDate = startDate.AddDays(1);
            }

            return new Series
            {
                Type = "area",
                Name = string.Format("{0} left", currency),
                Color = "#00ff98",
                NegativeColor = "#ff2a3e",
                Data = data,
                Marker = new Marker
                {
                    Radius = 0
                }
            };
        }

        private static Series CreateActualSeries(List<Transaction> transactions, DateTime currentDate, string currency, double expensesGoal, out double left)
        {
            var startDate = new DateTime(currentDate.Year, currentDate.Month, 1);

            var goal = expensesGoal;
            var series = new Series
            {
                Type = "area",
                Name = string.Format("{0} left", currency),
                Color = "#0094ff",
                NegativeColor = "#ff2a3e",
                Data = new List<Data>
                {
                    new Data
                    {
                        X = GraphBuilder.ConvertDateToMilliSeconds(new DateTime(currentDate.Year, currentDate.Month, 1).AddDays(-1)),
                        Y = goal,
                        DataLabels = new DataLabels
                        {
                            Enabled = false
                        }
                    }
                },
                Marker = new Marker
                {
                    Radius = 0
                }
            };
            var data = new List<Data>();

            while (startDate <= currentDate)
            {
                expensesGoal = Math.Round(expensesGoal - transactions.Where(s => s.Date == startDate).Sum(s => s.Amount), 1);
                
                data.Add(new Data
                {
                    X = GraphBuilder.ConvertDateToMilliSeconds(startDate),
                    Y = expensesGoal,
                    DataLabels = new DataLabels
                    {
                        Enabled = false
                    }
                });

                startDate = startDate.AddDays(1);
            }

            left = expensesGoal;

            series.Data.AddRange(data);

            return series;
        }
    }
}