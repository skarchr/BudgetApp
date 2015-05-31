﻿using System.Linq;
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
            var currency = db.Users.First(u => u.UserName == User.Identity.Name).Currency;

            var model = new HomeViewModel
            {
                Transactions = transactions,
                DailyExpensesGraph = GraphBuilder.DailyExpensesGraph(transactions, currency).ToJson(),
                TransactionDrilldownGraph = GraphBuilder.TransactionDrilldownGraph(transactions, currency).ToJson()
            };

            return View("Index", model);
        }

    }
}