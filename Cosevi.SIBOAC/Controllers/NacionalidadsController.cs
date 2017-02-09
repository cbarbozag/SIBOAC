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
    public class NacionalidadsController : BaseController<Nacionalidad>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Nacionalidads
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            
            var list = db.NACIONALIDAD.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
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
        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
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
                    mensaje = ValidarFechas(nacionalidad.FechaDeInicio, nacionalidad.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(nacionalidad, "I", "NACIONALIDAD");
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
                var nacionalidadAntes = db.NACIONALIDAD.AsNoTracking().Where(d => d.Id == nacionalidad.Id).FirstOrDefault();
                db.Entry(nacionalidad).State = EntityState.Modified;
                string mensaje=ValidarFechas(nacionalidad.FechaDeInicio, nacionalidad.FechaDeFin);
                if (mensaje=="")
                {
                    db.SaveChanges();
                    Bitacora(nacionalidad, "U", "NACIONALIDAD", nacionalidadAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
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
            Nacionalidad nacionalidadAntes = ObtenerCopia(nacionalidad);
            if (nacionalidad.Estado == "A")
                nacionalidad.Estado = "I";
            else
                nacionalidad.Estado = "A";
            db.SaveChanges();
            Bitacora(nacionalidad, "U", "NACIONALIDAD", nacionalidadAntes);
            return RedirectToAction("Index");
        }

        // GET: Nacionalidads/RealDelete/5
        public ActionResult RealDelete(string id, DateTime FechaInicio, DateTime FechaFin)
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

        // POST: Nacionalidads/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id, DateTime FechaInicio, DateTime FechaFin)
        {
            Nacionalidad nacionalidad = db.NACIONALIDAD.Find(id, FechaInicio, FechaFin);
            db.NACIONALIDAD.Remove(nacionalidad);
            db.SaveChanges();
            Bitacora(nacionalidad, "D", "NACIONALIDAD");
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
