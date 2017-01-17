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
    public class TIPODANOesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TIPODANOes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.TIPODANO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.TIPODANO.Any(x => x.codigod == id);
            if (exist)
            {
                mensaje = "El código " + id + " ya esta registrado";
            }
            return mensaje;
        }
        // GET: TIPODANOes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPODANO tIPODANO = db.TIPODANO.Find(id);
            if (tIPODANO == null)
            {
                return HttpNotFound();
            }
            return View(tIPODANO);
        }

        // GET: TIPODANOes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TIPODANOes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codigod,descripcion")] TIPODANO tIPODANO)
        {
            if (ModelState.IsValid)
            {
                db.TIPODANO.Add(tIPODANO);
                string mensaje = Verificar(tIPODANO.codigod);
                if (mensaje == "")
                {
                    int i=db.SaveChanges();
                    
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente"+ i;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(tIPODANO);
                }
            }

            return View(tIPODANO);
        }

        // GET: TIPODANOes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPODANO tIPODANO = db.TIPODANO.Find(id);
            if (tIPODANO == null)
            {
                return HttpNotFound();
            }
            return View(tIPODANO);
        }

        // POST: TIPODANOes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codigod,descripcion")] TIPODANO tIPODANO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIPODANO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tIPODANO);
        }

        // GET: TIPODANOes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPODANO tIPODANO = db.TIPODANO.Find(id);
            if (tIPODANO == null)
            {
                return HttpNotFound();
            }
            return View(tIPODANO);
        }

        // POST: TIPODANOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TIPODANO tIPODANO = db.TIPODANO.Find(id);
            db.TIPODANO.Remove(tIPODANO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: TIPODANOes/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIPODANO tIPODANO = db.TIPODANO.Find(id);
            if (tIPODANO == null)
            {
                return HttpNotFound();
            }
            return View(tIPODANO);
        }

        // POST: TIPODANOes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            TIPODANO tIPODANO = db.TIPODANO.Find(id);
            db.TIPODANO.Remove(tIPODANO);
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
