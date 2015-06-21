using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
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
            ViewBag.Link = TempData["ViewBagLink"];

            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList();
            var user = db.Users.First(u => u.UserName == User.Identity.Name);

            var model = new HomeViewModel
            {
                Transactions = transactions,
                DailyExpensesGraph = GraphBuilder.DailyExpensesGraph(transactions, user.Currency).ToJson(),
                TransactionDrilldownGraph = GraphBuilder.TransactionDrilldownGraph(transactions, user.Currency, "bar", true).ToJson(),
                DrilldownGraphTot = GraphBuilder.DrilldownGraph(transactions, user.Currency, "column", true).ToJson(),
                DrilldownGraphYtd = GraphBuilder.DrilldownGraph(transactions.Where(s => s.Date.Year == DateTime.Now.Year).ToList(), user.Currency, "column", true).ToJson(),
                ExpensesGoal = user.MonthlyExpensesGoal,
                Currency = user.Currency
            };

            return View("Index", model);
        }

    }
}