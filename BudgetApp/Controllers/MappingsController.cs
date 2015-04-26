using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BudgetApp.Models;

namespace BudgetApp.Controllers
{
    public class MappingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Mappings
        public ActionResult Index()
        {
            ViewBag.Success = TempData["Success"];
            ViewBag.Error = TempData["Error"];

            return View(db.Mappings.Where(s => s.UserName == User.Identity.Name).ToList());
        }

        // GET: Mappings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapping mapping = db.Mappings.Find(id);
            if (mapping == null)
            {
                return HttpNotFound();
            }
            return View(mapping);
        }

        // GET: Mappings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mappings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MappingId,Category,TextDescription")] Mapping mapping)
        {
            if (ModelState.IsValid && !db.Mappings.Any(s => s.TextDescription == mapping.TextDescription && s.UserName == User.Identity.Name))
            {
                mapping.UserName = User.Identity.Name;
                mapping.Created = DateTime.Now;

                db.Mappings.Add(mapping);
                db.SaveChanges();

                TempData["Success"] = "Mapping added";

                return RedirectToAction("Index");
            }

            ViewBag.Error = "Mapping not added";

            return View(mapping);
        }


        // GET: Mappings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapping mapping = db.Mappings.Find(id);
            if (mapping == null)
            {
                return HttpNotFound();
            }
            return View(mapping);
        }

        // POST: Mappings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MappingId,UserName,Category,TextDescription,Created")] Mapping mapping)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mapping).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Success"] = "Mapping edited";

                return RedirectToAction("Index");
            }

            ViewBag.Error = "Mapping was not edited";

            return View(mapping);
        }

        // GET: Mappings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mapping mapping = db.Mappings.Find(id);
            if (mapping == null)
            {
                return HttpNotFound();
            }
            return View(mapping);
        }

        // POST: Mappings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mapping mapping = db.Mappings.Find(id);
            db.Mappings.Remove(mapping);
            db.SaveChanges();

            TempData["Success"] = "Mapping deleted";

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
    }
}
