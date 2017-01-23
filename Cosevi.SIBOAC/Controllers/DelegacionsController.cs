using System;
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
    public class DelegacionsController : BaseController<Delegacion>
    {

        // GET: Delegacions
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var lsit = db.DELEGACION.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(lsit.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.DELEGACION.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
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
        // GET: Delegacions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delegacion delegacion = db.DELEGACION.Find(id);
            if (delegacion == null)
            {
                return HttpNotFound();
            }
            return View(delegacion);
        }

        // GET: Delegacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Delegacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Delegacion delegacion)
        {
            if (ModelState.IsValid)
            {
                db.DELEGACION.Add(delegacion);
                string mensaje = Verificar(delegacion.Id);

                if (mensaje == "")
                {
                    mensaje = ValidarFechas(delegacion.FechaDeInicio, delegacion.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(delegacion, "I", "DELEGACION");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(delegacion);
                    }

                    db.SaveChanges();
                    Bitacora(delegacion, "I", "DELEGACION");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(delegacion);
                }
            }

            return View(delegacion);
        }

        // GET: Delegacions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delegacion delegacion = db.DELEGACION.Find(id);
            if (delegacion == null)
            {
                return HttpNotFound();
            }
            return View(delegacion);
        }

        // POST: Delegacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Delegacion delegacion)
        {
            if (ModelState.IsValid)
            {
                var delegacionAntes = db.DELEGACION.AsNoTracking().Where(d => d.Id == delegacion.Id).FirstOrDefault();
                string mensaje = ValidarFechas(delegacionAntes.FechaDeInicio, delegacionAntes.FechaDeFin);
                if (mensaje == "")
                {
                    db.Entry(delegacion).State = EntityState.Modified;
                    db.SaveChanges();
                    Bitacora(delegacion, "U", "DELEGACION", delegacionAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(delegacion);
                }
            }
            return View(delegacion);
        }

        // GET: Delegacions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delegacion delegacion = db.DELEGACION.Find(id);
            if (delegacion == null)
            {
                return HttpNotFound();
            }
            return View(delegacion);
        }

        // POST: Delegacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Delegacion delegacion = db.DELEGACION.Find(id);
            Delegacion delegacionAntes = ObtenerCopia(delegacion);
            if (delegacion.Estado == "I")
                delegacion.Estado = "A";
            else
                delegacion.Estado = "I";
            db.SaveChanges();
            Bitacora(delegacion, "U", "DELEGACION", delegacionAntes);
            return RedirectToAction("Index");
        }


        // GET: Delegacions/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delegacion delegacion = db.DELEGACION.Find(id);
            if (delegacion == null)
            {
                return HttpNotFound();
            }
            return View(delegacion);
        }

        // POST: Delegacions/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            Delegacion delegacion = db.DELEGACION.Find(id);
            db.DELEGACION.Remove(delegacion);
            db.SaveChanges();
            Bitacora(delegacion, "D", "DELEGACION");
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
