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
    public class DepositoDeVehiculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DepositoDeVehiculoes
        public ActionResult Index()
        {
            return View(db.DEPOSITOVEHICULO.ToList());
        }

        // GET: DepositoDeVehiculoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            if (depositoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(depositoDeVehiculo);
        }

        // GET: DepositoDeVehiculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepositoDeVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DepositoDeVehiculo depositoDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.DEPOSITOVEHICULO.Add(depositoDeVehiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(depositoDeVehiculo);
        }

        // GET: DepositoDeVehiculoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            if (depositoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(depositoDeVehiculo);
        }

        // POST: DepositoDeVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DepositoDeVehiculo depositoDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depositoDeVehiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depositoDeVehiculo);
        }

        // GET: DepositoDeVehiculoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            if (depositoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(depositoDeVehiculo);
        }

        // POST: DepositoDeVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            depositoDeVehiculo.Estado = "I";
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
