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
    public class TipoDeCalzadasController : BaseController<TipoDeCalzada>
    {
        // GET: TipoDeCalzadas
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.TIPOCALZADA.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.TIPOCALZADA.Any(x => x.Id == id);
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

        // GET: TipoDeCalzadas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeCalzada tipoDeCalzada = db.TIPOCALZADA.Find(id);
            if (tipoDeCalzada == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeCalzada);
        }

        // GET: TipoDeCalzadas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeCalzadas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeCalzada tipoDeCalzada)
        {
            if (ModelState.IsValid)
            {
                db.TIPOCALZADA.Add(tipoDeCalzada);
                string mensaje = Verificar(tipoDeCalzada.Id);

                if (mensaje == "")
                {
                    mensaje = ValidarFechas(tipoDeCalzada.FechaDeInicio.Value, tipoDeCalzada.FechaDeFin.Value);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(tipoDeCalzada, "I", "TIPOCALZADA");

                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoDeCalzada);
                }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoDeCalzada);
                }
            }

            return View(tipoDeCalzada);
        }

        // GET: TipoDeCalzadas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeCalzada tipoDeCalzada = db.TIPOCALZADA.Find(id);
            if (tipoDeCalzada == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeCalzada);
        }

        // POST: TipoDeCalzadas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeCalzada tipoDeCalzada)
        {
            if (ModelState.IsValid)
            {

                var tipoDeCalzadaAntes = db.TIPOCALZADA.AsNoTracking().Where(d => d.Id == tipoDeCalzada.Id).FirstOrDefault();

                db.Entry(tipoDeCalzada).State = EntityState.Modified;


                string mensaje = ValidarFechas(tipoDeCalzada.FechaDeInicio.Value, tipoDeCalzada.FechaDeFin.Value);
                if (mensaje == "")
                {

                    db.SaveChanges();
                    Bitacora(tipoDeCalzada, "U", "TIPOCALZADA", tipoDeCalzadaAntes);
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tipoDeCalzada);
                }
            }
                return View(tipoDeCalzada);
        }

        // GET: TipoDeCalzadas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeCalzada tipoDeCalzada = db.TIPOCALZADA.Find(id);
            if (tipoDeCalzada == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeCalzada);
        }

        // POST: TipoDeCalzadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDeCalzada tipoDeCalzada = db.TIPOCALZADA.Find(id);
            TipoDeCalzada tipoDeCalzadaAntes = ObtenerCopia(tipoDeCalzada);

            if (tipoDeCalzada.Estado == "I")
                tipoDeCalzada.Estado = "A";
            else
                tipoDeCalzada.Estado = "I";
            db.SaveChanges();
            Bitacora(tipoDeCalzada, "U", "TIPOCALZADA", tipoDeCalzadaAntes);
            return RedirectToAction("Index");
        }

        // GET: TipoDeCalzadas/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeCalzada tipoDeCalzada = db.TIPOCALZADA.Find(id);
            if (tipoDeCalzada == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeCalzada);
        }

        // POST: TipoDeCalzadas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            TipoDeCalzada tipoDeCalzada = db.TIPOCALZADA.Find(id);
            db.TIPOCALZADA.Remove(tipoDeCalzada);
            db.SaveChanges();
            Bitacora(tipoDeCalzada, "D", "TIPOCALZADA");
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
