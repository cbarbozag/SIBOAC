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
    public class MantenimientoRolesController : Controller
    {
        private SIBOACSecurityEntities db = new SIBOACSecurityEntities();

        // GET: MantenimientoRoles
        [SessionExpire]
        public ActionResult Index()
        {
            return View(db.SIBOACRoles.ToList());
        }

        // GET: MantenimientoRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACRoles sIBOACRoles = db.SIBOACRoles.Find(id);
            if (sIBOACRoles == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACRoles);
        }

        // GET: MantenimientoRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MantenimientoRoles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Activo")] SIBOACRoles sIBOACRoles)
        {
            if (ModelState.IsValid)
            {
                db.SIBOACRoles.Add(sIBOACRoles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sIBOACRoles);
        }

        // GET: PRUEBARoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACRoles sIBOACRoles = db.SIBOACRoles.Find(id);
            if (sIBOACRoles == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACRoles);
        }

        // POST: PRUEBARoles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Activo")] SIBOACRoles sIBOACRoles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sIBOACRoles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sIBOACRoles);
        }

        // GET: MantenimientoRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACRoles sIBOACRoles = db.SIBOACRoles.Find(id);
            if (sIBOACRoles == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACRoles);
        }

        // POST: MantenimientoRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACRoles sIBOACRoles = db.SIBOACRoles.Find(id);
            if (sIBOACRoles.Activo == false)

                sIBOACRoles.Activo = true;
            else
                sIBOACRoles.Activo = false;
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
