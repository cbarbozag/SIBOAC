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
    public class OficinaParaImpugnarsController : BaseController<OficinaParaImpugnar>
    {


        // GET: OficinaParaImpugnars
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            
            var list = db.OficinaParaImpugnars.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.OficinaParaImpugnars.Any(x => x.Id == id);
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
        // GET: OficinaParaImpugnars/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            if (oficinaParaImpugnar == null)
            {
                return HttpNotFound();
            }
            return View(oficinaParaImpugnar);
        }

        // GET: OficinaParaImpugnars/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OficinaParaImpugnars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OficinaParaImpugnar oficinaParaImpugnar)
        {
            if (ModelState.IsValid)
            {
                db.OficinaParaImpugnars.Add(oficinaParaImpugnar);
                string mensaje = Verificar(oficinaParaImpugnar.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(oficinaParaImpugnar, "I", "OFICINAIMPUGNA");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(oficinaParaImpugnar);
                }
            }

            return View(oficinaParaImpugnar);
        }

        // GET: OficinaParaImpugnars/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            if (oficinaParaImpugnar == null)
            {
                return HttpNotFound();
            }
            return View(oficinaParaImpugnar);
        }

        // POST: OficinaParaImpugnars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OficinaParaImpugnar oficinaParaImpugnar)
        {
            if (ModelState.IsValid)
            {
                var oficinaParaImpugnarAntes = db.OficinaParaImpugnars.AsNoTracking().Where(d => d.Id == oficinaParaImpugnar.Id).FirstOrDefault();
                db.Entry(oficinaParaImpugnar).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(oficinaParaImpugnar, "U", "OFICINAIMPUGNA", oficinaParaImpugnarAntes);
                return RedirectToAction("Index");
            }
            return View(oficinaParaImpugnar);
        }

        // GET: OficinaParaImpugnars/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            if (oficinaParaImpugnar == null)
            {
                return HttpNotFound();
            }
            return View(oficinaParaImpugnar);
        }

        // POST: OficinaParaImpugnars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            OficinaParaImpugnar oficinaParaImpugnarAntes = ObtenerCopia(oficinaParaImpugnar);
            if (oficinaParaImpugnar.Estado == "I")
                oficinaParaImpugnar.Estado = "A";
            else
                oficinaParaImpugnar.Estado = "I";
            db.SaveChanges();
            Bitacora(oficinaParaImpugnar, "U", "OFICINAIMPUGNA", oficinaParaImpugnarAntes);
            return RedirectToAction("Index");
        }

        // GET: OficinaParaImpugnars/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            if (oficinaParaImpugnar == null)
            {
                return HttpNotFound();
            }
            return View(oficinaParaImpugnar);
        }

        // POST: OficinaParaImpugnars/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            OficinaParaImpugnar oficinaParaImpugnar = db.OficinaParaImpugnars.Find(id);
            db.OficinaParaImpugnars.Remove(oficinaParaImpugnar);
            db.SaveChanges();
            Bitacora(oficinaParaImpugnar, "D", "OFICINAIMPUGNA");
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
