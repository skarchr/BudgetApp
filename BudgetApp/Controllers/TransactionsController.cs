using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
            return View(db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
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
                return RedirectToAction("Index");
            }

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

                    return View("Index");
                }

                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);

                file.SaveAs(path);

                int found;

                var model = ExcelReader.ReadFile(path, User.Identity.Name, out found);

                ViewBag.Info = string.Format("Found {0} new transactions", found);

                if (model.Count == 0)
                    return View("Index", db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList());

                return View("Import", model);
            }

            ViewBag.Error = "Please select an excel file!";

            return View("Index");
        }

        [HttpPost]
        public ActionResult Import(List<Transaction> transactions)
        {

            for (int i = 0; i < transactions.Count(); i++)
            {
                if (!transactions[i].Import)
                {
                    ModelState["[" + i + "].Category"].Errors.Clear();
                    try
                    {
                        ModelState["[" + i + "].Description"].Errors.Clear();
                        ModelState["[" + i + "].Amount"].Errors.Clear();
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

                return RedirectToAction("Index", db.Transactions.Where(s => s.UserName == User.Identity.Name).ToList());
            }

            ViewBag.Error = "Please fill in all mandatory fields";

            return View("Import", transactions);
        }



    }
}
