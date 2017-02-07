using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Controllers
{
    public class MantenimientoTablasController : BaseController<SIBOACTablas>
    {
        private SIBOACSecurityEntities dbs = new SIBOACSecurityEntities();

        // GET: MantenimientoTablas
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            return View(dbs.SIBOACTablas.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = dbs.SIBOACTablas.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: MantenimientoTablas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACTablas sIBOACTablas = dbs.SIBOACTablas.Find(id);
            if (sIBOACTablas == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACTablas);
        }

        // GET: MantenimientoTablas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MantenimientoTablas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] SIBOACTablas sIBOACTablas)
        {
            if (ModelState.IsValid)
            {
                dbs.SIBOACTablas.Add(sIBOACTablas);
                string mensaje = Verificar(sIBOACTablas.Id);

                if (mensaje == "")
                {
                    dbs.SaveChanges();
                    Bitacora(sIBOACTablas, "I", "SIBOACTablas");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(sIBOACTablas);
                }
            }

            return View(sIBOACTablas);
        }

        // GET: MantenimientoTablas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACTablas sIBOACTablas = dbs.SIBOACTablas.Find(id);
            if (sIBOACTablas == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACTablas);
        }

        // POST: MantenimientoTablas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] SIBOACTablas sIBOACTablas)
        {
            if (ModelState.IsValid)
            {
                var sIBOACTablasAntes = dbs.SIBOACTablas.AsNoTracking().Where(d => d.Id == sIBOACTablas.Id).FirstOrDefault();
                dbs.Entry(sIBOACTablas).State = EntityState.Modified;
                dbs.SaveChanges();
                Bitacora(sIBOACTablas, "U", "SIBOACTablas", sIBOACTablasAntes);
                TempData["Type"] = "info";
                TempData["Message"] = "La edición se realizó correctamente";
                return RedirectToAction("Index");
            }
            return View(sIBOACTablas);
        }

        // GET: MantenimientoTablas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACTablas sIBOACTablas = dbs.SIBOACTablas.Find(id);
            if (sIBOACTablas == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACTablas);
        }

        // POST: MantenimientoTablas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACTablas sIBOACTablas = dbs.SIBOACTablas.Find(id);
            SIBOACTablas sIBOACTablasAntes = ObtenerCopia(sIBOACTablas);
            dbs.SIBOACTablas.Remove(sIBOACTablas);
            db.SaveChanges();
            Bitacora(sIBOACTablas, "U", "SIBOACTablas", sIBOACTablasAntes);
            TempData["Type"] = "error";
            TempData["Message"] = "El registro se eliminó correctamente";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbs.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
