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
    public class DispositivoPorRolPersonasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DispositivoPorRolPersonas
        public ActionResult Index()
        {
            return View(db.DISPXROLPERSONA.ToList());
        }

        // GET: DispositivoPorRolPersonas/Details/5
        public ActionResult Details(string CodRol, int CodDisp)
        {
            if (CodRol == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivoPorRolPersona dispositivoPorRolPersona = db.DISPXROLPERSONA.Find(CodRol, CodDisp);
            if (dispositivoPorRolPersona == null)
            {
                return HttpNotFound();
            }
            return View(dispositivoPorRolPersona);
        }

        // GET: DispositivoPorRolPersonas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DispositivoPorRolPersonas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoRolPersona,CodigoDispositivo")] DispositivoPorRolPersona dispositivoPorRolPersona)
        {
            if (ModelState.IsValid)
            {
                db.DISPXROLPERSONA.Add(dispositivoPorRolPersona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dispositivoPorRolPersona);
        }

        // GET: DispositivoPorRolPersonas/Edit/5
        public ActionResult Edit(string CodRol, int CodDisp)
        {
            if (CodRol == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivoPorRolPersona dispositivoPorRolPersona = db.DISPXROLPERSONA.Find(CodRol, CodDisp);
            if (dispositivoPorRolPersona == null)
            {
                return HttpNotFound();
            }
            return View(dispositivoPorRolPersona);
        }

        // POST: DispositivoPorRolPersonas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoRolPersona,CodigoDispositivo")] DispositivoPorRolPersona dispositivoPorRolPersona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dispositivoPorRolPersona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dispositivoPorRolPersona);
        }

        // GET: DispositivoPorRolPersonas/Delete/5
        public ActionResult Delete(string CodRol, int CodDisp)
        {
            if (CodRol == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivoPorRolPersona dispositivoPorRolPersona = db.DISPXROLPERSONA.Find(CodRol, CodDisp);
            if (dispositivoPorRolPersona == null)
            {
                return HttpNotFound();
            }
            return View(dispositivoPorRolPersona);
        }

        // POST: DispositivoPorRolPersonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string CodRol, int CodDisp)
        {
            DispositivoPorRolPersona dispositivoPorRolPersona = db.DISPXROLPERSONA.Find(CodRol, CodDisp);
            db.DISPXROLPERSONA.Remove(dispositivoPorRolPersona);
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
