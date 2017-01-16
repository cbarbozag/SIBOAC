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
    public class MantenimientoTablasController : Controller
    {
        private SIBOACSecurityEntities db = new SIBOACSecurityEntities();

        // GET: MantenimientoTablas
        public ActionResult Index()
        {
            return View(db.SIBOACTablas.ToList());
        }

        // GET: MantenimientoTablas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACTablas sIBOACTablas = db.SIBOACTablas.Find(id);
            if (sIBOACTablas == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACTablas);
        }

        // GET: MantenimientoTablas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MantenimientoTablas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] SIBOACTablas sIBOACTablas)
        {
            if (ModelState.IsValid)
            {
                db.SIBOACTablas.Add(sIBOACTablas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sIBOACTablas);
        }

        // GET: MantenimientoTablas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACTablas sIBOACTablas = db.SIBOACTablas.Find(id);
            if (sIBOACTablas == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACTablas);
        }

        // POST: MantenimientoTablas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] SIBOACTablas sIBOACTablas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sIBOACTablas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sIBOACTablas);
        }

        // GET: MantenimientoTablas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACTablas sIBOACTablas = db.SIBOACTablas.Find(id);
            if (sIBOACTablas == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACTablas);
        }

        // POST: MantenimientoTablas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACTablas sIBOACTablas = db.SIBOACTablas.Find(id);
            db.SIBOACTablas.Remove(sIBOACTablas);
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
