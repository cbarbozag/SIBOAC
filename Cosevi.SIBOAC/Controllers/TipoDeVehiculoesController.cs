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
    public class TipoDeVehiculoesController : BaseController<TipoDeVehiculo>
    {
         // GET: TipoDeVehiculoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.TIPOVEH.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }



        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.TIPOVEH.Any(x => x.Id == id);
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

        // GET: TipoDeVehiculoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeVehiculo tipoDeVehiculo = db.TIPOVEH.Find(id);
            if (tipoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeVehiculo);
        }

        // GET: TipoDeVehiculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeVehiculo tipoDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.TIPOVEH.Add(tipoDeVehiculo);
                string mensaje = Verificar(tipoDeVehiculo.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(tipoDeVehiculo.FechaDeInicio,tipoDeVehiculo.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(tipoDeVehiculo, "I", "TIPOVEH");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(tipoDeVehiculo);
                    }

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoDeVehiculo);
                }
            }

            return View(tipoDeVehiculo);
        }

        // GET: TipoDeVehiculoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeVehiculo tipoDeVehiculo = db.TIPOVEH.Find(id);
            if (tipoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeVehiculo);
        }

        // POST: TipoDeVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeVehiculo tipoDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                var tipoDeVehiculoAntes = db.TIPOVEH.AsNoTracking().Where(d => d.Id == tipoDeVehiculo.Id).FirstOrDefault();

                db.Entry(tipoDeVehiculo).State = EntityState.Modified;
                string mensaje = ValidarFechas(tipoDeVehiculo.FechaDeInicio, tipoDeVehiculo.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(tipoDeVehiculo, "U", "TIPOVEH", tipoDeVehiculoAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {                
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoDeVehiculo);                    
                }
            }
            return View(tipoDeVehiculo);
        }

        // GET: TipoDeVehiculoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeVehiculo tipoDeVehiculo = db.TIPOVEH.Find(id);
            if (tipoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeVehiculo);
        }

        // POST: TipoDeVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDeVehiculo tipoDeVehiculo = db.TIPOVEH.Find(id);
            TipoDeVehiculo tipoDeVehiculoAntes = ObtenerCopia(tipoDeVehiculo);
            if (tipoDeVehiculo.Estado == "I")
                tipoDeVehiculo.Estado = "A";
            else
                tipoDeVehiculo.Estado = "I";
            db.SaveChanges();
            Bitacora(tipoDeVehiculo, "U", "TIPOVEH", tipoDeVehiculoAntes);
            return RedirectToAction("Index");
        }

        // GET: TipoDeVehiculoes/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeVehiculo tipoDeVehiculo = db.TIPOVEH.Find(id);
            if (tipoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeVehiculo);
        }

        // POST: TipoDeVehiculoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            TipoDeVehiculo tipoDeVehiculo = db.TIPOVEH.Find(id);
            db.TIPOVEH.Remove(tipoDeVehiculo);
            db.SaveChanges();
            Bitacora(tipoDeVehiculo, "D", "TIPOVEH");
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
