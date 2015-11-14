using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using BudgetApp.Extensions;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Models;

namespace BudgetApp.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public const string BoxPlot = "_BoxPlot";
        public const string SPC = "_SPC";
        public const string Treemap = "_Treemap";
        public const string Drilldown = "_Drilldown";

        public class ReportViewModel
        {
            public string Name { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public bool IsDrilldown { get; set; }
            public ChartRange Range { get; set; }
            public string ChartType { get; set; }

            public List<string> Categories { get; set; }
            
            public List<string> Charts
            {
                get
                {
                    return new List<string>
                    {
                        //BoxPlot,
                        SPC,
                        Drilldown,
                        Treemap
                    };
                }
            }
        }

        // GET: Report
        public ActionResult Index()
        {
            var transactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).OrderBy(s => s.Date).ToList();

            var categories = CreateCategoryList();

            return View(new ReportViewModel
            {
                FromDate = transactions.FirstOrDefault() != null ? transactions.First().Date : DateTime.Now,
                ToDate = transactions.LastOrDefault() != null ? transactions.Last().Date : DateTime.Now,
                Categories = categories
            });
        }

        private List<string> CreateCategoryList()
        {
            var categories = new List<string>();

            foreach (var hovedCat in Constants.Categories.Grouped)
            {
                categories.AddRange(hovedCat.Value.Select(category => category.ToString()));
            }
            return categories;
        }

        [HttpPost]
        public ActionResult CreateChart(ReportViewModel model)
        {
            var transactions = FilterTransactions(db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList(), model);

            return PartialView(FindPartialView(model), FindChart(model, transactions));
        }

        private List<Transaction> FilterTransactions(List<Transaction> transactions, ReportViewModel model)
        {
            var result = DateFilter(transactions, model);

            return CategoryFilter(result, model);
        }

        public List<Transaction> DateFilter(List<Transaction> transactions, ReportViewModel model)
        {
            return transactions.Where(s => s.Date >= model.FromDate && s.Date <= model.ToDate).ToList();
        }

        public List<Transaction> CategoryFilter(List<Transaction> transactions, ReportViewModel model)
        {
            return transactions.Where(t => model.Categories.Contains(t.Category.Value.ToString())).ToList();
        } 

        private string FindChart(ReportViewModel model, List<Transaction> transactions)
        {
            switch (model.Name)
            {
                //case BoxPlot:
                case SPC:
                    return GraphBuilder.SpcGraph(transactions, "Nok", model.Range).ToJson();
                case Treemap:
                    return TreemapGenerator.CreateChart(transactions).ToJson();
                case Drilldown:
                    return GraphBuilder.DrilldownGraph(transactions, "NOK", model.ChartType ?? model.ChartType, true).ToJson();
                default:
                    throw new ArgumentException("No such graph exists!");
            }
        }

        private string FindPartialView(ReportViewModel model)
        {
            switch (model.Name)
            {
                case SPC:
                    return "_BoxPlot";
                default:
                    return model.Name;
            }
        }

        public ActionResult CreateTable()
        {
            return PartialView("_ReportTable");
        }

        public class CategoryViewModel
        {
            public string Category { get; set; }
            public bool IsChecked { get; set; }
        }
    }
}