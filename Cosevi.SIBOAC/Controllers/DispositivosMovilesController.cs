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
    public class DispositivosMovilesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DispositivosMoviles
        public ActionResult Index()
        {
            return View(db.DispositivosMoviles.ToList());
        }

        // GET: DispositivosMoviles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            if (dispositivosMoviles == null)
            {
                return HttpNotFound();
            }
            return View(dispositivosMoviles);
        }

        // GET: DispositivosMoviles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DispositivosMoviles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IMEI,Descripcion,Activo")] DispositivosMoviles dispositivosMoviles)
        {
            if (ModelState.IsValid)
            {
                db.DispositivosMoviles.Add(dispositivosMoviles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dispositivosMoviles);
        }

        // GET: DispositivosMoviles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            if (dispositivosMoviles == null)
            {
                return HttpNotFound();
            }
            return View(dispositivosMoviles);
        }

        // POST: DispositivosMoviles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IMEI,Descripcion,Activo")] DispositivosMoviles dispositivosMoviles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dispositivosMoviles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dispositivosMoviles);
        }

        // GET: DispositivosMoviles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            if (dispositivosMoviles == null)
            {
                return HttpNotFound();
            }
            return View(dispositivosMoviles);
        }

        // POST: DispositivosMoviles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            dispositivosMoviles.Activo = false;
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
