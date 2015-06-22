using System;
using System.Collections.Generic;
using System.Linq;
using BudgetApp.Models;

namespace BudgetApp.Extensions
{
    public static class SavingHelper
    {
        public static List<SavingModel> CreateSavingsList(List<Transaction> transactions, double? goal)
        {
            var result = new List<SavingModel>();
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

            foreach (var year in dict)
            {
                foreach (var month in year.Value)
                {
                    var saved = month.Value.Where(s => s.Category == Category.Saving).Sum(s => s.Amount);

                    result.Add(new SavingModel
                    {
                        Date = new DateTime(year.Key, month.Key, 1),
                        Saved = saved,
                        Percentage = goal != null ? (saved * 100.0) / goal.Value : 0.0
                    });
                }
            }
            return result.OrderByDescending(s => s.Date).ToList();
        } 
    }
}