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
    public class DetallePorTipoSenialsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DetallePorTipoSenials
        public ActionResult Index()
        {
            return View(db.DETALLETIPOSEÑAL.ToList());
        }

        // GET: DetallePorTipoSenials/Details/5
        public ActionResult Details(string codigose, string codsenex )
        {
            if (codigose == null || codsenex == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codigose, codsenex);
            if (detallePorTipoSenial == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoSenial);
        }

        // GET: DetallePorTipoSenials/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DetallePorTipoSenials/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTipoSenial,Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoSenial detallePorTipoSenial)
        {
            if (ModelState.IsValid)
            {
                db.DETALLETIPOSEÑAL.Add(detallePorTipoSenial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(detallePorTipoSenial);
        }

        // GET: DetallePorTipoSenials/Edit/5
        public ActionResult Edit(string codigose, string codsenex)
        {
            if (codigose == null || codsenex == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codigose, codsenex);
            if (detallePorTipoSenial == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoSenial);
        }

        // POST: DetallePorTipoSenials/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTipoSenial,Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoSenial detallePorTipoSenial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallePorTipoSenial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(detallePorTipoSenial);
        }

        // GET: DetallePorTipoSenials/Delete/5
        public ActionResult Delete(string codigose, string codsenex)
        {
            if (codigose == null || codsenex == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codigose, codsenex);
            if (detallePorTipoSenial == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoSenial);
        }

        // POST: DetallePorTipoSenials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codigose, string codsenex)
        {
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codigose, codsenex);
            db.DETALLETIPOSEÑAL.Remove(detallePorTipoSenial);
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
