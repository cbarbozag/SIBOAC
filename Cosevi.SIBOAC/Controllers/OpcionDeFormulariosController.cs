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
    public class OpcionDeFormulariosController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: OpcionDeFormularios
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.OPCIONFORMULARIO.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.OPCIONFORMULARIO.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: OpcionDeFormularios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            if (opcionDeFormulario == null)
            {
                return HttpNotFound();
            }
            return View(opcionDeFormulario);
        }

        // GET: OpcionDeFormularios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OpcionDeFormularios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OpcionDeFormulario opcionDeFormulario)
        {
            if (ModelState.IsValid)
            {
                db.OPCIONFORMULARIO.Add(opcionDeFormulario);
                string mensaje = Verificar(opcionDeFormulario.Id);
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
                    return View(opcionDeFormulario);
                }
            }

            return View(opcionDeFormulario);
        }

        // GET: OpcionDeFormularios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            if (opcionDeFormulario == null)
            {
                return HttpNotFound();
            }
            return View(opcionDeFormulario);
        }

        // POST: OpcionDeFormularios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] OpcionDeFormulario opcionDeFormulario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opcionDeFormulario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(opcionDeFormulario);
        }

        // GET: OpcionDeFormularios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            if (opcionDeFormulario == null)
            {
                return HttpNotFound();
            }
            return View(opcionDeFormulario);
        }

        // POST: OpcionDeFormularios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            if (opcionDeFormulario.Estado == "I")
                opcionDeFormulario.Estado = "A";
            else
                opcionDeFormulario.Estado = "I";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: OpcionDeFormularios/RealDelete/5
        public ActionResult RealDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            if (opcionDeFormulario == null)
            {
                return HttpNotFound();
            }
            return View(opcionDeFormulario);
        }

        // POST: OpcionDeFormularios/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            OpcionDeFormulario opcionDeFormulario = db.OPCIONFORMULARIO.Find(id);
            db.OPCIONFORMULARIO.Remove(opcionDeFormulario);
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
