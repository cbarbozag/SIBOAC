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
    public class EstadoCivilsController : BaseController<EstadoCivil>
    {
        

        // GET: EstadoCivils
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.EstadoCivil.ToList());
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.EstadoCivil.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: EstadoCivils/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoCivil estadoCivil = db.EstadoCivil.Find(id);
            if (estadoCivil == null)
            {
                return HttpNotFound();
            }
            return View(estadoCivil);
        }

        // GET: EstadoCivils/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstadoCivils/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] EstadoCivil estadoCivil)
        {
            if (ModelState.IsValid)
            {
                db.EstadoCivil.Add(estadoCivil);
                string mensaje = Verificar(estadoCivil.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(estadoCivil, "I");

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(estadoCivil);
                }
            }

            return View(estadoCivil);
        }

        // GET: EstadoCivils/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoCivil estadoCivil = db.EstadoCivil.Find(id);
            if (estadoCivil == null)
            {
                return HttpNotFound();
            }
            return View(estadoCivil);
        }

        // POST: EstadoCivils/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] EstadoCivil estadoCivil)
        {
            if (ModelState.IsValid)
            {
                var estadoCivilAntes = db.EstadoCivil.AsNoTracking().Where(d => d.Id == estadoCivil.Id).FirstOrDefault();
                db.Entry(estadoCivil).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(estadoCivil, "U", estadoCivilAntes);
                return RedirectToAction("Index");
            }
            return View(estadoCivil);
        }

        // GET: EstadoCivils/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoCivil estadoCivil = db.EstadoCivil.Find(id);
            if (estadoCivil == null)
            {
                return HttpNotFound();
            }
            return View(estadoCivil);
        }

        // POST: EstadoCivils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            EstadoCivil estadoCivil = db.EstadoCivil.Find(id);
            EstadoCivil estadoCivilAntes = ObtenerCopia(estadoCivil);
            if (estadoCivil.Estado == "I")
                estadoCivil.Estado = "A";
            else
                estadoCivil.Estado = "I";
            db.SaveChanges();
            Bitacora(estadoCivil, "U", estadoCivilAntes);
            return RedirectToAction("Index");
        }

        // GET: EstadoCivils/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoCivil estadoCivil = db.EstadoCivil.Find(id);
            if (estadoCivil == null)
            {
                return HttpNotFound();
            }
            return View(estadoCivil);
        }

        // POST: EstadoCivils/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            EstadoCivil estadoCivil = db.EstadoCivil.Find(id);
            db.EstadoCivil.Remove(estadoCivil);
            db.SaveChanges();
            Bitacora(estadoCivil, "D");
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
