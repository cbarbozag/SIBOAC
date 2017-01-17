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
    public class SentidoesController : BaseController<Sentido>
    {


        // GET: Sentidoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.SENTIDO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.SENTIDO.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Sentidoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentido sentido = db.SENTIDO.Find(id);
            if (sentido == null)
            {
                return HttpNotFound();
            }
            return View(sentido);
        }

        // GET: Sentidoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sentidoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Sentido sentido)
        {
            if (ModelState.IsValid)
            {
                db.SENTIDO.Add(sentido);
                string mensaje = Verificar(sentido.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(sentido, "I", "SENTIDO");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(sentido);
                }
            }

            return View(sentido);
        }

        // GET: Sentidoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentido sentido = db.SENTIDO.Find(id);
            if (sentido == null)
            {
                return HttpNotFound();
            }
            return View(sentido);
        }

        // POST: Sentidoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Sentido sentido)
        {
            if (ModelState.IsValid)
            {
                var sentidoAntes = db.SENTIDO.AsNoTracking().Where(d => d.Id == sentido.Id).FirstOrDefault();
                db.Entry(sentido).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(sentido, "U", "SENTIDO", sentidoAntes);
                return RedirectToAction("Index");
            }
            return View(sentido);
        }

        // GET: Sentidoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentido sentido = db.SENTIDO.Find(id);
            if (sentido == null)
            {
                return HttpNotFound();
            }
            return View(sentido);
        }

        // POST: Sentidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Sentido sentido = db.SENTIDO.Find(id);
            Sentido sentidoAntes = ObtenerCopia(sentido);
            if (sentido.Estado == "I")
                sentido.Estado = "A";
            else
                sentido.Estado = "I";
            db.SaveChanges();
            Bitacora(sentido, "U", "SENTIDO", sentidoAntes);
            return RedirectToAction("Index");
        }

        // GET: Sentidoes/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sentido sentido = db.SENTIDO.Find(id);
            if (sentido == null)
            {
                return HttpNotFound();
            }
            return View(sentido);
        }

        // POST: Sentidoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            Sentido sentido = db.SENTIDO.Find(id);
            db.SENTIDO.Remove(sentido);
            db.SaveChanges();
            Bitacora(sentido, "D", "SENTIDO");
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
