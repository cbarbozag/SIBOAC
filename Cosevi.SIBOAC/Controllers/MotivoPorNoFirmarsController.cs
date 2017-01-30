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
    public class MotivoPorNoFirmarsController : BaseController<MotivoPorNoFirmar>
    {


        // GET: MotivoPorNoFirmars
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            
            var list = db.MotivoPorNoFirmars.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.MotivoPorNoFirmars.Any(x => x.Id == id);
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

        // GET: MotivoPorNoFirmars/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotivoPorNoFirmar motivoPorNoFirmar = db.MotivoPorNoFirmars.Find(id);
            if (motivoPorNoFirmar == null)
            {
                return HttpNotFound();
            }
            return View(motivoPorNoFirmar);
        }

        // GET: MotivoPorNoFirmars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MotivoPorNoFirmars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] MotivoPorNoFirmar motivoPorNoFirmar)
        {
            if (ModelState.IsValid)
            {
                db.MotivoPorNoFirmars.Add(motivoPorNoFirmar);
                string mensaje = Verificar(motivoPorNoFirmar.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(motivoPorNoFirmar.FechaDeInicio, motivoPorNoFirmar.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(motivoPorNoFirmar, "I", "MOTIVONOFIRMA");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(motivoPorNoFirmar);
                    } 
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(motivoPorNoFirmar);
                }
            }

            return View(motivoPorNoFirmar);
        }

        // GET: MotivoPorNoFirmars/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotivoPorNoFirmar motivoPorNoFirmar = db.MotivoPorNoFirmars.Find(id);
            if (motivoPorNoFirmar == null)
            {
                return HttpNotFound();
            }
            return View(motivoPorNoFirmar);
        }

        // POST: MotivoPorNoFirmars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] MotivoPorNoFirmar motivoPorNoFirmar)
        {
            if (ModelState.IsValid)
            {
                var motivoPorNoFirmarAntes = db.MotivoPorNoFirmars.AsNoTracking().Where(d => d.Id == motivoPorNoFirmar.Id).FirstOrDefault();
                db.Entry(motivoPorNoFirmar).State = EntityState.Modified;
                string mensaje = ValidarFechas(motivoPorNoFirmar.FechaDeInicio, motivoPorNoFirmar.FechaDeFin);
                if (mensaje=="")
                {
                    db.SaveChanges();
                    Bitacora(motivoPorNoFirmar, "U", "MOTIVONOFIRMA", motivoPorNoFirmarAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(motivoPorNoFirmar);
                }                
            }
            return View(motivoPorNoFirmar);
        }

        // GET: MotivoPorNoFirmars/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotivoPorNoFirmar motivoPorNoFirmar = db.MotivoPorNoFirmars.Find(id);
            if (motivoPorNoFirmar == null)
            {
                return HttpNotFound();
            }
            return View(motivoPorNoFirmar);
        }

        // POST: MotivoPorNoFirmars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MotivoPorNoFirmar motivoPorNoFirmar = db.MotivoPorNoFirmars.Find(id);
            MotivoPorNoFirmar motivoPorNoFirmarAntes = ObtenerCopia(motivoPorNoFirmar);
            if (motivoPorNoFirmar.Estado == "I")
                motivoPorNoFirmar.Estado = "A";
            else
                motivoPorNoFirmar.Estado = "I";
            db.SaveChanges();
            Bitacora(motivoPorNoFirmar, "U", "MOTIVONOFIRMA", motivoPorNoFirmarAntes);
            return RedirectToAction("Index");
        }

        // GET: MotivoPorNoFirmars/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MotivoPorNoFirmar motivoPorNoFirmar = db.MotivoPorNoFirmars.Find(id);
            if (motivoPorNoFirmar == null)
            {
                return HttpNotFound();
            }
            return View(motivoPorNoFirmar);
        }

        // POST: MotivoPorNoFirmars/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            MotivoPorNoFirmar motivoPorNoFirmar = db.MotivoPorNoFirmars.Find(id);
            db.MotivoPorNoFirmars.Remove(motivoPorNoFirmar);
            db.SaveChanges();
            Bitacora(motivoPorNoFirmar, "D", "MOTIVONOFIRMA");
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
