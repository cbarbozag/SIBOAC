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
    public class DetallePorTipoDaniosController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DetallePorTipoDanios
        public ActionResult Index()
        {
            return View(db.DETALLETIPODAÑO.ToList());
        }

        // GET: DetallePorTipoDanios/Details/5
        public ActionResult Details(string codigod, string codigotd )
        {
            if (codigod == null  || codigotd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);
            if (detallePorTipoDanio == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoDanio);
        }

        // GET: DetallePorTipoDanios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DetallePorTipoDanios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoDanio,CodigoTipoDanio,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoDanio detallePorTipoDanio)
        {
            if (ModelState.IsValid)
            {
                db.DETALLETIPODAÑO.Add(detallePorTipoDanio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(detallePorTipoDanio);
        }

        // GET: DetallePorTipoDanios/Edit/5
        public ActionResult Edit(string codigod, string codigotd)
        {
            if (codigod == null || codigotd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);
            if (detallePorTipoDanio == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoDanio);
        }

        // POST: DetallePorTipoDanios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoDanio,CodigoTipoDanio,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoDanio detallePorTipoDanio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallePorTipoDanio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(detallePorTipoDanio);
        }

        // GET: DetallePorTipoDanios/Delete/5
        public ActionResult Delete(string codigod, string codigotd)
        {
            if (codigod == null || codigotd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);
            if (detallePorTipoDanio == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoDanio);
        }

        // POST: DetallePorTipoDanios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codigod, string codigotd)
        {
            DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);
            db.DETALLETIPODAÑO.Remove(detallePorTipoDanio);
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
