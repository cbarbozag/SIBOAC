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
    public class InspectorsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Inspectors
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
                        
            var list = db.INSPECTOR.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Inspectors/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inspector inspector = db.INSPECTOR.Find(id);
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
        public ActionResult Create([Bind(Include = "Id,TipoDeIdentificacion,Identificacion,Nombre,Apellido1,Apellido2,Adonoren,FechaDeInclusion,FechaDeExclusion,DocumentoDeInclusion,DocumentoDeExclusion,FechaReag,DocumentoReag,CodigoDeDelegacion,Email,Estado,FechaDeInicio,FechaDeFin")] Inspector inspector)
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
            Inspector inspector = db.INSPECTOR.Find(id);
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
        public ActionResult Edit([Bind(Include = "Id,TipoDeIdentificacion,Identificacion,Nombre,Apellido1,Apellido2,Adonoren,FechaDeInclusion,FechaDeExclusion,DocumentoDeInclusion,DocumentoDeExclusion,FechaReag,DocumentoReag,CodigoDeDelegacion,Email,Estado,FechaDeInicio,FechaDeFin")] Inspector inspector)
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
            Inspector inspector = db.INSPECTOR.Find(id);
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
            Inspector inspector = db.INSPECTOR.Find(id);
            if (inspector.Estado == "A")
                inspector.Estado = "I";
            else
                inspector.Estado = "A";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Inspectors/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inspector inspector = db.INSPECTOR.Find(id);
            if (inspector == null)
            {
                return HttpNotFound();
            }
            return View(inspector);
        }

        // POST: Inspectors/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            Inspector inspector = db.INSPECTOR.Find(id);
            db.INSPECTOR.Remove(inspector);
            db.SaveChanges();
            TempData["Type"] = "error";
            TempData["Message"] = "El registro se eliminó correctamente";
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
