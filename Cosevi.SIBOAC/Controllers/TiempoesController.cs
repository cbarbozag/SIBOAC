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
    public class TiempoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Tiempoes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.Tiempo.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.Tiempo.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Tiempoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tiempo tiempo = db.Tiempo.Find(id);
            if (tiempo == null)
            {
                return HttpNotFound();
            }
            return View(tiempo);
        }

        // GET: Tiempoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tiempoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Tiempo tiempo)
        {
            if (ModelState.IsValid)
            {
                db.Tiempo.Add(tiempo);
                string mensaje = Verificar(tiempo.Id);
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
                    return View(tiempo);
                }
            }

            return View(tiempo);
        }

        // GET: Tiempoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tiempo tiempo = db.Tiempo.Find(id);
            if (tiempo == null)
            {
                return HttpNotFound();
            }
            return View(tiempo);
        }

        // POST: Tiempoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Tiempo tiempo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tiempo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tiempo);
        }

        // GET: Tiempoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tiempo tiempo = db.Tiempo.Find(id);
            if (tiempo == null)
            {
                return HttpNotFound();
            }
            return View(tiempo);
        }

        // POST: Tiempoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tiempo tiempo = db.Tiempo.Find(id);
            if (tiempo.Estado == "I")
                tiempo.Estado = "A";
            else
                tiempo.Estado = "I";
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
