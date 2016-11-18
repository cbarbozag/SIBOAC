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
    public class OpcionesDelPlanoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: OpcionesDelPlanoes
        public ActionResult Index()
        {
            return View(db.OPCIONPLANO.ToList());
        }

        // GET: OpcionesDelPlanoes/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            if (opcionesDelPlano == null)
            {
                return HttpNotFound();
            }
            return View(opcionesDelPlano);
        }

        // GET: OpcionesDelPlanoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OpcionesDelPlanoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OpcionesDelPlano opcionesDelPlano)
        {
            if (ModelState.IsValid)
            {
                db.OPCIONPLANO.Add(opcionesDelPlano);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(opcionesDelPlano);
        }

        // GET: OpcionesDelPlanoes/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            if (opcionesDelPlano == null)
            {
                return HttpNotFound();
            }
            return View(opcionesDelPlano);
        }

        // POST: OpcionesDelPlanoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OpcionesDelPlano opcionesDelPlano)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opcionesDelPlano).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(opcionesDelPlano);
        }

        // GET: OpcionesDelPlanoes/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            if (opcionesDelPlano == null)
            {
                return HttpNotFound();
            }
            return View(opcionesDelPlano);
        }

        // POST: OpcionesDelPlanoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            if (opcionesDelPlano.Estado == "I")
                opcionesDelPlano.Estado = "A";
            else
                opcionesDelPlano.Estado = "I";
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
