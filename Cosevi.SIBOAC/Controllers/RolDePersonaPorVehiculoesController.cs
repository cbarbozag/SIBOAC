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
    public class RolDePersonaPorVehiculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RolDePersonaPorVehiculoes
        public ActionResult Index()
        {
            return View(db.RolDePersonaPorVehiculoes.ToList());
        }

        // GET: RolDePersonaPorVehiculoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolDePersonaPorVehiculo rolDePersonaPorVehiculo = db.RolDePersonaPorVehiculoes.Find(id);
            if (rolDePersonaPorVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(rolDePersonaPorVehiculo);
        }

        // GET: RolDePersonaPorVehiculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolDePersonaPorVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ActivarVehiculo,Estado,FechaDeInicio,FechaDeFin")] RolDePersonaPorVehiculo rolDePersonaPorVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.RolDePersonaPorVehiculoes.Add(rolDePersonaPorVehiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rolDePersonaPorVehiculo);
        }

        // GET: RolDePersonaPorVehiculoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolDePersonaPorVehiculo rolDePersonaPorVehiculo = db.RolDePersonaPorVehiculoes.Find(id);
            if (rolDePersonaPorVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(rolDePersonaPorVehiculo);
        }

        // POST: RolDePersonaPorVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ActivarVehiculo,Estado,FechaDeInicio,FechaDeFin")] RolDePersonaPorVehiculo rolDePersonaPorVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolDePersonaPorVehiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rolDePersonaPorVehiculo);
        }

        // GET: RolDePersonaPorVehiculoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolDePersonaPorVehiculo rolDePersonaPorVehiculo = db.RolDePersonaPorVehiculoes.Find(id);
            if (rolDePersonaPorVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(rolDePersonaPorVehiculo);
        }

        // POST: RolDePersonaPorVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RolDePersonaPorVehiculo rolDePersonaPorVehiculo = db.RolDePersonaPorVehiculoes.Find(id);
            db.RolDePersonaPorVehiculoes.Remove(rolDePersonaPorVehiculo);
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
