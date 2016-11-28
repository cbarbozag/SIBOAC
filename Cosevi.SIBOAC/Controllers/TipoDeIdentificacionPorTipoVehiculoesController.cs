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
    public class TipoDeIdentificacionPorTipoVehiculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoDeIdentificacionPorTipoVehiculoes
        public ActionResult Index()
        {
            return View(db.TIPOIDEVEHICULOXTIPOVEH.ToList());
        }

        // GET: TipoDeIdentificacionPorTipoVehiculoes/Details/5
        public ActionResult Details(string Codigo, int? CodVeh)
        {
            if (Codigo== null ||CodVeh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TipoDeIdentificacionPorTipoVehiculo tipoDeIdentificacionPorTipoVehiculo = db.TIPOIDEVEHICULOXTIPOVEH.Find(Codigo,CodVeh);
            if (tipoDeIdentificacionPorTipoVehiculo == null)
            {
                return HttpNotFound();
            }
           
            return View(tipoDeIdentificacionPorTipoVehiculo);
        }

        // GET: TipoDeIdentificacionPorTipoVehiculoes/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> itemsTipoIdVehiculo = db.TIPOIDEVEHICULO
           .Select(o => new SelectListItem
           {
               Value = o.Id,
               Text = o.Descripcion
           });
            ViewBag.ComboTipoIdVehiculo = itemsTipoIdVehiculo;
            IEnumerable<SelectListItem> itemsTipoVeh = db.TIPOVEH
             .Select(c => new SelectListItem
             {
                 Value = c.Id.ToString(),
                 Text = c.Descripcion
             });
            ViewBag.ComboTipoVeh = itemsTipoVeh;
            return View();
        }

        // POST: TipoDeIdentificacionPorTipoVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTipoIDEVehiculo,CodigoTipoVeh,Estado,FechaDeInicio,FechaDeFin")] TipoDeIdentificacionPorTipoVehiculo tipoDeIdentificacionPorTipoVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.TIPOIDEVEHICULOXTIPOVEH.Add(tipoDeIdentificacionPorTipoVehiculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDeIdentificacionPorTipoVehiculo);
        }

        // GET: TipoDeIdentificacionPorTipoVehiculoes/Edit/5
        public ActionResult Edit(string Codigo, int? CodVeh)
        {
            if (Codigo == null || CodVeh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeIdentificacionPorTipoVehiculo tipoDeIdentificacionPorTipoVehiculo = db.TIPOIDEVEHICULOXTIPOVEH.Find(Codigo,CodVeh);
            if (tipoDeIdentificacionPorTipoVehiculo == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboTipoIdVehiculo = new SelectList(db.TIPOIDEVEHICULO.OrderBy(x => x.Descripcion), "Id", "Descripcion", Codigo);
            ViewBag.ComboCodVeh = new SelectList(db.TIPOVEH.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodVeh);

            return View(tipoDeIdentificacionPorTipoVehiculo);
        }

        // POST: TipoDeIdentificacionPorTipoVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTipoIDEVehiculo,CodigoTipoVeh,Estado,FechaDeInicio,FechaDeFin")] TipoDeIdentificacionPorTipoVehiculo tipoDeIdentificacionPorTipoVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeIdentificacionPorTipoVehiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDeIdentificacionPorTipoVehiculo);
        }

        // GET: TipoDeIdentificacionPorTipoVehiculoes/Delete/5
        public ActionResult Delete(string Codigo, int? CodVeh)
        {
            if (Codigo  == null|| CodVeh ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeIdentificacionPorTipoVehiculo tipoDeIdentificacionPorTipoVehiculo = db.TIPOIDEVEHICULOXTIPOVEH.Find(Codigo,CodVeh);
            if (tipoDeIdentificacionPorTipoVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeIdentificacionPorTipoVehiculo);
        }

        // POST: TipoDeIdentificacionPorTipoVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string Codigo, int? CodVeh)
        {
            TipoDeIdentificacionPorTipoVehiculo tipoDeIdentificacionPorTipoVehiculo = db.TIPOIDEVEHICULOXTIPOVEH.Find(Codigo,CodVeh);
            if (tipoDeIdentificacionPorTipoVehiculo.Estado == "I")
                tipoDeIdentificacionPorTipoVehiculo.Estado = "A";
            else
                tipoDeIdentificacionPorTipoVehiculo.Estado = "I";
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
