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
    public class RutasController : BaseController<Ruta>
    {


        // GET: Rutas
        [SessionExpire]
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list = from s in db.Ruta.ToList() select s;            

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.Id.ToString().ToUpper().Contains(searchString.ToUpper())
                                        || s.DescripcionRuta.ToUpper().Contains(searchString.ToUpper())
                                        || s.Inicia.Contains(searchString)
                                        || s.Termina.Contains(searchString)
                                        || s.Estado.ToUpper().Contains(searchString.ToUpper()));
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.Ruta.Any(x => x.Id == id);
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
        // GET: Rutas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ruta ruta = db.Ruta.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        // GET: Rutas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rutas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Inicia,Termina,Estado,FechaDeInicio,FechaDeFin")] Ruta ruta)
        {
            if (ModelState.IsValid)
            {
                db.Ruta.Add(ruta);
                string mensaje = Verificar(ruta.Id);

                if (mensaje == "")
                {
                    mensaje = ValidarFechas(ruta.FechaDeInicio, ruta.FechaDeFin);

                    if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(ruta, "I", "RUTA");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(ruta);
                }

                }

                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(ruta);
                }
            }

            return View(ruta);
        }

        // GET: Rutas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ruta ruta = db.Ruta.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        // POST: Rutas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Inicia,Termina,Estado,FechaDeInicio,FechaDeFin")] Ruta ruta)
        {
            if (ModelState.IsValid)
            {
                var rutaAntes = db.Ruta.AsNoTracking().Where(d => d.Id == ruta.Id).FirstOrDefault();
                db.Entry(ruta).State = EntityState.Modified;

                string mensaje = ValidarFechas(ruta.FechaDeInicio, ruta.FechaDeFin);
                if (mensaje == "")
                {

                    db.SaveChanges();
                    Bitacora(ruta, "U", "RUTA", rutaAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(ruta);
                }
            }
                return View(ruta);
        }

        // GET: Rutas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ruta ruta = db.Ruta.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        // POST: Rutas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ruta ruta = db.Ruta.Find(id);
            Ruta rutaAntes = ObtenerCopia(ruta);
            if (ruta.Estado=="I")
                  ruta.Estado = "A";
            else
                ruta.Estado = "I";
            db.SaveChanges();
            Bitacora(ruta, "U", "RUTA", rutaAntes);
            return RedirectToAction("Index");
        }

        // GET: Rutas/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ruta ruta = db.Ruta.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        // POST: Rutas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Ruta ruta = db.Ruta.Find(id);
            db.Ruta.Remove(ruta);
            db.SaveChanges();
            Bitacora(ruta, "D", "RUTA");
            TempData["Type"] = "error";
            TempData["Message"] = "El registro se eliminó correctamente";
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
