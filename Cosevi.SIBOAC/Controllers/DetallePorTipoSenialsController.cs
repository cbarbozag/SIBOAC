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
    public class DetallePorTipoSenialsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DetallePorTipoSenials
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.DETALLETIPOSEÑAL.ToList());
        }

        // GET: DetallePorTipoSenials/Details/5
        public ActionResult Details(string codsenex, string codigose)
        {
            if (codsenex == null || codigose == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codsenex, codigose);
            if (detallePorTipoSenial == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoSenial);
        }

        // GET: DetallePorTipoSenials/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> itemsTipoSenial = db.TIPOSEÑALEXISTE
            .Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Descripcion
            });
            ViewBag.ComboTipoSenialExiste = itemsTipoSenial;
            
            return View();
        }

        // POST: DetallePorTipoSenials/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTipoSenial,Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoSenial detallePorTipoSenial)
        {
            if (ModelState.IsValid)
            {
                db.DETALLETIPOSEÑAL.Add(detallePorTipoSenial);
                string mensaje = Verificar(detallePorTipoSenial.CodigoTipoSenial,
                                          detallePorTipoSenial.Id);
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
                    return View(detallePorTipoSenial);
                }
            }

            return View(detallePorTipoSenial);
        }

        // GET: DetallePorTipoSenials/Edit/5
        public ActionResult Edit(string codsenex, string codigose)
        {
            if (codsenex == null || codigose == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codsenex, codigose);
            if (detallePorTipoSenial == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComboTipoSenialExiste = new SelectList(db.TIPOSEÑALEXISTE.OrderBy(x => x.Descripcion), "Id", "Descripcion", codsenex);

            return View(detallePorTipoSenial);
        }

        // POST: DetallePorTipoSenials/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTipoSenial,Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoSenial detallePorTipoSenial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallePorTipoSenial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(detallePorTipoSenial);
        }

        // GET: DetallePorTipoSenials/Delete/5
        public ActionResult Delete(string codsenex, string codigose)
        {
            if (codsenex == null || codigose == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codsenex, codigose);
            if (detallePorTipoSenial == null)
            {
                return HttpNotFound();
            }
            return View(detallePorTipoSenial);
        }

        // POST: DetallePorTipoSenials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codsenex, string codigose)
        {
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codsenex, codigose);
            if (detallePorTipoSenial.Estado == "A")
                detallePorTipoSenial.Estado = "I";
            else
                detallePorTipoSenial.Estado = "A";
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

        public string Verificar(string codsenex, string codigose)
        {
            string mensaje = "";
            bool exist = db.DETALLETIPOSEÑAL.Any(x => x.CodigoTipoSenial == codsenex
                                                    && x.Id == codigose);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados:" +
                           " código de Tipo Señal" + codsenex +
                           ", código" + codigose;

            }
            return mensaje;
        }
    }
}
