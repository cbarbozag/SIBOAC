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
    public class ConsecutivoNumeroMarcoesController : BaseController<ConsecutivoNumeroMarco>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: ConsecutivoNumeroMarcoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.CONSECUTIVONUMEROMARCO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
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

        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
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
                    Bitacora(consecutivoNumeroMarco, "I", "CONSECUTIVONUMEROMARCO");
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
                var consecutivoNumeroMarcoAntes = db.CONSECUTIVONUMEROMARCO.AsNoTracking().Where(d => d.Id == consecutivoNumeroMarco.Id &&
                                                                                                        d.IdAnterior == consecutivoNumeroMarco.IdAnterior &&
                                                                                                        d.FechaDeInicio == consecutivoNumeroMarco.FechaDeInicio &&
                                                                                                        d.FechaDeFin == consecutivoNumeroMarco.FechaDeFin).FirstOrDefault();

                db.Entry(consecutivoNumeroMarco).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(consecutivoNumeroMarco, "U", "CONSECUTIVONUMEROMARCO", consecutivoNumeroMarcoAntes);
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
            ConsecutivoNumeroMarco consecutivoNumeroMarcoAntes = ObtenerCopia(consecutivoNumeroMarco);
            if (consecutivoNumeroMarco.Estado == "A")
                consecutivoNumeroMarco.Estado = "I";
            else
                consecutivoNumeroMarco.Estado = "A";
            db.SaveChanges();
            Bitacora(consecutivoNumeroMarco, "U", "CONSECUTIVONUMEROMARCO", consecutivoNumeroMarcoAntes);
            return RedirectToAction("Index");
        }

        // GET: ConsecutivoNumeroMarcoes/RealDelete/5
        public ActionResult RealDelete(int? id, int? consecutivoNumeroMarcoAnterior, DateTime FechaInicio, DateTime FechaFin)
        {
            if (id == null || consecutivoNumeroMarcoAnterior == null || FechaInicio == null || FechaFin == null)
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

        // POST: ConsecutivoNumeroMarcoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int? id, int? consecutivoNumeroMarcoAnterior, DateTime FechaInicio, DateTime FechaFin)
        {
            ConsecutivoNumeroMarco consecutivoNumeroMarco = db.CONSECUTIVONUMEROMARCO.Find(id, consecutivoNumeroMarcoAnterior, FechaInicio, FechaFin);
            db.CONSECUTIVONUMEROMARCO.Remove(consecutivoNumeroMarco);
            db.SaveChanges();
            Bitacora(consecutivoNumeroMarco, "D", "CONSECUTIVONUMEROMARCO");
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
