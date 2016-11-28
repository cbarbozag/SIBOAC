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
    public class RolUsuarioSIBOACsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RolUsuarioSIBOACs
        public ActionResult Index()
        {
            return View(db.RolUsuarioSIBOAC.ToList());
        }

        // GET: RolUsuarioSIBOACs/Details/5
        public ActionResult Details(int? idRol, int? idUsuario)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            RolUsuarioSIBOAC rolUsuarioSIBOAC = db.RolUsuarioSIBOAC.Find(idRol, idUsuario);
            if (rolUsuarioSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(rolUsuarioSIBOAC);
        }

        // GET: RolUsuarioSIBOACs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolUsuarioSIBOACs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUsuario,IdRol")] RolUsuarioSIBOAC rolUsuarioSIBOAC)
        {
            if (ModelState.IsValid)
            {
                db.RolUsuarioSIBOAC.Add(rolUsuarioSIBOAC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rolUsuarioSIBOAC);
        }

        // GET: RolUsuarioSIBOACs/Edit/5
        public ActionResult Edit(int? idRol, int? idUsuario)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            RolUsuarioSIBOAC rolUsuarioSIBOAC = db.RolUsuarioSIBOAC.Find(idRol, idUsuario);
            if (rolUsuarioSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(rolUsuarioSIBOAC);
        }

        // POST: RolUsuarioSIBOACs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUsuario,IdRol")] RolUsuarioSIBOAC rolUsuarioSIBOAC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolUsuarioSIBOAC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rolUsuarioSIBOAC);
        }

        // GET: RolUsuarioSIBOACs/Delete/5
        public ActionResult Delete(int? idRol, int? idUsuario)
        {
           // if (id == null)
           // {
          //      return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
          //  }
            RolUsuarioSIBOAC rolUsuarioSIBOAC = db.RolUsuarioSIBOAC.Find(idRol, idUsuario);
            if (rolUsuarioSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(rolUsuarioSIBOAC);
        }

        // POST: RolUsuarioSIBOACs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? idRol, int? idUsuario)
        {
            RolUsuarioSIBOAC rolUsuarioSIBOAC = db.RolUsuarioSIBOAC.Find(idRol, idUsuario);
            db.RolUsuarioSIBOAC.Remove(rolUsuarioSIBOAC);
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
