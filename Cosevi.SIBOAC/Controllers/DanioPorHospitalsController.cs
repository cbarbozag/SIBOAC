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
    public class DanioPorHospitalsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DanioPorHospitals
        public ActionResult Index()
        {
            return View(db.DAÑOXHOSPITAL.ToList());
        }

        // GET: DanioPorHospitals/Details/5
        public ActionResult Details(string codHosp, int codDanio)
        {
            if (codHosp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(codHosp, codDanio);
            if (danioPorHospital == null)
            {
                return HttpNotFound();
            }
            return View(danioPorHospital);
        }

        // GET: DanioPorHospitals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DanioPorHospitals/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdHospital,IdDanio,Estado,FechaDeInicio,FechaDeFin")] DanioPorHospital danioPorHospital)
        {
            if (ModelState.IsValid)
            {
                db.DAÑOXHOSPITAL.Add(danioPorHospital);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(danioPorHospital);
        }

        // GET: DanioPorHospitals/Edit/5
        public ActionResult Edit(string codHosp, int codDanio)
        {
            if (codHosp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(codHosp, codDanio);
            if (danioPorHospital == null)
            {
                return HttpNotFound();
            }
            return View(danioPorHospital);
        }

        // POST: DanioPorHospitals/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdHospital,IdDanio,Estado,FechaDeInicio,FechaDeFin")] DanioPorHospital danioPorHospital)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danioPorHospital).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danioPorHospital);
        }

        // GET: DanioPorHospitals/Delete/5
        public ActionResult Delete(string codHosp, int codDanio)
        {
            if (codHosp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(codHosp, codDanio);
            if (danioPorHospital == null)
            {
                return HttpNotFound();
            }
            return View(danioPorHospital);
        }

        // POST: DanioPorHospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codHosp, int codDanio)
        {
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(codHosp, codDanio);
            db.DAÑOXHOSPITAL.Remove(danioPorHospital);
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
