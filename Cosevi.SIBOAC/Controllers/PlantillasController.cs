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
    public class PlantillasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Plantillas
        public ActionResult Index()
        {
            return View(db.PLANTILLAS.ToList());
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
                db.SaveChanges();
                return RedirectToAction("Index");
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
                db.Entry(plantillas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            plantillas.Estado = "I";
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
