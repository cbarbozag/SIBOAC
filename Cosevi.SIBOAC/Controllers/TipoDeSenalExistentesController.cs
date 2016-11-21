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
    public class TipoDeSenalExistentesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoDeSenalExistentes
        public ActionResult Index()
        {
            return View(db.TIPOSEÑALEXISTE.ToList());
        }

        // GET: TipoDeSenalExistentes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            if (tipoDeSenalExistente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeSenalExistente);
        }

        // GET: TipoDeSenalExistentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeSenalExistentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeSenalExistente tipoDeSenalExistente)
        {
            if (ModelState.IsValid)
            {
                db.TIPOSEÑALEXISTE.Add(tipoDeSenalExistente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDeSenalExistente);
        }

        // GET: TipoDeSenalExistentes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            if (tipoDeSenalExistente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeSenalExistente);
        }

        // POST: TipoDeSenalExistentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeSenalExistente tipoDeSenalExistente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeSenalExistente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDeSenalExistente);
        }

        // GET: TipoDeSenalExistentes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            if (tipoDeSenalExistente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeSenalExistente);
        }

        // POST: TipoDeSenalExistentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            if (tipoDeSenalExistente.Estado == "I")
                tipoDeSenalExistente.Estado = "A";
            else
                tipoDeSenalExistente.Estado = "I";
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
