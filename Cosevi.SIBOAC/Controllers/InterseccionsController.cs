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
    public class InterseccionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Interseccions
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.INTERSECCION.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.INTERSECCION.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Interseccions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interseccion interseccion = db.INTERSECCION.Find(id);
            if (interseccion == null)
            {
                return HttpNotFound();
            }
            return View(interseccion);
        }

        // GET: Interseccions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Interseccions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Interseccion interseccion)
        {
            if (ModelState.IsValid)
            {
                db.INTERSECCION.Add(interseccion);
                string mensaje = Verificar(interseccion.Id);
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
                    return View(interseccion);
                }
            }

            return View(interseccion);
        }

        // GET: Interseccions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interseccion interseccion = db.INTERSECCION.Find(id);
            if (interseccion == null)
            {
                return HttpNotFound();
            }
            return View(interseccion);
        }

        // POST: Interseccions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Interseccion interseccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interseccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(interseccion);
        }

        // GET: Interseccions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interseccion interseccion = db.INTERSECCION.Find(id);
            if (interseccion == null)
            {
                return HttpNotFound();
            }
            return View(interseccion);
        }

        // POST: Interseccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interseccion interseccion = db.INTERSECCION.Find(id);
            if (interseccion.Estado == "I")
                interseccion.Estado = "A";
            else
                interseccion.Estado = "I";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Interseccions/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interseccion interseccion = db.INTERSECCION.Find(id);
            if (interseccion == null)
            {
                return HttpNotFound();
            }
            return View(interseccion);
        }

        // POST: Interseccions/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Interseccion interseccion = db.INTERSECCION.Find(id);
            db.INTERSECCION.Remove(interseccion);
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
