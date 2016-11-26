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
    public class DivisionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Divisions
        public ActionResult Index()
        {
            return View(db.DIVISION.ToList());
        }

        // GET: Divisions/Details/5
        public ActionResult Details(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton== null||OficinaImpugna == null|| FechaInicio== null || FechaFin ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            if (division == null)
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // GET: Divisions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Divisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCanton,CodigoOficinaImpugna,Estado,FechaDeInicio,FechaDeFin")] Division division)
        {
            if (ModelState.IsValid)
            {
                db.DIVISION.Add(division);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(division);
        }

        // GET: Divisions/Edit/5
        public ActionResult Edit(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton == null || OficinaImpugna == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            if (division == null)
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // POST: Divisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCanton,CodigoOficinaImpugna,Estado,FechaDeInicio,FechaDeFin")] Division division)
        {
            if (ModelState.IsValid)
            {
                db.Entry(division).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(division);
        }

        // GET: Divisions/Delete/5
        public ActionResult Delete(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton == null || OficinaImpugna == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            if (division == null)
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            db.DIVISION.Remove(division);
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
