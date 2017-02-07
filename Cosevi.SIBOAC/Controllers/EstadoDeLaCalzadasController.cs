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
    public class EstadoDeLaCalzadasController : BaseController<EstadoDeLaCalzada>
    {


        // GET: EstadoDeLaCalzadas
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.ESTCALZADA.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.ESTCALZADA.Any(x => x.Id == id);
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
        // GET: EstadoDeLaCalzadas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            if (estadoDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(estadoDeLaCalzada);
        }

        // GET: EstadoDeLaCalzadas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstadoDeLaCalzadas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] EstadoDeLaCalzada estadoDeLaCalzada)
        {
            if (ModelState.IsValid)
            {
                db.ESTCALZADA.Add(estadoDeLaCalzada);
                string mensaje = Verificar(estadoDeLaCalzada.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(estadoDeLaCalzada.FechaDeInicio, estadoDeLaCalzada.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(estadoDeLaCalzada, "I", "ESTCALZADA");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(estadoDeLaCalzada);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(estadoDeLaCalzada);
                }
            }

            return View(estadoDeLaCalzada);
        }

        // GET: EstadoDeLaCalzadas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            if (estadoDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(estadoDeLaCalzada);
        }

        // POST: EstadoDeLaCalzadas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] EstadoDeLaCalzada estadoDeLaCalzada)
        {
            if (ModelState.IsValid)
            {            
                var estadoDeLaCalzadaAntes = db.ESTCALZADA.AsNoTracking().Where(d => d.Id == estadoDeLaCalzada.Id).FirstOrDefault();
                db.Entry(estadoDeLaCalzada).State = EntityState.Modified;
                string mensaje = ValidarFechas(estadoDeLaCalzada.FechaDeInicio, estadoDeLaCalzada.FechaDeFin);
                if (mensaje=="")
                {
                    db.SaveChanges();
                    Bitacora(estadoDeLaCalzada, "U", "ESTCALZADA", estadoDeLaCalzadaAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(estadoDeLaCalzada);
                }                            
            }
            return View(estadoDeLaCalzada);
        }

        // GET: EstadoDeLaCalzadas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            if (estadoDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(estadoDeLaCalzada);
        }

        // POST: EstadoDeLaCalzadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            EstadoDeLaCalzada estadoDeLaCalzadaAntes = ObtenerCopia(estadoDeLaCalzada);
            if (estadoDeLaCalzada.Estado == "I")
                estadoDeLaCalzada.Estado = "A";
            else
                estadoDeLaCalzada.Estado = "I";
            db.SaveChanges();
            Bitacora(estadoDeLaCalzada, "U", "ESTCALZADA", estadoDeLaCalzadaAntes);
            return RedirectToAction("Index");
        }

        // GET: EstadoDeLaCalzadas/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            if (estadoDeLaCalzada == null)
            {
                return HttpNotFound();
            }
            return View(estadoDeLaCalzada);
        }

        // POST: EstadoDeLaCalzadas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            EstadoDeLaCalzada estadoDeLaCalzada = db.ESTCALZADA.Find(id);
            db.ESTCALZADA.Remove(estadoDeLaCalzada);
            db.SaveChanges();
            Bitacora(estadoDeLaCalzada, "D", "ESTCALZADA");
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
