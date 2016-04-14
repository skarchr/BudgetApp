using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public class Frequency
    {
        public static Highchart CreateGraph(List<Transaction> transactions)
        {

            var series = new List<Series>();
            var xAxis = new List<Axis>();

            if (transactions.Count > 0)
            {
                
                var dict = new Dictionary<string, List<Transaction>>();
                var categories = new List<string>();
                var data = new List<Data>();

                foreach (var transaction in transactions)
                {
                    var main = transaction.MainCategory;
                    if (!dict.ContainsKey(main))
                        dict.Add(main, new List<Transaction>());

                    dict[main].Add(transaction);
                }
                var index = 0;
                foreach (var category in dict.OrderBy(s => s.Key))
                {
                    data.Add(new Data
                    {
                        X = index,
                        Y = Math.Round(category.Value.Sum(s => s.Amount),1),
                        Color = HighchartUtilities.Colors[CategoryExt.GetCategoryColor(category.Value[0].Category)],
                        Z = category.Value.Count,
                        Name = category.Key,
                        DataLabels = new DataLabels
                        {
                            Enabled = false
                        }
                    });

                    categories.Add(category.Key);
                    index++;
                }

                var serie = new Series
                {
                    Data = data
                };
                
                xAxis.Add(new Axis
                {
                    Categories = categories
                });

                series.Add(serie);
            }
            
            return new Highchart
            {
                Title = new Title
                {
                    Text = "Frequency"
                },
                Series = series,
                XAxis = xAxis
            };
        }
    }
}