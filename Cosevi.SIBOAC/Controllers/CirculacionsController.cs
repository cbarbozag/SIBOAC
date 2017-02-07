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
    public class CirculacionsController : BaseController<Circulacion>
    {

        // GET: Circulacions
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";           

            var list = db.CIRCULACION.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.CIRCULACION.Any(x => x.Id == id);
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
        // GET: Circulacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Circulacion circulacion = db.CIRCULACION.Find(id);
            if (circulacion == null)
            {
                return HttpNotFound();
            }
            return View(circulacion);
        }

        // GET: Circulacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Circulacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Circulacion circulacion)
        {
            if (ModelState.IsValid)
            {
                db.CIRCULACION.Add(circulacion);
                string mensaje = Verificar(circulacion.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(circulacion.FechaDeInicio, circulacion.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(circulacion, "I", "CIRCULACION");

                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(circulacion);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(circulacion);
                }
            }

            return View(circulacion);
        }

        // GET: Circulacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Circulacion circulacion = db.CIRCULACION.Find(id);
            if (circulacion == null)
            {
                return HttpNotFound();
            }
            return View(circulacion);
        }

        // POST: Circulacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Circulacion circulacion)
        {
            if (ModelState.IsValid)
            {
                var circulacionAntes = db.CIRCULACION.AsNoTracking().Where(d => d.Id == circulacion.Id).FirstOrDefault();

                db.Entry(circulacion).State = EntityState.Modified;
                string mensaje = ValidarFechas(circulacion.FechaDeInicio, circulacion.FechaDeFin);

                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(circulacion, "U", "CIRCULACION", circulacionAntes);
                    TempData["Type"] = "success";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(circulacion);
                }

            }
            return View(circulacion);
        }

        // GET: Circulacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Circulacion circulacion = db.CIRCULACION.Find(id);
            if (circulacion == null)
            {
                return HttpNotFound();
            }
            return View(circulacion);
        }

        // POST: Circulacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Circulacion circulacion = db.CIRCULACION.Find(id);
            Circulacion circulacionAntes = ObtenerCopia(circulacion);

            if (circulacion.Estado == "A")
                circulacion.Estado = "I";
            else
                circulacion.Estado = "A";
            db.SaveChanges();
            Bitacora(circulacion, "U", "CIRCULACION", circulacionAntes);

            return RedirectToAction("Index");
        }


        // GET: Circulacions/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Circulacion circulacion = db.CIRCULACION.Find(id);
            if (circulacion == null)
            {
                return HttpNotFound();
            }
            return View(circulacion);
        }

        // POST: Circulacions/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Circulacion circulacion = db.CIRCULACION.Find(id);
            db.CIRCULACION.Remove(circulacion);
            db.SaveChanges();
            Bitacora(circulacion, "D", "CIRCULACION");
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
