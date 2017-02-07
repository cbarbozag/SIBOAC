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
    public class TiempoesController : BaseController<Tiempo>
    {
        // GET: Tiempoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.Tiempo.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
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
        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
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
                    mensaje = ValidarFechas(tiempo.FechaDeInicio, tiempo.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(tiempo, "I", "Tiempo");

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
                var tiempoAntes = db.Tiempo.AsNoTracking().Where(d => d.Id == tiempo.Id).FirstOrDefault();

                db.Entry(tiempo).State = EntityState.Modified;
                string mensaje = ValidarFechas(tiempo.FechaDeInicio, tiempo.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(tiempo, "U", "Tiempo", tiempoAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
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
            Tiempo tiempoAntes = ObtenerCopia(tiempo);
            if (tiempo.Estado == "I")
                tiempo.Estado = "A";
            else
                tiempo.Estado = "I";
            db.SaveChanges();
            Bitacora(tiempo, "U", "Tiempo", tiempoAntes);
            return RedirectToAction("Index");
        }

        // GET: Tiempoes/RealDelete/5
        public ActionResult RealDelete(int? id)
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

        // POST: Tiempoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Tiempo tiempo = db.Tiempo.Find(id);
            db.Tiempo.Remove(tiempo);
            db.SaveChanges();
            Bitacora(tiempo, "D", "Tiempo");
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
