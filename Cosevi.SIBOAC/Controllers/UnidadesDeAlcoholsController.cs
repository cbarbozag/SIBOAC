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
    public class UnidadesDeAlcoholsController : BaseController<UnidadesDeAlcohol>
    {
        
        // GET: UnidadesDeAlcohols
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = from s in db.UNIDADES_ALCOHOL.ToList().OrderBy(s => s.Codigo) select s; ;

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
        }
        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.UNIDADES_ALCOHOL.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El código " + id + " ya esta registrado";
            }
            return mensaje;
        }


        // GET: UnidadesDeAlcohols/Details/5
        public ActionResult Details(string id, DateTime? FechaIni, DateTime? FechaFin)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadesDeAlcohol unidadesDeAlcohol = db.UNIDADES_ALCOHOL.Find(id, FechaIni, FechaFin);
            if (unidadesDeAlcohol == null)
            {
                return HttpNotFound();
            }
            return View(unidadesDeAlcohol);
        }

        // GET: UnidadesDeAlcohols/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnidadesDeAlcohols/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin,Codigo,Descripcion")] UnidadesDeAlcohol unidadesDeAlcohol)
        {
            if (ModelState.IsValid)
            {
                db.UNIDADES_ALCOHOL.Add(unidadesDeAlcohol);
                string mensaje = Verificar(unidadesDeAlcohol.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(unidadesDeAlcohol.FechaDeInicio,unidadesDeAlcohol.FechaDeFin);
                    if (mensaje == "")
                    {

                        db.SaveChanges();
                        Bitacora(unidadesDeAlcohol, "I", "UNIDADES_ALCOHOL");

                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(unidadesDeAlcohol);
                    }

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(unidadesDeAlcohol);
                }
            }

            return View(unidadesDeAlcohol);
        }

        // GET: UnidadesDeAlcohols/Edit/5
        public ActionResult Edit(string id, DateTime? FechaIni, DateTime? FechaFin)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadesDeAlcohol unidadesDeAlcohol = db.UNIDADES_ALCOHOL.Find(id, FechaIni, FechaFin);
            if (unidadesDeAlcohol == null)
            {
                return HttpNotFound();
            }
            return View(unidadesDeAlcohol);
        }

        // POST: UnidadesDeAlcohols/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin,Codigo,Descripcion")] UnidadesDeAlcohol unidadesDeAlcohol)
        {
            if (ModelState.IsValid)
            {
                var unidadesDeAlcoholAntes = db.UNIDADES_ALCOHOL.AsNoTracking().Where(d => d.Id == unidadesDeAlcohol.Id).FirstOrDefault();

                db.Entry(unidadesDeAlcohol).State = EntityState.Modified;
                string mensaje = ValidarFechas(unidadesDeAlcohol.FechaDeInicio, unidadesDeAlcohol.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(unidadesDeAlcohol, "U", "UNIDADES_ALCOHOL", unidadesDeAlcoholAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(unidadesDeAlcohol);
                }
            }
            return View(unidadesDeAlcohol);
        }

        // GET: UnidadesDeAlcohols/Delete/5
        public ActionResult Delete(string id, DateTime? FechaIni, DateTime? FechaFin)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadesDeAlcohol unidadesDeAlcohol = db.UNIDADES_ALCOHOL.Find(id, FechaIni, FechaFin);
            if (unidadesDeAlcohol == null)
            {
                return HttpNotFound();
            }
            return View(unidadesDeAlcohol);
        }

        // POST: UnidadesDeAlcohols/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, DateTime? FechaIni, DateTime? FechaFin)
        {
            UnidadesDeAlcohol unidadesDeAlcohol = db.UNIDADES_ALCOHOL.Find(id, FechaIni, FechaFin);
            UnidadesDeAlcohol unidadesDeAlcoholAntes = ObtenerCopia(unidadesDeAlcohol);
            if (unidadesDeAlcohol.Estado == "I")
                unidadesDeAlcohol.Estado = "A";
            else
                unidadesDeAlcohol.Estado = "I";
            db.SaveChanges();
            Bitacora(unidadesDeAlcohol, "U", "UNIDADES_ALCOHOL", unidadesDeAlcoholAntes);
            return RedirectToAction("Index");
        }

        // GET: UnidadesDeAlcohols/RealDelete/5
        public ActionResult RealDelete(string id, DateTime? FechaIni, DateTime? FechaFin)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadesDeAlcohol unidadesDeAlcohol = db.UNIDADES_ALCOHOL.Find(id, FechaIni, FechaFin);
            if (unidadesDeAlcohol == null)
            {
                return HttpNotFound();
            }
            return View(unidadesDeAlcohol);
        }

        // POST: UnidadesDeAlcohols/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id, DateTime? FechaIni, DateTime? FechaFin)
        {
            UnidadesDeAlcohol unidadesDeAlcohol = db.UNIDADES_ALCOHOL.Find(id, FechaIni, FechaFin);
            db.UNIDADES_ALCOHOL.Remove(unidadesDeAlcohol);
            db.SaveChanges();
            Bitacora(unidadesDeAlcohol, "D", "UNIDADES_ALCOHOL");
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
