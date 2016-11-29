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
            return View(db.DETALLETIPODAÑO.ToList());
        }

        // GET: DetallePorTipoDanios/Details/5
        public ActionResult Details(string codigod, string codigotd )
        {
            if (codigod == null  || codigotd == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);
            if (detallePorTipoDanio == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoDanio);
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
            DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);
            if (detallePorTipoDanio == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoDanio);
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
            DetallePorTipoDanio detallePorTipoDanio = db.DETALLETIPODAÑO.Find(codigod, codigotd);
            if (detallePorTipoDanio == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoDanio);
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
