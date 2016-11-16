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
    public class EstadoDeLaCalzadasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: EstadoDeLaCalzadas
        public ActionResult Index()
        {
            return View(db.ESTCALZADA.ToList());
        }

        // GET: EstadoDeLaCalzadas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            if (estadoDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(estadoDeLaCalzada);
        }

        // GET: EstadoDeLaCalzadas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstadoDeLaCalzadas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] EstadoDeLaCalzada estadoDeLaCalzada)
        {
            if (ModelState.IsValid)
            {
                db.ESTCALZADA.Add(estadoDeLaCalzada);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estadoDeLaCalzada);
        }

        // GET: EstadoDeLaCalzadas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            if (estadoDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(estadoDeLaCalzada);
        }

        // POST: EstadoDeLaCalzadas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] EstadoDeLaCalzada estadoDeLaCalzada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadoDeLaCalzada).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estadoDeLaCalzada);
        }

        // GET: EstadoDeLaCalzadas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            if (estadoDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(estadoDeLaCalzada);
        }

        // POST: EstadoDeLaCalzadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            estadoDeLaCalzada.Estado = "I";
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
