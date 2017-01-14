using Cosevi.SIBOAC.Controllers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Cosevi.SIBOAC.Models
{
    public class SexoesController : BaseController<Sexo>
    {
        

        // GET: Sexoes
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list = db.SEXO.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.SEXO.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: Sexoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sexo sexo = db.SEXO.Find(id);
            if (sexo == null)
            {
                return HttpNotFound();
            }
            return View(sexo);
        }

        // GET: Sexoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sexoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Sexo sexo)
        {
            if (ModelState.IsValid)
            {
                db.SEXO.Add(sexo);
                string mensaje = Verificar(sexo.Id);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(sexo, "I", "SEXO");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(sexo);
                }
            }

            return View(sexo);
        }

        // GET: Sexoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sexo sexo = db.SEXO.Find(id);
            if (sexo == null)
            {
                return HttpNotFound();
            }
            return View(sexo);
        }

        // POST: Sexoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Sexo sexo)
        {
            var sexoAntes = db.SEXO.AsNoTracking().Where(d => d.Id == sexo.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.Entry(sexo).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(sexo, "U", "SEXO", sexoAntes);
                return RedirectToAction("Index");
            }
            return View(sexo);
        }

        // GET: Sexoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sexo sexo = db.SEXO.Find(id);
            if (sexo == null)
            {
                return HttpNotFound();
            }
            return View(sexo);
        }

        // POST: Sexoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Sexo sexo = db.SEXO.Find(id);
            Sexo sexoAntes = ObtenerCopia(sexo);
            if (sexo.Estado == "I")
                sexo.Estado = "A";
            else
                sexo.Estado = "I";
            db.SaveChanges();
            Bitacora(sexo, "U", "SEXO", sexoAntes);
            return RedirectToAction("Index");
        }

        // GET: Sexoes/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sexo sexo = db.SEXO.Find(id);
            if (sexo == null)
            {
                return HttpNotFound();
            }
            return View(sexo);
        }

        // POST: Sexoes/Delete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            Sexo sexo = db.SEXO.Find(id);
            db.SEXO.Remove(sexo);
            db.SaveChanges();
            Bitacora(sexo, "D", "SEXO");
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
