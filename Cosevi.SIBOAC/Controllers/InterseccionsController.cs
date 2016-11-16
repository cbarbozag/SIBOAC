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
    public class InterseccionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Interseccions
        public ActionResult Index()
        {
            return View(db.INTERSECCION.ToList());
        }

        // GET: Interseccions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interseccion interseccion = db.INTERSECCION.Find(id);
            if (interseccion == null)
            {
                return HttpNotFound();
            }
            return View(interseccion);
        }

        // GET: Interseccions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Interseccions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Interseccion interseccion)
        {
            if (ModelState.IsValid)
            {
                db.INTERSECCION.Add(interseccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(interseccion);
        }

        // GET: Interseccions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interseccion interseccion = db.INTERSECCION.Find(id);
            if (interseccion == null)
            {
                return HttpNotFound();
            }
            return View(interseccion);
        }

        // POST: Interseccions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Interseccion interseccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interseccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(interseccion);
        }

        // GET: Interseccions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interseccion interseccion = db.INTERSECCION.Find(id);
            if (interseccion == null)
            {
                return HttpNotFound();
            }
            return View(interseccion);
        }

        // POST: Interseccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interseccion interseccion = db.INTERSECCION.Find(id);
            interseccion.Estado = "I";
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
