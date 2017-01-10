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
    public class AutoridadsController : BaseController<Autoridad>
    {

        // GET: Autoridads
        public ActionResult Index(int ? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list =
             (
                from a in db.AUTORIDAD
                join o in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = a.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = o.Id } into o_join
                from o in o_join.DefaultIfEmpty()
                select new
                {
                    Id= a.Id,
                    Descripcion= a.Descripcion,
                    CodigoOpcionFormulario =a.CodigoOpcionFormulario,
                    Estado = a.Estado,
                    FechaDeInicio= a.FechaDeInicio,
                    FechaDeFin= a.FechaDeFin,
                    DescripcionCodigoOpcionFormulario = o.Descripcion
              }).ToList()

              .Select(x => new Autoridad
              {
                  Id = x.Id,
                  Descripcion = x.Descripcion,
                  CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                  Estado = x.Estado,
                  FechaDeInicio = x.FechaDeInicio,
                  FechaDeFin = x.FechaDeFin,
                  DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario

              });
            

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
        }

        // GET: Autoridads/Details/5
        public ActionResult Details(string codigo, int? codFormulario)
        {
            if (codigo == null|| codFormulario == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
             (
                from a in db.AUTORIDAD
                join o in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = a.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = o.Id } into o_join
                where a.CodigoOpcionFormulario == codFormulario && a.Id == codigo
                from o in o_join.DefaultIfEmpty()
                select new
                {
                    Id = a.Id,
                    Descripcion = a.Descripcion,
                    CodigoOpcionFormulario = a.CodigoOpcionFormulario,
                    Estado = a.Estado,
                    FechaDeInicio = a.FechaDeInicio,
                    FechaDeFin = a.FechaDeFin,
                    DescripcionCodigoOpcionFormulario = o.Descripcion
                }).ToList()
              .Select(x => new Autoridad
              {
                  Id = x.Id,
                  Descripcion = x.Descripcion,
                  CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                  Estado = x.Estado,
                  FechaDeInicio = x.FechaDeInicio,
                  FechaDeFin = x.FechaDeFin,
                  DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario
                }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: Autoridads/Create
        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsOpcionformulario = db.OPCIONFORMULARIO
              .Select(o => new SelectListItem
              {
                  Value = o.Id.ToString(),
                  Text = o.Descripcion

              });

            ViewBag.ComboOpcionformulario = itemsOpcionformulario;
            return View();
        }

        // POST: Autoridads/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,CodigoOpcionFormulario,Estado,FechaDeInicio,FechaDeFin")] Autoridad autoridad)
        {
            if (ModelState.IsValid)
            {
                db.AUTORIDAD.Add(autoridad);
                string mensaje = Verificar(autoridad.Id,
                                             autoridad.CodigoOpcionFormulario);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(autoridad, "I", "AUTORIDAD");


                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsOpcionformulario = db.OPCIONFORMULARIO.Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.Descripcion

                    });

                    ViewBag.ComboOpcionformulario = itemsOpcionformulario;
                    return View(autoridad);
                }
            }

            return View(autoridad);
        }

        // GET: Autoridads/Edit/5
        public ActionResult Edit(string codigo, int? codFormulario)
        {
            if (codigo == null|| codFormulario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
            (
               from a in db.AUTORIDAD
               join o in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = a.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = o.Id } into o_join
               where a.CodigoOpcionFormulario == codFormulario && a.Id == codigo
               from o in o_join.DefaultIfEmpty()
               select new
               {
                   Id = a.Id,
                   Descripcion = a.Descripcion,
                   CodigoOpcionFormulario = a.CodigoOpcionFormulario,
                   Estado = a.Estado,
                   FechaDeInicio = a.FechaDeInicio,
                   FechaDeFin = a.FechaDeFin,
                   DescripcionCodigoOpcionFormulario = o.Descripcion
               }).ToList()
             .Select(x => new Autoridad
             {
                 Id = x.Id,
                 Descripcion = x.Descripcion,
                 CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                 Estado = x.Estado,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario
             }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComboOpcionformulario = new SelectList(db.OPCIONFORMULARIO.OrderBy(x => x.Descripcion), "Id", "Descripcion", codFormulario);

            return View(list);
        }

        // POST: Autoridads/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,CodigoOpcionFormulario,Estado,FechaDeInicio,FechaDeFin")] Autoridad autoridad)
        {
            if (ModelState.IsValid)
            {
                var autoridadAntes = db.AUTORIDAD.AsNoTracking().Where(d => d.Id == autoridad.Id && d.CodigoOpcionFormulario == autoridad.CodigoOpcionFormulario).FirstOrDefault();

                db.Entry(autoridad).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(autoridad, "U", "AUTORIDAD", autoridadAntes);

                return RedirectToAction("Index");
            }
            return View(autoridad);
        }

        // GET: Autoridads/Delete/5
        public ActionResult Delete(string codigo, int? codFormulario)
        {
            if (codigo == null||codFormulario ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
           (
              from a in db.AUTORIDAD
              join o in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = a.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = o.Id } into o_join
              where a.CodigoOpcionFormulario == codFormulario && a.Id == codigo
              from o in o_join.DefaultIfEmpty()
              select new
              {
                  Id = a.Id,
                  Descripcion = a.Descripcion,
                  CodigoOpcionFormulario = a.CodigoOpcionFormulario,
                  Estado = a.Estado,
                  FechaDeInicio = a.FechaDeInicio,
                  FechaDeFin = a.FechaDeFin,
                  DescripcionCodigoOpcionFormulario = o.Descripcion
              }).ToList()
            .Select(x => new Autoridad
            {
                Id = x.Id,
                Descripcion = x.Descripcion,
                CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario
            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: Autoridads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codigo, int codFormulario)
        {
            Autoridad autoridad = db.AUTORIDAD.Find(codigo, codFormulario);
            Autoridad autoridadAntes = ObtenerCopia(autoridad);

            if (autoridad.Estado == "A")
                autoridad.Estado = "I";
            else
                autoridad.Estado = "A";
            db.SaveChanges();
            Bitacora(autoridad, "U", "AUTORIDAD", autoridadAntes);

            return RedirectToAction("Index");
        }



        // GET: Autoridads/RealDelete/5
        public ActionResult RealDelete(string codigo, int? codFormulario)
        {
            if (codigo == null || codFormulario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
           (
              from a in db.AUTORIDAD
              join o in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = a.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = o.Id } into o_join
              where a.CodigoOpcionFormulario == codFormulario && a.Id == codigo
              from o in o_join.DefaultIfEmpty()
              select new
              {
                  Id = a.Id,
                  Descripcion = a.Descripcion,
                  CodigoOpcionFormulario = a.CodigoOpcionFormulario,
                  Estado = a.Estado,
                  FechaDeInicio = a.FechaDeInicio,
                  FechaDeFin = a.FechaDeFin,
                  DescripcionCodigoOpcionFormulario = o.Descripcion
              }).ToList()
            .Select(x => new Autoridad
            {
                Id = x.Id,
                Descripcion = x.Descripcion,
                CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario
            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: Autoridads/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string codigo, int codFormulario)
        {
            Autoridad autoridad = db.AUTORIDAD.Find(codigo, codFormulario);
            db.AUTORIDAD.Remove(autoridad);
            db.SaveChanges();
            Bitacora(autoridad, "D", "AUTORIDAD");

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

        public string Verificar(string codigo, int? codFormulario)
        {
            string mensaje = "";
            bool exist = db.AUTORIDAD.Any(x => x.Id == codigo
                                                    && x.CodigoOpcionFormulario == codFormulario);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados:"+
                           " código de Autoridad " + codigo +
                           ", código formulario " + codFormulario;

            }
            return mensaje;
        }
    }
}
