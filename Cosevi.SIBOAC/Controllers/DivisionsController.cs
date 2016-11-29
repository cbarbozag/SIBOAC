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
    public class DivisionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Divisions
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.DIVISION.ToList());
        }

        // GET: Divisions/Details/5
        public ActionResult Details(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton== null||OficinaImpugna == null|| FechaInicio== null || FechaFin ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            if (division == null)
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // GET: Divisions/Create
        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsCanton = db.CANTON
              .Select(o => new SelectListItem
              {
                  Value = o.Id.ToString(),
                  Text = o.Descripcion

              });


            ViewBag.ComboCanton = itemsCanton;

            IEnumerable<SelectListItem> itemsOficina = db.OficinaParaImpugnars
          .Select(o => new SelectListItem
          {
              Value = o.Id.ToString(),
              Text = o.Descripcion

          });

            ViewBag.ComboOficina = itemsOficina;
            return View();
        }

        public string Verificar(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            string mensaje = "";
            bool exist = db.DIVISION.Any(x => x.IdCanton == canton
                                              && x.CodigoOficinaImpugna == OficinaImpugna
                                              &&x.FechaDeInicio == FechaInicio
                                              &&x.FechaDeFin == FechaFin);
            if (exist)
            {
                mensaje = "El codigo cantón " + canton +
                           ", código Oficina impugna " + OficinaImpugna +
                           ", fecha inicio " + FechaInicio +
                           ", fecha fin " + FechaFin +
                            " ya esta registrado";
            }
            return mensaje;
        }


        // POST: Divisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCanton,CodigoOficinaImpugna,Estado,FechaDeInicio,FechaDeFin")] Division division)
        {
            if (ModelState.IsValid)
            {
                db.DIVISION.Add(division);
                string mensaje = Verificar(division.IdCanton, division.CodigoOficinaImpugna, division.FechaDeInicio, division.FechaDeFin);
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
                    return View(division);
                }
            }

            return View(division);
        }

        // GET: Divisions/Edit/5
        public ActionResult Edit(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton == null || OficinaImpugna == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            if (division == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboCanton = new SelectList(db.CANTON.OrderBy(x => x.Descripcion), "Id", "Descripcion", canton);
            ViewBag.ComboOficina = new SelectList(db.OficinaParaImpugnars.OrderBy(x => x.Descripcion), "Id", "Descripcion", OficinaImpugna);

            division.CodigoOficinaImpugna = division.CodigoOficinaImpugna.Trim();

            return View(division);
        }

        // POST: Divisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCanton,CodigoOficinaImpugna,Estado,FechaDeInicio,FechaDeFin")] Division division)
        {
            if (ModelState.IsValid)
            {
                db.Entry(division).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(division);
        }

        // GET: Divisions/Delete/5
        public ActionResult Delete(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton == null || OficinaImpugna == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            if (division == null)
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            if(division.Estado =="A")
                division.Estado = "I";
            else
                division.Estado = "A";
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
