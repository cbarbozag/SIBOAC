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
    public class CodigoDeLaPlacasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: CodigoDeLaPlacas
        public ActionResult Index()
        {
            return View(db.CODIGO.ToList());
        }

        // GET: CodigoDeLaPlacas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            if (codigoDeLaPlaca == null)
            {
                return HttpNotFound();
            }
            return View(codigoDeLaPlaca);
        }

        // GET: CodigoDeLaPlacas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CodigoDeLaPlacas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] CodigoDeLaPlaca codigoDeLaPlaca)
        {
            if (ModelState.IsValid)
            {
                db.CODIGO.Add(codigoDeLaPlaca);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(codigoDeLaPlaca);
        }

        // GET: CodigoDeLaPlacas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            if (codigoDeLaPlaca == null)
            {
                return HttpNotFound();
            }
            return View(codigoDeLaPlaca);
        }

        // POST: CodigoDeLaPlacas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] CodigoDeLaPlaca codigoDeLaPlaca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(codigoDeLaPlaca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(codigoDeLaPlaca);
        }

        // GET: CodigoDeLaPlacas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            if (codigoDeLaPlaca == null)
            {
                return HttpNotFound();
            }
            return View(codigoDeLaPlaca);
        }

        // POST: CodigoDeLaPlacas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            if (codigoDeLaPlaca.Estado == "A")
                codigoDeLaPlaca.Estado = "I";
            else
                codigoDeLaPlaca.Estado = "A";
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
