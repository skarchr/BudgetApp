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
            ViewBag.Success = TempData["Success"];

            return View(db.Transactions.Where(s => s.UserName == User.Identity.Name).OrderByDescending(s => s.Date).ToList());
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

        [HttpPost]
        public void CreateInline(Transaction transaction)
        {

            if (transaction.Category.HasValue && transaction.Description.Count() > 2 && transaction.Amount > 0.0)
            {
                transaction.UserName = User.Identity.Name;
                transaction.Created = DateTime.Now;

                db.Transactions.Add(transaction);
                db.SaveChanges();

                ViewBag.Success = "Transaction added";
            }
            ViewBag.Error = "Invalid input";
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

            return View(new ExcelColumns());
        }

        [HttpPost]
        public ActionResult Upload(ExcelColumns excelColumns)
        {
            if(excelColumns == null)
                excelColumns = new ExcelColumns();

            var file = excelColumns.File;

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

                var transactions = ExcelReader.ReadFile(path, User.Identity.Name, excelColumns, out found);

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

            return View("Upload", model: new ExcelColumns());
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
