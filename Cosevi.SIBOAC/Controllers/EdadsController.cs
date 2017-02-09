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
    public class EdadsController : BaseController<Edad>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Edads
        [SessionExpire]
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

        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
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
                    mensaje = ValidarFechas(edad.FechaDeInicio, edad.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(edad, "I", "EDAD");
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
            ViewBag.FechaMinNacimiento =DateTime.Parse(edad.FechaMinNacimiento.ToString()).ToString("dd/MM/yyyy");
            ViewBag.FechaMaxNacimiento = DateTime.Parse(edad.FechaMaxNacimiento.ToString()).ToString("dd/MM/yyyy");
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
                var edadAntes = db.EDAD.AsNoTracking().Where(d => d.FechaMinNacimiento == edad.FechaMinNacimiento &&
                                                                                    d.FechaMaxNacimiento == edad.FechaMaxNacimiento).FirstOrDefault();

                string mensaje = ValidarFechas(edad.FechaDeInicio, edad.FechaDeFin);
                Edad edadTem = db.EDAD.Find(edad.FechaMinNacimiento, edad.FechaMaxNacimiento);
                edadTem.FechaPorDefecto = edad.FechaPorDefecto;
                edadTem.Estado = edad.Estado;
                edadTem.FechaDeInicio = edad.FechaDeInicio;
                edadTem.FechaDeFin = edad.FechaDeFin;
                db.Entry(edadTem).State = EntityState.Modified;

                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(edad,"U","EDAD", edadAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
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
            Edad edadAntes = ObtenerCopia(edad);
            if (edad.Estado == "I")
                edad.Estado = "A";
            else
                edad.Estado = "I";
            db.SaveChanges();
            Bitacora(edad, "U", "EDAD", edadAntes);
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
            Bitacora(edad, "D", "EDAD");
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
