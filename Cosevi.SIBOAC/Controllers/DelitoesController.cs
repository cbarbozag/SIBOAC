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
    public class DelitoesController : BaseController<Delito>
    {
        

        // GET: Delitoes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.DELITO.ToList());
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.DELITO.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Delitoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delito delito = db.DELITO.Find(id);
            if (delito == null)
            {
                return HttpNotFound();
            }
            return View(delito);
        }

        // GET: Delitoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Delitoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Delito delito)
        {
            if (ModelState.IsValid)
            {
                db.DELITO.Add(delito);
                string mensaje = Verificar(delito.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(delito, "I");

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(delito);
                }
            }

            return View(delito);
        }

        // GET: Delitoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delito delito = db.DELITO.Find(id);
            if (delito == null)
            {
                return HttpNotFound();
            }
            return View(delito);
        }

        // POST: Delitoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Delito delito)
        {
            if (ModelState.IsValid)
            {
                var delitoAntes = db.DELITO.AsNoTracking().Where(d => d.Id == delito.Id).FirstOrDefault();
                db.Entry(delito).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(delito, "U", delitoAntes);
                return RedirectToAction("Index");
            }
            return View(delito);
        }

        // GET: Delitoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delito delito = db.DELITO.Find(id);
            if (delito == null)
            {
                return HttpNotFound();
            }
            return View(delito);
        }

        // POST: Delitoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Delito delito = db.DELITO.Find(id);
            Delito delitoAntes = ObtenerCopia(delito);
            if (delito.Estado == "I")
                delito.Estado = "A";
            else
                delito.Estado = "I";
            db.SaveChanges();
            Bitacora(delito, "U", delitoAntes);
            return RedirectToAction("Index");
        }

        // GET: Delitoes/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delito delito = db.DELITO.Find(id);
            if (delito == null)
            {
                return HttpNotFound();
            }
            return View(delito);
        }

        // POST: Delitoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            Delito delito = db.DELITO.Find(id);
            db.DELITO.Remove(delito);
            db.SaveChanges();
            Bitacora(delito, "D");
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
