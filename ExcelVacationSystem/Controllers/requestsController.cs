using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExcelVacationSystem.Models;

namespace ExcelVacationSystem.Controllers
{
    public class requestsController : Controller
    {
        private ExcelsystemsEntities2 db = new ExcelsystemsEntities2();

        // GET: requests
        public ActionResult Index()
        {
            var requests = db.requests.Include(r => r.Employee).Include(r => r.vacation);
            return View(requests.ToList());
        }

        // GET: requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            request request = db.requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: requests/Create
        public ActionResult Create()
        {
            ViewBag.empID = new SelectList(db.Employees, "emp_ssn", "name");
            ViewBag.vacID = new SelectList(db.vacations, "vac_ID", "vac_ID");
            return View();
        }

        // POST: requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "empID,vacID,date")] request request)
        {
            if (ModelState.IsValid)
            {
                db.requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.empID = new SelectList(db.Employees, "emp_ssn", "name", request.empID);
            ViewBag.vacID = new SelectList(db.vacations, "vac_ID", "vac_ID", request.vacID);
            return View(request);
        }

        // GET: requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            request request = db.requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewBag.empID = new SelectList(db.Employees, "emp_ssn", "name", request.empID);
            ViewBag.vacID = new SelectList(db.vacations, "vac_ID", "vac_ID", request.vacID);
            return View(request);
        }

        // POST: requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "empID,vacID,date")] request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.empID = new SelectList(db.Employees, "emp_ssn", "name", request.empID);
            ViewBag.vacID = new SelectList(db.vacations, "vac_ID", "vac_ID", request.vacID);
            return View(request);
        }

        // GET: requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            request request = db.requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            request request = db.requests.Find(id);
            db.requests.Remove(request);
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
    }
}
