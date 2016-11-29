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
    public class ConsecutivoNumeroMarcoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: ConsecutivoNumeroMarcoes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.CONSECUTIVONUMEROMARCO.ToList());
        }

        // GET: ConsecutivoNumeroMarcoes/Details/5
        public ActionResult Details(int? id, int? consecutivoNumeroMarcoAnterior, DateTime FechaInicio, DateTime FechaFin)
        {
            if (id == null || consecutivoNumeroMarcoAnterior==null  || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsecutivoNumeroMarco consecutivoNumeroMarco = db.CONSECUTIVONUMEROMARCO.Find(id,consecutivoNumeroMarcoAnterior,FechaInicio,FechaFin);
            if (consecutivoNumeroMarco == null)
            {
                return HttpNotFound();
            }
            return View(consecutivoNumeroMarco);
        }

        // GET: ConsecutivoNumeroMarcoes/Create
        public ActionResult Create()
        {
            return View();
        }

        public string Verificar(int? id, int? consecutivoNumeroMarcoAnterior, DateTime FechaInicio, DateTime FechaFin)
        {
            string mensaje = "";
            bool exist = db.CONSECUTIVONUMEROMARCO.Any(x => x.Id == id
                                                       &&x.IdAnterior == consecutivoNumeroMarcoAnterior
                                                       &&x.FechaDeInicio == FechaInicio
                                                       &&x.FechaDeFin == FechaFin);
            if (exist)
            {
                mensaje = "El codigo consecutivo marco " + id + 
                           ", código anterior marco "+ consecutivoNumeroMarcoAnterior+
                           ", Fecha Inicio "+ FechaInicio +
                           ", Fecha Fin "+ FechaFin+
                            " ya esta registrado";
            }
            return mensaje;
        }


        // POST: ConsecutivoNumeroMarcoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdAnterior,Estado,FechaDeInicio,FechaDeFin")] ConsecutivoNumeroMarco consecutivoNumeroMarco)
        {
            if (ModelState.IsValid)
            {
                db.CONSECUTIVONUMEROMARCO.Add(consecutivoNumeroMarco);
                string mensaje = Verificar(consecutivoNumeroMarco.Id, consecutivoNumeroMarco.IdAnterior, consecutivoNumeroMarco.FechaDeInicio, consecutivoNumeroMarco.FechaDeFin);
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
                    return View(consecutivoNumeroMarco);
                }
            }

            return View(consecutivoNumeroMarco);
        }

        // GET: ConsecutivoNumeroMarcoes/Edit/5
        public ActionResult Edit(int? id, int? consecutivoNumeroMarcoAnterior, DateTime FechaInicio, DateTime FechaFin)
        {
            if (id == null || consecutivoNumeroMarcoAnterior==null  || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsecutivoNumeroMarco consecutivoNumeroMarco = db.CONSECUTIVONUMEROMARCO.Find(id, consecutivoNumeroMarcoAnterior, FechaInicio, FechaFin);
            if (consecutivoNumeroMarco == null)
            {
                return HttpNotFound();
            }
            return View(consecutivoNumeroMarco);
        }

        // POST: ConsecutivoNumeroMarcoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdAnterior,Estado,FechaDeInicio,FechaDeFin")] ConsecutivoNumeroMarco consecutivoNumeroMarco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consecutivoNumeroMarco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consecutivoNumeroMarco);
        }

        // GET: ConsecutivoNumeroMarcoes/Delete/5
        public ActionResult Delete(int? id, int? consecutivoNumeroMarcoAnterior, DateTime FechaInicio, DateTime FechaFin)
        {
            if (id == null || consecutivoNumeroMarcoAnterior == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsecutivoNumeroMarco consecutivoNumeroMarco = db.CONSECUTIVONUMEROMARCO.Find(id,consecutivoNumeroMarcoAnterior,FechaInicio,FechaFin);
            if (consecutivoNumeroMarco == null)
            {
                return HttpNotFound();
            }
            return View(consecutivoNumeroMarco);
        }

        // POST: ConsecutivoNumeroMarcoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, int? consecutivoNumeroMarcoAnterior, DateTime FechaInicio, DateTime FechaFin)
        {
            ConsecutivoNumeroMarco consecutivoNumeroMarco = db.CONSECUTIVONUMEROMARCO.Find(id, consecutivoNumeroMarcoAnterior, FechaInicio, FechaFin);
            if (consecutivoNumeroMarco.Estado == "A")
                consecutivoNumeroMarco.Estado = "I";
            else
                consecutivoNumeroMarco.Estado = "A";
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
