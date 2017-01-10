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
    public class DispositivosMovilesController : BaseController<DispositivosMoviles>
    {
        

        // GET: DispositivosMoviles
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list = db.DispositivosMoviles.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));

        }

        // GET: DispositivosMoviles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            if (dispositivosMoviles == null)
            {
                return HttpNotFound();
            }
            return View(dispositivosMoviles);
        }

        // GET: DispositivosMoviles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DispositivosMoviles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IMEI,Descripcion,Activo")] DispositivosMoviles dispositivosMoviles)
        {
            if (ModelState.IsValid)
            {
                db.DispositivosMoviles.Add(dispositivosMoviles);
                db.SaveChanges();
                Bitacora(dispositivosMoviles, "I", "DispositivosMoviles");
                return RedirectToAction("Index");
            }

            return View(dispositivosMoviles);
        }

        // GET: DispositivosMoviles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            if (dispositivosMoviles == null)
            {
                return HttpNotFound();
            }
            return View(dispositivosMoviles);
        }

        // POST: DispositivosMoviles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IMEI,Descripcion,Activo")] DispositivosMoviles dispositivosMoviles)
        {
            if (ModelState.IsValid)
            {
                var dispositivosMovilesAntes = db.DispositivosMoviles.AsNoTracking().Where(d => d.Id == dispositivosMoviles.Id).FirstOrDefault();
                db.Entry(dispositivosMoviles).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(dispositivosMoviles, "U", "DispositivosMoviles", dispositivosMovilesAntes);
                return RedirectToAction("Index");
            }
            return View(dispositivosMoviles);
        }

        // GET: DispositivosMoviles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            if (dispositivosMoviles == null)
            {
                return HttpNotFound();
            }
            return View(dispositivosMoviles);
        }

        // POST: DispositivosMoviles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            DispositivosMoviles dispositivosMovilesAntes = ObtenerCopia(dispositivosMoviles);
            if (dispositivosMoviles.Activo == false)
                dispositivosMoviles.Activo = true;
            else
                dispositivosMoviles.Activo = false;
            db.SaveChanges();
            Bitacora(dispositivosMoviles, "U", "DispositivosMoviles", dispositivosMovilesAntes);
            return RedirectToAction("Index");
        }

        // GET: DispositivosMoviles/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            if (dispositivosMoviles == null)
            {
                return HttpNotFound();
            }
            return View(dispositivosMoviles);
        }

        // POST: DispositivosMoviles/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            DispositivosMoviles dispositivosMoviles = db.DispositivosMoviles.Find(id);
            db.DispositivosMoviles.Remove(dispositivosMoviles);
            db.SaveChanges();
            Bitacora(dispositivosMoviles, "D", "DispositivosMoviles");
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
