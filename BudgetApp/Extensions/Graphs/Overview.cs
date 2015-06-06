using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public static class Overview
    {
        public static Highchart CreateChart(List<RangeViewer> rangeViewers, ApplicationUser user)
        {

            var expenseSeries = CreateSeries(rangeViewers, true);

            var incomeSeries = CreateSeries(rangeViewers, false);

            var categories = CreateCategories(rangeViewers);

            var plotlineX = CreatePlotLineX(rangeViewers);

            return new Highchart
            {
                Currency = user.Currency,
                Categories = categories,
                Title = new Title
                {
                    Text = "Overview"
                },
                Series = new List<Series>
                {
                    expenseSeries, incomeSeries
                },
                PlotLinesY = new List<PlotLines>
                {
                    new PlotLines
                    {
                        Color = "#FF0000",
                        DashStyle = "dash",
                        Width = 1,
                        Value = user.MonthlyExpensesGoal != null ? user.MonthlyExpensesGoal.Value : 0.0
                    }
                },
                PlotLinesX = plotlineX
            };
        }

        private static List<PlotLines> CreatePlotLineX(List<RangeViewer> rangeViewers)
        {
            var result = new List<PlotLines>();
            var list = new Dictionary<int,int>();
            
            for (var i = 0; i < rangeViewers.Count; i++)
            {
                if (rangeViewers.Count != i +1)
                {
                    if(rangeViewers[i].Year != rangeViewers[i+1].Year)
                        list.Add(rangeViewers[i+1].Year, i+1);
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

        private static Series CreateSeries(List<RangeViewer> rangeViewers, bool expense)
        {
            
            var data = new List<Data>();

            var index = 0;
            foreach (var rangeViewer in rangeViewers)
            {
                data.Add(new Data
                {
                    Color = expense ? "#FF0000" : "#48DDb8",
                    Name = rangeViewer.Title,
                    X = index,
                    Y = expense ? rangeViewer.TotalExpenses : rangeViewer.TotalIncome,
                    DataLabels = new DataLabels { Enabled = false }
                });
                index++;
            }

            return new Series
            {
                Data = data,
                Color = expense ? "#FF0000" : "#48DDb8",
                Id = expense ? "expenses":"income",
                Name = expense ? "Expenses" : "Income",
                Type = "column"
            };

        }

        private static List<string> CreateCategories(List<RangeViewer> rangeViewers)
        {
            var result = new List<string>();

            foreach (var rangeViewer in rangeViewers)
            {
                var text = rangeViewer.Range == Range.Month ? DateHelper.GetMonthText(rangeViewer.StartDate, true) : rangeViewer.Title;

                result.Add(text);
            }
            return result;
        }

    }
}