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
    public class CondicionDeLaPersonasController : BaseController<CondicionDeLaPersona>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: CondicionDeLaPersonas
        [SessionExpire]
        public ActionResult Index(int ? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.CONDPERSONA.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.CONDPERSONA.Any(x => x.Id == id);
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
        // GET: CondicionDeLaPersonas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            if (condicionDeLaPersona == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaPersona);
        }

        // GET: CondicionDeLaPersonas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CondicionDeLaPersonas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CondicionDeLaPersona condicionDeLaPersona)
        {
            if (ModelState.IsValid)
            {
                db.CONDPERSONA.Add(condicionDeLaPersona);
                string mensaje = Verificar(condicionDeLaPersona.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(condicionDeLaPersona.FechaDeInicio, condicionDeLaPersona.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(condicionDeLaPersona, "I", "CONDPERSONA");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(condicionDeLaPersona);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(condicionDeLaPersona);
                }
            }

            return View(condicionDeLaPersona);
        }

        // GET: CondicionDeLaPersonas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            if (condicionDeLaPersona == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaPersona);
        }

        // POST: CondicionDeLaPersonas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] CondicionDeLaPersona condicionDeLaPersona)
        {
            if (ModelState.IsValid)
            {
                var condicionDeLaPersonaAntes = db.CONDPERSONA.AsNoTracking().Where(d => d.Id == condicionDeLaPersona.Id).FirstOrDefault();

                db.Entry(condicionDeLaPersona).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(condicionDeLaPersona, "U", "CIRCULACION", condicionDeLaPersonaAntes);
                TempData["Type"] = "success";
                TempData["Message"] = "La edición se realizó correctamente";
                return RedirectToAction("Index");
            }
            return View(condicionDeLaPersona);
        }

        // GET: CondicionDeLaPersonas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            if (condicionDeLaPersona == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaPersona);
        }

        // POST: CondicionDeLaPersonas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            CondicionDeLaPersona condicionDeLaPersonaAntes = ObtenerCopia(condicionDeLaPersona);
            if (condicionDeLaPersona.Estado == "A")
                condicionDeLaPersona.Estado = "I";
            else
                condicionDeLaPersona.Estado = "A";
            db.SaveChanges();
            Bitacora(condicionDeLaPersona, "U", "CIRCULACION", condicionDeLaPersonaAntes);
            return RedirectToAction("Index");
        }


        // GET: CondicionDeLaPersonas/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            if (condicionDeLaPersona == null)
            {
                return HttpNotFound();
            }
            return View(condicionDeLaPersona);
        }

        // POST: CondicionDeLaPersonas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            CondicionDeLaPersona condicionDeLaPersona = db.CONDPERSONA.Find(id);
            db.CONDPERSONA.Remove(condicionDeLaPersona);
            db.SaveChanges();
            Bitacora(condicionDeLaPersona, "D", "CONDPERSONA");
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
