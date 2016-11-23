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
    public class AlineacionVerticalsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: AlineacionVerticals
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.ALINVERT.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.ALINVERT.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: AlineacionVerticals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionVertical alineacionVertical = db.ALINVERT.Find(id);
            if (alineacionVertical == null)
            {
                return HttpNotFound();
            }
            return View(alineacionVertical);
        }

        // GET: AlineacionVerticals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlineacionVerticals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] AlineacionVertical alineacionVertical)
        {
            if (ModelState.IsValid)
            {
                db.ALINVERT.Add(alineacionVertical);
                string mensaje = Verificar(alineacionVertical.Id);
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
                    return View(alineacionVertical);
                }
            }

            return View(alineacionVertical);
        }

        // GET: AlineacionVerticals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionVertical alineacionVertical = db.ALINVERT.Find(id);
            if (alineacionVertical == null)
            {
                return HttpNotFound();
            }
            return View(alineacionVertical);
        }

        // POST: AlineacionVerticals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] AlineacionVertical alineacionVertical)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alineacionVertical).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alineacionVertical);
        }

        // GET: AlineacionVerticals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionVertical alineacionVertical = db.ALINVERT.Find(id);
            if (alineacionVertical == null)
            {
                return HttpNotFound();
            }
            return View(alineacionVertical);
        }

        // POST: AlineacionVerticals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AlineacionVertical alineacionVertical = db.ALINVERT.Find(id);
            if (alineacionVertical.Estado == "A")
                alineacionVertical.Estado = "I";
            else
                alineacionVertical.Estado = "A";
            db.SaveChanges();
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
