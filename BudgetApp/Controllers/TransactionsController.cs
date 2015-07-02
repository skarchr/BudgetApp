using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BudgetApp.Constants;
using BudgetApp.Extensions;
using BudgetApp.Extensions.Graphs;
using BudgetApp.Importer;
using BudgetApp.Models;

namespace BudgetApp.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var allTransactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList();

            var user = db.Users.First(s => s.UserName == User.Identity.Name);

            var range = user != null ? user.Range : Range.Annual;

            var rangeViewers = new List<RangeViewer>();

            switch (range)
            {
                case Range.All:
                    rangeViewers = new List<RangeViewer>
                    {
                        new RangeViewer
                        {
                            StartDate = allTransactions.OrderBy(s => s.Date).First().Date,
                            EndDate = allTransactions.OrderByDescending(s => s.Date).First().Date,
                            Range = Range.All,
                            Title = "All",
                            Transactions = allTransactions,
                            Graph = GraphBuilder.TransactionDrilldownGraph(allTransactions, user.Currency).ToJson()
                            
                        }
                    };
                    break;
                case Range.Annual:
                    rangeViewers = GetAnnual(allTransactions, user.Currency);
                    break;
                case Range.Month:
                    rangeViewers = GetMonth(allTransactions, user.Currency);
                    break;
                case Range.Week:
                    rangeViewers = GetWeek(allTransactions, user.Currency);
                    break;

            }

            var totalPages = (int)Math.Ceiling((double)rangeViewers.Count / 12);

            var pageViewer = rangeViewers.OrderByDescending(s => s.StartDate).Take(12).ToList();

            var model = new TransactionsViewModel 
            {
                RangeViewers = pageViewer.OrderBy(s => s.StartDate).ToList(), 
                Range = range,
                Currency = user.Currency,
                OverviewGraph = GraphBuilder.OverviewGraph(pageViewer.OrderBy(s => s.StartDate).ThenBy(f => f.Year).ToList(), user).ToJson(),
                CurrentPage = totalPages,
                TotalPages = totalPages,
                TotalExpenses = allTransactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income).Sum(s => s.Amount),
                TotalIncome = allTransactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) == Categories.Income).Sum(s => s.Amount)
            };

            ViewBag.Success = TempData["Success"];

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(TransactionsViewModel model)
        {
            var allTransactions = db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList();

            var user = db.Users.First(u => u.UserName == User.Identity.Name);

            if (model.Range != user.Range)
            {
                user.Range = model.Range;
                db.SaveChanges();
            }

            var rangeViewers = new List<RangeViewer>();

            switch (model.Range)
            {
                case Range.All:
                    rangeViewers = new List<RangeViewer>
                    {
                        new RangeViewer
                        {
                            StartDate = allTransactions.OrderBy(s => s.Date).First().Date,
                            EndDate = allTransactions.OrderByDescending(s => s.Date).First().Date,
                            Range = Range.All,
                            Title = "All",
                            Transactions = allTransactions,
                            Graph = GraphBuilder.TransactionDrilldownGraph(allTransactions, user.Currency).ToJson()
                            
                        }
                    };
                    break;
                case Range.Annual:
                    rangeViewers = GetAnnual(allTransactions, user.Currency);
                    break;
                case Range.Month:
                    rangeViewers = GetMonth(allTransactions, user.Currency);
                    break;
                case Range.Week:
                    rangeViewers = GetWeek(allTransactions, user.Currency);
                    break;

            }

            var totalPages = (int)Math.Ceiling((double)rangeViewers.Count / 12);

            if (model.CurrentPage == 0)
                model.CurrentPage = totalPages;

            var pageViewer = rangeViewers.OrderByDescending(s => s.StartDate).Skip(12 * (totalPages - model.CurrentPage)).Take(12).ToList();

            model.RangeViewers = pageViewer.OrderBy(s => s.StartDate).ToList();
            model.Currency = user.Currency;
            model.OverviewGraph = GraphBuilder.OverviewGraph(pageViewer.OrderBy(s => s.StartDate).ThenBy(f => f.Year).ToList(), user).ToJson();
            model.CurrentPage = model.CurrentPage;
            model.TotalPages = totalPages;
            model.TotalExpenses = allTransactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) != Categories.Income).Sum(s => s.Amount);
            model.TotalIncome = allTransactions.Where(s => CategoryExt.GetMainCategory(s.Category.Value) == Categories.Income).Sum(s => s.Amount);

            return View(model);
        }


        #region ModelBuilder

        private List<RangeViewer> GetWeek(List<Transaction> transactions, string currency)
        {
            var model = new List<RangeViewer>();
            var dict = new Dictionary<int, Dictionary<int, List<Transaction>>>();

            foreach (var expense in transactions)
            {
                var year = expense.Date.Year;

                if (!dict.ContainsKey(year))
                {
                    dict.Add(year, new Dictionary<int, List<Transaction>>());
                }

                var week = DateHelper.GetWeekNumber(expense.Date);

                if (!dict[year].ContainsKey(week))
                {
                    dict[year].Add(week, new List<Transaction>());
                }

                dict[year][week].Add(expense);
            }

            foreach (var year in dict)
            {
                foreach (var week in year.Value)
                {
                    model.Add(new RangeViewer
                    {
                        Year = year.Key,
                        Range = Range.Week,
                        Title = week.Key.ToString(),
                        StartDate = DateHelper.GetWeekStartDate(year.Key, week.Key),
                        EndDate = DateHelper.GetWeekEndDate(year.Key, week.Key),
                        Transactions = week.Value.OrderByDescending(s => s.Date).ToList(),
                        Graph = GraphBuilder.TransactionDrilldownGraph(week.Value, currency).ToJson()
                    });
                }
            }


            return model.OrderByDescending(s => s.StartDate).ToList();
        }

        private List<RangeViewer> GetMonth(List<Transaction> transactions, string currency)
        {
            var model = new List<RangeViewer>();

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
                    model.Add(new RangeViewer
                    {
                        Year = year.Key,
                        Range = Range.Month,
                        Title = month.Key.ToString(),
                        StartDate = new DateTime(year.Key, month.Key, 1),
                        EndDate = new DateTime(year.Key, month.Key, DateTime.DaysInMonth(year.Key, month.Key)),
                        Transactions = month.Value.OrderByDescending(s => s.Date).ToList(),
                        Graph = GraphBuilder.TransactionDrilldownGraph(month.Value, currency).ToJson()
                    });
                }
            }
            return model.OrderByDescending(s => s.StartDate).ToList();
        }

        private List<RangeViewer> GetAnnual(List<Transaction> transactions, string currency)
        {
            var model = new List<RangeViewer>();

            var dict = new Dictionary<int, List<Transaction>>();

            foreach (var transaction in transactions)
            {
                var year = transaction.Date.Year;

                if (!dict.ContainsKey(year))
                {
                    dict.Add(year, new List<Transaction>());
                }

                dict[year].Add(transaction);
            }

            foreach (var year in dict)
            {

                    model.Add(new RangeViewer
                    {
                        Year = year.Key,
                        Range = Range.Annual,
                        Title = year.Key.ToString(),
                        StartDate = new DateTime(year.Key,1,1),
                        EndDate = new DateTime(year.Key, 12,31),
                        Transactions = year.Value.OrderByDescending(s => s.Date).ToList(),
                        Graph = GraphBuilder.TransactionDrilldownGraph(year.Value, currency).ToJson()
                    });

            }
            return model.OrderByDescending(s => s.StartDate).ToList();
        }
        #endregion

        // GET: Transactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionId,Amount,Category,Description,Date,Created")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.UserName = User.Identity.Name;
                transaction.Created = DateTime.Now;

                db.Transactions.Add(transaction);
                db.SaveChanges();

                TempData["Success"] = "Transaction added";

                return RedirectToAction("Index");
            }
            ViewBag.Error = "Invalid input";
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionId,UserName,Amount,Category,Description,Date,Created")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Transaction edited";

                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();

            TempData["Success"] = "Transaction deleted";

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Upload()
        {
            ViewBag.Error = TempData["Error"];

            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                if (fileName == null)
                {
                    ViewBag.Error = "Something went wrong!";

                    return View("Upload");
                }

                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);

                file.SaveAs(path);

                int found;

                var transactions = ExcelReader.ReadFile(path, User.Identity.Name, out found);

                ViewBag.Info = string.Format("Found {0} new transactions", found);

                if (transactions.Count == 0)
                {
                    TempData["Error"] = "No transactions found";
                    return RedirectToAction("Upload");
                }

                return View("Import", new ImportViewModel{Transactions = transactions, Mapping = new Mapping()});
            }

            ViewBag.Error = "Please select an excel file!";

            return View("Upload");
        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public ActionResult InlineMapping(ImportViewModel viewModel)
        {

            if (!db.Mappings.Any(s => s.TextDescription == viewModel.Mapping.TextDescription && s.UserName == User.Identity.Name))
            {
                viewModel.Mapping.UserName = User.Identity.Name;
                viewModel.Mapping.Created = DateTime.Now;

                db.Mappings.Add(viewModel.Mapping);
                db.SaveChanges();
            }            

            viewModel.Mapping = new Mapping();
            

            var trans = new List<Transaction>();

            foreach (var transaction in viewModel.Transactions)
            {
                if (transaction.Category == null) 
                { 
                    var findCategory = ExcelReader.FindCategory(transaction.Description, User.Identity.Name);
                    if (findCategory != null)
                        transaction.Category = findCategory.Value;
                }
                trans.Add(transaction);
            }

            viewModel.Transactions = trans;

            return View("Import", viewModel);
        }

        [HttpParamAction]
        [ValidateAntiForgeryToken]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Import(List<Transaction> transactions)
        {

            for (int i = 0; i < transactions.Count(); i++)
            {
                if (!transactions[i].Import)
                {                    
                    try
                    {                        
                        ModelState["transactions[" + i + "].Description"].Errors.Clear();
                        ModelState["transactions[" + i + "].Amount"].Errors.Clear();
                        ModelState["transactions[" + i + "].Category"].Errors.Clear();
                    }
                    catch (Exception)
                    {

                    }

                }
            }

            if (ModelState.IsValid)
            {
                foreach (var transaction in transactions.Where(s => s.Import))
                {
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                }

                TempData["Success"] = string.Format("{0} transaction{1} added!", transactions.Count(s => s.Import), transactions.Count(s => s.Import).Equals(1) ? " was" : "s were");

                return RedirectToAction("Index", "Transactions");
            }

            ViewBag.Error = "Please fill in all mandatory fields";

            return View("Import", new ImportViewModel { Transactions = transactions, Mapping = new Mapping() });
        }



    }

    public class HttpParamActionAttribute : ActionNameSelectorAttribute
    {
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            if (actionName.Equals(methodInfo.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (!actionName.Equals("Action", StringComparison.InvariantCultureIgnoreCase))
                return false;

            var request = controllerContext.RequestContext.HttpContext.Request;
            return request[methodInfo.Name] != null;
        }
    }
}
