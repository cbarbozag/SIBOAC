using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Content
{
    public class ValidarCarroceriasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: ValidarCarrocerias
        public ActionResult Index()
        {
            return View(db.VALIDARCARROCERIA.ToList());
        }

        // GET: ValidarCarrocerias/Details/5
        public ActionResult Details(string codigoIdentificacionVehiculo, int codigoTipoVehiculo, int codigoCarroceria )
        {
            if (codigoIdentificacionVehiculo ==null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ValidarCarroceria validarCarroceria = db.VALIDARCARROCERIA.Find(codigoIdentificacionVehiculo, codigoTipoVehiculo, codigoCarroceria);
            if (validarCarroceria == null)
            {
                return HttpNotFound();
            }
            return View(validarCarroceria);
        }

        // GET: ValidarCarrocerias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ValidarCarrocerias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTipoIdVehiculo,CodigoTiposVehiculos,CodigoCarroceria,Estado,FechaDeInicio,FechaDeFin")] ValidarCarroceria validarCarroceria)
        {
            if (ModelState.IsValid)
            {
                db.VALIDARCARROCERIA.Add(validarCarroceria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(validarCarroceria);
        }

        // GET: ValidarCarrocerias/Edit/5
        public ActionResult Edit(string codigoIdentificacionVehiculo, int codigoTipoVehiculo, int codigoCarroceria)
        {
            if (codigoIdentificacionVehiculo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ValidarCarroceria validarCarroceria = db.VALIDARCARROCERIA.Find(codigoIdentificacionVehiculo,  codigoTipoVehiculo, codigoCarroceria);
            if (validarCarroceria == null)
            {
                return HttpNotFound();
            }
            return View(validarCarroceria);
        }

        // POST: ValidarCarrocerias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTipoIdVehiculo,CodigoTiposVehiculos,CodigoCarroceria,Estado,FechaDeInicio,FechaDeFin")] ValidarCarroceria validarCarroceria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(validarCarroceria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(validarCarroceria);
        }

        // GET: ValidarCarrocerias/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ValidarCarroceria validarCarroceria = db.VALIDARCARROCERIA.Find(id);
            if (validarCarroceria == null)
            {
                return HttpNotFound();
            }
            return View(validarCarroceria);
        }

        // POST: ValidarCarrocerias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ValidarCarroceria validarCarroceria = db.VALIDARCARROCERIA.Find(id);
            db.VALIDARCARROCERIA.Remove(validarCarroceria);
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
