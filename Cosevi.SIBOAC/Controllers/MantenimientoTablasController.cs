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
    public class MantenimientoTablasController : BaseController<SIBOACTablas>
    {
        private SIBOACSecurityEntities dbs = new SIBOACSecurityEntities();

        // GET: MantenimientoTablas
        public ActionResult Index()
        {
            return View(dbs.SIBOACTablas.ToList());
        }

        // GET: MantenimientoTablas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACTablas sIBOACTablas = dbs.SIBOACTablas.Find(id);
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
                dbs.SIBOACTablas.Add(sIBOACTablas);
                dbs.SaveChanges();
                Bitacora(sIBOACTablas, "I", "SIBOACTablas");
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
            SIBOACTablas sIBOACTablas = dbs.SIBOACTablas.Find(id);
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
                var sIBOACTablasAntes = dbs.SIBOACTablas.AsNoTracking().Where(d => d.Id == sIBOACTablas.Id).FirstOrDefault();
                dbs.Entry(sIBOACTablas).State = EntityState.Modified;
                dbs.SaveChanges();
                Bitacora(sIBOACTablas, "U", "SIBOACTablas", sIBOACTablasAntes);
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
            SIBOACTablas sIBOACTablas = dbs.SIBOACTablas.Find(id);
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
            SIBOACTablas sIBOACTablas = dbs.SIBOACTablas.Find(id);
            SIBOACTablas sIBOACTablasAntes = ObtenerCopia(sIBOACTablas);
            dbs.SIBOACTablas.Remove(sIBOACTablas);
            db.SaveChanges();
            Bitacora(sIBOACTablas, "U", "SIBOACTablas", sIBOACTablasAntes);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbs.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
