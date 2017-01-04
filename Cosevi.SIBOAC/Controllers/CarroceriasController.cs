using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Controllers
{
    public class CarroceriasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Carrocerias
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.CARROCERIA.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.CARROCERIA.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Carrocerias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carroceria carroceria = db.CARROCERIA.Find(id);
            if (carroceria == null)
            {
                return HttpNotFound();
            }
            return View(carroceria);
        }

        // GET: Carrocerias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrocerias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Carroceria carroceria)
        {
            if (ModelState.IsValid)
            {
                db.CARROCERIA.Add(carroceria);
                string mensaje = Verificar(carroceria.Id);
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
                    return View(carroceria);
                }
            }

            return View(carroceria);
        }

        // GET: Carrocerias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carroceria carroceria = db.CARROCERIA.Find(id);
            if (carroceria == null)
            {
                return HttpNotFound();
            }
            return View(carroceria);
        }

        // POST: Carrocerias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Carroceria carroceria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carroceria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carroceria);
        }

        // GET: Carrocerias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carroceria carroceria = db.CARROCERIA.Find(id);
            if (carroceria == null)
            {
                return HttpNotFound();
            }
            return View(carroceria);
        }

        // POST: Carrocerias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Carroceria carroceria = db.CARROCERIA.Find(id);
            if (carroceria.Estado == "A")
                carroceria.Estado = "I";
            else
                carroceria.Estado = "A";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Carrocerias/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carroceria carroceria = db.CARROCERIA.Find(id);
            if (carroceria == null)
            {
                return HttpNotFound();
            }
            return View(carroceria);
        }

        // POST: Carrocerias/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Carroceria carroceria = db.CARROCERIA.Find(id);
            db.CARROCERIA.Remove(carroceria);
            db.SaveChanges();
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
