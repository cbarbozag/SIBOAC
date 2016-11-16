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
    public class TipoDeDocumentoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoDeDocumentoes
        public ActionResult Index()
        {
            return View(db.TIPODOCUMENTO.ToList());
        }

        // GET: TipoDeDocumentoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeDocumento tipoDeDocumento = db.TIPODOCUMENTO.Find(id);
            if (tipoDeDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeDocumento);
        }

        // GET: TipoDeDocumentoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeDocumentoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeDocumento tipoDeDocumento)
        {
            if (ModelState.IsValid)
            {
                db.TIPODOCUMENTO.Add(tipoDeDocumento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDeDocumento);
        }

        // GET: TipoDeDocumentoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeDocumento tipoDeDocumento = db.TIPODOCUMENTO.Find(id);
            if (tipoDeDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeDocumento);
        }

        // POST: TipoDeDocumentoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeDocumento tipoDeDocumento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeDocumento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDeDocumento);
        }

        // GET: TipoDeDocumentoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeDocumento tipoDeDocumento = db.TIPODOCUMENTO.Find(id);
            if (tipoDeDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeDocumento);
        }

        // POST: TipoDeDocumentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TipoDeDocumento tipoDeDocumento = db.TIPODOCUMENTO.Find(id);
            db.TIPODOCUMENTO.Remove(tipoDeDocumento);
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
