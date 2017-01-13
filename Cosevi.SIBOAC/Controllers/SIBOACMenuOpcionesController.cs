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
    public class SIBOACMenuOpcionesController : Controller
    {
        private SIBOACSecurityEntities db = new SIBOACSecurityEntities();

        // GET: SIBOACMenuOpciones
        public ActionResult Index()
        {
            return View(db.SIBOACMenuOpciones.OrderBy(a=> new { a.ParentID, a.Orden, a.Descripcion}).ToList());
        }

        // GET: SIBOACMenuOpciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACMenuOpciones sIBOACMenuOpciones = db.SIBOACMenuOpciones.Find(id);
            if (sIBOACMenuOpciones == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACMenuOpciones);
        }

        // GET: SIBOACMenuOpciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SIBOACMenuOpciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuOpcionesID,Descripcion,URL,Estado,ParentID,Orden")] SIBOACMenuOpciones sIBOACMenuOpciones)
        {
            if (ModelState.IsValid)
            {
                db.SIBOACMenuOpciones.Add(sIBOACMenuOpciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sIBOACMenuOpciones);
        }

        // GET: SIBOACMenuOpciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACMenuOpciones sIBOACMenuOpciones = db.SIBOACMenuOpciones.Find(id);
            if (sIBOACMenuOpciones == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACMenuOpciones);
        }

        // POST: SIBOACMenuOpciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuOpcionesID,Descripcion,URL,Estado,ParentID,Orden")] SIBOACMenuOpciones sIBOACMenuOpciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sIBOACMenuOpciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sIBOACMenuOpciones);
        }

        // GET: SIBOACMenuOpciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACMenuOpciones sIBOACMenuOpciones = db.SIBOACMenuOpciones.Find(id);
            if (sIBOACMenuOpciones == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACMenuOpciones);
        }

        // POST: SIBOACMenuOpciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACMenuOpciones sIBOACMenuOpciones = db.SIBOACMenuOpciones.Find(id);
            db.SIBOACMenuOpciones.Remove(sIBOACMenuOpciones);
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
