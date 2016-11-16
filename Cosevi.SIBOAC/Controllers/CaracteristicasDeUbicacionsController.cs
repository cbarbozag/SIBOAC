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
    public class CaracteristicasDeUbicacionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: CaracteristicasDeUbicacions
        public ActionResult Index()
        {
            return View(db.CARACUBI.ToList());
        }

        // GET: CaracteristicasDeUbicacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            if (caracteristicasDeUbicacion == null)
            {
                return HttpNotFound();
            }
            return View(caracteristicasDeUbicacion);
        }

        // GET: CaracteristicasDeUbicacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CaracteristicasDeUbicacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CaracteristicasDeUbicacion caracteristicasDeUbicacion)
        {
            if (ModelState.IsValid)
            {
                db.CARACUBI.Add(caracteristicasDeUbicacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(caracteristicasDeUbicacion);
        }

        // GET: CaracteristicasDeUbicacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            if (caracteristicasDeUbicacion == null)
            {
                return HttpNotFound();
            }
            return View(caracteristicasDeUbicacion);
        }

        // POST: CaracteristicasDeUbicacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CaracteristicasDeUbicacion caracteristicasDeUbicacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caracteristicasDeUbicacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caracteristicasDeUbicacion);
        }

        // GET: CaracteristicasDeUbicacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            if (caracteristicasDeUbicacion == null)
            {
                return HttpNotFound();
            }
            return View(caracteristicasDeUbicacion);
        }

        // POST: CaracteristicasDeUbicacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            db.CARACUBI.Remove(caracteristicasDeUbicacion);
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
