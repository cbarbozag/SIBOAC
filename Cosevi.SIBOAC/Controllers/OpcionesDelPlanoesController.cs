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
    public class OpcionesDelPlanoesController : BaseController<OpcionesDelPlano>
    {


        // GET: OpcionesDelPlanoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.OPCIONPLANO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.OPCIONPLANO.Any(x => x.Id == id);
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
        // GET: OpcionesDelPlanoes/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            if (opcionesDelPlano == null)
            {
                return HttpNotFound();
            }
            return View(opcionesDelPlano);
        }

        // GET: OpcionesDelPlanoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OpcionesDelPlanoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OpcionesDelPlano opcionesDelPlano)
        {
            if (ModelState.IsValid)
            {
                db.OPCIONPLANO.Add(opcionesDelPlano);
                string mensaje = Verificar(opcionesDelPlano.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(opcionesDelPlano.FechaDeInicio, opcionesDelPlano.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(opcionesDelPlano, "I", "OPCIONPLANO");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(opcionesDelPlano);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(opcionesDelPlano);
                }
            }

            return View(opcionesDelPlano);
        }

        // GET: OpcionesDelPlanoes/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            if (opcionesDelPlano == null)
            {
                return HttpNotFound();
            }
            return View(opcionesDelPlano);
        }

        // POST: OpcionesDelPlanoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OpcionesDelPlano opcionesDelPlano)
        {
            if (ModelState.IsValid)
            {
                var opcionesDelPlanoAntes = db.OPCIONPLANO.AsNoTracking().Where(d => d.Id == opcionesDelPlano.Id).FirstOrDefault();
                db.Entry(opcionesDelPlano).State = EntityState.Modified;
                string mensaje = ValidarFechas(opcionesDelPlano.FechaDeInicio, opcionesDelPlano.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(opcionesDelPlano, "U", "OPCIONPLANO", opcionesDelPlanoAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(opcionesDelPlano);
                }
               
            }
            return View(opcionesDelPlano);
        }

        // GET: OpcionesDelPlanoes/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            if (opcionesDelPlano == null)
            {
                return HttpNotFound();
            }
            return View(opcionesDelPlano);
        }

        // POST: OpcionesDelPlanoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            OpcionesDelPlano opcionesDelPlanoAntes = ObtenerCopia(opcionesDelPlano);
            if (opcionesDelPlano.Estado == "I")
                opcionesDelPlano.Estado = "A";
            else
                opcionesDelPlano.Estado = "I";
            db.SaveChanges();
            Bitacora(opcionesDelPlano, "U", "OPCIONPLANO", opcionesDelPlanoAntes);
            return RedirectToAction("Index");
        }

        // GET: OpcionesDelPlanoes/RealDelete/5
        public ActionResult RealDelete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            if (opcionesDelPlano == null)
            {
                return HttpNotFound();
            }
            return View(opcionesDelPlano);
        }

        // POST: OpcionesDelPlanoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(short id)
        {
            OpcionesDelPlano opcionesDelPlano = db.OPCIONPLANO.Find(id);
            db.OPCIONPLANO.Remove(opcionesDelPlano);
            db.SaveChanges();
            Bitacora(opcionesDelPlano, "D", "OPCIONPLANO");
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
