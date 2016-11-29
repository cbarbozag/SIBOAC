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
    public class RutasPorDistritosController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RutasPorDistritos
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.RUTASXDISTRITO.ToList());
        }

        // GET: RutasPorDistritos/Details/5
        public ActionResult Details(int? codigo_distrito, int ? codigo_ruta, int? km)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            RutasPorDistritos rutasPorDistritos = db.RUTASXDISTRITO.Find(codigo_distrito, codigo_ruta, km);
            if (rutasPorDistritos == null)
            {
                return HttpNotFound();
            }
            return View(rutasPorDistritos);
        }

        // GET: RutasPorDistritos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RutasPorDistritos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoDistrito,CodigoRuta,Km,Estado,FechaDeInicio,FechaDeFin")] RutasPorDistritos rutasPorDistritos)
        {
            if (ModelState.IsValid)
            {
                db.RUTASXDISTRITO.Add(rutasPorDistritos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rutasPorDistritos);
        }

        // GET: RutasPorDistritos/Edit/5
        public ActionResult Edit(int? codigo_distrito, int? codigo_ruta, int? km)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            RutasPorDistritos rutasPorDistritos = db.RUTASXDISTRITO.Find(codigo_distrito, codigo_ruta, km);
            if (rutasPorDistritos == null)
            {
                return HttpNotFound();
            }
            return View(rutasPorDistritos);
        }

        // POST: RutasPorDistritos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoDistrito,CodigoRuta,Km,Estado,FechaDeInicio,FechaDeFin")] RutasPorDistritos rutasPorDistritos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rutasPorDistritos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rutasPorDistritos);
        }

        // GET: RutasPorDistritos/Delete/5
        public ActionResult Delete(int? codigo_distrito, int? codigo_ruta, int? km)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            RutasPorDistritos rutasPorDistritos = db.RUTASXDISTRITO.Find(codigo_distrito, codigo_ruta, km);
            if (rutasPorDistritos == null)
            {
                return HttpNotFound();
            }
            return View(rutasPorDistritos);
        }

        // POST: RutasPorDistritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? codigo_distrito, int? codigo_ruta, int? km)
        {
            RutasPorDistritos rutasPorDistritos = db.RUTASXDISTRITO.Find(codigo_distrito, codigo_ruta, km);
            db.RUTASXDISTRITO.Remove(rutasPorDistritos);
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
