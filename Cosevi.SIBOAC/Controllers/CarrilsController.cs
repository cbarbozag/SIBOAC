using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;
using  PagedList;

namespace Cosevi.SIBOAC.Controllers
{
    public class CarrilsController : BaseController<Carril>
    {
        [SessionExpire]
        public ViewResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var sCarril = db.CARRIL.ToList();
                         
         
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(sCarril.ToPagedList(pageNumber, pageSize));
        }
        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.CARRIL.Any(x => x.Id.Trim() == id.Trim());
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
        }
        // GET: Carrils/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carril carril = db.CARRIL.Find(id.Trim());
            if (carril == null)
            {
                return HttpNotFound();
            }
            return View(carril);
        }

        // GET: Carrils/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrils/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Carril carril)
        {
            if (ModelState.IsValid)
            {
                db.CARRIL.Add(carril);
                string mensaje = Verificar(carril.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(carril.FechaDeInicio, carril.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(carril, "I", "CARRIL");

                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(carril);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(carril);
                }
            }

            return View(carril);
        }

        // GET: Carrils/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carril carril = db.CARRIL.Find(id.Trim());
            if (carril == null)
            {
                return HttpNotFound();
            }
            return View(carril);
        }

        // POST: Carrils/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Carril carril)
        {
            if (ModelState.IsValid)
            {
                var carrilAntes = db.CARRIL.AsNoTracking().Where(d => d.Id.Trim() == carril.Id.Trim()).FirstOrDefault();

                db.Entry(carril).State = EntityState.Modified;
                string  mensaje = ValidarFechas(carril.FechaDeInicio, carril.FechaDeFin);

                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(carril, "U", "CARRIL", carrilAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(carril);
                }
            }
            return View(carril);
        }

        // GET: Carrils/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carril carril = db.CARRIL.Find(id);
            if (carril == null)
            {
                return HttpNotFound();
            }
            return View(carril);
        }

        // POST: Carrils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Carril carril = db.CARRIL.Find(id.Trim());
            Carril carrilAntes = ObtenerCopia(carril);

            if (carril.Estado == "A")
                carril.Estado = "I";
            else
                carril.Estado = "A";
            db.SaveChanges();
            Bitacora(carril, "U", "CARRIL", carrilAntes);

            return RedirectToAction("Index");
        }


        // GET: Carrils/RealDelete/5
        public ActionResult RealDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carril carril = db.CARRIL.Find(id.Trim());
            if (carril == null)
            {
                return HttpNotFound();
            }
            return View(carril);
        }

        // POST: Carrils/Delete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id)
        {
            Carril carril = db.CARRIL.Find(id.Trim());
            db.CARRIL.Remove(carril);
            db.SaveChanges();
            Bitacora(carril, "D", "CARRIL");

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
