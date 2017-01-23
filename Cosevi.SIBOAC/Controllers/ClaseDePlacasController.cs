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
    public class ClaseDePlacasController : BaseController<ClaseDePlaca>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: ClaseDePlacas
        [SessionExpire]
        public ActionResult Index(int ? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";           

            var list = db.CLASE.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.CLASE.Any(x => x.Id == id);
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

        // GET: ClaseDePlacas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            if (claseDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(claseDePlaca);
        }

        // GET: ClaseDePlacas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClaseDePlacas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] ClaseDePlaca claseDePlaca)
        {
            if (ModelState.IsValid)
            {
                db.CLASE.Add(claseDePlaca);
                string mensaje = Verificar(claseDePlaca.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(claseDePlaca.FechaDeInicio, claseDePlaca.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();

                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(claseDePlaca);
                    }
                    db.SaveChanges();
                    Bitacora(claseDePlaca, "I", "CLASE");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(claseDePlaca);
                }
            }

            return View(claseDePlaca);
        }

        // GET: ClaseDePlacas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            if (claseDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(claseDePlaca);
        }

        // POST: ClaseDePlacas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] ClaseDePlaca claseDePlaca)
        {
            if (ModelState.IsValid)
            {
                var claseDePlacaAntes = db.CLASE.AsNoTracking().Where(d => d.Id == claseDePlaca.Id).FirstOrDefault();

                db.Entry(claseDePlaca).State = EntityState.Modified;
               string  mensaje = ValidarFechas(claseDePlaca.FechaDeInicio, claseDePlaca.FechaDeFin);

                if (mensaje == "")
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(claseDePlaca);
                }
            }
                db.SaveChanges();
                Bitacora(claseDePlaca, "U", "CLASE", claseDePlacaAntes);
                return RedirectToAction("Index");
            }
            return View(claseDePlaca);
        }

        // GET: ClaseDePlacas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            if (claseDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(claseDePlaca);
        }

        // POST: ClaseDePlacas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            ClaseDePlaca claseDePlacaAntes = ObtenerCopia(claseDePlaca);

            if (claseDePlaca.Estado == "A")
                claseDePlaca.Estado = "I";
            else
                claseDePlaca.Estado = "A";
                db.SaveChanges();
                Bitacora(claseDePlaca, "U", "CLASE", claseDePlacaAntes);
            return RedirectToAction("Index");
        }


        // GET: ClaseDePlacas/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            if (claseDePlaca == null)
            {
                return HttpNotFound();
            }
            return View(claseDePlaca);
        }

        // POST: ClaseDePlacas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            ClaseDePlaca claseDePlaca = db.CLASE.Find(id);
            db.CLASE.Remove(claseDePlaca);
            db.SaveChanges();
            Bitacora(claseDePlaca, "D", "CLASE");
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
