﻿using System;
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
    public class ObstaculoesController : BaseController<Obstaculo>
    {
        

        // GET: Obstaculoes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.Obstaculo.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.Obstaculo.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Obstaculoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // GET: Obstaculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Obstaculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Obstaculo obstaculo)
        {
            if (ModelState.IsValid)
            {
                db.Obstaculo.Add(obstaculo);
                string mensaje = Verificar(obstaculo.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(obstaculo, "I", "OBSTACULO");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(obstaculo);
                }
            }

            return View(obstaculo);
        }

        // GET: Obstaculoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // POST: Obstaculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Obstaculo obstaculo)
        {
            if (ModelState.IsValid)
            {
                var obstaculoAntes = db.Obstaculo.AsNoTracking().Where(d => d.Id == obstaculo.Id).FirstOrDefault();
                db.Entry(obstaculo).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(obstaculo, "U", "OBSTACULO",  obstaculoAntes);
                return RedirectToAction("Index");
            }
            return View(obstaculo);
        }

        // GET: Obstaculoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // POST: Obstaculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            Obstaculo obstaculoAntes = ObtenerCopia(obstaculo);
            if (obstaculo.Estado == "I")
                obstaculo.Estado = "A";
            else
                obstaculo.Estado = "I";
            db.SaveChanges();
            Bitacora(obstaculo, "U", "OBSTACULO", obstaculoAntes);
            return RedirectToAction("Index");
        }

        // GET: Obstaculoes/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // POST: Obstaculoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            db.Obstaculo.Remove(obstaculo);
            db.SaveChanges();
            Bitacora(obstaculo, "D", "OBSTACULO");
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
