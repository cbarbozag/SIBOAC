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
    public class AlineacionVerticalsController : BaseController<AlineacionVertical>
    {

        // GET: AlineacionVerticals
        [SessionExpire]
        public ActionResult Index(int ? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            
            var sCanton = db.ALINVERT.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(sCanton.ToPagedList(pageNumber, pageSize));
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
        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
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
                    mensaje = ValidarFechas(alineacionVertical.FechaDeInicio, alineacionVertical.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(alineacionVertical, "I", "ALINVERT");

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
                var alineacionVerticalAntes = db.ALINVERT.AsNoTracking().Where(d => d.Id == alineacionVertical.Id).FirstOrDefault();

                db.Entry(alineacionVertical).State = EntityState.Modified;
                string mensaje = ValidarFechas(alineacionVertical.FechaDeInicio, alineacionVertical.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(alineacionVertical, "U", "ALINVERT", alineacionVerticalAntes);
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
            AlineacionVertical alineacionVerticalAntes = ObtenerCopia(alineacionVertical);

            if (alineacionVertical.Estado == "A")
                alineacionVertical.Estado = "I";
            else
                alineacionVertical.Estado = "A";
            db.SaveChanges();
            Bitacora(alineacionVertical, "U", "ALINVERT", alineacionVerticalAntes);

            return RedirectToAction("Index");
        }


        // GET: AlineacionVerticals/RealDelete/5
        public ActionResult RealDelete(int? id)
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
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int id)
        {
            AlineacionVertical alineacionVertical = db.ALINVERT.Find(id);
            db.ALINVERT.Remove(alineacionVertical);
            db.SaveChanges();
            Bitacora(alineacionVertical, "D", "ALINVERT");

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
