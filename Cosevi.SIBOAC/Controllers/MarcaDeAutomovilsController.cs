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
    public class MarcaDeAutomovilsController : BaseController<MarcaDeAutomovil>
    {
        

        // GET: MarcaDeAutomovils
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            
            var list = db.MARCA.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.MARCA.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: MarcaDeAutomovils/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            if (marcaDeAutomovil == null)
            {
                return HttpNotFound();
            }
            return View(marcaDeAutomovil);
        }

        // GET: MarcaDeAutomovils/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MarcaDeAutomovils/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Topmarca,Estado,FechaDeInicio,FechaDeFin")] MarcaDeAutomovil marcaDeAutomovil)
        {
            if (ModelState.IsValid)
            {
                db.MARCA.Add(marcaDeAutomovil);
                string mensaje = Verificar(marcaDeAutomovil.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(marcaDeAutomovil, "I", "MARCA");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(marcaDeAutomovil);
                }
            }

            return View(marcaDeAutomovil);
        }

        // GET: MarcaDeAutomovils/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            if (marcaDeAutomovil == null)
            {
                return HttpNotFound();
            }
            return View(marcaDeAutomovil);
        }

        // POST: MarcaDeAutomovils/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Topmarca,Estado,FechaDeInicio,FechaDeFin")] MarcaDeAutomovil marcaDeAutomovil)
        {
            if (ModelState.IsValid)
            {
                var marcaDeAutomovilAntes = db.MARCA.AsNoTracking().Where(d => d.Id == marcaDeAutomovil.Id).FirstOrDefault();
                db.Entry(marcaDeAutomovil).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(marcaDeAutomovil, "U", "MARCA", marcaDeAutomovilAntes);
                return RedirectToAction("Index");
            }
            return View(marcaDeAutomovil);
        }

        // GET: MarcaDeAutomovils/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            if (marcaDeAutomovil == null)
            {
                return HttpNotFound();
            }
            return View(marcaDeAutomovil);
        }

        // POST: MarcaDeAutomovils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            MarcaDeAutomovil marcaDeAutomovilAntes = ObtenerCopia(marcaDeAutomovil);
            if (marcaDeAutomovil.Estado == "I")
                marcaDeAutomovil.Estado = "A";
            else
                marcaDeAutomovil.Estado = "I";
            db.SaveChanges();
            Bitacora(marcaDeAutomovil, "U", "MARCA", marcaDeAutomovilAntes);
            return RedirectToAction("Index");
        }

        // GET: MarcaDeAutomovils/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            if (marcaDeAutomovil == null)
            {
                return HttpNotFound();
            }
            return View(marcaDeAutomovil);
        }

        // POST: MarcaDeAutomovils/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            MarcaDeAutomovil marcaDeAutomovil = db.MARCA.Find(id);
            db.MARCA.Remove(marcaDeAutomovil);
            db.SaveChanges();
            Bitacora(marcaDeAutomovil, "D", "MARCA");
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
