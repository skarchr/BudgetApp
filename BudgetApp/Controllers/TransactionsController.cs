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
using BudgetApp.Extensions.Ranger;
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
                case Range.Annual:
                    rangeViewers = RangeHelper.GetAnnual(allTransactions, user.Currency);
                    break;
                case Range.Month:
                    rangeViewers = RangeHelper.GetMonth(allTransactions, user.Currency);
                    break;
                case Range.Week:
                    rangeViewers = RangeHelper.GetWeek(allTransactions, user.Currency);
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
                case Range.Annual:
                    rangeViewers = RangeHelper.GetAnnual(allTransactions, user.Currency);
                    break;
                case Range.Month:
                    rangeViewers = RangeHelper.GetMonth(allTransactions, user.Currency);
                    break;
                case Range.Week:
                    rangeViewers = RangeHelper.GetWeek(allTransactions, user.Currency);
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

                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), string.Format("{0}_{1}", Guid.NewGuid(),fileName));

                file.SaveAs(path);

                int found;

                var transactions = ExcelReader.ReadFile(path, User.Identity.Name, out found);

                ViewBag.Info = string.Format("Found {0} new transactions", found);

                if (transactions.Count == 0)
                {
                    TempData["Error"] = "No transactions found";
                    return RedirectToAction("Upload");
                }

                //Delete file

                new FileInfo(path).Delete();

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
