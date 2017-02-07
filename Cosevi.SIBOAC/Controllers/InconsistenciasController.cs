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
    public class InconsistenciasController : BaseController<Inconsistencia>
    {


        // GET: Inconsistencias
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.INCONSISTENCIA.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.INCONSISTENCIA.Any(x => x.Id == id);
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
                string mensaje = Verificar(inconsistencia.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(inconsistencia, "I", "INCONSISTENCIA");

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(inconsistencia);
                }
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
                var inconsistenciaAntes = db.INCONSISTENCIA.AsNoTracking().Where(d => d.Id == inconsistencia.Id).FirstOrDefault();
                db.Entry(inconsistencia).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(inconsistencia, "U", "INCONSISTENCIA", inconsistenciaAntes);
                TempData["Type"] = "success";
                TempData["Message"] = "La edición se realizó correctamente";
                return RedirectToAction("Index");
            }
            return View(inconsistencia);
        }

        // GET: Obstaculoes/RealDelete/5
        public ActionResult RealDelete(short? id)
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

        // POST: Obstaculoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(short id)
        {
            Inconsistencia inconsistencia = db.INCONSISTENCIA.Find(id);
            db.INCONSISTENCIA.Remove(inconsistencia);
            db.SaveChanges();
            Bitacora(inconsistencia, "D", "INCONSISTENCIA");
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
