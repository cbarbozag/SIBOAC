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
    public class TiposPorDocumentoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TiposPorDocumentoes
        public ActionResult Index()
        {
            return View(db.TIPOSXDOCUMENTO.ToList());
        }

        // GET: TiposPorDocumentoes/Details/5
        public ActionResult Details(string codigod,string codigot)
        {
            if (codigod== null || codigot == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod,codigot);
            if (tiposPorDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tiposPorDocumento);
        }

        // GET: TiposPorDocumentoes/Create
        public ActionResult Create()
        {

            //se llenan los combos
            IEnumerable<SelectListItem> itemsTipoDocumento = db.TIPODOCUMENTO
              .Select(o => new SelectListItem
              {
                  Value = o.Id,
                  Text = o.Descripcion
              });
            ViewBag.ComboTipoDocumento = itemsTipoDocumento;

            IEnumerable<SelectListItem> itemsTipoIdentificacion = db.TIPO_IDENTIFICACION
             .Select(c => new SelectListItem
             {
                 Value = c.Id.ToString(),
                 Text = c.Descripcion
             });
            ViewBag.ComboTipoIdentificacion = itemsTipoIdentificacion;
            return View();
        }

        // POST: TiposPorDocumentoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTipoDocumento,CodigoTipoDeIdentificacion,Estado,FechaDeInicio,FechaDeFin")] TiposPorDocumento tiposPorDocumento)
        {
            if (ModelState.IsValid)
            {
                db.TIPOSXDOCUMENTO.Add(tiposPorDocumento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tiposPorDocumento);
        }

        // GET: TiposPorDocumentoes/Edit/5
        public ActionResult Edit(string codigod, string codigot)
        {
            if (codigod == null || codigot == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod, codigot);
            if (tiposPorDocumento == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboTipoDocumento = new SelectList(db.TIPODOCUMENTO.OrderBy(x => x.Descripcion), "Id", "Descripcion", codigod);
            ViewBag.ComboTipoIdentificacion = new SelectList(db.TIPO_IDENTIFICACION.OrderBy(x => x.Descripcion), "Id", "Descripcion", codigot);


            return View(tiposPorDocumento);
        }

        // POST: TiposPorDocumentoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTipoDocumento,CodigoTipoDeIdentificacion,Estado,FechaDeInicio,FechaDeFin")] TiposPorDocumento tiposPorDocumento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiposPorDocumento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiposPorDocumento);
        }

        // GET: TiposPorDocumentoes/Delete/5
        public ActionResult Delete(string codigod, string codigot)
        {
            if (codigod == null || codigot == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod, codigot);
            if (tiposPorDocumento == null)
            {
                return HttpNotFound();
            }
            return View(tiposPorDocumento);
        }

        // POST: TiposPorDocumentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codigod, string codigot)
        {
            TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod, codigot);
            if (tiposPorDocumento.Estado == "A")
                tiposPorDocumento.Estado = "I";
            else
                tiposPorDocumento.Estado = "A";
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
