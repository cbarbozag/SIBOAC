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
    public class CarrilsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Carrils
        public ActionResult Index()
        {
            return View(db.CARRIL.ToList());
        }

        // GET: Carrils/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carril carril = db.CARRIL.Find(id);
            if (carril == null)
            {
                return HttpNotFound();
            }
            return View(carril);
        }

        // GET: Carrils/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrils/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Carril carril)
        {
            if (ModelState.IsValid)
            {
                db.CARRIL.Add(carril);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carril);
        }

        // GET: Carrils/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carril carril = db.CARRIL.Find(id);
            if (carril == null)
            {
                return HttpNotFound();
            }
            return View(carril);
        }

        // POST: Carrils/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Carril carril)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carril).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carril);
        }

        // GET: Carrils/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carril carril = db.CARRIL.Find(id);
            if (carril == null)
            {
                return HttpNotFound();
            }
            return View(carril);
        }

        // POST: Carrils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Carril carril = db.CARRIL.Find(id);
            if (carril.Estado == "A")
                carril.Estado = "I";
            else
                carril.Estado = "A";
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
