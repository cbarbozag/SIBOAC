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
    public class TipoDeAccidentesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoDeAccidentes
        public ActionResult Index()
        {
            return View(db.TIPOACCIDENTE.ToList());
        }

        // GET: TipoDeAccidentes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeAccidente tipoDeAccidente = db.TIPOACCIDENTE.Find(id);
            if (tipoDeAccidente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeAccidente);
        }

        // GET: TipoDeAccidentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeAccidentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeAccidente tipoDeAccidente)
        {
            if (ModelState.IsValid)
            {
                db.TIPOACCIDENTE.Add(tipoDeAccidente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDeAccidente);
        }

        // GET: TipoDeAccidentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeAccidente tipoDeAccidente = db.TIPOACCIDENTE.Find(id);
            if (tipoDeAccidente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeAccidente);
        }

        // POST: TipoDeAccidentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeAccidente tipoDeAccidente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeAccidente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDeAccidente);
        }

        // GET: TipoDeAccidentes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeAccidente tipoDeAccidente = db.TIPOACCIDENTE.Find(id);
            if (tipoDeAccidente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeAccidente);
        }

        // POST: TipoDeAccidentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDeAccidente tipoDeAccidente = db.TIPOACCIDENTE.Find(id);
            db.TIPOACCIDENTE.Remove(tipoDeAccidente);
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
