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
    public class SentidoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Sentidoes
        public ActionResult Index()
        {
            return View(db.SENTIDO.ToList());
        }

        // GET: Sentidoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentido sentido = db.SENTIDO.Find(id);
            if (sentido == null)
            {
                return HttpNotFound();
            }
            return View(sentido);
        }

        // GET: Sentidoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sentidoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Sentido sentido)
        {
            if (ModelState.IsValid)
            {
                db.SENTIDO.Add(sentido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sentido);
        }

        // GET: Sentidoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentido sentido = db.SENTIDO.Find(id);
            if (sentido == null)
            {
                return HttpNotFound();
            }
            return View(sentido);
        }

        // POST: Sentidoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Sentido sentido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sentido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sentido);
        }

        // GET: Sentidoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentido sentido = db.SENTIDO.Find(id);
            if (sentido == null)
            {
                return HttpNotFound();
            }
            return View(sentido);
        }

        // POST: Sentidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Sentido sentido = db.SENTIDO.Find(id);
            db.SENTIDO.Remove(sentido);
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
