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
    public class ObstaculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Obstaculoes
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            
            var list = db.Obstaculo.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.Obstaculo.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Obstaculoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // GET: Obstaculoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Obstaculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Obstaculo obstaculo)
        {
            if (ModelState.IsValid)
            {
                db.Obstaculo.Add(obstaculo);
                string mensaje = Verificar(obstaculo.Id);
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
                    return View(obstaculo);
                }
            }

            return View(obstaculo);
        }

        // GET: Obstaculoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // POST: Obstaculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Obstaculo obstaculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(obstaculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obstaculo);
        }

        // GET: Obstaculoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // POST: Obstaculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo.Estado == "I")
                obstaculo.Estado = "A";
            else
                obstaculo.Estado = "I";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Obstaculoes/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            if (obstaculo == null)
            {
                return HttpNotFound();
            }
            return View(obstaculo);
        }

        // POST: Obstaculoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            Obstaculo obstaculo = db.Obstaculo.Find(id);
            db.Obstaculo.Remove(obstaculo);
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
