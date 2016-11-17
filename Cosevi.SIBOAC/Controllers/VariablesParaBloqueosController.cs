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
    public class VariablesParaBloqueosController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: VariablesParaBloqueos
        public ActionResult Index()
        {
            return View(db.VARIABLESBLOQUEO.ToList());
        }

        // GET: VariablesParaBloqueos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            if (variablesParaBloqueo == null)
            {
                return HttpNotFound();
            }
            return View(variablesParaBloqueo);
        }

        // GET: VariablesParaBloqueos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VariablesParaBloqueos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] VariablesParaBloqueo variablesParaBloqueo)
        {
            if (ModelState.IsValid)
            {
                db.VARIABLESBLOQUEO.Add(variablesParaBloqueo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(variablesParaBloqueo);
        }

        // GET: VariablesParaBloqueos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            if (variablesParaBloqueo == null)
            {
                return HttpNotFound();
            }
            return View(variablesParaBloqueo);
        }

        // POST: VariablesParaBloqueos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] VariablesParaBloqueo variablesParaBloqueo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(variablesParaBloqueo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(variablesParaBloqueo);
        }

        // GET: VariablesParaBloqueos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            if (variablesParaBloqueo == null)
            {
                return HttpNotFound();
            }
            return View(variablesParaBloqueo);
        }

        // POST: VariablesParaBloqueos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            variablesParaBloqueo.Estado = "I";
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
