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
    public class SentidoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Sentidoes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.SENTIDO.ToList());
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
                db.Entry(sentido).State = EntityState.Modified;
                db.SaveChanges();
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
            if (sentido.Estado == "I")
                sentido.Estado = "A";
            else
                sentido.Estado = "I";
            db.SaveChanges();
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
