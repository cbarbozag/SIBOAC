﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;
using Cosevi.SIBOAC.Security;
using PagedList;

namespace Cosevi.SIBOAC.Controllers
{
    public class AlineacionHorizontalsController : BaseController<AlineacionHorizontal>
    {
        // GET: AlineacionHorizontals
        [SessionExpire]
        public ActionResult Index(int ? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list = db.ALINHORI.ToList();
    

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));

        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.ALINHORI.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
           
            return mensaje;
        }

        // GET: AlineacionHorizontals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            if (alineacionHorizontal == null)
            {
                return HttpNotFound();
            }
            return View(alineacionHorizontal);
        }

        // GET: AlineacionHorizontals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlineacionHorizontals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] AlineacionHorizontal alineacionHorizontal)
        {
            if (ModelState.IsValid)
            {
                db.ALINHORI.Add(alineacionHorizontal);
                string mensaje = Verificar(alineacionHorizontal.Id);
                if(mensaje =="")
                {
                    mensaje = ValidarFechas(alineacionHorizontal.FechaDeInicio, alineacionHorizontal.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(alineacionHorizontal, "I", "ALINHORI");

                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(alineacionHorizontal);
                    }                   

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(alineacionHorizontal);
                }
                
            }

            return View(alineacionHorizontal);
        }

        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
        }
        // GET: AlineacionHorizontals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            if (alineacionHorizontal == null)
            {
                return HttpNotFound();
            }
            return View(alineacionHorizontal);
        }

        // POST: AlineacionHorizontals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] AlineacionHorizontal alineacionHorizontal)
        {
            if (ModelState.IsValid)
            {
                var alineacionHorizontalAntes = db.ALINHORI.AsNoTracking().Where(d => d.Id == alineacionHorizontal.Id).FirstOrDefault();

                db.Entry(alineacionHorizontal).State = EntityState.Modified;
                string  mensaje = ValidarFechas(alineacionHorizontal.FechaDeInicio, alineacionHorizontal.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(alineacionHorizontal, "U", "ALINHORI", alineacionHorizontalAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(alineacionHorizontal);
                }              
            }
            return View(alineacionHorizontal);
        }

        // GET: AlineacionHorizontals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            if (alineacionHorizontal == null)
            {
                return HttpNotFound();
            }
            return View(alineacionHorizontal);
        }

        // POST: AlineacionHorizontals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            AlineacionHorizontal alineacionHorizontalAntes = ObtenerCopia(alineacionHorizontal);
            if (alineacionHorizontal.Estado=="A")
                alineacionHorizontal.Estado = "I";
            else
                alineacionHorizontal.Estado = "A";
            db.SaveChanges();
            Bitacora(alineacionHorizontal, "U", "ALINHORI", alineacionHorizontalAntes);
            return RedirectToAction("Index");
        }
        // GET: AlineacionHorizontals/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            if (alineacionHorizontal == null)
            {
                return HttpNotFound();
            }
            return View(alineacionHorizontal);
        }

        // POST: AlineacionHorizontals/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            db.ALINHORI.Remove(alineacionHorizontal);
            db.SaveChanges();
            Bitacora(alineacionHorizontal, "D", "ALINHORI");
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
