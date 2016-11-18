using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Controllers
{
    public class PeatonsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Peatons
        public ActionResult Index()
        {
            return View(db.Peaton.ToList());
        }

        // GET: Peatons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peaton peaton = db.Peaton.Find(id);
            if (peaton == null)
            {
                return HttpNotFound();
            }
            return View(peaton);
        }

        // GET: Peatons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Peatons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Peaton peaton)
        {
            if (ModelState.IsValid)
            {
                db.Peaton.Add(peaton);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(peaton);
        }

        // GET: Peatons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peaton peaton = db.Peaton.Find(id);
            if (peaton == null)
            {
                return HttpNotFound();
            }
            return View(peaton);
        }

        // POST: Peatons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Peaton peaton)
        {
            if (ModelState.IsValid)
            {
                db.Entry(peaton).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(peaton);
        }

        // GET: Peatons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peaton peaton = db.Peaton.Find(id);
            if (peaton == null)
            {
                return HttpNotFound();
            }
            return View(peaton);
        }

        // POST: Peatons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Peaton peaton = db.Peaton.Find(id);
            if (peaton.Estado == "I")
                peaton.Estado = "A";
            else
                peaton.Estado = "I";
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
