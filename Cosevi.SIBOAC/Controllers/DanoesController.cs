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
    public class DanoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Danoes
        public ActionResult Index()
        {
            return View(db.DAÑO.ToList());
        }

        // GET: Danoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dano dano = db.DAÑO.Find(id);
            if (dano == null)
            {
                return HttpNotFound();
            }
            return View(dano);
        }

        // GET: Danoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Danoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Dano dano)
        {
            if (ModelState.IsValid)
            {
                db.DAÑO.Add(dano);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dano);
        }

        // GET: Danoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dano dano = db.DAÑO.Find(id);
            if (dano == null)
            {
                return HttpNotFound();
            }
            return View(dano);
        }

        // POST: Danoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Dano dano)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dano).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dano);
        }

        // GET: Danoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dano dano = db.DAÑO.Find(id);
            if (dano == null)
            {
                return HttpNotFound();
            }
            return View(dano);
        }

        // POST: Danoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dano dano = db.DAÑO.Find(id);
            db.DAÑO.Remove(dano);
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
