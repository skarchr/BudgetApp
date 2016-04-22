using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Util;
using BudgetApp.Constants;
using BudgetApp.Extensions;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;

namespace BudgetApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).OrderBy(s => s.Date).ToList();

            if (transactions.Count == 0)
                return View("Empty");
            var exp = transactions.Where(s => s.CategoryType == "Expenses").ToList();
            var timeSpan = 0;
            if ( exp.Count > 1)
                timeSpan = (exp.Last().Date - exp.First().Date).Days;

            var range = timeSpan >= 700 ?
                ChartRange.Monthly :
                    timeSpan >= 175 ?
                    ChartRange.Weekly :
                    ChartRange.Daily;

            var now = DateTime.Now;

            var from = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);
            var to = now;

            var userInfo = db.Users.First(s => s.UserName == User.Identity.Name);

            var model = new DashboardViewModel
            {
                FromDate = from,
                ToDate = to,
                First = transactions.First().Date,
                Last = transactions.Last().Date,
                Variables = CreateModel(transactions, from, to),
                SpcGraph = Spc.CreateChart(exp, "", range),
                BurnGraph = BurnRate.CreateChart(exp, DateTime.Now, userInfo.MonthlyExpensesGoal,userInfo.Currency),
                PrognosisChart = GraphBuilder.PrognosisGraph(transactions.Where(s => s.Date.Year == DateTime.Now.Year).ToList(), userInfo.Currency),
                PrognosisIncomeChart = GraphBuilder.PrognosisGraph(transactions.Where(s => s.Date.Year == DateTime.Now.Year).ToList(), userInfo.Currency, true)
            };

            return View(model);
        }

        [HttpGet]
        public string Refresh(DateTime from, DateTime to)
        {
            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).OrderBy(s => s.Date).ToList();

            return CreateModel(transactions, from, to).ToJson();
        }

        private DashboardVariables CreateModel(List<Transaction> transactions, DateTime from, DateTime to)
        {
            var trans = transactions.Where(s => s.Date >= from && s.Date <= to).ToList();

            var nod = (to - from).Days;

            if (trans.Count == 0)
            {
                return new DashboardVariables
                {
                    Income = 0,
                    Expenses = 0,
                    Balance = 0,
                    TreemapChart = TreemapGenerator.CreateChart(trans),
                    TransactionGraph = GraphBuilder.DrilldownGraph(trans, "", "column", true),
                    BalanceGraph = GraphGenerator.CreateMonthlyGraph(trans, "Monthly income vs expenses by category", true),
                    Frequency = Frequency.CreateGraph(trans),
                    CategoryGraph = GraphGenerator.CreateMonthlyGraph(trans,"Monthly expenses"),
                    MountainGraph = BurnRate.CreateMountain(trans),
                    DailyGraph = GraphGenerator.CreateDailyGraph(trans),
                    DailyExpense = 0.0,
                    Transactions = null,
                    Nod = nod
                };
            }
                

            var exp = trans.Where(s => s.CategoryType == "Expenses").ToList();

            var income = Math.Round(trans.Where(s => s.CategoryType == "Income").Sum(t => t.Amount), 1);
            var expenses = Math.Round(exp.Sum(t => t.Amount), 1);

            var first = exp.OrderBy(s => s.Date).First().Date;
            var sec = exp.OrderBy(s => s.Date).Last().Date;

            double days = (sec - first).Days;
            var daily = 0.0;

            if (days > 0)
                daily = exp.Sum(s => s.Amount) / days;

            var result = new DashboardVariables
            {
                Income = income,
                Expenses = expenses,
                Balance = Math.Round(income - expenses, 1),
                TreemapChart = TreemapGenerator.CreateChart(trans),
                TransactionGraph = GraphBuilder.DrilldownGraph(trans, "", "column", true),
                BalanceGraph = GraphGenerator.CreateMonthlyGraph(trans, "Monthly income vs expenses by category", true),                
                Frequency = Frequency.CreateGraph(exp),
                CategoryGraph = GraphGenerator.CreateMonthlyGraph(exp, "Monthly expenses by category"),
                MountainGraph = BurnRate.CreateMountain(exp),
                DailyGraph = GraphGenerator.CreateDailyGraph(exp),
                DailyExpense = daily,
                Transactions = trans.OrderByDescending(s => s.Date).ToList(),
                Nod = nod
            };

            return result;
        }

    }
}