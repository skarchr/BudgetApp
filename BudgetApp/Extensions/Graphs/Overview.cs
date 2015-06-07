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
        private const string Type = "column";
        private const string ColorExpense = "#FF0000";
        private const string ColorIncome = "#48DDb8";

        public static Highchart CreateChart(List<RangeViewer> rangeViewers, ApplicationUser user)
        {
            var categories = CreateCategories(rangeViewers);

            var plotlinesX = CreatePlotLineX(rangeViewers);

            var plotLinesY = new List<PlotLines>();

            if (user.MonthlyExpensesGoal != null)
            {
                var goal = rangeViewers[0].Range == Range.Annual ? user.MonthlyExpensesGoal.Value * 12 : rangeViewers[0].Range == Range.Month ?  user.MonthlyExpensesGoal.Value : 0.0;

                plotLinesY.Add(
                    new PlotLines
                    {
                        Color = ColorExpense,
                        DashStyle = "dash",
                        Width = 1,
                        Value = goal,
                        Label = new Label
                        {
                            Align = "right",
                            Text = "Goal",
                            Y = -5,
                            Style = new Style
                            {
                                Color = ColorExpense
                            }
                        }
                    });
            }

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
                    CreateSeries(rangeViewers, true), CreateSeries(rangeViewers, false), CreateBalanceSeries(rangeViewers)
                },
                PlotLinesY = plotLinesY,
                PlotLinesX = plotlinesX
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
                    Color = expense ? ColorExpense : ColorIncome,
                    Name = rangeViewer.Title,
                    X = index,
                    Y = expense ?  rangeViewer.TotalExpenses : rangeViewer.TotalIncome,
                    DataLabels = new DataLabels { Enabled = false }
                });
                index++;
            }

            return new Series
            {
                Data = data,
                Color = expense ? ColorExpense : ColorIncome,
                Id = expense ? "expenses":"income",
                Name = expense ? "Expenses" : "Income",
                Type = Type
            };

        }

        private static Series CreateBalanceSeries(List<RangeViewer> rangeViewers)
        {
            var data = new List<Data>();

            var index = 0;
            foreach (var rangeViewer in rangeViewers)
            {
                data.Add(new Data
                {
                    Color = "#296ad4",
                    Name = "Balance",
                    X = index,
                    Y = rangeViewer.TotalIncome - rangeViewer.TotalExpenses,
                    DataLabels = new DataLabels { Enabled = false }
                });
                index++;
            }

            return new Series
            {
                Data = data,
                Color = "#296ad4",
                Id = "balance",
                Name = "Balance",
                Type = "line",
                Visible = false
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