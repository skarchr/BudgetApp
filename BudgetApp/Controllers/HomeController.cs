using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using BudgetApp.Constants;
using BudgetApp.Extensions;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;

namespace BudgetApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (ConfigurationManager.AppSettings["debugmode"] == "true")
                ViewBag.Link = TempData["ViewBagLink"];

            ViewBag.Success = TempData["Success"];

            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList();
            var user = db.Users.First(u => u.UserName == User.Identity.Name);

            var expenses = transactions.Where(s => (CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income)).OrderBy(s => s.Date);

            var timeSpan = 0;
            
            if(expenses.Count() > 1)
                timeSpan = (expenses.Last().Date - expenses.First().Date).Days;

            var range = timeSpan >= 700 ?
                ChartRange.Monthly :
                    timeSpan >= 175 ?
                    ChartRange.Weekly :
                    ChartRange.Daily;

            var model = new HomeViewModel
            {
                ChartRange = range,
                Transactions = transactions,
                DailyExpensesGraph = GraphBuilder.DailyExpensesGraph(transactions, user.Currency).ToJson(),
                TransactionDrilldownGraph = GraphBuilder.TransactionDrilldownGraph(transactions, user.Currency, "bar", true).ToJson(),
                DrilldownGraphTot = GraphBuilder.DrilldownGraph(transactions, user.Currency, "column", true).ToJson(),
                DrilldownGraphYtd = GraphBuilder.DrilldownGraph(transactions.Where(s => s.Date.Year == DateTime.Now.Year).ToList(), user.Currency, "column", true).ToJson(),
                ScpExpensesChart = GraphBuilder.SpcGraph(transactions, user.Currency, range).ToJson(),
                ExpensesGoal = user.MonthlyExpensesGoal,
                SavingGoal = user.MonthlySavingGoal,
                Currency = user.Currency
            };

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult ChangeRange(ChartRange range)
        {
            if (ConfigurationManager.AppSettings["debugmode"] == "true")
                ViewBag.Link = TempData["ViewBagLink"];

            ViewBag.Success = TempData["Success"];

            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList();
            var user = db.Users.First(u => u.UserName == User.Identity.Name);

            var model = new HomeViewModel
            {
                ChartRange = range,
                Transactions = transactions,
                DailyExpensesGraph = GraphBuilder.DailyExpensesGraph(transactions, user.Currency).ToJson(),
                TransactionDrilldownGraph = GraphBuilder.TransactionDrilldownGraph(transactions, user.Currency, "bar", true).ToJson(),
                DrilldownGraphTot = GraphBuilder.DrilldownGraph(transactions, user.Currency, "column", true).ToJson(),
                DrilldownGraphYtd = GraphBuilder.DrilldownGraph(transactions.Where(s => s.Date.Year == DateTime.Now.Year).ToList(), user.Currency, "column", true).ToJson(),
                ScpExpensesChart = GraphBuilder.SpcGraph(transactions, user.Currency, range).ToJson(),
                ExpensesGoal = user.MonthlyExpensesGoal,
                SavingGoal = user.MonthlySavingGoal,
                Currency = user.Currency
            };

            return View("Index", model);
        }

    }
}