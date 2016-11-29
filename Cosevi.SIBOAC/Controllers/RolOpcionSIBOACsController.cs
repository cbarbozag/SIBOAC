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
    public class RolOpcionSIBOACsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RolOpcionSIBOACs
        public ActionResult Index()
        {
            return View(db.RolOpcionSIBOAC.ToList());
        }

        // GET: RolOpcionSIBOACs/Details/5
        public ActionResult Details(int? idRol, int? idOpcion)
        {
            if (idRol == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcionSIBOAC rolOpcionSIBOAC = db.RolOpcionSIBOAC.Find(idRol, idOpcion);
            if (rolOpcionSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(rolOpcionSIBOAC);
        }

        // GET: RolOpcionSIBOACs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolOpcionSIBOACs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRol,IdOpcion")] RolOpcionSIBOAC rolOpcionSIBOAC)
        {
            if (ModelState.IsValid)
            {
                db.RolOpcionSIBOAC.Add(rolOpcionSIBOAC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rolOpcionSIBOAC);
        }

        // GET: RolOpcionSIBOACs/Edit/5
        public ActionResult Edit(int? idRol, int? idOpcion)
        {
            if (idRol == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcionSIBOAC rolOpcionSIBOAC = db.RolOpcionSIBOAC.Find(idRol, idOpcion);
            if (rolOpcionSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(rolOpcionSIBOAC);
        }

        // POST: RolOpcionSIBOACs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRol,IdOpcion")] RolOpcionSIBOAC rolOpcionSIBOAC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolOpcionSIBOAC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rolOpcionSIBOAC);
        }

        // GET: RolOpcionSIBOACs/Delete/5
        public ActionResult Delete(int? idRol, int? idOpcion)
        {
            if (idRol == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolOpcionSIBOAC rolOpcionSIBOAC = db.RolOpcionSIBOAC.Find(idRol, idOpcion);
            if (rolOpcionSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(rolOpcionSIBOAC);
        }

        // POST: RolOpcionSIBOACs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? idRol, int? idOpcion)
        {
            RolOpcionSIBOAC rolOpcionSIBOAC = db.RolOpcionSIBOAC.Find(idRol, idOpcion);
            db.RolOpcionSIBOAC.Remove(rolOpcionSIBOAC);
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
