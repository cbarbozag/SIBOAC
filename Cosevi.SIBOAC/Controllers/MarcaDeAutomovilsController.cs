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
    public class MarcaDeAutomovilsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: MarcaDeAutomovils
        public ActionResult Index()
        {
            return View(db.MARCA.ToList());
        }

        // GET: MarcaDeAutomovils/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            if (marcaDeAutomovil == null)
            {
                return HttpNotFound();
            }
            return View(marcaDeAutomovil);
        }

        // GET: MarcaDeAutomovils/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MarcaDeAutomovils/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Topmarca,Estado,FechaDeInicio,FechaDeFin")] MarcaDeAutomovil marcaDeAutomovil)
        {
            if (ModelState.IsValid)
            {
                db.MARCA.Add(marcaDeAutomovil);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marcaDeAutomovil);
        }

        // GET: MarcaDeAutomovils/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            if (marcaDeAutomovil == null)
            {
                return HttpNotFound();
            }
            return View(marcaDeAutomovil);
        }

        // POST: MarcaDeAutomovils/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Topmarca,Estado,FechaDeInicio,FechaDeFin")] MarcaDeAutomovil marcaDeAutomovil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marcaDeAutomovil).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marcaDeAutomovil);
        }

        // GET: MarcaDeAutomovils/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            if (marcaDeAutomovil == null)
            {
                return HttpNotFound();
            }
            return View(marcaDeAutomovil);
        }

        // POST: MarcaDeAutomovils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            db.MARCA.Remove(marcaDeAutomovil);
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
