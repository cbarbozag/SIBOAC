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
    public class TipoDeEstructurasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoDeEstructuras
        public ActionResult Index()
        {
            return View(db.ESTRUCTURA.ToList());
        }

        // GET: TipoDeEstructuras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // GET: TipoDeEstructuras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeEstructuras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeEstructura tipoDeEstructura)
        {
            if (ModelState.IsValid)
            {
                db.ESTRUCTURA.Add(tipoDeEstructura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDeEstructura);
        }

        // GET: TipoDeEstructuras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // POST: TipoDeEstructuras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeEstructura tipoDeEstructura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeEstructura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDeEstructura);
        }

        // GET: TipoDeEstructuras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // POST: TipoDeEstructuras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            db.ESTRUCTURA.Remove(tipoDeEstructura);
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
