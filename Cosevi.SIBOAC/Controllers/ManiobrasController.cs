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
    public class ManiobrasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Maniobras
        public ActionResult Index()
        {
            return View(db.Maniobra.ToList());
        }

        // GET: Maniobras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maniobra maniobra = db.Maniobra.Find(id);
            if (maniobra == null)
            {
                return HttpNotFound();
            }
            return View(maniobra);
        }

        // GET: Maniobras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maniobras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Maniobra maniobra)
        {
            if (ModelState.IsValid)
            {
                db.Maniobra.Add(maniobra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(maniobra);
        }

        // GET: Maniobras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maniobra maniobra = db.Maniobra.Find(id);
            if (maniobra == null)
            {
                return HttpNotFound();
            }
            return View(maniobra);
        }

        // POST: Maniobras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Maniobra maniobra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maniobra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(maniobra);
        }

        // GET: Maniobras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maniobra maniobra = db.Maniobra.Find(id);
            if (maniobra == null)
            {
                return HttpNotFound();
            }
            return View(maniobra);
        }

        // POST: Maniobras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maniobra maniobra = db.Maniobra.Find(id);
            if (maniobra.Estado == "I")
                maniobra.Estado = "A";
            else
                maniobra.Estado = "I";
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
