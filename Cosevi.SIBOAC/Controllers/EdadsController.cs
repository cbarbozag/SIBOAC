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
    public class EdadsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Edads
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            
            var list = db.EDAD.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        public string Verificar(DateTime FechaMinNacimiento, DateTime FechaMaxNacimiento)
        {
            string mensaje = "";
            bool exist = this.db.EDAD.Any(x => x.FechaMinNacimiento == FechaMinNacimiento && x.FechaMaxNacimiento == FechaMaxNacimiento);

            if (exist)
            {
                mensaje = " La fecha ya esta registrada";

            }

            return mensaje;

        }



        // GET: Edads/Details/5
        public ActionResult Details(DateTime FechaMinNacimiento, DateTime FechaMaxNacimiento)
        {
            if (FechaMinNacimiento == null || FechaMaxNacimiento == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Edad edad = db.EDAD.Find(FechaMinNacimiento, FechaMaxNacimiento);
            if (edad == null)
            {
                return HttpNotFound();
            }
            return View(edad);
        }

        // GET: Edads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Edads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FechaMinNacimiento,FechaMaxNacimiento,FechaPorDefecto,Estado,FechaDeInicio,FechaDeFin")] Edad edad)
        {
            if (ModelState.IsValid)
            {
                db.EDAD.Add(edad);
                string mensaje = Verificar(edad.FechaMinNacimiento, edad.FechaMaxNacimiento);
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
                    return View(edad);
                }
            }

            return View(edad);
        }

        // GET: Edads/Edit/5
        public ActionResult Edit(DateTime FechaMinNacimiento, DateTime FechaMaxNacimiento)
        {
            if (FechaMinNacimiento == null || FechaMaxNacimiento == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Edad edad = db.EDAD.Find(FechaMinNacimiento, FechaMaxNacimiento);
            if (edad == null)
            {
                return HttpNotFound();
            }
            edad.FechaMinNacimiento =DateTime.Parse(edad.FechaMinNacimiento.ToString("yyyy/MM/dd"));
            
            return View(edad);
        }

        // POST: Edads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FechaMinNacimiento,FechaMaxNacimiento,FechaPorDefecto,Estado,FechaDeInicio,FechaDeFin")] Edad edad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(edad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(edad);
        }

        // GET: Edads/Delete/5
        public ActionResult Delete(DateTime FechaMinNacimiento, DateTime FechaMaxNacimiento)
        {
            if (FechaMinNacimiento == null || FechaMaxNacimiento == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Edad edad = db.EDAD.Find(FechaMinNacimiento, FechaMaxNacimiento);
            if (edad == null)
            {
                return HttpNotFound();
            }
            return View(edad);
        }

        // POST: Edads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime FechaMinNacimiento, DateTime FechaMaxNacimiento)
        {
            Edad edad = db.EDAD.Find(FechaMinNacimiento, FechaMaxNacimiento);
            if (edad.Estado == "I")
                edad.Estado = "A";
            else
                edad.Estado = "I";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edads/RealDelete/5
        public ActionResult RealDelete(DateTime FechaMinNacimiento, DateTime FechaMaxNacimiento)
        {
            if (FechaMinNacimiento == null || FechaMaxNacimiento == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Edad edad = db.EDAD.Find(FechaMinNacimiento, FechaMaxNacimiento);
            if (edad == null)
            {
                return HttpNotFound();
            }
            return View(edad);
        }

        // POST: Edads/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(DateTime FechaMinNacimiento, DateTime FechaMaxNacimiento)
        {
            Edad edad = db.EDAD.Find(FechaMinNacimiento, FechaMaxNacimiento);
            db.EDAD.Remove(edad);
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
