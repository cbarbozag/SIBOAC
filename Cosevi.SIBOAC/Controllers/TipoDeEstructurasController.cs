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
    public class TipoDeEstructurasController : BaseController<TipoDeEstructura>
    {
        // GET: TipoDeEstructuras
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.ESTRUCTURA.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.ESTRUCTURA.Any(x => x.Id == id);
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
        // GET: TipoDeEstructuras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // GET: TipoDeEstructuras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeEstructuras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeEstructura tipoDeEstructura)
        {
            if (ModelState.IsValid)
            {
                db.ESTRUCTURA.Add(tipoDeEstructura);
                string mensaje = Verificar(tipoDeEstructura.Id);

                if (mensaje == "")
                {
                    mensaje = ValidarFechas(tipoDeEstructura.FechaDeInicio.Value, tipoDeEstructura.FechaDeFin.Value);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(tipoDeEstructura, "I", "ESTRUCTURA");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }

                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoDeEstructura);
                }

            }
            else
            {
                ViewBag.Type = "warning";
                ViewBag.Message = mensaje;
                return View(tipoDeEstructura);
            }
        }

            return View(tipoDeEstructura);
        }

        // GET: TipoDeEstructuras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // POST: TipoDeEstructuras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeEstructura tipoDeEstructura)
        {
            if (ModelState.IsValid)
            {
                var tipoDeEstructuraAntes = db.ESTRUCTURA.AsNoTracking().Where(d => d.Id == tipoDeEstructura.Id).FirstOrDefault();

                db.Entry(tipoDeEstructura).State = EntityState.Modified;

                string mensaje = ValidarFechas(tipoDeEstructura.FechaDeInicio.Value, tipoDeEstructura.FechaDeFin.Value);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(tipoDeEstructura, "U", "ESTRUCTURA", tipoDeEstructuraAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoDeEstructura);
                }
            }
            return View(tipoDeEstructura);
        }

        // GET: TipoDeEstructuras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // POST: TipoDeEstructuras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            TipoDeEstructura tipoDeEstructuraAntes = ObtenerCopia(tipoDeEstructura);

            if (tipoDeEstructura.Estado == "I")
                tipoDeEstructura.Estado = "A";
            else
                tipoDeEstructura.Estado = "I";
            db.SaveChanges();
            Bitacora(tipoDeEstructura, "U", "ESTRUCTURA", tipoDeEstructuraAntes);
            return RedirectToAction("Index");
        }

        // GET: TipoDeEstructuras/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // POST: TipoDeEstructuras/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            db.ESTRUCTURA.Remove(tipoDeEstructura);
            db.SaveChanges();
            Bitacora(tipoDeEstructura, "D", "ESTRUCTURA");
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
