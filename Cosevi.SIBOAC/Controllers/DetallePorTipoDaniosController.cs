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
    public class DetallePorTipoDaniosController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DetallePorTipoDanios
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list =
            (from dtd in db.DETALLETIPODAÑO
             join da in db.DAÑO on new { CodigoDanio = (string)dtd.CodigoDanio } equals new { CodigoDanio = (da.Id.ToString()) } into da_join
             from da in da_join.DefaultIfEmpty()
             join td in db.TIPODANO on new { CodigoTipoDanio = (string)dtd.CodigoTipoDanio } equals new { CodigoTipoDanio = td.codigod } into td_join
             from td in td_join.DefaultIfEmpty()
             select new
             {
                 CodigoDanio = dtd.CodigoDanio,
                 CodigoTipoDanio = dtd.CodigoTipoDanio,
                 Descripcion = dtd.Descripcion,
                 Estado = dtd.Estado,
                 FechaDeInicio = dtd.FechaDeInicio,
                 FechaDeFin = dtd.FechaDeFin,
                 DescripcionCodigoDano = da.Descripcion,
                 DescripcionCodigoTipoDano = td.descripcion

             }).ToList()


             .Select(x => new DetallePorTipoDanio
             {
                 CodigoDanio = x.CodigoDanio,
                 CodigoTipoDanio = x.CodigoTipoDanio,
                 Descripcion = x.Descripcion,
                 Estado = x.Estado,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 DescripcionCodigoDano = x.DescripcionCodigoDano,
                 DescripcionCodigoTipoDano = x.DescripcionCodigoTipoDano

             });

            return View(list);
        }

        // GET: DetallePorTipoDanios/Details/5
        public ActionResult Details(string codigod, string codigotd )
        {
            if (codigod == null  || codigotd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);


            var list =
            (from dtd in db.DETALLETIPODAÑO
             join da in db.DAÑO on new { CodigoDanio = (string)dtd.CodigoDanio } equals new { CodigoDanio = (da.Id.ToString()) } into da_join
             where dtd.CodigoDanio == codigod
             from da in da_join.DefaultIfEmpty()
             join td in db.TIPODANO on new { CodigoTipoDanio = (string)dtd.CodigoTipoDanio } equals new { CodigoTipoDanio = td.codigod } into td_join
             where dtd.CodigoTipoDanio == codigotd
             from td in td_join.DefaultIfEmpty()
             select new
             {
                 CodigoDanio = dtd.CodigoDanio,
                 CodigoTipoDanio = dtd.CodigoTipoDanio,
                 Descripcion = dtd.Descripcion,
                 Estado = dtd.Estado,
                 FechaDeInicio = dtd.FechaDeInicio,
                 FechaDeFin = dtd.FechaDeFin,
                 DescripcionCodigoDano = da.Descripcion,
                 DescripcionCodigoTipoDano = td.descripcion

             }).ToList()


             .Select(x => new DetallePorTipoDanio
             {
                 CodigoDanio = x.CodigoDanio,
                 CodigoTipoDanio = x.CodigoTipoDanio,
                 Descripcion = x.Descripcion,
                 Estado = x.Estado,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 DescripcionCodigoDano = x.DescripcionCodigoDano,
                 DescripcionCodigoTipoDano = x.DescripcionCodigoTipoDano

             }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: DetallePorTipoDanios/Create
        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsDanio = db.DAÑO
              .Select(o => new SelectListItem
              {
                  Value = o.Id.ToString(),
                  Text = o.Descripcion
              });
            ViewBag.ComboDanio = itemsDanio;
            
            IEnumerable<SelectListItem> itemsTipoDanio = db.TIPODANO
              .Select(o => new SelectListItem
              {
                  Value = o.codigod,
                  Text = o.descripcion
              });
            ViewBag.ComboTipoDanio = itemsTipoDanio;

            return View();
        }
        public string Verificar(string codigod, string codigotd)
        {
            string mensaje = "";
            bool exist = db.DETALLETIPODAÑO.Any(x => x.CodigoDanio == codigod
                                                       && x.CodigoTipoDanio == codigotd);
            if (exist)
            {
                mensaje = "El codigo daño " + codigod +
                           ", código tipo daño " + codigotd +
                            " ya esta registrado";
            }
            return mensaje;
        }

        // POST: DetallePorTipoDanios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoDanio,CodigoTipoDanio,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoDanio detallePorTipoDanio)
        {
            if (ModelState.IsValid)
            {
                db.DETALLETIPODAÑO.Add(detallePorTipoDanio);
                string mensaje = Verificar(detallePorTipoDanio.CodigoDanio, detallePorTipoDanio.CodigoTipoDanio);
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
                    return View(detallePorTipoDanio);
                }
            }

            return View(detallePorTipoDanio);
        }

        // GET: DetallePorTipoDanios/Edit/5
        public ActionResult Edit(string codigod, string codigotd)
        {
            if (codigod == null || codigotd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);


            var list =
            (from dtd in db.DETALLETIPODAÑO
             join da in db.DAÑO on new { CodigoDanio = (string)dtd.CodigoDanio } equals new { CodigoDanio = (da.Id.ToString()) } into da_join
             where dtd.CodigoDanio == codigod
             from da in da_join.DefaultIfEmpty()
             join td in db.TIPODANO on new { CodigoTipoDanio = (string)dtd.CodigoTipoDanio } equals new { CodigoTipoDanio = td.codigod } into td_join
             where dtd.CodigoTipoDanio == codigotd
             from td in td_join.DefaultIfEmpty()
             select new
             {
                 CodigoDanio = dtd.CodigoDanio,
                 CodigoTipoDanio = dtd.CodigoTipoDanio,
                 Descripcion = dtd.Descripcion,
                 Estado = dtd.Estado,
                 FechaDeInicio = dtd.FechaDeInicio,
                 FechaDeFin = dtd.FechaDeFin,
                 DescripcionCodigoDano = da.Descripcion,
                 DescripcionCodigoTipoDano = td.descripcion

             }).ToList()


             .Select(x => new DetallePorTipoDanio
             {
                 CodigoDanio = x.CodigoDanio,
                 CodigoTipoDanio = x.CodigoTipoDanio,
                 Descripcion = x.Descripcion,
                 Estado = x.Estado,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 DescripcionCodigoDano = x.DescripcionCodigoDano,
                 DescripcionCodigoTipoDano = x.DescripcionCodigoTipoDano

             }).SingleOrDefault();



            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboDanio = new SelectList(db.DAÑO.OrderBy(x => x.Descripcion), "Id", "Descripcion", codigod);
            ViewBag.ComboTipoDanio = new SelectList(db.TIPODANO.OrderBy(x => x.descripcion), "codigod", "descripcion", codigotd);
            return View(list);
        }

        // POST: DetallePorTipoDanios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoDanio,CodigoTipoDanio,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoDanio detallePorTipoDanio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallePorTipoDanio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(detallePorTipoDanio);
        }

        // GET: DetallePorTipoDanios/Delete/5
        public ActionResult Delete(string codigod, string codigotd)
        {
            if (codigod == null || codigotd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);


            var list =
            (from dtd in db.DETALLETIPODAÑO
             join da in db.DAÑO on new { CodigoDanio = (string)dtd.CodigoDanio } equals new { CodigoDanio = (da.Id.ToString()) } into da_join
             where dtd.CodigoDanio == codigod
             from da in da_join.DefaultIfEmpty()
             join td in db.TIPODANO on new { CodigoTipoDanio = (string)dtd.CodigoTipoDanio } equals new { CodigoTipoDanio = td.codigod } into td_join
             where dtd.CodigoTipoDanio == codigotd
             from td in td_join.DefaultIfEmpty()
             select new
             {
                 CodigoDanio = dtd.CodigoDanio,
                 CodigoTipoDanio = dtd.CodigoTipoDanio,
                 Descripcion = dtd.Descripcion,
                 Estado = dtd.Estado,
                 FechaDeInicio = dtd.FechaDeInicio,
                 FechaDeFin = dtd.FechaDeFin,
                 DescripcionCodigoDano = da.Descripcion,
                 DescripcionCodigoTipoDano = td.descripcion

             }).ToList()


             .Select(x => new DetallePorTipoDanio
             {
                 CodigoDanio = x.CodigoDanio,
                 CodigoTipoDanio = x.CodigoTipoDanio,
                 Descripcion = x.Descripcion,
                 Estado = x.Estado,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 DescripcionCodigoDano = x.DescripcionCodigoDano,
                 DescripcionCodigoTipoDano = x.DescripcionCodigoTipoDano

             }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: DetallePorTipoDanios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codigod, string codigotd)
        {
            DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);
            if (detallePorTipoDanio.Estado == "A")
                detallePorTipoDanio.Estado = "I";
            else
                detallePorTipoDanio.Estado = "A";
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
