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
    public class CatalogoDeArticulosController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: CatalogoDeArticulos
        public ActionResult Index()
        {
            return View(db.CATARTICULO.ToList());
        }

        // GET: CatalogoDeArticulos/Details/5
        public ActionResult Details(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal )
        {
            if (codigo == null || conducta == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            if (catalogoDeArticulos == null)
            {
                return HttpNotFound();
            }
            return View(catalogoDeArticulos);
        }

        // GET: CatalogoDeArticulos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CatalogoDeArticulos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Conducta,FechaDeInicio,FechaDeFin,Descripcion,Multa,Estado,Puntos,Totalidad")] CatalogoDeArticulos catalogoDeArticulos)
        {
            if (ModelState.IsValid)
            {
                db.CATARTICULO.Add(catalogoDeArticulos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catalogoDeArticulos);
        }

        // GET: CatalogoDeArticulos/Edit/5
        public ActionResult Edit(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            if (codigo == null || conducta == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            if (catalogoDeArticulos == null)
            {
                return HttpNotFound();
            }
            return View(catalogoDeArticulos);
        }

        // POST: CatalogoDeArticulos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Conducta,FechaDeInicio,FechaDeFin,Descripcion,Multa,Estado,Puntos,Totalidad")] CatalogoDeArticulos catalogoDeArticulos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(catalogoDeArticulos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catalogoDeArticulos);
        }

        // GET: CatalogoDeArticulos/Delete/5
        public ActionResult Delete(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            if (codigo == null || conducta == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            if (catalogoDeArticulos == null)
            {
                return HttpNotFound();
            }
            return View(catalogoDeArticulos);
        }

        // POST: CatalogoDeArticulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            db.CATARTICULO.Remove(catalogoDeArticulos);
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
