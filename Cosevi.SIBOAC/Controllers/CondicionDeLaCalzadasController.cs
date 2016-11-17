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
    public class CondicionDeLaCalzadasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: CondicionDeLaCalzadas
        public ActionResult Index()
        {
            return View(db.CONDCALZADA.ToList());
        }

        // GET: CondicionDeLaCalzadas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaCalzada condicionDeLaCalzada = db.CONDCALZADA.Find(id);
            if (condicionDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaCalzada);
        }

        // GET: CondicionDeLaCalzadas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CondicionDeLaCalzadas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CondicionDeLaCalzada condicionDeLaCalzada)
        {
            if (ModelState.IsValid)
            {
                db.CONDCALZADA.Add(condicionDeLaCalzada);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(condicionDeLaCalzada);
        }

        // GET: CondicionDeLaCalzadas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaCalzada condicionDeLaCalzada = db.CONDCALZADA.Find(id);
            if (condicionDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaCalzada);
        }

        // POST: CondicionDeLaCalzadas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CondicionDeLaCalzada condicionDeLaCalzada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(condicionDeLaCalzada).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(condicionDeLaCalzada);
        }

        // GET: CondicionDeLaCalzadas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaCalzada condicionDeLaCalzada = db.CONDCALZADA.Find(id);
            if (condicionDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaCalzada);
        }

        // POST: CondicionDeLaCalzadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CondicionDeLaCalzada condicionDeLaCalzada = db.CONDCALZADA.Find(id);
            condicionDeLaCalzada.Estado = "I";
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
