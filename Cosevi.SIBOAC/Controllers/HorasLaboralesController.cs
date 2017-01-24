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
    public class HorasLaboralesController : BaseController<HorasLaborales>
    {


        // GET: HorasLaborales
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.HORASLABORALES.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }



        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.HORASLABORALES.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El código " + id + " ya esta registrado";
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

        // GET: HorasLaborales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasLaborales horasLaborales = db.HORASLABORALES.Find(id);
            if (horasLaborales == null)
            {
                return HttpNotFound();
            }
            return View(horasLaborales);
        }

        // GET: HorasLaborales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HorasLaborales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] HorasLaborales horasLaborales)
        {
            if (ModelState.IsValid)
            {
                db.HORASLABORALES.Add(horasLaborales);
                string mensaje = Verificar(horasLaborales.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(horasLaborales.FechaDeInicio, horasLaborales.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(horasLaborales, "I", "HORASLABORALES");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(horasLaborales);
                    }

                    db.SaveChanges();
                    Bitacora(horasLaborales, "I", "HORASLABORALES");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(horasLaborales);
                }

            }

            return View(horasLaborales);
        }

        // GET: HorasLaborales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasLaborales horasLaborales = db.HORASLABORALES.Find(id);
            if (horasLaborales == null)
            {
                return HttpNotFound();
            }
            return View(horasLaborales);
        }

        // POST: HorasLaborales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] HorasLaborales horasLaborales)
        {
            if (ModelState.IsValid)
            {
                var horasLaboralesAntes = db.HORASLABORALES.AsNoTracking().Where(d => d.Id == horasLaborales.Id).FirstOrDefault();
                db.Entry(horasLaborales).State = EntityState.Modified;
                string mensaje = ValidarFechas(horasLaborales.FechaDeInicio, horasLaborales.FechaDeFin);

                if (mensaje=="")
                {
                    db.SaveChanges();
                    Bitacora(horasLaborales, "U", "HORASLABORALES", horasLaboralesAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(horasLaborales);
                }
                
            }
            return View(horasLaborales);
        }

        // GET: HorasLaborales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasLaborales horasLaborales = db.HORASLABORALES.Find(id);
            if (horasLaborales == null)
            {
                return HttpNotFound();
            }
            return View(horasLaborales);
        }

        // POST: HorasLaborales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HorasLaborales horasLaborales = db.HORASLABORALES.Find(id);
            HorasLaborales horasLaboralesAntes = ObtenerCopia(horasLaborales);

            if (horasLaborales.Estado == "A")
                horasLaborales.Estado = "I";
            else
                horasLaborales.Estado = "A";
            db.SaveChanges();
            Bitacora(horasLaborales, "U", "HORASLABORALES", horasLaboralesAntes);
            return RedirectToAction("Index");
        }

        // GET: HorasLaborales/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HorasLaborales horasLaborales = db.HORASLABORALES.Find(id);
            if (horasLaborales == null)
            {
                return HttpNotFound();
            }
            return View(horasLaborales);
        }

        // POST: HorasLaborales/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            HorasLaborales horasLaborales = db.HORASLABORALES.Find(id);
            db.HORASLABORALES.Remove(horasLaborales);
            db.SaveChanges();
            Bitacora(horasLaborales, "D", "HORASLABORALES");
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
