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
    public class DelitoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Delitoes
        public ActionResult Index()
        {
            return View(db.DELITO.ToList());
        }

        // GET: Delitoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delito delito = db.DELITO.Find(id);
            if (delito == null)
            {
                return HttpNotFound();
            }
            return View(delito);
        }

        // GET: Delitoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Delitoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Delito delito)
        {
            if (ModelState.IsValid)
            {
                db.DELITO.Add(delito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(delito);
        }

        // GET: Delitoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delito delito = db.DELITO.Find(id);
            if (delito == null)
            {
                return HttpNotFound();
            }
            return View(delito);
        }

        // POST: Delitoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Delito delito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(delito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(delito);
        }

        // GET: Delitoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delito delito = db.DELITO.Find(id);
            if (delito == null)
            {
                return HttpNotFound();
            }
            return View(delito);
        }

        // POST: Delitoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Delito delito = db.DELITO.Find(id);
            db.DELITO.Remove(delito);
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
