using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;
using PagedList;

namespace Cosevi.SIBOAC.Controllers
{
    public class SIBOACUsuariosController : Controller
    {
        private SIBOACSecurityEntities db = new SIBOACSecurityEntities();

        // GET: SIBOACUsuarios
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.SIBOACUsuarios.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: SIBOACUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACUsuarios);
        }

        // GET: SIBOACUsuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SIBOACUsuarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,Contrasena,Nombre,Usuario")] SIBOACUsuarios sIBOACUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.SIBOACUsuarios.Add(sIBOACUsuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sIBOACUsuarios);
        }

        // GET: SIBOACUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACUsuarios);
        }

        // POST: SIBOACUsuarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Contrasena,Nombre,Usuario")] SIBOACUsuarios sIBOACUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sIBOACUsuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sIBOACUsuarios);
        }

        // GET: SIBOACUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACUsuarios);
        }

        // POST: SIBOACUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            db.SIBOACUsuarios.Remove(sIBOACUsuarios);
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
