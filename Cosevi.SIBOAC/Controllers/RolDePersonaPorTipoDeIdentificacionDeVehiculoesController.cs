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
    public class RolDePersonaPorTipoDeIdentificacionDeVehiculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes
        public ActionResult Index()
        {
            return View(db.ROLPERSONAXTIPOIDEVEHICULO.ToList());
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(id);
            if (rolDePersonaPorTipoDeIdentificacionDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoDeRol,CodigoDeIdentificacionVehiculo,Estado,FechaDeInicio,FechaDeFin")] RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.ROLPERSONAXTIPOIDEVEHICULO.Add(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Edit/5
        public ActionResult Edit(string codRol, string CodIdVeh)
        {
            if (codRol == null || CodIdVeh ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(codRol, CodIdVeh);
            if (rolDePersonaPorTipoDeIdentificacionDeVehiculo == null)
            {
                return HttpNotFound();
            }
            ViewBag.codRol = new SelectList(db.ROLPERSONA, "codigo", "codigo", rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeRol);
            ViewBag.CodIdVeh = new SelectList(db.TIPOVEH, "CodVeh", "codVeh", rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo);
        
            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
        }

        // POST: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoDeRol,CodigoDeIdentificacionVehiculo,Estado,FechaDeInicio,FechaDeFin")] RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo)
        {
            /* if (ModelState.IsValid)
             {
                 db.Entry(rolDePersonaPorTipoDeIdentificacionDeVehiculo).State = EntityState.Modified;
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }
             return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);*/
            if(ModelState.IsValid)
            {
                string OCodRol = Request["CodigoDeRol"];
                string OCodIdVeh = Request["CodigoDeIdentificacionVehiculo"];

                var services = db.ROLPERSONAXTIPOIDEVEHICULO.Where(a => a.CodigoDeRol == OCodRol)
                                           .Where(a => a.CodigoDeIdentificacionVehiculo == OCodIdVeh);

                foreach (var s in services)
                {
                    db.ROLPERSONAXTIPOIDEVEHICULO.Remove(s);
                }

                db.ROLPERSONAXTIPOIDEVEHICULO.Add(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return RedirectToAction("Index");
                }
                db.Entry(rolDePersonaPorTipoDeIdentificacionDeVehiculo).State = EntityState.Modified;
                return RedirectToAction("Index");
            }
            ViewBag.codRol = new SelectList(db.ROLPERSONA, "codigo", "codigo", rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeRol);
            ViewBag.CodIdVeh = new SelectList(db.TIPOVEH, "CodVeh", "codVeh", rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo);

            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(id);
            if (rolDePersonaPorTipoDeIdentificacionDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
        }

        // POST: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(id);
            db.ROLPERSONAXTIPOIDEVEHICULO.Remove(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
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
