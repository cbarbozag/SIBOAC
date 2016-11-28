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
    public class OpcionFormularioPorArticuloesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: OpcionFormularioPorArticuloes
        public ActionResult Index()
        {
            return View(db.OPCFORMULARIOXARTICULO.ToList());
        }

        // GET: OpcionFormularioPorArticuloes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id);
            if (opcionFormularioPorArticulo == null)
            {
                return HttpNotFound();
            }
            return View(opcionFormularioPorArticulo);
        }

        // GET: OpcionFormularioPorArticuloes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OpcionFormularioPorArticuloes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Conducta,FechaDeInicio,FechaDeFin,CodigoOpcionFormulario,Estado")] OpcionFormularioPorArticulo opcionFormularioPorArticulo)
        {
            if (ModelState.IsValid)
            {
                db.OPCFORMULARIOXARTICULO.Add(opcionFormularioPorArticulo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(opcionFormularioPorArticulo);
        }

        // GET: OpcionFormularioPorArticuloes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id);
            if (opcionFormularioPorArticulo == null)
            {
                return HttpNotFound();
            }
            return View(opcionFormularioPorArticulo);
        }

        // POST: OpcionFormularioPorArticuloes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Conducta,FechaDeInicio,FechaDeFin,CodigoOpcionFormulario,Estado")] OpcionFormularioPorArticulo opcionFormularioPorArticulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opcionFormularioPorArticulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(opcionFormularioPorArticulo);
        }

        // GET: OpcionFormularioPorArticuloes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id);
            if (opcionFormularioPorArticulo == null)
            {
                return HttpNotFound();
            }
            return View(opcionFormularioPorArticulo);
        }

        // POST: OpcionFormularioPorArticuloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id);
            db.OPCFORMULARIOXARTICULO.Remove(opcionFormularioPorArticulo);
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
