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
    public class LeyendaPorAutoridadController :BaseController<LEYENDAPORAUTORIDAD>
    {
      
        // GET: LeyendaPorAutoridad
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            var list =
           (from la in db.LEYENDAPORAUTORIDAD
            join a in db.AUTORIDAD on new { Id = la.IdAutoridad } equals new { Id = a.Id } 
            select new
            {
                IdAutoridad = a.Id,
                Descripcion =a.Descripcion,
                PiePagina = la.PiePagina,
                Estado = la.Estado,
                FechaDeInicio = la.Fecha_Inicio,
                FechaDeFin = la.Fecha_Fin               
            }).ToList().Distinct()
           .Select(x => new LEYENDAPORAUTORIDAD
           {
               IdAutoridad = x.IdAutoridad,
               PiePagina = x.PiePagina,
               Estado = x.Estado,
               Fecha_Inicio = x.FechaDeInicio,
               Fecha_Fin = x.FechaDeFin ,
               DescripcionAutoridad = x.Descripcion          
           });
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: LeyendaPorAutoridad/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
            (from la in db.LEYENDAPORAUTORIDAD
             where la.IdAutoridad == id
             join a in db.AUTORIDAD on new { Id = la.IdAutoridad } equals new { Id = a.Id }
             select new
             {
                 IdAutoridad = a.Id,
                 PiePagina = la.PiePagina,
                 Estado = la.Estado,
                 FechaDeInicio = la.Fecha_Inicio,
                 FechaDeFin = la.Fecha_Fin,
                 Descripcion = a.Descripcion
             }).ToList().Take(1)
            .Select(x => new LEYENDAPORAUTORIDAD
            {
                IdAutoridad = x.IdAutoridad,
                PiePagina = x.PiePagina,
                Estado = x.Estado,
                Fecha_Inicio = x.FechaDeInicio,
                Fecha_Fin = x.FechaDeFin,
                DescripcionAutoridad = x.Descripcion
            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: LeyendaPorAutoridad/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> items = (from o in db.AUTORIDAD
                                                             where o.Estado == "A"
                                                             select new { o.Id ,o.Descripcion}).ToList().Distinct()
                                                             .Select(o => new SelectListItem
                                                             {
                                                                 Value = o.Id.ToString(),
                                                                 Text = o.Descripcion.ToString()
                                                             });
            ViewBag.ComboAutoridad = items;
            return View();
        }

        // POST: LeyendaPorAutoridad/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAutoridad,PiePagina,Estado,Fecha_Inicio,Fecha_Fin")] LEYENDAPORAUTORIDAD lEYENDAPORAUTORIDAD)
        {
            if (ModelState.IsValid)
            {
                db.LEYENDAPORAUTORIDAD.Add(lEYENDAPORAUTORIDAD);
                string mensaje = Verificar(lEYENDAPORAUTORIDAD.IdAutoridad);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(lEYENDAPORAUTORIDAD, "I", "LEYENDAPORAUTORIDAD");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;

                    ViewBag.ComboAutoridad = null;
                    ViewBag.ComboAutoridad= new SelectList((from o in db.AUTORIDAD
                                                             where o.Estado == "A"
                                                             select new { o.Id,o.Descripcion }).ToList().Distinct(), "Id", "Descripcion", lEYENDAPORAUTORIDAD.IdAutoridad);

             

                    return View(lEYENDAPORAUTORIDAD);
                }
            }

            return View(lEYENDAPORAUTORIDAD);
        }

        // GET: LeyendaPorAutoridad/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
            (from la in db.LEYENDAPORAUTORIDAD
             where la.IdAutoridad == id
             join a in db.AUTORIDAD on new { Id = la.IdAutoridad } equals new { Id = a.Id }
             select new
             {
                 IdAutoridad = a.Id,
                 PiePagina = la.PiePagina,
                 Estado = la.Estado,
                 FechaDeInicio = la.Fecha_Inicio,
                 FechaDeFin = la.Fecha_Fin
             }).ToList().Take(1)
            .Select(x => new LEYENDAPORAUTORIDAD
            {
                IdAutoridad = x.IdAutoridad,
                PiePagina = x.PiePagina,
                Estado = x.Estado,
                Fecha_Inicio = x.FechaDeInicio,
                Fecha_Fin = x.FechaDeFin
            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboAutoridad = new SelectList((from o in db.AUTORIDAD                                                    
                                                     select new { o.Id,o.Descripcion }).ToList().Distinct(), "Id", "Descripcion", id);


            return View(list);
        }

        // POST: LeyendaPorAutoridad/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAutoridad,PiePagina,Estado,Fecha_Inicio,Fecha_Fin")] LEYENDAPORAUTORIDAD lEYENDAPORAUTORIDAD)
        {
            if (ModelState.IsValid)
            {
                var lEYENDAPORAUTORIDADAntes = db.LEYENDAPORAUTORIDAD.AsNoTracking().Where(d => d.IdAutoridad == lEYENDAPORAUTORIDAD.IdAutoridad).FirstOrDefault();
                db.Entry(lEYENDAPORAUTORIDAD).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(lEYENDAPORAUTORIDAD, "U", "LEYENDAPORAUTORIDAD", lEYENDAPORAUTORIDADAntes);
                return RedirectToAction("Index");
            }
            return View(lEYENDAPORAUTORIDAD);
        }

        // GET: LeyendaPorAutoridad/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
          (from la in db.LEYENDAPORAUTORIDAD
           where la.IdAutoridad == id
           join a in db.AUTORIDAD on new { Id = la.IdAutoridad } equals new { Id = a.Id }
           select new
           {
               IdAutoridad = a.Id,
               PiePagina = la.PiePagina,
               Estado = la.Estado,
               FechaDeInicio = la.Fecha_Inicio,
               FechaDeFin = la.Fecha_Fin,
               Descripcion = a.Descripcion
           }).ToList().Take(1)
          .Select(x => new LEYENDAPORAUTORIDAD
          {
              IdAutoridad = x.IdAutoridad,
              PiePagina = x.PiePagina,
              Estado = x.Estado,
              Fecha_Inicio = x.FechaDeInicio,
              Fecha_Fin = x.FechaDeFin,
              DescripcionAutoridad = x.Descripcion
          }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: LeyendaPorAutoridad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LEYENDAPORAUTORIDAD lEYENDAPORAUTORIDAD = db.LEYENDAPORAUTORIDAD.Find(id);
            LEYENDAPORAUTORIDAD lEYENDAPORAUTORIDADAntes = ObtenerCopia(lEYENDAPORAUTORIDAD);
            db.LEYENDAPORAUTORIDAD.Remove(lEYENDAPORAUTORIDAD);
            db.SaveChanges();
            Bitacora(lEYENDAPORAUTORIDAD, "D", "LEYENDAPORAUTORIDAD", lEYENDAPORAUTORIDADAntes);
            return RedirectToAction("Index");
        }
        public string Verificar(string idAutoridad)
        {
            string mensaje = "";
            bool exist = db.LEYENDAPORAUTORIDAD.Any(x => x.IdAutoridad == idAutoridad);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados: Autoridad" + idAutoridad;
                          

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
        public ActionResult Inactivar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
          (from la in db.LEYENDAPORAUTORIDAD
           where la.IdAutoridad == id
           join a in db.AUTORIDAD on new { Id = la.IdAutoridad } equals new { Id = a.Id }
           select new
           {
               IdAutoridad = a.Id,
               PiePagina = la.PiePagina,
               Estado = la.Estado,
               FechaDeInicio = la.Fecha_Inicio,
               FechaDeFin = la.Fecha_Fin,
               Descripcion = a.Descripcion
           }).ToList().Take(1)
          .Select(x => new LEYENDAPORAUTORIDAD
          {
              IdAutoridad = x.IdAutoridad,
              PiePagina = x.PiePagina,
              Estado = x.Estado,
              Fecha_Inicio = x.FechaDeInicio,
              Fecha_Fin = x.FechaDeFin,
              DescripcionAutoridad = x.Descripcion
          }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: LeyendaPorAutoridad/Delete/5
        [HttpPost, ActionName("Inactivar")]
        [ValidateAntiForgeryToken]
        public ActionResult InactivarConfirmed(string id)
        {
            LEYENDAPORAUTORIDAD lEYENDAPORAUTORIDAD = db.LEYENDAPORAUTORIDAD.Find(id);
            LEYENDAPORAUTORIDAD lEYENDAPORAUTORIDADAntes = ObtenerCopia(lEYENDAPORAUTORIDAD);
            if (lEYENDAPORAUTORIDAD.Estado == "I")
                lEYENDAPORAUTORIDAD.Estado = "A";
            else
                lEYENDAPORAUTORIDAD.Estado = "I";
            db.SaveChanges();
            Bitacora(lEYENDAPORAUTORIDAD, "U", "LEYENDAPORAUTORIDAD", lEYENDAPORAUTORIDADAntes);
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
