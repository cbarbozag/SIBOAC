using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;
using Cosevi.SIBOAC.Controllers;
using PagedList;

namespace Cosevi.SIBOAC.Controllers
{
    public class DanoesController : BaseController<Dano>
    {

        // GET: Danoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            

            var list = db.DAÑO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.DAÑO.Any(x => x.Id == id);
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
        // GET: Danoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dano dano = db.DAÑO.Find(id);
            if (dano == null)
            {
                return HttpNotFound();
            }
            return View(dano);
        }

        // GET: Danoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Danoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Dano dano)
        {
            if (ModelState.IsValid)
            {
                db.DAÑO.Add(dano);
                string mensaje = Verificar(dano.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(dano, "I", "DAÑO");

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(dano);
                }
            }

            return View(dano);
        }

        // GET: Danoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dano dano = db.DAÑO.Find(id);
            if (dano == null)
            {
                return HttpNotFound();
            }
            return View(dano);
        }

        // POST: Danoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Dano dano)
        {
            if (ModelState.IsValid)
            {
                var danoAntes = db.DAÑO.AsNoTracking().Where(d => d.Id == dano.Id).FirstOrDefault();

                db.Entry(dano).State = EntityState.Modified;

                db.SaveChanges();
                Bitacora(dano, "U", "DAÑO", danoAntes);

                return RedirectToAction("Index");
            }
            return View(dano);
        }

        // GET: Danoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dano dano = db.DAÑO.Find(id);
            if (dano == null)
            {
                return HttpNotFound();
            }
            return View(dano);
        }

        // POST: Danoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dano dano = db.DAÑO.Find(id);
            Dano danoAntes = ObtenerCopia(dano);

            if (dano.Estado == "I")
                dano.Estado = "A";
            else
                dano.Estado = "I";
            db.SaveChanges();

            Bitacora(dano, "U", "DAÑO", danoAntes);
            return RedirectToAction("Index");
        }



        // GET: Danoes/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dano dano = db.DAÑO.Find(id);
            if (dano == null)
            {
                return HttpNotFound();
            }
            return View(dano);
        }

        // POST: Danoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Dano dano = db.DAÑO.Find(id);
            db.DAÑO.Remove(dano);
            db.SaveChanges();
            Bitacora(dano, "D", "DAÑO");
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
