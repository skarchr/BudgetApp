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
                return View();

            var from = transactions.First().Date;
            var to = transactions.Last().Date;

            var model = CreateModel(transactions, from, to);

            return View(model);
        }

        [HttpGet]
        public string Refresh(DateTime from, DateTime to)
        {
            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).OrderBy(s => s.Date).ToList();

            return CreateModel(transactions, from, to).ToJson();
        }

        private DashboardViewModel CreateModel(List<Transaction> transactions, DateTime from, DateTime to)
        {
            if (transactions.Count == 0)
                throw new Exception("No transactions found");

            var income = Math.Round(transactions.Where(
                s => s.CategoryType == Categories.Income && s.Date >= from &&
                     s.Date <= to).Sum(t => t.Amount), 1);
            var expenses = Math.Round(transactions.Where(
                s =>
                    s.CategoryType != Categories.Income && s.Date >= from &&
                    s.Date <= to).Sum(t => t.Amount), 1);

            var result = new DashboardViewModel
            {
                FromDate = from,
                ToDate = to,
                Income = income,
                Expenses = expenses,
                Balance = Math.Round(income - expenses, 0),
                TreemapChart = TreemapGenerator.CreateChart(transactions.Where(s => s.Date >= from && s.Date <= to).ToList()),
                TransactionGraph = GraphBuilder.DrilldownGraph(transactions.Where(s => s.Date >= from && s.Date <= to).ToList(), "", "column", true),
                BalanceGraph = Balance.CreateChart(transactions.Where(s => s.Date >= from && s.Date <= to).ToList())
            };

            return result;
        }

    }
}