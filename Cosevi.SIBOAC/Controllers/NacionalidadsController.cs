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
    public class NacionalidadsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Nacionalidads
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.NACIONALIDAD.ToList());
        }

        public string Verificar(string id, DateTime FechaInicio, DateTime FechaFin)
        {
            string mensaje = "";
            bool exist = this.db.NACIONALIDAD.Any( x => x.Id == id && x.FechaDeInicio == FechaInicio && x.FechaDeFin == FechaFin);

            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";

            }

            return mensaje;

        }

        // GET: Nacionalidads/Details/5
        public ActionResult Details(string id, DateTime FechaInicio, DateTime FechaFin)
        {
            if (id == null || FechaFin == null || FechaFin== null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nacionalidad nacionalidad = db.NACIONALIDAD.Find(id, FechaInicio, FechaFin);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        // GET: Nacionalidads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Nacionalidads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin,Prioridad")] Nacionalidad nacionalidad)
        {
            if (ModelState.IsValid)
            {
                db.NACIONALIDAD.Add(nacionalidad);
                string mensaje = Verificar(nacionalidad.Id, nacionalidad.FechaDeInicio, nacionalidad.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(nacionalidad);
                }
            }

            return View(nacionalidad);
        }

        // GET: Nacionalidads/Edit/5
        public ActionResult Edit(string id, DateTime FechaInicio, DateTime FechaFin)
        {
            if (id == null || FechaFin == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nacionalidad nacionalidad = db.NACIONALIDAD.Find(id, FechaInicio, FechaFin);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        // POST: Nacionalidads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin,Prioridad")] Nacionalidad nacionalidad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nacionalidad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nacionalidad);
        }

        // GET: Nacionalidads/Delete/5
        public ActionResult Delete(string id, DateTime FechaInicio, DateTime FechaFin)
        {
            if (id == null || FechaFin == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nacionalidad nacionalidad = db.NACIONALIDAD.Find(id, FechaInicio, FechaFin);
            if (nacionalidad == null)
            {
                return HttpNotFound();
            }
            return View(nacionalidad);
        }

        // POST: Nacionalidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, DateTime FechaInicio, DateTime FechaFin)
        {
            Nacionalidad nacionalidad = db.NACIONALIDAD.Find(id, FechaInicio, FechaFin);
            if (nacionalidad.Estado == "A")
                nacionalidad.Estado = "I";
            else
                nacionalidad.Estado = "A";
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
