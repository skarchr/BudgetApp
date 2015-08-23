using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Models;

namespace BudgetApp.Extensions.Graphs
{
    public static class TreemapGenerator
    {
        public static List<Treemap> CreateChart(List<Transaction> transactions)
        {
            var categories = Categories.Grouped.Keys.Where(s => s != "Income");

            var dict = new Dictionary<string, Dictionary<string, List<Transaction>>>();

            var result = new List<Treemap>();

            foreach (var category in categories)
            {

                dict.Add(category, new Dictionary<string, List<Transaction>>());

                foreach (var tran in transactions)
                {

                    var cat = tran.Category.Value;

                    if (CategoryExt.GetMainCategory(cat) == category)
                    {
                        if (!dict[category].ContainsKey(cat.ToString()))
                        {
                            dict[category].Add(cat.ToString(), new List<Transaction>());
                        }
                        dict[category][cat.ToString()].Add(tran);

                    }

                }
            }


            foreach (var category in dict.Keys)
            {

                if (dict[category].Values.Count == 0)
                    continue;

                var mainCatTotal = 0.0;

                foreach (var trans in dict[category].OrderBy(s => s.Key))
                {
                    var value = trans.Value.Sum(s => s.Amount);

                    result.Add(new Treemap
                    {
                        Id = trans.Key.ToLower(),
                        Name = CategoryExt.CamelCaseToNormal(trans.Key),
                        Parent = category.ToLower(),
                        Value = value
                    });

                    mainCatTotal += value;
                }

                result.Add(new Treemap
                {
                    Value = mainCatTotal,
                    Name = category,
                    Id = category.ToLower(),
                    Color = HighchartUtilities.Colors[CategoryExt.GetCategoryColor(category)]
                });

            }

            return result;
        }
    }
}