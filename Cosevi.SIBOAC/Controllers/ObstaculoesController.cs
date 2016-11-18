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
    public class ObstaculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Obstaculoes
        public ActionResult Index()
        {
            return View(db.Obstaculo.ToList());
        }

        // GET: Obstaculoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // GET: Obstaculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Obstaculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Obstaculo obstaculo)
        {
            if (ModelState.IsValid)
            {
                db.Obstaculo.Add(obstaculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(obstaculo);
        }

        // GET: Obstaculoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // POST: Obstaculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Obstaculo obstaculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obstaculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obstaculo);
        }

        // GET: Obstaculoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // POST: Obstaculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo.Estado == "I")
                obstaculo.Estado = "A";
            else
                obstaculo.Estado = "I";
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
