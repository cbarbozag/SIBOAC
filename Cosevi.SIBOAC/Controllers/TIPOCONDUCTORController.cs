using Cosevi.SIBOAC.Controllers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Cosevi.SIBOAC.Models
{
    public class TIPOCONDUCTORController : BaseController<TIPOCONDUCTOR>
    {


        // GET: Sexoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.TIPOCONDUCTOR.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.TIPOCONDUCTOR.Any(x => x.codigo == id);
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
        // GET: Sexoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPOCONDUCTOR tipo = db.TIPOCONDUCTOR.Find(id);
            if (tipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo);
        }

        // GET: Sexoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sexoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codigo,descripcion,estado,fecha_inicio,fecha_fin")] TIPOCONDUCTOR tipo)
        {
            if (ModelState.IsValid)
            {
                db.TIPOCONDUCTOR.Add(tipo);
                string mensaje = Verificar(tipo.codigo);

                if (mensaje == "")
                {
                    mensaje = ValidarFechas(tipo.fecha_inicio, tipo.fecha_fin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(tipo, "I", "TIPO CONDUCTOR");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(tipo);

                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipo);
                }
                }

            return View(tipo);
        }

        // GET: Sexoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPOCONDUCTOR tipo = db.TIPOCONDUCTOR.Find(id);
            if (tipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo);
        }

        // POST: Sexoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codigo,descripcion,estado,fecha_inicio,fecha_fin")] TIPOCONDUCTOR tipo)
        {
            var tipoAntes = db.TIPOCONDUCTOR.AsNoTracking().Where(d => d.codigo == tipo.codigo).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(tipo).State = EntityState.Modified;

                string mensaje = ValidarFechas(tipo.fecha_inicio, tipo.fecha_fin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(tipo, "U", "TIPO CONDUCTOR", tipoAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipo);
                }
            }

                return View(tipo);
        }

        // GET: Sexoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPOCONDUCTOR tipo = db.TIPOCONDUCTOR.Find(id);
            if (tipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo);
        }

        // POST: Sexoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TIPOCONDUCTOR tipo = db.TIPOCONDUCTOR.Find(id);
            TIPOCONDUCTOR tipoAntes = ObtenerCopia(tipo);
            if (tipo.estado == "I")
                tipo.estado = "A";
            else
                tipo.estado = "I";
            db.SaveChanges();
            Bitacora(tipo, "U", "TIPO CONDUCTOR", tipoAntes);
            return RedirectToAction("Index");
        }

        // GET: Sexoes/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPOCONDUCTOR tipo = db.TIPOCONDUCTOR.Find(id);
            if (tipo == null)
            {
                return HttpNotFound();
            }
            return View(tipo);
        }

        // POST: Sexoes/Delete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            TIPOCONDUCTOR tipo = db.TIPOCONDUCTOR.Find(id);
            db.TIPOCONDUCTOR.Remove(tipo);
            db.SaveChanges();
            Bitacora(tipo, "D", "TIPO CONDICTOR");
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
