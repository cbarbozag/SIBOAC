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
    public class TipoIdDeVehiculoesController : BaseController<TipoIdDeVehiculo>
    {
        
        // GET: TipoIdDeVehiculoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.TIPOIDEVEHICULO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.TIPOIDEVEHICULO.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El código " + id + " ya esta registrado";
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
        // GET: TipoIdDeVehiculoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            if (tipoIdDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoIdDeVehiculo);
        }

        // GET: TipoIdDeVehiculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoIdDeVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoIdDeVehiculo tipoIdDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.TIPOIDEVEHICULO.Add(tipoIdDeVehiculo);
                string mensaje = Verificar(tipoIdDeVehiculo.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(tipoIdDeVehiculo.FechaDeInicio, tipoIdDeVehiculo.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(tipoIdDeVehiculo, "I", "TIPOIDEVEHICULO");

                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(tipoIdDeVehiculo);
                    }

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoIdDeVehiculo);
                }
            }

            return View(tipoIdDeVehiculo);
        }

        // GET: TipoIdDeVehiculoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            if (tipoIdDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoIdDeVehiculo);
        }

        // POST: TipoIdDeVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoIdDeVehiculo tipoIdDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                var tipoIdDeVehiculoAntes = db.TIPOIDEVEHICULO.AsNoTracking().Where(d => d.Id == tipoIdDeVehiculo.Id).FirstOrDefault();

                db.Entry(tipoIdDeVehiculo).State = EntityState.Modified;
                string mensaje = ValidarFechas(tipoIdDeVehiculo.FechaDeInicio, tipoIdDeVehiculo.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(tipoIdDeVehiculo, "U", "TIPOIDEVEHICULO", tipoIdDeVehiculoAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoIdDeVehiculo);
                }
            }
            return View(tipoIdDeVehiculo);
        }

        // GET: TipoIdDeVehiculoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            if (tipoIdDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoIdDeVehiculo);
        }

        // POST: TipoIdDeVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            TipoIdDeVehiculo tipoIdDeVehiculoAntes = ObtenerCopia(tipoIdDeVehiculo);
            if (tipoIdDeVehiculo.Estado == "I")
                tipoIdDeVehiculo.Estado = "A";
            else
                tipoIdDeVehiculo.Estado = "I";
            db.SaveChanges();
            Bitacora(tipoIdDeVehiculo, "U", "TIPOIDEVEHICULO", tipoIdDeVehiculoAntes);
            return RedirectToAction("Index");
        }

        // GET: TipoIdDeVehiculoes/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            if (tipoIdDeVehiculo == null)
            {
                return HttpNotFound();
            }
            return View(tipoIdDeVehiculo);
        }

        // POST: TipoIdDeVehiculoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            TipoIdDeVehiculo tipoIdDeVehiculo = db.TIPOIDEVEHICULO.Find(id);
            db.TIPOIDEVEHICULO.Remove(tipoIdDeVehiculo);
            db.SaveChanges();
            Bitacora(tipoIdDeVehiculo, "D", "TIPOIDEVEHICULO");
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
