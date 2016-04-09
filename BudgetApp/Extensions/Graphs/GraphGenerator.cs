using System;
using System.Collections.Generic;
using System.Linq;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public class GraphGenerator
    {
        public static Highchart CreateMonthlyGraph(List<Transaction> transactions, string name, bool inOut = false)
        {
            var series = new List<Series>();
            var xPlot = new List<PlotLine>();

            if (transactions.Count > 0)
            {                
                var dict = new Dictionary<string, List<Transaction>>();
                var first = transactions.OrderBy(s => s.Date).First();

                foreach (var transaction in transactions)
                {
                    var main = !inOut ? transaction.MainCategory : transaction.CategoryType;
                    if(!dict.ContainsKey(main))
                        dict.Add(main, new List<Transaction>());
                    dict[main].Add(transaction);
                }

                foreach (var main in dict)
                {
                    var gModel = new GraphModel
                    {
                        Color = HighchartUtilities.Colors[CategoryExt.GetCategoryColor(main.Key)],
                        Type = "column",
                        Name = main.Key
                    };

                    series.Add(CreateMontlySeries(main.Value, first.Date, gModel));
                }

            }

            return new Highchart
            {
                Type = "column",
                Title = new Title
                {
                    Text = name
                },
                XAxis = new List<Axis>
                {
                    new Axis
                    {
                        Categories = CreateMontlyCategories(transactions, out xPlot),
                        PlotLines = xPlot
                    }
                },
                YAxis = new List<Axis>
                {
                    new Axis()
                },
                Series = series,
                Legend = true
            };
        } 


        public static Series CreateMontlySeries(List<Transaction> transactions, DateTime? first, GraphModel model)
        {
            if (transactions.Count == 0 || first == null)
                return null;

            
            var dict = new Dictionary<int, Dictionary<int, List<Transaction>>>();

            foreach (var transaction in transactions)
            {
                var year = transaction.Date.Year;

                if (!dict.ContainsKey(year))
                    dict.Add(year, new Dictionary<int, List<Transaction>>());

                var month = transaction.Date.Month;

                if (!dict[year].ContainsKey(month))
                    dict[year].Add(month, new List<Transaction>());

                dict[year][month].Add(transaction);
            }

            var data = new List<Data>();
            foreach (var year in dict)
            {
                foreach (var month in year.Value)
                {                 
                    data.Add(new Data
                    {
                        X = FindXValue(first.Value, month.Value.First()),
                        Y = month.Value.Sum(s => s.Amount),
                        DataLabels = new DataLabels
                        {
                            Enabled = false
                        }
                    });
                }
            }


            return new Series
            {
                Name = model.Name,
                Type = model.Type,
                Color = model.Color,
                Data = data
            };
        }

        public static List<string> CreateMontlyCategories(List<Transaction> transactions, out List<PlotLine> plotLines)
        {
            var categories = new List<string>();
            plotLines = new List<PlotLine>();

            if (transactions.Count == 0)
                return categories;

            var first = transactions.OrderBy(s => s.Date).First();
            var last = transactions.OrderBy(s => s.Date).Last();

            var init = new DateTime(first.Date.Year, first.Date.Month,1);
            var index = 0;
            do
            {
                if (init.Month == 12)
                {
                    plotLines.Add(new PlotLine
                    {
                        Color = "#c0c0c0",
                        DashStyle = "",
                        Value = index + 0.5,
                        Width = 1,
                        Label = new Label
                        {
                            Text = (init.Year + 1).ToString(),
                            Style = new Style
                            {
                                Color = "#c0c0c0"
                            },
                            Y = 15
                        },
                    });
                }

                categories.Add(DateHelper.GetMonthText(init,true));


                init = init.AddMonths(1);
                index++;
            } while (init <= last.Date);

            return categories;
        }

        private static double FindXValue(DateTime first, Transaction transaction)
        {
            var init = new DateTime(first.Date.Year, first.Date.Month, 1);
            var until = new DateTime(transaction.Date.Year, transaction.Date.Month, 1);
            var index = 0;

            while (init < until)
            {
                init = init.AddMonths(1);
                index++;
            }

            return index;

        }

    }
}