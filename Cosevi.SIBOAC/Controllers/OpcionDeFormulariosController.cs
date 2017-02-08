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
    public class OpcionDeFormulariosController : BaseController<OpcionDeFormulario>
    {


        // GET: OpcionDeFormularios
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.OPCIONFORMULARIO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));

        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.OPCIONFORMULARIO.Any(x => x.Id == id);
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

        // GET: OpcionDeFormularios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            if (opcionDeFormulario == null)
            {
                return HttpNotFound();
            }
            return View(opcionDeFormulario);
        }

        // GET: OpcionDeFormularios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OpcionDeFormularios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OpcionDeFormulario opcionDeFormulario)
        {
            if (ModelState.IsValid)
            {
                db.OPCIONFORMULARIO.Add(opcionDeFormulario);
                string mensaje = Verificar(opcionDeFormulario.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(opcionDeFormulario.FechaDeInicio, opcionDeFormulario.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(opcionDeFormulario, "I", "OPCIONFORMULARIO");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(opcionDeFormulario);
                    }    
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(opcionDeFormulario);
                }
            }

            return View(opcionDeFormulario);
        }

        // GET: OpcionDeFormularios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            if (opcionDeFormulario == null)
            {
                return HttpNotFound();
            }
            return View(opcionDeFormulario);
        }

        // POST: OpcionDeFormularios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OpcionDeFormulario opcionDeFormulario)
        {
            if (ModelState.IsValid)
            {
                var opcionDeFormularioAntes = db.OPCIONFORMULARIO.AsNoTracking().Where(d => d.Id == opcionDeFormulario.Id).FirstOrDefault();
                db.Entry(opcionDeFormulario).State = EntityState.Modified;
                string mensaje = ValidarFechas(opcionDeFormulario.FechaDeInicio, opcionDeFormulario.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(opcionDeFormulario, "U", "OPCIONFORMULARIO", opcionDeFormularioAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(opcionDeFormulario);
                }
                
            }
            return View(opcionDeFormulario);
        }

        // GET: OpcionDeFormularios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            if (opcionDeFormulario == null)
            {
                return HttpNotFound();
            }
            return View(opcionDeFormulario);
        }

        // POST: OpcionDeFormularios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            OpcionDeFormulario opcionDeFormularioAntes = ObtenerCopia(opcionDeFormulario);
            if (opcionDeFormulario.Estado == "I")
                opcionDeFormulario.Estado = "A";
            else
                opcionDeFormulario.Estado = "I";
            db.SaveChanges();
            Bitacora(opcionDeFormulario, "U", "OPCIONFORMULARIO", opcionDeFormularioAntes);
            return RedirectToAction("Index");
        }

        // GET: OpcionDeFormularios/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            if (opcionDeFormulario == null)
            {
                return HttpNotFound();
            }
            return View(opcionDeFormulario);
        }

        // POST: OpcionDeFormularios/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            db.OPCIONFORMULARIO.Remove(opcionDeFormulario);
            db.SaveChanges();
            Bitacora(opcionDeFormulario, "D", "OPCIONFORMULARIO");
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
