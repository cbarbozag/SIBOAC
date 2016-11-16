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
    public class CarroceriasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Carrocerias
        public ActionResult Index()
        {
            return View(db.CARROCERIA.ToList());
        }

        // GET: Carrocerias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carroceria carroceria = db.CARROCERIA.Find(id);
            if (carroceria == null)
            {
                return HttpNotFound();
            }
            return View(carroceria);
        }

        // GET: Carrocerias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrocerias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Carroceria carroceria)
        {
            if (ModelState.IsValid)
            {
                db.CARROCERIA.Add(carroceria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carroceria);
        }

        // GET: Carrocerias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carroceria carroceria = db.CARROCERIA.Find(id);
            if (carroceria == null)
            {
                return HttpNotFound();
            }
            return View(carroceria);
        }

        // POST: Carrocerias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Carroceria carroceria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carroceria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carroceria);
        }

        // GET: Carrocerias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carroceria carroceria = db.CARROCERIA.Find(id);
            if (carroceria == null)
            {
                return HttpNotFound();
            }
            return View(carroceria);
        }

        // POST: Carrocerias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carroceria carroceria = db.CARROCERIA.Find(id);
            db.CARROCERIA.Remove(carroceria);
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
