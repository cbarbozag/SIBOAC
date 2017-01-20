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
    public class DepositoDePlacasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DepositoDePlacas
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.DEPOSITOPLACA.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.DEPOSITOPLACA.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
        }

        // GET: DepositoDePlacas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            if (depositoDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(depositoDePlaca);
        }

        // GET: DepositoDePlacas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepositoDePlacas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DepositoDePlaca depositoDePlaca)
        {
            if (ModelState.IsValid)
            {
                db.DEPOSITOPLACA.Add(depositoDePlaca);
                string mensaje = Verificar(depositoDePlaca.Id);
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
                    return View(depositoDePlaca);
                }
            }

            return View(depositoDePlaca);
        }

        // GET: DepositoDePlacas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            if (depositoDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(depositoDePlaca);
        }

        // POST: DepositoDePlacas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DepositoDePlaca depositoDePlaca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depositoDePlaca).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(depositoDePlaca);
        }

        // GET: DepositoDePlacas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            if (depositoDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(depositoDePlaca);
        }

        // POST: DepositoDePlacas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            if (depositoDePlaca.Estado == "I")
                depositoDePlaca.Estado = "A";
            else
                depositoDePlaca.Estado = "I";
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: DepositoDePlacas/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            if (depositoDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(depositoDePlaca);
        }

        // POST: DepositoDePlacas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            DepositoDePlaca depositoDePlaca = db.DEPOSITOPLACA.Find(id);
            db.DEPOSITOPLACA.Remove(depositoDePlaca);
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
