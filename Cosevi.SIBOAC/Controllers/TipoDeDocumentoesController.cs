﻿using System;
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
    public class TipoDeDocumentoesController : BaseController<TipoDeDocumento>
    {
        // GET: TipoDeDocumentoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.TIPODOCUMENTO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.TIPODOCUMENTO.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El código " + id + " ya esta registrado";
            }
            return mensaje;
        }
        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
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
                string mensaje = Verificar(tipoDeDocumento.Id);
                if (mensaje == "")
                {

                    mensaje = ValidarFechas(tipoDeDocumento.FechaDeInicio.Value, tipoDeDocumento.FechaDeFin.Value);
                    if (mensaje == "")
                    {

                        db.SaveChanges();
                        Bitacora(tipoDeDocumento, "I", "TIPODOCUMENTO");

                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(tipoDeDocumento);
                    }

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoDeDocumento);
                }
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
                var tipoDeDocumentoAntes = db.TIPODOCUMENTO.AsNoTracking().Where(d => d.Id == tipoDeDocumento.Id).FirstOrDefault();

                db.Entry(tipoDeDocumento).State = EntityState.Modified;

                string mensaje = ValidarFechas(tipoDeDocumento.FechaDeInicio.Value, tipoDeDocumento.FechaDeFin.Value);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(tipoDeDocumento, "U", "TIPODOCUMENTO", tipoDeDocumentoAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoDeDocumento);
                }
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
            TipoDeDocumento tipoDeDocumentoAntes = ObtenerCopia(tipoDeDocumento);
            if (tipoDeDocumento.Estado == "I")
                tipoDeDocumento.Estado = "A";
            else
                tipoDeDocumento.Estado = "I";
            db.SaveChanges();
            Bitacora(tipoDeDocumento, "U", "TIPODOCUMENTO", tipoDeDocumentoAntes);
            return RedirectToAction("Index");
        }

        // GET: TipoDeDocumentoes/RealDelete/5
        public ActionResult RealDelete(string id)
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

        // POST: TipoDeDocumentoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            TipoDeDocumento tipoDeDocumento = db.TIPODOCUMENTO.Find(id);
            db.TIPODOCUMENTO.Remove(tipoDeDocumento);
            db.SaveChanges();
            Bitacora(tipoDeDocumento, "D", "TIPODOCUMENTO");
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
