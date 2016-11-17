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
    public class AlineacionVerticalsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: AlineacionVerticals
        public ActionResult Index()
        {
            return View(db.ALINVERT.ToList());
        }

        // GET: AlineacionVerticals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionVertical alineacionVertical = db.ALINVERT.Find(id);
            if (alineacionVertical == null)
            {
                return HttpNotFound();
            }
            return View(alineacionVertical);
        }

        // GET: AlineacionVerticals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlineacionVerticals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] AlineacionVertical alineacionVertical)
        {
            if (ModelState.IsValid)
            {
                db.ALINVERT.Add(alineacionVertical);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alineacionVertical);
        }

        // GET: AlineacionVerticals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionVertical alineacionVertical = db.ALINVERT.Find(id);
            if (alineacionVertical == null)
            {
                return HttpNotFound();
            }
            return View(alineacionVertical);
        }

        // POST: AlineacionVerticals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] AlineacionVertical alineacionVertical)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alineacionVertical).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alineacionVertical);
        }

        // GET: AlineacionVerticals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionVertical alineacionVertical = db.ALINVERT.Find(id);
            if (alineacionVertical == null)
            {
                return HttpNotFound();
            }
            return View(alineacionVertical);
        }

        // POST: AlineacionVerticals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AlineacionVertical alineacionVertical = db.ALINVERT.Find(id);
            alineacionVertical.Estado = "I";
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
