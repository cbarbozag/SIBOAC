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
    public class TiposDeVehiculosController : BaseController<TiposDeVehiculos>
    {
          // GET: TiposDeVehiculos
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.TIPOSVEHICULOS.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }



        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.TIPOSVEHICULOS.Any(x => x.Id == id);
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

        // GET: TiposDeVehiculos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposDeVehiculos tiposDeVehiculos = db.TIPOSVEHICULOS.Find(id);
            if (tiposDeVehiculos == null)
            {
                return HttpNotFound();
            }
            return View(tiposDeVehiculos);
        }

        // GET: TiposDeVehiculos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TiposDeVehiculos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Estado,FechaDeInicio,FechaDeFin")] TiposDeVehiculos tiposDeVehiculos)
        {
            if (ModelState.IsValid)
            {
                db.TIPOSVEHICULOS.Add(tiposDeVehiculos);
                string mensaje = Verificar(tiposDeVehiculos.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(tiposDeVehiculos.FechaDeInicio,tiposDeVehiculos.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(tiposDeVehiculos, "I", "TIPOSVEHICULOS");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(tiposDeVehiculos);
                    }
                  

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tiposDeVehiculos);
                }

            }

            return View(tiposDeVehiculos);
        }

        // GET: TiposDeVehiculos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposDeVehiculos tiposDeVehiculos = db.TIPOSVEHICULOS.Find(id);
            if (tiposDeVehiculos == null)
            {
                return HttpNotFound();
            }
            return View(tiposDeVehiculos);
        }

        // POST: TiposDeVehiculos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Estado,FechaDeInicio,FechaDeFin")] TiposDeVehiculos tiposDeVehiculos)
        {
            if (ModelState.IsValid)
            {
                var tiposDeVehiculosAntes = db.TIPOSVEHICULOS.AsNoTracking().Where(d => d.Id == tiposDeVehiculos.Id).FirstOrDefault();

                db.Entry(tiposDeVehiculos).State = EntityState.Modified;
                string mensaje = ValidarFechas(tiposDeVehiculos.FechaDeInicio, tiposDeVehiculos.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(tiposDeVehiculos, "U", "TIPOSVEHICULOS", tiposDeVehiculosAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tiposDeVehiculos);
                }
            }
            return View(tiposDeVehiculos);
        }

        // GET: TiposDeVehiculos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposDeVehiculos tiposDeVehiculos = db.TIPOSVEHICULOS.Find(id);
            if (tiposDeVehiculos == null)
            {
                return HttpNotFound();
            }
            return View(tiposDeVehiculos);
        }

        // POST: TiposDeVehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TiposDeVehiculos tiposDeVehiculos = db.TIPOSVEHICULOS.Find(id);
            TiposDeVehiculos tiposDeVehiculosAntes = ObtenerCopia(tiposDeVehiculos);
            if (tiposDeVehiculos.Estado == "A")
                tiposDeVehiculos.Estado = "I";
            else
                tiposDeVehiculos.Estado = "A";
            db.SaveChanges();
            Bitacora(tiposDeVehiculos, "U", "TIPOSVEHICULOS", tiposDeVehiculosAntes);
            return RedirectToAction("Index");
        }

        // GET: TiposDeVehiculos/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TiposDeVehiculos tiposDeVehiculos = db.TIPOSVEHICULOS.Find(id);
            if (tiposDeVehiculos == null)
            {
                return HttpNotFound();
            }
            return View(tiposDeVehiculos);
        }

        // POST: TiposDeVehiculos/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            TiposDeVehiculos tiposDeVehiculos = db.TIPOSVEHICULOS.Find(id);
            db.TIPOSVEHICULOS.Remove(tiposDeVehiculos);
            db.SaveChanges();
            Bitacora(tiposDeVehiculos, "D", "TIPOSVEHICULOS");
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
