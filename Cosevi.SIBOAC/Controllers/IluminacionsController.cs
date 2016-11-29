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
    public class IluminacionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Iluminacions
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.Iluminacion.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.Iluminacion.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Iluminacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iluminacion iluminacion = db.Iluminacion.Find(id);
            if (iluminacion == null)
            {
                return HttpNotFound();
            }
            return View(iluminacion);
        }

        // GET: Iluminacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Iluminacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Iluminacion iluminacion)
        {
            if (ModelState.IsValid)
            {
                db.Iluminacion.Add(iluminacion);
                string mensaje = Verificar(iluminacion.Id);
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
                    return View(iluminacion);
                }
            }

            return View(iluminacion);
        }

        // GET: Iluminacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iluminacion iluminacion = db.Iluminacion.Find(id);
            if (iluminacion == null)
            {
                return HttpNotFound();
            }
            return View(iluminacion);
        }

        // POST: Iluminacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Iluminacion iluminacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iluminacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iluminacion);
        }

        // GET: Iluminacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iluminacion iluminacion = db.Iluminacion.Find(id);
            if (iluminacion == null)
            {
                return HttpNotFound();
            }
            return View(iluminacion);
        }

        // POST: Iluminacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Iluminacion iluminacion = db.Iluminacion.Find(id);
            if (iluminacion.Estado == "I")
                iluminacion.Estado = "A";
            else
                iluminacion.Estado = "I";
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
