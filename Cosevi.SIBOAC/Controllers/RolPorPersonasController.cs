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
    public class RolPorPersonasController : BaseController<RolPorPersona>
    {
        // GET: RolPorPersonas
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.ROLPERSONA.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
        }

        // GET: RolPorPersonas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            if (rolPorPersona == null)
            {
                return HttpNotFound();
            }
            return View(rolPorPersona);
        }

        // GET: RolPorPersonas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolPorPersonas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] RolPorPersona rolPorPersona)
        {
            if (ModelState.IsValid)
            {
                db.ROLPERSONA.Add(rolPorPersona);
                string mensaje = Verificar(rolPorPersona.Id);

                if (mensaje == "")
                {
                    mensaje = ValidarFechas(rolPorPersona.FechaDeInicio, rolPorPersona.FechaDeFin);

                    if (mensaje == "")
                {
                        db.SaveChanges();
                        Bitacora(rolPorPersona, "I", "ROLPERSONA");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(rolPorPersona);
                }

                }

                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(rolPorPersona);
                }

            }

            return View(rolPorPersona);
        }
     
        // GET: RolPorPersonas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            if (rolPorPersona == null)
            {
                return HttpNotFound();
            }
            return View(rolPorPersona);
        }

        // POST: RolPorPersonas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] RolPorPersona rolPorPersona)
        {
            if (ModelState.IsValid)
            {
                var rolPorPersonaAntes = db.ROLPERSONA.AsNoTracking().Where(d => d.Id == rolPorPersona.Id).FirstOrDefault();

                db.Entry(rolPorPersona).State = EntityState.Modified;

                string mensaje = ValidarFechas(rolPorPersona.FechaDeInicio, rolPorPersona.FechaDeFin);
                if (mensaje == "")
                {

                    db.SaveChanges();
                    Bitacora(rolPorPersona, "U", "ROLPERSONA", rolPorPersonaAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(rolPorPersona);
                }
                }
            return View(rolPorPersona);
        }

        // GET: RolPorPersonas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            if (rolPorPersona == null)
            {
                return HttpNotFound();
            }
            return View(rolPorPersona);
        }

        // POST: RolPorPersonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            RolPorPersona rolPorPersonaAntes = ObtenerCopia(rolPorPersona);

            if (rolPorPersona.Estado == "I")
                rolPorPersona.Estado = "A";
            else
                rolPorPersona.Estado = "I";
            db.SaveChanges();
            Bitacora(rolPorPersona, "U", "ROLPERSONA", rolPorPersonaAntes);
            return RedirectToAction("Index");
        }

        // GET: RolPorPersonas/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            if (rolPorPersona == null)
            {
                return HttpNotFound();
            }
            return View(rolPorPersona);
        }
        
        // POST: RolPorPersonas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            RolPorPersona rolPorPersona = db.ROLPERSONA.Find(id);
            db.ROLPERSONA.Remove(rolPorPersona);
            db.SaveChanges();
            Bitacora(rolPorPersona, "D", "ROLPERSONA");
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

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.ROLPERSONA.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El código del rol de la persona " + id + " ya esta registrado";
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
    }
}
