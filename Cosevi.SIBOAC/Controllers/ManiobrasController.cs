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
    public class ManiobrasController : BaseController<Maniobra>
    {


        // GET: Maniobras
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            
            var list = db.Maniobra.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.Maniobra.Any(x => x.Id == id);
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
        // GET: Maniobras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maniobra maniobra = db.Maniobra.Find(id);
            if (maniobra == null)
            {
                return HttpNotFound();
            }
            return View(maniobra);
        }

        // GET: Maniobras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Maniobras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Maniobra maniobra)
        {
            if (ModelState.IsValid)
            {
                db.Maniobra.Add(maniobra);
                string mensaje = Verificar(maniobra.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(maniobra, "I", "MANIOBRA");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(maniobra);
                }
            }

            return View(maniobra);
        }

        // GET: Maniobras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maniobra maniobra = db.Maniobra.Find(id);
            if (maniobra == null)
            {
                return HttpNotFound();
            }
            return View(maniobra);
        }

        // POST: Maniobras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Maniobra maniobra)
        {
            if (ModelState.IsValid)
            {
                var maniobraAntes = db.Maniobra.AsNoTracking().Where(d => d.Id == maniobra.Id).FirstOrDefault();
                db.Entry(maniobra).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(maniobra, "U", "MANIOBRA", maniobraAntes);
                return RedirectToAction("Index");
            }
            return View(maniobra);
        }

        // GET: Maniobras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maniobra maniobra = db.Maniobra.Find(id);
            if (maniobra == null)
            {
                return HttpNotFound();
            }
            return View(maniobra);
        }

        // POST: Maniobras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maniobra maniobra = db.Maniobra.Find(id);
            Maniobra maniobraAntes = ObtenerCopia(maniobra);
            if (maniobra.Estado == "I")
                maniobra.Estado = "A";
            else
                maniobra.Estado = "I";
            db.SaveChanges();
            Bitacora(maniobra, "U", "MANIOBRA", maniobraAntes);
            return RedirectToAction("Index");
        }

        // GET: Maniobras/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maniobra maniobra = db.Maniobra.Find(id);
            if (maniobra == null)
            {
                return HttpNotFound();
            }
            return View(maniobra);
        }

        // POST: Maniobras/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Maniobra maniobra = db.Maniobra.Find(id);
            db.Maniobra.Remove(maniobra);
            db.SaveChanges();
            Bitacora(maniobra, "D", "MANIOBRA");
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
