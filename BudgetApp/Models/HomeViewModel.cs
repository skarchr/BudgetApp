using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BudgetApp.Constants;
using BudgetApp.Extensions;

namespace BudgetApp.Models
{
    public class HomeViewModel
    {
        public List<Transaction> Transactions { get; set; }

        public string DailyExpensesGraph { get; set; }
        public string TransactionDrilldownGraph { get; set; }
        public double? ExpensesGoal { get; set; }
        public string Currency { get; set; }
        public double AverageDailyExpenses
        {
            get
            {
                if (Transactions.Count > 0)
                {
                    var startDate = Transactions.OrderBy(s => s.Date).First().Date;
                    var endDate = Transactions.OrderBy(s => s.Date).Last().Date;
                    var total = Transactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income).Sum(s => s.Amount);

                    if (startDate == endDate)
                        return total;

                    return total / (endDate - startDate).Days;
                }

                return 0.0;
            }
        }
    }
}