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
using System.Data.Entity.Validation;

namespace Cosevi.SIBOAC.Controllers
{
    public class CatalogoDeArticulosController : BaseController<CatalogoDeArticulos>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: CatalogoDeArticulos
        [SessionExpire]
        public ActionResult Index(int? page, string searchString, int? radio)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";            
            var list = from s in db.CATARTICULO.ToList().OrderBy(s => s.Estado) select s;
            if (radio == 1)
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(s => s.Id.Contains(searchString)
                                            || s.Conducta.Contains(searchString)
                                            || s.Descripcion.Contains(searchString)                                            
                                            || s.Multa.ToString().Contains(searchString)).OrderBy(s => s.Estado);

                    list = list.Where(s => s.Estado.Equals("A"));
                }
                else
                {
                    list = list.Where(s => s.Estado.Equals("A"));
                }
            }
            if (radio == 2)
            {

                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(s => s.Id.Contains(searchString)
                                            || s.Conducta.Contains(searchString)
                                            || s.Descripcion.Contains(searchString)                                            
                                            || s.Multa.ToString().Contains(searchString)).OrderBy(s => s.Estado);

                    list = list.Where(s => s.Estado.Equals("I"));
                }
                else
                {
                    list = list.Where(s => s.Estado.Equals("I"));
                }
            }
            if (radio.ToString() == "")
            {
                if (!String.IsNullOrEmpty(searchString))
                {
                    list = list.Where(s => s.Id.Contains(searchString)
                                            || s.Conducta.Contains(searchString)
                                            || s.Descripcion.Contains(searchString)                                            
                                            || s.Multa.ToString().Contains(searchString));
                }
            }

            int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));

            
        }


        // GET: CatalogoDeArticulos/Details/5
        public ActionResult Details(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            if (codigo == null || conducta == null || fechaInicio == null || fechaFinal == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            if (catalogoDeArticulos == null)
            {
                return HttpNotFound();
            }
            return View(catalogoDeArticulos);
        }

        // GET: CatalogoDeArticulos/Create
        public ActionResult Create()
        {
            return View();
        }

        public string Verificar(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            string mensaje = "";
            bool exist = db.CATARTICULO.Any(x => x.Id == codigo
                                          && x.Conducta == conducta
                                          && x.FechaDeInicio == fechaInicio
                                          && x.FechaDeFin == fechaFinal);
            if (exist)
            {
                mensaje = "El codigo " + codigo +
                    ",con la conducta " + conducta +
                    ",Fecha Inicio " + fechaInicio +
                    ",Fecha Fin " + fechaFinal
                    + " ya esta registrado";
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

        // POST: CatalogoDeArticulos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Conducta,FechaDeInicio,FechaDeFin,Descripcion,Multa,Estado,Puntos,Totalidad")] CatalogoDeArticulos catalogoDeArticulos)
        {
            if (ModelState.IsValid)
            {
                db.CATARTICULO.Add(catalogoDeArticulos);
                string mensaje = Verificar(catalogoDeArticulos.Id,
                                            catalogoDeArticulos.Conducta,
                                            catalogoDeArticulos.FechaDeInicio,
                                            catalogoDeArticulos.FechaDeFin);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(catalogoDeArticulos.FechaDeInicio, catalogoDeArticulos.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(catalogoDeArticulos, "I", "CATARTICULO");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(catalogoDeArticulos);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(catalogoDeArticulos);
                }
            }

            return View(catalogoDeArticulos);
        }

        // GET: CatalogoDeArticulos/Edit/5
        public ActionResult Edit(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            if (codigo == null || conducta == null || fechaInicio == null || fechaFinal == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            if (catalogoDeArticulos == null)
            {
                return HttpNotFound();
            }
            return View(catalogoDeArticulos);
        }

        // POST: CatalogoDeArticulos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Conducta,FechaDeInicio,FechaDeFin,Descripcion,Multa,Estado,Puntos,Totalidad")] CatalogoDeArticulos catalogoDeArticulos)
        {
            if (ModelState.IsValid)
            {
                var catalogoDeArticulosAntes = db.CATARTICULO.AsNoTracking().Where(d => d.Id == catalogoDeArticulos.Id && 
                                                                                        d.Conducta == catalogoDeArticulos.Conducta &&
                                                                                        d.FechaDeInicio == catalogoDeArticulos.FechaDeInicio &&
                                                                                        d.FechaDeFin == catalogoDeArticulos.FechaDeFin).FirstOrDefault();

                db.Entry(catalogoDeArticulos).State = EntityState.Modified;
                string mensaje = ValidarFechas(catalogoDeArticulos.FechaDeInicio, catalogoDeArticulos.FechaDeFin);

                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(catalogoDeArticulos, "U", "CATARTICULO", catalogoDeArticulosAntes);
                    return RedirectToAction("Index");
                }
                else
                {

                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(catalogoDeArticulos);
                }
            }
            return View();
        }

        // GET: CatalogoDeArticulos/Delete/5
        public ActionResult Delete(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            if (codigo == null || conducta == null || fechaInicio == null || fechaFinal == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            if (catalogoDeArticulos == null)
            {
                return HttpNotFound();
            }
            return View(catalogoDeArticulos);
        }

        // POST: CatalogoDeArticulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            CatalogoDeArticulos catalogoDeArticulosAntes = ObtenerCopia(catalogoDeArticulos);
            if (catalogoDeArticulos.Estado == "A")
                catalogoDeArticulos.Estado = "I";
            else
                catalogoDeArticulos.Estado = "A";
            try
            {
                db.SaveChanges();
            }
             
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            Bitacora(catalogoDeArticulos, "U", "CATARTICULO", catalogoDeArticulosAntes);
            return RedirectToAction("Index");
        }


        // GET: CatalogoDeArticulos/RealDelete/5
        public ActionResult RealDelete(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            if (codigo == null || conducta == null || fechaInicio == null || fechaFinal == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            if (catalogoDeArticulos == null)
            {
                return HttpNotFound();
            }
            return View(catalogoDeArticulos);
        }

        // POST: CatalogoDeArticulos/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string codigo, string conducta, DateTime fechaInicio, DateTime fechaFinal)
        {
            CatalogoDeArticulos catalogoDeArticulos = db.CATARTICULO.Find(codigo, conducta, fechaInicio, fechaFinal);
            db.CATARTICULO.Remove(catalogoDeArticulos);
            db.SaveChanges();
            Bitacora(catalogoDeArticulos, "D", "CATARTICULO");
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
