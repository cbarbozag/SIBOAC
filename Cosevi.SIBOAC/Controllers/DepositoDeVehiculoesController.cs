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
    public class DepositoDeVehiculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DepositoDeVehiculoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.DEPOSITOVEHICULO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.DEPOSITOVEHICULO.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: DepositoDeVehiculoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            if (depositoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(depositoDeVehiculo);
        }

        // GET: DepositoDeVehiculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepositoDeVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DepositoDeVehiculo depositoDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.DEPOSITOVEHICULO.Add(depositoDeVehiculo);
                string mensaje = Verificar(depositoDeVehiculo.Id);
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
                    return View(depositoDeVehiculo);
                }
            }

            return View(depositoDeVehiculo);
        }

        // GET: DepositoDeVehiculoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            if (depositoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(depositoDeVehiculo);
        }

        // POST: DepositoDeVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DepositoDeVehiculo depositoDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depositoDeVehiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depositoDeVehiculo);
        }

        // GET: DepositoDeVehiculoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            if (depositoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(depositoDeVehiculo);
        }

        // POST: DepositoDeVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            if (depositoDeVehiculo.Estado == "I")
                depositoDeVehiculo.Estado = "A";
            else
                depositoDeVehiculo.Estado = "I";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: DepositoDeVehiculoes/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            if (depositoDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(depositoDeVehiculo);
        }

        // POST: DepositoDeVehiculoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            DepositoDeVehiculo depositoDeVehiculo = db.DEPOSITOVEHICULO.Find(id);
            db.DEPOSITOVEHICULO.Remove(depositoDeVehiculo);
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
