using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;
using PagedList;

namespace Cosevi.SIBOAC.Controllers
{
    public class CaracteristicasDeUbicacionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: CaracteristicasDeUbicacions
        [SessionExpire]
        public ActionResult Index(int ? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
                  
            var list = db.CARACUBI.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.CARACUBI.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: CaracteristicasDeUbicacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            if (caracteristicasDeUbicacion == null)
            {
                return HttpNotFound();
            }
            return View(caracteristicasDeUbicacion);
        }

        // GET: CaracteristicasDeUbicacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CaracteristicasDeUbicacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CaracteristicasDeUbicacion caracteristicasDeUbicacion)
        {
            if (ModelState.IsValid)
            {
                db.CARACUBI.Add(caracteristicasDeUbicacion);
                string mensaje = Verificar(caracteristicasDeUbicacion.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(caracteristicasDeUbicacion);
                }
            }

            return View(caracteristicasDeUbicacion);
        }

        // GET: CaracteristicasDeUbicacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            if (caracteristicasDeUbicacion == null)
            {
                return HttpNotFound();
            }
            return View(caracteristicasDeUbicacion);
        }

        // POST: CaracteristicasDeUbicacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CaracteristicasDeUbicacion caracteristicasDeUbicacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(caracteristicasDeUbicacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(caracteristicasDeUbicacion);
        }

        // GET: CaracteristicasDeUbicacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            if (caracteristicasDeUbicacion == null)
            {
                return HttpNotFound();
            }
            return View(caracteristicasDeUbicacion);
        }

        // POST: CaracteristicasDeUbicacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            if (caracteristicasDeUbicacion.Estado == "A")
                caracteristicasDeUbicacion.Estado = "I";
            else
                caracteristicasDeUbicacion.Estado = "A";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: CaracteristicasDeUbicacions/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            if (caracteristicasDeUbicacion == null)
            {
                return HttpNotFound();
            }
            return View(caracteristicasDeUbicacion);
        }

        // POST: CaracteristicasDeUbicacions/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            CaracteristicasDeUbicacion caracteristicasDeUbicacion = db.CARACUBI.Find(id);
            db.CARACUBI.Remove(caracteristicasDeUbicacion);
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
