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
    public class ClaseDePlacasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: ClaseDePlacas
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.CLASE.ToList());
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.CLASE.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: ClaseDePlacas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            if (claseDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(claseDePlaca);
        }

        // GET: ClaseDePlacas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClaseDePlacas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] ClaseDePlaca claseDePlaca)
        {
            if (ModelState.IsValid)
            {
                db.CLASE.Add(claseDePlaca);
                string mensaje = Verificar(claseDePlaca.Id);
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
                    return View(claseDePlaca);
                }
            }

            return View(claseDePlaca);
        }

        // GET: ClaseDePlacas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            if (claseDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(claseDePlaca);
        }

        // POST: ClaseDePlacas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Clasedeplaca,estado,fecha_inicio,fecha_fin")] ClaseDePlaca claseDePlaca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claseDePlaca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(claseDePlaca);
        }

        // GET: ClaseDePlacas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            if (claseDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(claseDePlaca);
        }

        // POST: ClaseDePlacas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            if (claseDePlaca.Estado == "A")
                claseDePlaca.Estado = "I";
            else
                claseDePlaca.Estado = "A";
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
