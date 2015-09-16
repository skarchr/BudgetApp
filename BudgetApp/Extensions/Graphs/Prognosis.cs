using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;
using WebGrease.Css.Extensions;

namespace BudgetApp.Extensions.Graphs
{
    public static class Prognosis
    {
        public static Highchart CreateChart(List<Transaction> transactions, string currency, bool income = false)
        {
            List<Transaction> filteredTransactions;

            if (income)
            {
                filteredTransactions = transactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) == Categories.Income).ToList();
            }
            else
            {
                filteredTransactions = transactions.Where(s =>  CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income).ToList();
            }


            double predicted;
            var series = CreateSeries(filteredTransactions, DateHelper.ShortMonths, income);

            var trans = CreateColumnSeries(filteredTransactions, income, out predicted);

            if(trans != null)
                series.Insert(0, trans);

            var plotPredicted = PredictedEndOfYear(predicted, income);

            return new Highchart
            {
                
                Currency = currency,
                Series = series,
                Title = new Title
                {
                    Text = income ? "Income" : "Expenses"
                },
                XAxis = new List<Axis>
                {
                    new Axis
                    {
                        Id = "x-line",
                        Categories = DateHelper.ShortMonths
                    }
                },
                YAxis = new List<Axis>
                {
                    new Axis
                    {
                        Id = "line-series",
                        Opposite = true,
                        PlotLines = new List<PlotLine>
                        {
                            plotPredicted
                        },
                        Labels = new Labels
                        {
                            Style = new Style
                            {
                                Color = income ? "#48DDb8" : "#b94a48"
                            }
                        },
                        Max = (int?)plotPredicted.Value
                    },
                    new Axis
                    {
                        Id = "column-series"
                    }
                }
            };
        }

        private static PlotLine PredictedEndOfYear(double value, bool income)
        {
            return new PlotLine
            {
                Value = value,
                Color = income ? "#48DDb8" : "#b94a48",
                DashStyle = "dash",
                Width = 1,
                Id = "predicted",
                Label = new Label
                {
                    Text = value.ToString(),
                    Align = "right",
                    Y = -6,
                    Style = new Style
                    {
                        Color = income ? "#48DDb8" : "#b94a48"
                    }
                }
            };
        }

        private static Series CreateColumnSeries(List<Transaction> transactions, bool income, out double predicted)
        {
            predicted = 0.0;

            var trans = transactions.Where(s => s.Date.Year == DateTime.Now.Year).ToList();

            if(trans.Count == 0)
                return null;

            var data = new List<Data>();

            for (var i = 1; i <= 12; i++)
            {
                data.Add(new Data
                {
                    X = i-1,
                    Y = trans.Where(t => t.Date.Month == i).Sum(s => s.Amount),
                    DataLabels = new DataLabels
                    {
                        Enabled = false
                    },
                    Color = i == DateTime.Now.Month ? "#0094f4" : "#C0C0C0"
                });
            }

            var includedCalc = data.Where(s => s.X < (DateTime.Now.Month - 1)).Average(s => s.Y);

            if (includedCalc != null)
            {
                predicted = (double) includedCalc* 12;
            }

            return new Series
            {
                Data = data,
                Name = string.Format("{0} ({1})", income ? "Income":"Expenses", DateTime.Now.Year),
                Color = "#C0C0C0",
                Type = "column",
                YAxis = 1
            };
        }
 

        private static List<Series> CreateSeries(List<Transaction> transactions, List<string> categories, bool income)
        {
            

            var dict = new Dictionary<int, List<Transaction>>();
            var series = new List<Series>();
            foreach (var transaction in transactions)
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
                        if (!(year.Key == DateTime.Now.Year && i == DateTime.Now.Month))
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

                        
                    }
                    index++;
                }

                series.Add(new Series
                {
                    Color = income ? "#48DDb8" : "#b94a48",
                    Id = year.Key.ToString().ToLower(),
                    Name = year.Key.ToString(),
                    Data = data,
                    Type = "line",
                    Visible = year.Key == DateTime.Now.Year,
                    YAxis = 0
                });
                mainIndex++;
            }

            return series;
        }
    }
}