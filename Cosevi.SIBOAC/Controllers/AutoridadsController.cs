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
    public class AutoridadsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Autoridads
        public ActionResult Index()
        {
            return View(db.AUTORIDAD.ToList());
        }

        // GET: Autoridads/Details/5
        public ActionResult Details(string codigo, int codFormulario)
        {
            if (codigo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autoridad autoridad = db.AUTORIDAD.Find(codigo, codFormulario);
            if (autoridad == null)
            {
                return HttpNotFound();
            }
            return View(autoridad);
        }

        // GET: Autoridads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Autoridads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,CodigoOpcionFormulario,Estado,FechaDeInicio,FechaDeFin")] Autoridad autoridad)
        {
            if (ModelState.IsValid)
            {
                db.AUTORIDAD.Add(autoridad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(autoridad);
        }

        // GET: Autoridads/Edit/5
        public ActionResult Edit(string codigo, int codFormulario)
        {
            if (codigo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autoridad autoridad = db.AUTORIDAD.Find(codigo, codFormulario);
            if (autoridad == null)
            {
                return HttpNotFound();
            }
            return View(autoridad);
        }

        // POST: Autoridads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,CodigoOpcionFormulario,Estado,FechaDeInicio,FechaDeFin")] Autoridad autoridad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(autoridad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(autoridad);
        }

        // GET: Autoridads/Delete/5
        public ActionResult Delete(string codigo, int codFormulario)
        {
            if (codigo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autoridad autoridad = db.AUTORIDAD.Find(codigo,codFormulario);
            if (autoridad == null)
            {
                return HttpNotFound();
            }
            return View(autoridad);
        }

        // POST: Autoridads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codigo, int codFormulario)
        {
            Autoridad autoridad = db.AUTORIDAD.Find(codigo, codFormulario);
            db.AUTORIDAD.Remove(autoridad);
            db.SaveChanges();
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
