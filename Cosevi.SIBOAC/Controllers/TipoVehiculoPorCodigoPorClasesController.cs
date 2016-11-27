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
    public class TipoVehiculoPorCodigoPorClasesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoVehiculoPorCodigoPorClases
        public ActionResult Index()
        {
            return View(db.TIPOVEHCODIGOCLASE.ToList());
        }

        // GET: TipoVehiculoPorCodigoPorClases/Details/5
        public ActionResult Details(int? codigoTipoVehic, string clase, string codigo, int? codigoVehiculo)
        {
            if (clase == null || codigo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase = db.TIPOVEHCODIGOCLASE.Find(codigoTipoVehic, clase, codigo, codigoVehiculo);
            if (tipoVehiculoPorCodigoPorClase == null)
            {
                return HttpNotFound();
            }
            return View(tipoVehiculoPorCodigoPorClase);
        }

        // GET: TipoVehiculoPorCodigoPorClases/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoVehiculoPorCodigoPorClases/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTiposVehiculos,CodigoClasePlaca,CodigoCodigoPlaca,CodigoTipoVeh,Estado,FechaDeInicio,FechaDeFin")] TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase)
        {
            if (ModelState.IsValid)
            {
                db.TIPOVEHCODIGOCLASE.Add(tipoVehiculoPorCodigoPorClase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoVehiculoPorCodigoPorClase);
        }

        // GET: TipoVehiculoPorCodigoPorClases/Edit/5
        public ActionResult Edit(int? codigoTipoVehic, string clase, string codigo, int? codigoVehiculo)
        {
            if (clase == null || codigo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase = db.TIPOVEHCODIGOCLASE.Find(codigoTipoVehic, clase, codigo, codigoVehiculo);
            TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase = db.TIPOVEHCODIGOCLASE.Find(23, "INC", "OP ", 3);
            if (tipoVehiculoPorCodigoPorClase == null)
            {
                return HttpNotFound();
            }
            return View(tipoVehiculoPorCodigoPorClase);
        }

        // POST: TipoVehiculoPorCodigoPorClases/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTiposVehiculos,CodigoClasePlaca,CodigoCodigoPlaca,CodigoTipoVeh,Estado,FechaDeInicio,FechaDeFin")] TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoVehiculoPorCodigoPorClase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoVehiculoPorCodigoPorClase);
        }

        // GET: TipoVehiculoPorCodigoPorClases/Delete/5
        public ActionResult Delete(int? codigoTipoVehic, string clase, string codigo, int? codigoVehiculo)
        {
            if (clase == null || codigo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase = db.TIPOVEHCODIGOCLASE.Find(codigoTipoVehic, clase, codigo, codigoVehiculo);
            if (tipoVehiculoPorCodigoPorClase == null)
            {
                return HttpNotFound();
            }
            return View(tipoVehiculoPorCodigoPorClase);
        }

        // POST: TipoVehiculoPorCodigoPorClases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? codigoTipoVehic, string clase, string codigo, int? codigoVehiculo)
        {
            TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase = db.TIPOVEHCODIGOCLASE.Find(codigoTipoVehic, clase, codigo, codigoVehiculo);
            db.TIPOVEHCODIGOCLASE.Remove(tipoVehiculoPorCodigoPorClase);
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
