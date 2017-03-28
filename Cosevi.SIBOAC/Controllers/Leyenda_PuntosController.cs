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
    public class Leyenda_PuntosController : BaseController<Leyenda_Puntos>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Leyenda_Puntos
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.Leyenda_Puntos.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.Leyenda_Puntos.Any(x => x.Codigo == id);
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
    
        // GET: Leyenda_Puntos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leyenda_Puntos leyenda_Puntos = db.Leyenda_Puntos.Find(id);
            if (leyenda_Puntos == null)
            {
                return HttpNotFound();
            }
            return View(leyenda_Puntos);
        }

        // GET: Leyenda_Puntos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leyenda_Puntos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codigo,Descripcion,Estado,Fecha_Inicio,Fecha_Final")] Leyenda_Puntos leyenda_Puntos)
        {
            if (ModelState.IsValid)
            {
                db.Leyenda_Puntos.Add(leyenda_Puntos);
                string mensaje = Verificar(leyenda_Puntos.Codigo);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(leyenda_Puntos.Fecha_Inicio, leyenda_Puntos.Fecha_Final);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(leyenda_Puntos, "I", "Leyenda de Puntos");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(leyenda_Puntos);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(leyenda_Puntos);
                }
            }

            return View(leyenda_Puntos);
        }

        // GET: Leyenda_Puntos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leyenda_Puntos leyenda_Puntos = db.Leyenda_Puntos.Find(id);
            if (leyenda_Puntos == null)
            {
                return HttpNotFound();
            }
            return View(leyenda_Puntos);
        }

        // POST: Leyenda_Puntos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,Descripcion,Estado,Fecha_Inicio,Fecha_Final")] Leyenda_Puntos leyenda_Puntos)
        {
            if (ModelState.IsValid)
            {
                var leyendaPuntosAntes = db.Leyenda_Puntos.AsNoTracking().Where(d => d.Codigo == leyenda_Puntos.Codigo).FirstOrDefault();
                db.Entry(leyenda_Puntos).State = EntityState.Modified;
                string mensaje = ValidarFechas(leyenda_Puntos.Fecha_Inicio, leyenda_Puntos.Fecha_Final);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(leyenda_Puntos, "U", "Leyenda de Puntos", leyendaPuntosAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(leyenda_Puntos);
                }
            }
            return View(leyenda_Puntos);
        }

        // GET: Leyenda_Puntos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leyenda_Puntos leyenda_Puntos = db.Leyenda_Puntos.Find(id);
            if (leyenda_Puntos == null)
            {
                return HttpNotFound();
            }
            return View(leyenda_Puntos);
        }

        // POST: Leyenda_Puntos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leyenda_Puntos leyenda_Puntos = db.Leyenda_Puntos.Find(id);
            Leyenda_Puntos leyendaAntes = ObtenerCopia(leyenda_Puntos);
            if (leyenda_Puntos.Estado == "I")
                leyenda_Puntos.Estado = "A";
            else
                leyenda_Puntos.Estado = "I";
            db.SaveChanges();
            Bitacora(leyenda_Puntos, "U", "Leyenda por Poder Judicial", leyendaAntes);
            return RedirectToAction("Index");
        }

        // GET: Leyenda_Puntos/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leyenda_Puntos leyenda_Puntos = db.Leyenda_Puntos.Find(id);
            if (leyenda_Puntos == null)
            {
                return HttpNotFound();
            }
            return View(leyenda_Puntos);
        }

        // POST: Leyenda_Puntos/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Leyenda_Puntos leyenda_Puntos = db.Leyenda_Puntos.Find(id);
            db.Leyenda_Puntos.Remove(leyenda_Puntos);
            db.SaveChanges();
            Bitacora(leyenda_Puntos, "D", "Leyenda de Puntos");
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
