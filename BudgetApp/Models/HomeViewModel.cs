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
        public string DrilldownGraphTot { get; set; }
        public string DrilldownGraphYtd { get; set; }
        public double? ExpensesGoal { get; set; }
        public string Currency { get; set; }

        public double TotalExpenses
        {
            get
            {
                return Transactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income).Sum(s => s.Amount);
            }
        }

        public double TotalIncome
        {
            get
            {
                return Transactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) == Categories.Income).Sum(s => s.Amount);
            }
        }

        public double YtdExpenses
        {
            get
            {
                return Transactions.Where(s => s.Date.Year == DateTime.Now.Year && CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income).Sum(s => s.Amount);
            }
        }

        public double AverageDailyExpenses
        {
            get
            {
                if (Transactions.Count > 0)
                {
                    var startDate = Transactions.OrderBy(s => s.Date).First().Date;
                    var endDate = Transactions.OrderBy(s => s.Date).Last().Date;

                    if (startDate == endDate)
                        return TotalExpenses;

                    return TotalExpenses / (endDate - startDate).Days;
                }

                return 0.0;
            }
        }

        public double AverageMonthlyExpenses
        {
            get
            {
                if (Transactions.Count > 0)
                {
                    var startDate = Transactions.OrderBy(s => s.Date).First().Date;
                    var endDate = Transactions.OrderBy(s => s.Date).Last().Date;

                    if (startDate == endDate)
                        return TotalExpenses;

                    return TotalExpenses / ((endDate - startDate).Days / 30.0);
                }

                return 0.0;
            }
        }

        public double CurrentMonthExpenses
        {
            get
            {
                var today = DateTime.Now;
               
                return Transactions.Where(
                        s =>
                            CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income &&
                            s.Date.Month == today.Month && s.Date.Year == today.Year).Sum(s => s.Amount);
            }
        }

        public double AverageDailyIncome
        {
            get
            {
                if (Transactions.Count > 0)
                {
                    var startDate = Transactions.OrderBy(s => s.Date).First().Date;
                    var endDate = Transactions.OrderBy(s => s.Date).Last().Date;

                    if (startDate == endDate)
                        return TotalIncome;

                    return TotalIncome / (endDate - startDate).Days;
                }

                return 0.0;
            }
        }

        public double AverageMonthlyIncome
        {
            get
            {
                if (Transactions.Count > 0)
                {
                    var startDate = Transactions.OrderBy(s => s.Date).First().Date;
                    var endDate = Transactions.OrderBy(s => s.Date).Last().Date;

                    if (startDate == endDate)
                        return TotalIncome;

                    return TotalIncome / ((endDate - startDate).Days / 30.0);
                }

                return 0.0;
            }
        }

        public double YtdIncome
        {
            get
            {
               return Transactions.Where(s => s.Date.Year == DateTime.Now.Year && CategoryExt.GetMainCategory(s.Category.Value) == Categories.Income).Sum(s => s.Amount);
            }
        }
    }
}