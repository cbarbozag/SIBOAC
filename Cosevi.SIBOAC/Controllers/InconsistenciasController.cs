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
    public class InconsistenciasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Inconsistencias
        public ActionResult Index()
        {
            return View(db.INCONSISTENCIA.ToList());
        }

        // GET: Inconsistencias/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inconsistencia inconsistencia = db.INCONSISTENCIA.Find(id);
            if (inconsistencia == null)
            {
                return HttpNotFound();
            }
            return View(inconsistencia);
        }

        // GET: Inconsistencias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inconsistencias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] Inconsistencia inconsistencia)
        {
            if (ModelState.IsValid)
            {
                db.INCONSISTENCIA.Add(inconsistencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inconsistencia);
        }

        // GET: Inconsistencias/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inconsistencia inconsistencia = db.INCONSISTENCIA.Find(id);
            if (inconsistencia == null)
            {
                return HttpNotFound();
            }
            return View(inconsistencia);
        }

        // POST: Inconsistencias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] Inconsistencia inconsistencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inconsistencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inconsistencia);
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
