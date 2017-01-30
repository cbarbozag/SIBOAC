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
    public class CodigoDeLaPlacasController : BaseController<CodigoDeLaPlaca>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: CodigoDeLaPlacas
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
                        
            var list = db.CODIGO.ToList();            

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.CODIGO.Any(x => x.Id == id);
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

        // GET: CodigoDeLaPlacas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            if (codigoDeLaPlaca == null)
            {
                return HttpNotFound();
            }
            return View(codigoDeLaPlaca);
        }

        // GET: CodigoDeLaPlacas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CodigoDeLaPlacas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] CodigoDeLaPlaca codigoDeLaPlaca)
        {
            if (ModelState.IsValid)
            {
                db.CODIGO.Add(codigoDeLaPlaca);
                string mensaje = Verificar(codigoDeLaPlaca.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(codigoDeLaPlaca.FechaDeInicio, codigoDeLaPlaca.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(codigoDeLaPlaca, "I", "CODIGO");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(codigoDeLaPlaca);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(codigoDeLaPlaca);
                }
            }

            return View(codigoDeLaPlaca);
        }

        // GET: CodigoDeLaPlacas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            if (codigoDeLaPlaca == null)
            {
                return HttpNotFound();
            }
            return View(codigoDeLaPlaca);
        }

        // POST: CodigoDeLaPlacas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] CodigoDeLaPlaca codigoDeLaPlaca)
        {
            if (ModelState.IsValid)
            {
                var codigoDeLaPlacaAntes = db.CODIGO.AsNoTracking().Where(d => d.Id == codigoDeLaPlaca.Id).FirstOrDefault();

                db.Entry(codigoDeLaPlaca).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(codigoDeLaPlaca, "U", "CODIGO", codigoDeLaPlacaAntes);
                return RedirectToAction("Index");
            }
            return View(codigoDeLaPlaca);
        }

        // GET: CodigoDeLaPlacas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            if (codigoDeLaPlaca == null)
            {
                return HttpNotFound();
            }
            return View(codigoDeLaPlaca);
        }

        // POST: CodigoDeLaPlacas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            CodigoDeLaPlaca codigoDeLaPlacaAntes = ObtenerCopia(codigoDeLaPlaca);

            if (codigoDeLaPlaca.Estado == "A")
                codigoDeLaPlaca.Estado = "I";
            else
                codigoDeLaPlaca.Estado = "A";
            db.SaveChanges();
            Bitacora(codigoDeLaPlaca, "U", "CODIGO", codigoDeLaPlacaAntes);
            return RedirectToAction("Index");
        }


        // GET: CodigoDeLaPlacas/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            if (codigoDeLaPlaca == null)
            {
                return HttpNotFound();
            }
            return View(codigoDeLaPlaca);
        }

        // POST: CodigoDeLaPlacas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            CodigoDeLaPlaca codigoDeLaPlaca = db.CODIGO.Find(id);
            db.CODIGO.Remove(codigoDeLaPlaca);
            db.SaveChanges();
            Bitacora(codigoDeLaPlaca, "D", "CODIGO");
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
