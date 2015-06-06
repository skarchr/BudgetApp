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
        public static Highchart CreateChart(List<RangeViewer> rangeViewers, string currency)
        {

            var expenseSeries = CreateSeries(rangeViewers, true);

            var incomeSeries = CreateSeries(rangeViewers, false);

            var categories = CreateCategories(rangeViewers);

            return new Highchart
            {
                Currency = currency,
                Categories = categories,
                Title = new Title
                {
                    Text = "Overview"
                },
                Series = new List<Series>
                {
                    expenseSeries, incomeSeries
                }
            };
        }

        private static Series CreateSeries(List<RangeViewer> rangeViewers, bool expense)
        {
            
            var data = new List<Data>();

            var index = 0;
            foreach (var rangeViewer in rangeViewers)
            {
                data.Add(new Data
                {
                    Color = expense ? "red":"green",
                    Name = rangeViewer.Title,
                    X = index,
                    Y = expense ? rangeViewer.TotalExpenses : rangeViewer.TotalIncome
                });
                index++;
            }

            return new Series
            {
                Data = data,
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
                var text = rangeViewer.Range == Range.Month ? DateHelper.GetMonthText(rangeViewer.StartDate, true, true) : rangeViewer.Title;

                result.Add(text);
            }
            return result;
        }

    }
}