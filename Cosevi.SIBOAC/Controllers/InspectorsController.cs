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
    public class InspectorsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Inspectors
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.INSPECTOR.ToList());
        }

        // GET: Inspectors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioSIBOAC inspector = db.INSPECTOR.Find(id);
            if (inspector == null)
            {
                return HttpNotFound();
            }
            return View(inspector);
        }

        // GET: Inspectors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inspectors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TipoDeIdentificacion,Identificacion,Nombre,Apellido1,Apellido2,Adonoren,FechaDeInclusion,FechaDeExclusion,DocumentoDeInclusion,DocumentoDeExclusion,FechaReag,DocumentoReag,CodigoDeDelegacion,Email,Estado,FechaDeInicio,FechaDeFin")] UsuarioSIBOAC inspector)
        {
            if (ModelState.IsValid)
            {
                db.INSPECTOR.Add(inspector);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inspector);
        }

        // GET: Inspectors/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioSIBOAC inspector = db.INSPECTOR.Find(id);
            if (inspector == null)
            {
                return HttpNotFound();
            }
            return View(inspector);
        }

        // POST: Inspectors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TipoDeIdentificacion,Identificacion,Nombre,Apellido1,Apellido2,Adonoren,FechaDeInclusion,FechaDeExclusion,DocumentoDeInclusion,DocumentoDeExclusion,FechaReag,DocumentoReag,CodigoDeDelegacion,Email,Estado,FechaDeInicio,FechaDeFin")] UsuarioSIBOAC inspector)
        {
            if (ModelState.IsValid)
            {
                               
                db.Entry(inspector).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inspector);
        }

        // GET: Inspectors/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioSIBOAC inspector = db.INSPECTOR.Find(id);
            if (inspector == null)
            {
                return HttpNotFound();
            }
            return View(inspector);
        }

        // POST: Inspectors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UsuarioSIBOAC inspector = db.INSPECTOR.Find(id);
            if (inspector.Estado == "A")
                inspector.Estado = "I";
            else
                inspector.Estado = "A";
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
