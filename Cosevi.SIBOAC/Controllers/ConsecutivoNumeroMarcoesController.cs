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
    public class ConsecutivoNumeroMarcoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: ConsecutivoNumeroMarcoes
        public ActionResult Index()
        {
            return View(db.CONSECUTIVONUMEROMARCO.ToList());
        }

        // GET: ConsecutivoNumeroMarcoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsecutivoNumeroMarco consecutivoNumeroMarco = db.CONSECUTIVONUMEROMARCO.Find(id);
            if (consecutivoNumeroMarco == null)
            {
                return HttpNotFound();
            }
            return View(consecutivoNumeroMarco);
        }

        // GET: ConsecutivoNumeroMarcoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConsecutivoNumeroMarcoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdAnterior,Estado,FechaDeInicio,FechaDeFin")] ConsecutivoNumeroMarco consecutivoNumeroMarco)
        {
            if (ModelState.IsValid)
            {
                db.CONSECUTIVONUMEROMARCO.Add(consecutivoNumeroMarco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(consecutivoNumeroMarco);
        }

        // GET: ConsecutivoNumeroMarcoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsecutivoNumeroMarco consecutivoNumeroMarco = db.CONSECUTIVONUMEROMARCO.Find(id);
            if (consecutivoNumeroMarco == null)
            {
                return HttpNotFound();
            }
            return View(consecutivoNumeroMarco);
        }

        // POST: ConsecutivoNumeroMarcoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdAnterior,Estado,FechaDeInicio,FechaDeFin")] ConsecutivoNumeroMarco consecutivoNumeroMarco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consecutivoNumeroMarco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consecutivoNumeroMarco);
        }

        // GET: ConsecutivoNumeroMarcoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsecutivoNumeroMarco consecutivoNumeroMarco = db.CONSECUTIVONUMEROMARCO.Find(id);
            if (consecutivoNumeroMarco == null)
            {
                return HttpNotFound();
            }
            return View(consecutivoNumeroMarco);
        }

        // POST: ConsecutivoNumeroMarcoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConsecutivoNumeroMarco consecutivoNumeroMarco = db.CONSECUTIVONUMEROMARCO.Find(id);
            db.CONSECUTIVONUMEROMARCO.Remove(consecutivoNumeroMarco);
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
