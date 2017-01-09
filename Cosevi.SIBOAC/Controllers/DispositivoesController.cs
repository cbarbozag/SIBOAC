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
    public class DispositivoesController : BaseController<Dispositivo>
    {
        

        // GET: Dispositivoes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.Dispositivoes1.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.Dispositivoes1.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Dispositivoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dispositivo dispositivo = db.Dispositivoes1.Find(id);
            if (dispositivo == null)
            {
                return HttpNotFound();
            }
            return View(dispositivo);
        }

        // GET: Dispositivoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dispositivoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Dispositivo dispositivo)
        {
            if (ModelState.IsValid)
            {
                db.Dispositivoes1.Add(dispositivo);
                string mensaje = Verificar(dispositivo.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(dispositivo, "I");

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(dispositivo);
                }
            }

            return View(dispositivo);
        }

        // GET: Dispositivoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dispositivo dispositivo = db.Dispositivoes1.Find(id);
            if (dispositivo == null)
            {
                return HttpNotFound();
            }
            return View(dispositivo);
        }

        // POST: Dispositivoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Dispositivo dispositivo)
        {
            if (ModelState.IsValid)
            {
                var dispositivoAntes = db.Dispositivoes1.AsNoTracking().Where(d => d.Id == dispositivo.Id).FirstOrDefault();
                db.Entry(dispositivo).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(dispositivo, "U", dispositivoAntes);
                return RedirectToAction("Index");
            }
            return View(dispositivo);
        }


        // GET: Dispositivoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dispositivo dispositivo = db.Dispositivoes1.Find(id);
            if (dispositivo == null)
            {
                return HttpNotFound();
            }
            return View(dispositivo);
        }

        // POST: Dispositivoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dispositivo dispositivo = db.Dispositivoes1.Find(id);
            Dispositivo dispositivoAntes = ObtenerCopia(dispositivo);
            if (dispositivo.Estado == "I")
                dispositivo.Estado = "A";
            else
                dispositivo.Estado = "I";
            db.SaveChanges();
            Bitacora(dispositivo, "U", dispositivoAntes);
            return RedirectToAction("Index");
        }


        // GET: Dispositivoes/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dispositivo dispositivo = db.Dispositivoes1.Find(id);
            if (dispositivo == null)
            {
                return HttpNotFound();
            }
            return View(dispositivo);
        }

        // POST: Dispositivoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Dispositivo dispositivo = db.Dispositivoes1.Find(id);
            db.Dispositivoes1.Remove(dispositivo);
            db.SaveChanges();
            Bitacora(dispositivo, "D");
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
