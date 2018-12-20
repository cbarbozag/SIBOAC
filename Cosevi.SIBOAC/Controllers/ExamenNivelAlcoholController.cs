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
    public class ExamenNivelAlcoholController : BaseController<ExamenNivelAlcohol>
    {


        // GET: Examen
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
                        
            var list = db.ExamenNivelAlcohol.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int codexa)
        {
            string mensaje = "";
            bool exist = db.ExamenNivelAlcohol.Any(x => x.codexa == codexa);
            if (exist)
            {
                mensaje = "El codigo " + codexa + " ya esta registrado";
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

        // GET: Examen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamenNivelAlcohol examen = db.ExamenNivelAlcohol.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        // GET: Examen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Examen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codexa,descripcion,alcmin_aire,alcmax_aire,alcmin_sangre,alcmin_sangre,tipo_conductor,articulo_sugerido,estado,fecha_inicio,fecha_fin")] ExamenNivelAlcohol examen)
        {
            if (ModelState.IsValid)
            {
                db.ExamenNivelAlcohol.Add(examen);
                string mensaje = Verificar(examen.codexa);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(examen.fecha_inicio, examen.fecha_fin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(examen, "I", "EXAMEN NIVEL ALCOHOL");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(examen);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(examen);
                }
            }

            return View(examen);
        }

        // GET: Examen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamenNivelAlcohol examen = db.ExamenNivelAlcohol.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        // POST: Examen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codexa,descripcion,alcmin_aire,alcmax_aire,alcmin_sangre,alcmin_sangre,tipo_conductor,articulo_sugerido,estado,fecha_inicio,fecha_fin")] ExamenNivelAlcohol examen)
        {
            if (ModelState.IsValid)
            {
                var examenAntes = db.ExamenNivelAlcohol.AsNoTracking().Where(d => d.codexa == examen.codexa).FirstOrDefault();
                db.Entry(examen).State = EntityState.Modified;
                string mensaje = ValidarFechas(examen.fecha_inicio, examen.fecha_fin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(examen, "U", "EXAMEN NIVEL ALCOHOL", examenAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(examen);
                }                
            }
            return View(examen);
        }

        // GET: Examen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamenNivelAlcohol examen = db.ExamenNivelAlcohol.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        // POST: Examen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamenNivelAlcohol examen = db.ExamenNivelAlcohol.Find(id);
            ExamenNivelAlcohol examenAntes = ObtenerCopia(examen);
            if (examen.estado == "I")
                examen.estado = "A";
            else
                examen.estado = "I";
            db.SaveChanges();
            Bitacora(examen, "U", "EXAMEN NIVEL ALCOHOL", examenAntes);
            return RedirectToAction("Index");
        }

        // GET: Examen/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamenNivelAlcohol examen = db.ExamenNivelAlcohol.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        // POST: Examen/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            ExamenNivelAlcohol examen = db.ExamenNivelAlcohol.Find(id);
            db.ExamenNivelAlcohol.Remove(examen);
            db.SaveChanges();
            Bitacora(examen, "D", "EXAMEN NIVEL ALCOHOL");
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
