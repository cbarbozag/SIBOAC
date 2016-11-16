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
    public class RolPorPersonasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RolPorPersonas
        public ActionResult Index()
        {
            return View(db.ROLPERSONA.ToList());
        }

        // GET: RolPorPersonas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            if (rolPorPersona == null)
            {
                return HttpNotFound();
            }
            return View(rolPorPersona);
        }

        // GET: RolPorPersonas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolPorPersonas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] RolPorPersona rolPorPersona)
        {
            if (ModelState.IsValid)
            {
                db.ROLPERSONA.Add(rolPorPersona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rolPorPersona);
        }

        // GET: RolPorPersonas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            if (rolPorPersona == null)
            {
                return HttpNotFound();
            }
            return View(rolPorPersona);
        }

        // POST: RolPorPersonas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] RolPorPersona rolPorPersona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolPorPersona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rolPorPersona);
        }

        // GET: RolPorPersonas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            if (rolPorPersona == null)
            {
                return HttpNotFound();
            }
            return View(rolPorPersona);
        }

        // POST: RolPorPersonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            db.ROLPERSONA.Remove(rolPorPersona);
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
