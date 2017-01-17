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
    public class CantonsController : BaseController<Canton>
    {

        // GET: Cantons
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";           

            var sCanton = db.CANTON.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(sCanton.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.CANTON.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }


        // GET: Cantons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canton canton = db.CANTON.Find(id);
            if (canton == null)
            {
                return HttpNotFound();
            }
            return View(canton);
        }

        // GET: Cantons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cantons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Canton canton)
        {
            if (ModelState.IsValid)
            {
                db.CANTON.Add(canton);
                string mensaje = Verificar(canton.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(canton, "I", "CANTON");

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(canton);
                }
            }

            return View(canton);
        }

        // GET: Cantons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canton canton = db.CANTON.Find(id);
            if (canton == null)
            {
                return HttpNotFound();
            }
            return View(canton);
        }

        // POST: Cantons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Canton canton)
        {
            if (ModelState.IsValid)
            {
                var cantonAntes = db.CANTON.AsNoTracking().Where(d => d.Id == canton.Id).FirstOrDefault();

                db.Entry(canton).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(canton, "U", "CANTON", cantonAntes);

                return RedirectToAction("Index");
            }
            return View(canton);
        }

        // GET: Cantons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canton canton = db.CANTON.Find(id);
            if (canton == null)
            {
                return HttpNotFound();
            }
            return View(canton);
        }

        // POST: Cantons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Canton canton = db.CANTON.Find(id);
            Canton cantonAntes = ObtenerCopia(canton);

            if (canton.Estado == "A")
                canton.Estado = "I";
            else
                canton.Estado = "A";
            db.SaveChanges();
            Bitacora(canton, "U", "CANTON", cantonAntes);

            return RedirectToAction("Index");
        }


        // GET: Cantons/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canton canton = db.CANTON.Find(id);
            if (canton == null)
            {
                return HttpNotFound();
            }
            return View(canton);
        }

        // POST: Cantons/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Canton canton = db.CANTON.Find(id);
            db.CANTON.Remove(canton);
            db.SaveChanges();
            Bitacora(canton, "D", "CANTON");

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
