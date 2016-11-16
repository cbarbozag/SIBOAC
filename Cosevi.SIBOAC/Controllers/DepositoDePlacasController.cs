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
    public class DepositoDePlacasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DepositoDePlacas
        public ActionResult Index()
        {
            return View(db.DEPOSITOPLACA.ToList());
        }

        // GET: DepositoDePlacas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            if (depositoDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(depositoDePlaca);
        }

        // GET: DepositoDePlacas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepositoDePlacas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DepositoDePlaca depositoDePlaca)
        {
            if (ModelState.IsValid)
            {
                db.DEPOSITOPLACA.Add(depositoDePlaca);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(depositoDePlaca);
        }

        // GET: DepositoDePlacas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            if (depositoDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(depositoDePlaca);
        }

        // POST: DepositoDePlacas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DepositoDePlaca depositoDePlaca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depositoDePlaca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depositoDePlaca);
        }

        // GET: DepositoDePlacas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            if (depositoDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(depositoDePlaca);
        }

        // POST: DepositoDePlacas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            db.DEPOSITOPLACA.Remove(depositoDePlaca);
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
