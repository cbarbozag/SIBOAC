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
    public class TipoDeEstructurasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoDeEstructuras
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.ESTRUCTURA.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.ESTRUCTURA.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El código " + id + " ya esta registrado";
            }
            return mensaje;
        }


        // GET: TipoDeEstructuras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // GET: TipoDeEstructuras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeEstructuras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeEstructura tipoDeEstructura)
        {
            if (ModelState.IsValid)
            {
                db.ESTRUCTURA.Add(tipoDeEstructura);
                string mensaje = Verificar(tipoDeEstructura.Id);
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
                    return View(tipoDeEstructura);
                }
            }

            return View(tipoDeEstructura);
        }

        // GET: TipoDeEstructuras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // POST: TipoDeEstructuras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] TipoDeEstructura tipoDeEstructura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeEstructura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDeEstructura);
        }

        // GET: TipoDeEstructuras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeEstructura);
        }

        // POST: TipoDeEstructuras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDeEstructura tipoDeEstructura = db.ESTRUCTURA.Find(id);
            if (tipoDeEstructura.Estado == "I")
                tipoDeEstructura.Estado = "A";
            else
                tipoDeEstructura.Estado = "I";
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
