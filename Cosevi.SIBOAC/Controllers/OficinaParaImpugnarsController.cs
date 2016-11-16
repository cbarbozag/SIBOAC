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
    public class OficinaParaImpugnarsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: OficinaParaImpugnars
        public ActionResult Index()
        {
            return View(db.OficinaParaImpugnars.ToList());
        }

        // GET: OficinaParaImpugnars/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            if (oficinaParaImpugnar == null)
            {
                return HttpNotFound();
            }
            return View(oficinaParaImpugnar);
        }

        // GET: OficinaParaImpugnars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OficinaParaImpugnars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OficinaParaImpugnar oficinaParaImpugnar)
        {
            if (ModelState.IsValid)
            {
                db.OficinaParaImpugnars.Add(oficinaParaImpugnar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(oficinaParaImpugnar);
        }

        // GET: OficinaParaImpugnars/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            if (oficinaParaImpugnar == null)
            {
                return HttpNotFound();
            }
            return View(oficinaParaImpugnar);
        }

        // POST: OficinaParaImpugnars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OficinaParaImpugnar oficinaParaImpugnar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oficinaParaImpugnar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oficinaParaImpugnar);
        }

        // GET: OficinaParaImpugnars/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            if (oficinaParaImpugnar == null)
            {
                return HttpNotFound();
            }
            return View(oficinaParaImpugnar);
        }

        // POST: OficinaParaImpugnars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            oficinaParaImpugnar.Estado = "I";
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
