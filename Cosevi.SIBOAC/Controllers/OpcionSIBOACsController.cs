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
    public class OpcionSIBOACsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: OpcionSIBOACs
        public ActionResult Index()
        {
            return View(db.OpcionSIBOAC.ToList());
        }

        // GET: OpcionSIBOACs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionSIBOAC opcionSIBOAC = db.OpcionSIBOAC.Find(id);
            if (opcionSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(opcionSIBOAC);
        }

        // GET: OpcionSIBOACs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OpcionSIBOACs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CodigoOpcion,Descripcion")] OpcionSIBOAC opcionSIBOAC)
        {
            if (ModelState.IsValid)
            {
                db.OpcionSIBOAC.Add(opcionSIBOAC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(opcionSIBOAC);
        }

        // GET: OpcionSIBOACs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionSIBOAC opcionSIBOAC = db.OpcionSIBOAC.Find(id);
            if (opcionSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(opcionSIBOAC);
        }

        // POST: OpcionSIBOACs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodigoOpcion,Descripcion")] OpcionSIBOAC opcionSIBOAC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opcionSIBOAC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(opcionSIBOAC);
        }

        // GET: OpcionSIBOACs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionSIBOAC opcionSIBOAC = db.OpcionSIBOAC.Find(id);
            if (opcionSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(opcionSIBOAC);
        }

        // POST: OpcionSIBOACs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OpcionSIBOAC opcionSIBOAC = db.OpcionSIBOAC.Find(id);
            db.OpcionSIBOAC.Remove(opcionSIBOAC);
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
