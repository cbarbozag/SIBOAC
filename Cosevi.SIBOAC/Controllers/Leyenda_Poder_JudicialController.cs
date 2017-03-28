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
    public class Leyenda_Poder_JudicialController : BaseController<Leyenda_Poder_Judicial>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Leyenda_Poder_Judicial
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.Leyenda_Poder_Judicial.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.Leyenda_Poder_Judicial.Any(x => x.codigo_autoridad == id);
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

        // GET: Leyenda_Poder_Judicial/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leyenda_Poder_Judicial leyenda_Poder_Judicial = db.Leyenda_Poder_Judicial.Find(id);
            if (leyenda_Poder_Judicial == null)
            {
                return HttpNotFound();
            }
            return View(leyenda_Poder_Judicial);
        }

        // GET: Leyenda_Poder_Judicial/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leyenda_Poder_Judicial/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codigo_autoridad,leyenda,estado,fecha_inicio,fecha_fin")] Leyenda_Poder_Judicial leyenda_Poder_Judicial)
        {
            if (ModelState.IsValid)
            {
                db.Leyenda_Poder_Judicial.Add(leyenda_Poder_Judicial);
                string mensaje = Verificar(leyenda_Poder_Judicial.codigo_autoridad);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(leyenda_Poder_Judicial.fecha_inicio, leyenda_Poder_Judicial.fecha_fin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(leyenda_Poder_Judicial, "I", "Leyenda por Poder Judicial");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(leyenda_Poder_Judicial);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(leyenda_Poder_Judicial);
                }
            }

            return View(leyenda_Poder_Judicial);
        }

        // GET: Leyenda_Poder_Judicial/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leyenda_Poder_Judicial leyenda_Poder_Judicial = db.Leyenda_Poder_Judicial.Find(id);
            if (leyenda_Poder_Judicial == null)
            {
                return HttpNotFound();
            }
            return View(leyenda_Poder_Judicial);
        }

        // POST: Leyenda_Poder_Judicial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codigo_autoridad,leyenda,estado,fecha_inicio,fecha_fin")] Leyenda_Poder_Judicial leyenda_Poder_Judicial)
        {
            if (ModelState.IsValid)
            {
                var leyendaAntes = db.Leyenda_Poder_Judicial.AsNoTracking().Where(d => d.codigo_autoridad == leyenda_Poder_Judicial.codigo_autoridad).FirstOrDefault();
                db.Entry(leyenda_Poder_Judicial).State = EntityState.Modified;
                string mensaje = ValidarFechas(leyenda_Poder_Judicial.fecha_inicio, leyenda_Poder_Judicial.fecha_fin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(leyenda_Poder_Judicial, "U", "Leyenda por Poder Judicial", leyendaAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(leyenda_Poder_Judicial);
                }
            }
            return View(leyenda_Poder_Judicial);
        }

        // GET: Leyenda_Poder_Judicial/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leyenda_Poder_Judicial leyenda_Poder_Judicial = db.Leyenda_Poder_Judicial.Find(id);
            if (leyenda_Poder_Judicial == null)
            {
                return HttpNotFound();
            }
            return View(leyenda_Poder_Judicial);
        }

        // POST: Leyenda_Poder_Judicial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Leyenda_Poder_Judicial leyenda_Poder_Judicial = db.Leyenda_Poder_Judicial.Find(id);
            Leyenda_Poder_Judicial leyendaAntes = ObtenerCopia(leyenda_Poder_Judicial);
            if (leyenda_Poder_Judicial.estado == "I")
                leyenda_Poder_Judicial.estado = "A";
            else
                leyenda_Poder_Judicial.estado = "I";
            db.SaveChanges();
            Bitacora(leyenda_Poder_Judicial, "U", "Leyenda por Poder Judicial", leyendaAntes);            
            return RedirectToAction("Index");
        }

        // GET: Interseccions/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leyenda_Poder_Judicial leyenda_Poder_Judicial = db.Leyenda_Poder_Judicial.Find(id);
            if (leyenda_Poder_Judicial == null)
            {
                return HttpNotFound();
            }
            return View(leyenda_Poder_Judicial);
        }

        // POST: Interseccions/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            Leyenda_Poder_Judicial leyenda_Poder_Judicial = db.Leyenda_Poder_Judicial.Find(id);
            db.Leyenda_Poder_Judicial.Remove(leyenda_Poder_Judicial);
            db.SaveChanges();
            Bitacora(leyenda_Poder_Judicial, "D", "Leyenda por Poder Judicial");
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
