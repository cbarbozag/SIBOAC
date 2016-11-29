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
    public class TipoIdDeVehiculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoIdDeVehiculoes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.TIPOIDEVEHICULO.ToList());
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.TIPOIDEVEHICULO.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El código " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: TipoIdDeVehiculoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            if (tipoIdDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoIdDeVehiculo);
        }

        // GET: TipoIdDeVehiculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoIdDeVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoIdDeVehiculo tipoIdDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.TIPOIDEVEHICULO.Add(tipoIdDeVehiculo);
                string mensaje = Verificar(tipoIdDeVehiculo.Id);
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
                    return View(tipoIdDeVehiculo);
                }
            }

            return View(tipoIdDeVehiculo);
        }

        // GET: TipoIdDeVehiculoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            if (tipoIdDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoIdDeVehiculo);
        }

        // POST: TipoIdDeVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoIdDeVehiculo tipoIdDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoIdDeVehiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoIdDeVehiculo);
        }

        // GET: TipoIdDeVehiculoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            if (tipoIdDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoIdDeVehiculo);
        }

        // POST: TipoIdDeVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            if (tipoIdDeVehiculo.Estado == "I")
                tipoIdDeVehiculo.Estado = "A";
            else
                tipoIdDeVehiculo.Estado = "I";
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
