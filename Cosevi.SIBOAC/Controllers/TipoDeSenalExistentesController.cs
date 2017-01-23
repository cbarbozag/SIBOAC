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
    public class TipoDeSenalExistentesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoDeSenalExistentes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.TIPOSEÑALEXISTE.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }



        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.TIPOSEÑALEXISTE.Any(x => x.Id == id);
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

        // GET: TipoDeSenalExistentes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            if (tipoDeSenalExistente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeSenalExistente);
        }

        // GET: TipoDeSenalExistentes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeSenalExistentes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeSenalExistente tipoDeSenalExistente)
        {
            if (ModelState.IsValid)
            {
                db.TIPOSEÑALEXISTE.Add(tipoDeSenalExistente);
                string mensaje = Verificar(tipoDeSenalExistente.Id);
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
                    return View(tipoDeSenalExistente);
                }
            }

            return View(tipoDeSenalExistente);
        }

        // GET: TipoDeSenalExistentes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            if (tipoDeSenalExistente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeSenalExistente);
        }

        // POST: TipoDeSenalExistentes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeSenalExistente tipoDeSenalExistente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeSenalExistente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDeSenalExistente);
        }

        // GET: TipoDeSenalExistentes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            if (tipoDeSenalExistente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeSenalExistente);
        }

        // POST: TipoDeSenalExistentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            if (tipoDeSenalExistente.Estado == "I")
                tipoDeSenalExistente.Estado = "A";
            else
                tipoDeSenalExistente.Estado = "I";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: TipoDeSenalExistentes/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            if (tipoDeSenalExistente == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeSenalExistente);
        }

        // POST: TipoDeSenalExistentes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            TipoDeSenalExistente tipoDeSenalExistente = db.TIPOSEÑALEXISTE.Find(id);
            db.TIPOSEÑALEXISTE.Remove(tipoDeSenalExistente);
            db.SaveChanges();
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
