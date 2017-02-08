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
    public class PlantillasController : BaseController<Plantillas>
    {


        // GET: Plantillas
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";           

            var list = db.PLANTILLAS.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.PLANTILLAS.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
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
        // GET: Plantillas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plantillas plantillas = db.PLANTILLAS.Find(id);
            if (plantillas == null)
            {
                return HttpNotFound();
            }
            return View(plantillas);
        }

        // GET: Plantillas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Plantillas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Plantillas plantillas)
        {
            if (ModelState.IsValid)
            {
                db.PLANTILLAS.Add(plantillas);
                string mensaje = Verificar(plantillas.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(plantillas.FechaDeInicio, plantillas.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(plantillas, "I", "PLANTILLAS");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(plantillas);
                    }                 
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(plantillas);
                }
            }

            return View(plantillas);
        }

        // GET: Plantillas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plantillas plantillas = db.PLANTILLAS.Find(id);
            if (plantillas == null)
            {
                return HttpNotFound();
            }
            return View(plantillas);
        }

        // POST: Plantillas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Plantillas plantillas)
        {
            if (ModelState.IsValid)
            {
                var plantillasAntes = db.PLANTILLAS.AsNoTracking().Where(d => d.Id == plantillas.Id).FirstOrDefault();
                db.Entry(plantillas).State = EntityState.Modified;
                string mensaje = ValidarFechas(plantillas.FechaDeInicio, plantillas.FechaDeFin);
                if(mensaje=="")
                {
                    db.SaveChanges();
                    Bitacora(plantillas, "U", "PLANTILLAS", plantillasAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(plantillas);
                }
               
            }
            return View(plantillas);
        }

        // GET: Plantillas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plantillas plantillas = db.PLANTILLAS.Find(id);
            if (plantillas == null)
            {
                return HttpNotFound();
            }
            return View(plantillas);
        }

        // POST: Plantillas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Plantillas plantillas = db.PLANTILLAS.Find(id);
            Plantillas plantillasAntes = ObtenerCopia(plantillas);
            if (plantillas.Estado == "I")
                plantillas.Estado = "A";
            else
                plantillas.Estado = "I";
            db.SaveChanges();
            Bitacora(plantillas, "U", "PLANTILLAS", plantillasAntes);
            return RedirectToAction("Index");
        }

        // GET: Plantillas/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plantillas plantillas = db.PLANTILLAS.Find(id);
            if (plantillas == null)
            {
                return HttpNotFound();
            }
            return View(plantillas);
        }

        // POST: Plantillas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            Plantillas plantillas = db.PLANTILLAS.Find(id);
            db.PLANTILLAS.Remove(plantillas);
            db.SaveChanges();
            Bitacora(plantillas, "D", "PLANTILLAS");
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
