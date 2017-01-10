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
    public class VariablesParaBloqueosController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: VariablesParaBloqueos
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.VARIABLESBLOQUEO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }



        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.VARIABLESBLOQUEO.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }


        // GET: VariablesParaBloqueos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            if (variablesParaBloqueo == null)
            {
                return HttpNotFound();
            }
            return View(variablesParaBloqueo);
        }

        // GET: VariablesParaBloqueos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VariablesParaBloqueos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] VariablesParaBloqueo variablesParaBloqueo)
        {
            if (ModelState.IsValid)
            {
                db.VARIABLESBLOQUEO.Add(variablesParaBloqueo);
                string mensaje = Verificar(variablesParaBloqueo.Id);
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
                    return View(variablesParaBloqueo);
                }
            }

            return View(variablesParaBloqueo);
        }

        // GET: VariablesParaBloqueos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            if (variablesParaBloqueo == null)
            {
                return HttpNotFound();
            }
            return View(variablesParaBloqueo);
        }

        // POST: VariablesParaBloqueos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Estado,FechaDeInicio,FechaDeFin")] VariablesParaBloqueo variablesParaBloqueo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(variablesParaBloqueo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(variablesParaBloqueo);
        }

        // GET: VariablesParaBloqueos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            if (variablesParaBloqueo == null)
            {
                return HttpNotFound();
            }
            return View(variablesParaBloqueo);
        }

        // POST: VariablesParaBloqueos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            if (variablesParaBloqueo.Estado == "I")
                variablesParaBloqueo.Estado = "A";
            else
                variablesParaBloqueo.Estado = "I";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: AlineacionHorizontals/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            if (variablesParaBloqueo == null)
            {
                return HttpNotFound();
            }
            return View(variablesParaBloqueo);
        }

        // POST: AlineacionHorizontals/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            VariablesParaBloqueo variablesParaBloqueo = db.VARIABLESBLOQUEO.Find(id);
            db.VARIABLESBLOQUEO.Remove(variablesParaBloqueo);
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
