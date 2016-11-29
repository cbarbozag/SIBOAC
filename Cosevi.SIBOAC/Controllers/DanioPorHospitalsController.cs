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
    public class DanioPorHospitalsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DanioPorHospitals
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.DAÑOXHOSPITAL.ToList());
        }

        // GET: DanioPorHospitals/Details/5
        public ActionResult Details(string IdHospital, int? IdDanio)
        {
            if (IdHospital == null|| IdDanio == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital, IdDanio);
            if (danioPorHospital == null)
            {
                return HttpNotFound();
            }
            return View(danioPorHospital);
        }

        // GET: DanioPorHospitals/Create
        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsHospital = db.HOSPITAL
              .Select(o => new SelectListItem
              {
                  Value = o.Id,
                  Text = o.Descripcion
              });
            ViewBag.ComboHospital = itemsHospital;
            IEnumerable<SelectListItem> itemsDannio = db.DAÑO
             .Select(c => new SelectListItem
             {
                 Value = c.Id.ToString(),
                 Text = c.Descripcion
             });
            ViewBag.ComboDannio = itemsDannio;
            return View();
        }

        // POST: DanioPorHospitals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdHospital,IdDanio,Estado,FechaDeInicio,FechaDeFin")] DanioPorHospital danioPorHospital)
        {
            if (ModelState.IsValid)
            {
                db.DAÑOXHOSPITAL.Add(danioPorHospital);
                string mensaje = Verificar(danioPorHospital.IdHospital,
                                               danioPorHospital.IdDanio);
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
                    return View(danioPorHospital);
                }
            }

            return View(danioPorHospital);
        }

        // GET: DanioPorHospitals/Edit/5
        public ActionResult Edit(string IdHospital, int? IdDanio)
        {
            if (IdHospital == null || IdDanio == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital, IdDanio);
            if (danioPorHospital == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboHospital = new SelectList(db.HOSPITAL.OrderBy(x => x.Descripcion), "Id", "Descripcion", IdHospital);
            ViewBag.ComboDannio = new SelectList(db.TIPOVEH.OrderBy(x => x.Descripcion), "Id", "Descripcion", IdDanio.ToString().Trim());

            return View(danioPorHospital);
        }

        // POST: DanioPorHospitals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdHospital,IdDanio,Estado,FechaDeInicio,FechaDeFin")] DanioPorHospital danioPorHospital)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danioPorHospital).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danioPorHospital);
        }

        // GET: DanioPorHospitals/Delete/5
        public ActionResult Delete(string IdHospital, int? IdDanio)
        {
            if (IdHospital == null|| IdDanio ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital, IdDanio);
            if (danioPorHospital == null)
            {
                return HttpNotFound();
            }
            return View(danioPorHospital);
        }

        // POST: DanioPorHospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string IdHospital, int IdDanio)
        {
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital,IdDanio);
            if (danioPorHospital.Estado == "A")
                danioPorHospital.Estado = "I";
            else
                danioPorHospital.Estado = "A";
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

        public string Verificar(string IdHospital, int IdDanio)
        {
            string mensaje = "";
            bool exist = db.DAÑOXHOSPITAL.Any(x => x.IdHospital == IdHospital
                                                    && x.IdDanio == IdDanio);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrado:" +
                           " código de Hospital" + IdHospital +
                           ", código Daño" + IdDanio;

            }
            return mensaje;
        }

    }
}
