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
    public class CondicionDeLaPersonasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: CondicionDeLaPersonas
        public ActionResult Index()
        {
            return View(db.CONDPERSONA.ToList());
        }

        // GET: CondicionDeLaPersonas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            if (condicionDeLaPersona == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaPersona);
        }

        // GET: CondicionDeLaPersonas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CondicionDeLaPersonas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CondicionDeLaPersona condicionDeLaPersona)
        {
            if (ModelState.IsValid)
            {
                db.CONDPERSONA.Add(condicionDeLaPersona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(condicionDeLaPersona);
        }

        // GET: CondicionDeLaPersonas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            if (condicionDeLaPersona == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaPersona);
        }

        // POST: CondicionDeLaPersonas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CondicionDeLaPersona condicionDeLaPersona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(condicionDeLaPersona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(condicionDeLaPersona);
        }

        // GET: CondicionDeLaPersonas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            if (condicionDeLaPersona == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaPersona);
        }

        // POST: CondicionDeLaPersonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            db.CONDPERSONA.Remove(condicionDeLaPersona);
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
