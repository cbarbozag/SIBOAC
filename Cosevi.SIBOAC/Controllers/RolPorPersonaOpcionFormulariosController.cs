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
    public class RolPorPersonaOpcionFormulariosController : BaseController<RolPorPersonaOpcionFormulario>
    {
        // GET: RolPorPersonaOpcionFormularios
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            //ViewBag.ComboRolPersona = TempData["ComboRolPersona"] != null ? TempData["ComboRolPersona"].ToString() : "";

            var list =
           (from ro in db.ROLPERSONA_OPCIONFORMULARIO
            join r in db.ROLPERSONA on new { Id = ro.CodigoRolPersona.ToString() } equals new { Id = r.Id } into r_join
            from r in r_join.DefaultIfEmpty()
            join o in db.OPCIONFORMULARIO on new { Id = ro.CodigoOpcionFormulario } equals new { Id = o.Id } into o_join
            from o in o_join.DefaultIfEmpty()
            select new
            {
                CodigoRolPersona = ro.CodigoRolPersona,
                CodigoOpcionFormulario = ro.CodigoOpcionFormulario,
                Estado = ro.Estado,
                FechaDeInicio = ro.FechaDeInicio,
                FechaDeFin = ro.FechaDeFin,
                DescripcionCodigoOpcionFormulario = o.Descripcion,
                DescripcionCodigoRolPersona = r.Descripcion
            }).ToList()


            .Select(x => new RolPorPersonaOpcionFormulario
            {

                CodigoRolPersona = x.CodigoRolPersona,
                CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
                DescripcionCodigoRolPersona = x.DescripcionCodigoRolPersona

            });            
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
        }

        public string Verificar(int? codRol, int? codFormulario)
        {
            string mensaje = "";
            bool exist = db.ROLPERSONA_OPCIONFORMULARIO.Any(x => x.CodigoRolPersona == codRol
                                                && x.CodigoOpcionFormulario == codFormulario);
            if (exist)
            {
                mensaje = "El código del rol de la persona " + codRol +
                     ", código opción formulario " + codFormulario +
                     " ya esta registrado";
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
        // GET: RolPorPersonaOpcionFormularios/Details/5
        public ActionResult Details(int? codRol, int? codFormulario)
        {
            if (codRol == null|| codFormulario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);


            var list =
           (from ro in db.ROLPERSONA_OPCIONFORMULARIO
            join r in db.ROLPERSONA on ro.CodigoRolPersona.ToString() equals r.Id into r_join
            where ro.CodigoRolPersona == codRol
            from r in r_join.DefaultIfEmpty()
            join o in db.OPCIONFORMULARIO on new { Id = ro.CodigoOpcionFormulario } equals new { Id = o.Id } into o_join
            where ro.CodigoOpcionFormulario == codFormulario
            from o in o_join.DefaultIfEmpty()
            select new
            {
                CodigoRolPersona = ro.CodigoRolPersona,
                CodigoOpcionFormulario = ro.CodigoOpcionFormulario,
                Estado = ro.Estado,
                FechaDeInicio = ro.FechaDeInicio,
                FechaDeFin = ro.FechaDeFin,
                DescripcionCodigoOpcionFormulario = o.Descripcion,
                DescripcionCodigoRolPersona = r.Descripcion
            }).ToList()


            .Select(x => new RolPorPersonaOpcionFormulario
            {

                CodigoRolPersona = x.CodigoRolPersona,
                CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
                DescripcionCodigoRolPersona = x.DescripcionCodigoRolPersona

            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: RolPorPersonaOpcionFormularios/Create
        public ActionResult Create()
        {

            IEnumerable<SelectListItem> itemsRol = db.ROLPERSONA
             .Select(o => new SelectListItem
             {
                 Value = o.Id.Trim(),
                 Text = o.Descripcion
             });
            //TempData["ComboRolPersona"] = itemsRol;
            ViewBag.ComboRolPersona = itemsRol;

            IEnumerable<SelectListItem> itemsOpcionFormulario = db.OPCIONFORMULARIO
            .Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Descripcion
            });
            ViewBag.ComboOpcionFormulario = itemsOpcionFormulario;
           
            return View();
        }

        // POST: RolPorPersonaOpcionFormularios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoRolPersona,CodigoOpcionFormulario,Estado,FechaDeInicio,FechaDeFin")] RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario)
        {
            if (ModelState.IsValid)
            {
               
                db.ROLPERSONA_OPCIONFORMULARIO.Add(rolPorPersonaOpcionFormulario);
                string mensaje = Verificar(rolPorPersonaOpcionFormulario.CodigoRolPersona, rolPorPersonaOpcionFormulario.CodigoOpcionFormulario);

                if (mensaje == "")
                {
                    mensaje = ValidarFechas(rolPorPersonaOpcionFormulario.FechaDeInicio, rolPorPersonaOpcionFormulario.FechaDeFin);

                    if (mensaje == "")
                {
                    db.SaveChanges();

                        Bitacora(rolPorPersonaOpcionFormulario, "I", "ROLPERSONA_OPCIONFORMULARIO");
                        TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsRol = db.ROLPERSONA
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.Trim(),
                        Text = o.Descripcion
                    });
                    //TempData["ComboRolPersona"] = itemsRol;
                    ViewBag.ComboRolPersona = itemsRol;

                    IEnumerable<SelectListItem> itemsOpcionFormulario = db.OPCIONFORMULARIO
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.Descripcion
                    });
                    ViewBag.ComboOpcionFormulario = itemsOpcionFormulario;
                    return View(rolPorPersonaOpcionFormulario);
                }

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(rolPorPersonaOpcionFormulario);
                }
            }

            return View(rolPorPersonaOpcionFormulario);
        }

        // GET: RolPorPersonaOpcionFormularios/Edit/5
        public ActionResult Edit(int? codRol, int? codFormulario)
        {
            if (codRol == null || codFormulario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);

            var list =
           (from ro in db.ROLPERSONA_OPCIONFORMULARIO
           join r in db.ROLPERSONA on ro.CodigoRolPersona.ToString() equals r.Id into r_join
           where ro.CodigoRolPersona == codRol
           from r in r_join.DefaultIfEmpty()
           join o in db.OPCIONFORMULARIO on new { Id = ro.CodigoOpcionFormulario } equals new { Id = o.Id } into o_join
           where ro.CodigoOpcionFormulario == codFormulario
           from o in o_join.DefaultIfEmpty()
           select new
           {
               CodigoRolPersona = ro.CodigoRolPersona,
               CodigoOpcionFormulario = ro.CodigoOpcionFormulario,
               Estado = ro.Estado,
               FechaDeInicio = ro.FechaDeInicio,
               FechaDeFin = ro.FechaDeFin,
               DescripcionCodigoOpcionFormulario = o.Descripcion,
               DescripcionCodigoRolPersona = r.Descripcion
           }).ToList()


           .Select(x => new RolPorPersonaOpcionFormulario
           {

               CodigoRolPersona = x.CodigoRolPersona,
               CodigoOpcionFormulario = x.CodigoOpcionFormulario,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
               DescripcionCodigoRolPersona = x.DescripcionCodigoRolPersona

           }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboRolPersona = new SelectList(db.ROLPERSONA.OrderBy(x => x.Descripcion), "Id", "Descripcion", codRol);
            ViewBag.ComboOpcionFormulario = new SelectList(db.OPCIONFORMULARIO.OrderBy(x => x.Descripcion), "Id", "Descripcion", codFormulario);

            return View(list);
        }

        // POST: RolPorPersonaOpcionFormularios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoRolPersona,CodigoOpcionFormulario,Estado,FechaDeInicio,FechaDeFin")] RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario)
        {
            if (ModelState.IsValid)
            {

                var rolPorPersonaOpcionFormularioAntes = db.ROLPERSONA_OPCIONFORMULARIO.AsNoTracking().Where(d => d.CodigoRolPersona == rolPorPersonaOpcionFormulario.CodigoRolPersona &&
                                                                                                             d.CodigoOpcionFormulario == rolPorPersonaOpcionFormulario.CodigoOpcionFormulario).FirstOrDefault();

                db.Entry(rolPorPersonaOpcionFormulario).State = EntityState.Modified;

                string mensaje = ValidarFechas(rolPorPersonaOpcionFormulario.FechaDeInicio, rolPorPersonaOpcionFormulario.FechaDeFin);
                if (mensaje == "")
                {

                    db.SaveChanges();
                    Bitacora(rolPorPersonaOpcionFormulario, "U", "ROLPERSONA_OPCIONFORMULARIO", rolPorPersonaOpcionFormularioAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(rolPorPersonaOpcionFormulario);
                }
            }
                return View(rolPorPersonaOpcionFormulario);
        }

        // GET: RolPorPersonaOpcionFormularios/Delete/5
        public ActionResult Delete(int? codRol, int? codFormulario)
        {
            if (codRol == null|| codFormulario==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);

            var list =
           (from ro in db.ROLPERSONA_OPCIONFORMULARIO
            join r in db.ROLPERSONA on ro.CodigoRolPersona.ToString() equals r.Id into r_join
            where ro.CodigoRolPersona == codRol
            from r in r_join.DefaultIfEmpty()
            join o in db.OPCIONFORMULARIO on new { Id = ro.CodigoOpcionFormulario } equals new { Id = o.Id } into o_join
            where ro.CodigoOpcionFormulario == codFormulario
            from o in o_join.DefaultIfEmpty()
            select new
            {
                CodigoRolPersona = ro.CodigoRolPersona,
                CodigoOpcionFormulario = ro.CodigoOpcionFormulario,
                Estado = ro.Estado,
                FechaDeInicio = ro.FechaDeInicio,
                FechaDeFin = ro.FechaDeFin,
                DescripcionCodigoOpcionFormulario = o.Descripcion,
                DescripcionCodigoRolPersona = r.Descripcion
            }).ToList()


           .Select(x => new RolPorPersonaOpcionFormulario
           {

               CodigoRolPersona = x.CodigoRolPersona,
               CodigoOpcionFormulario = x.CodigoOpcionFormulario,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
               DescripcionCodigoRolPersona = x.DescripcionCodigoRolPersona

           }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: RolPorPersonaOpcionFormularios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int codRol, int codFormulario)
        {
            RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);
            RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormularioAntes = ObtenerCopia(rolPorPersonaOpcionFormulario);

            if (rolPorPersonaOpcionFormulario.Estado == "I")
                rolPorPersonaOpcionFormulario.Estado = "A";
            else
                rolPorPersonaOpcionFormulario.Estado = "I";
            db.SaveChanges();
            Bitacora(rolPorPersonaOpcionFormulario, "U", "ROLPERSONA_OPCIONFORMULARIO", rolPorPersonaOpcionFormularioAntes);
            return RedirectToAction("Index");
        }

        // GET: RolPorPersonaOpcionFormularios/RealDelete/5
        public ActionResult RealDelete(int? codRol, int? codFormulario)
        {
            if (codRol == null || codFormulario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);

            var list =
           (from ro in db.ROLPERSONA_OPCIONFORMULARIO
            join r in db.ROLPERSONA on ro.CodigoRolPersona.ToString() equals r.Id into r_join
            where ro.CodigoRolPersona == codRol
            from r in r_join.DefaultIfEmpty()
            join o in db.OPCIONFORMULARIO on new { Id = ro.CodigoOpcionFormulario } equals new { Id = o.Id } into o_join
            where ro.CodigoOpcionFormulario == codFormulario
            from o in o_join.DefaultIfEmpty()
            select new
            {
                CodigoRolPersona = ro.CodigoRolPersona,
                CodigoOpcionFormulario = ro.CodigoOpcionFormulario,
                Estado = ro.Estado,
                FechaDeInicio = ro.FechaDeInicio,
                FechaDeFin = ro.FechaDeFin,
                DescripcionCodigoOpcionFormulario = o.Descripcion,
                DescripcionCodigoRolPersona = r.Descripcion
            }).ToList()


           .Select(x => new RolPorPersonaOpcionFormulario
           {

               CodigoRolPersona = x.CodigoRolPersona,
               CodigoOpcionFormulario = x.CodigoOpcionFormulario,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
               DescripcionCodigoRolPersona = x.DescripcionCodigoRolPersona

           }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: RolPorPersonaOpcionFormularios/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int codRol, int codFormulario)
        {
            RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);
            db.ROLPERSONA_OPCIONFORMULARIO.Remove(rolPorPersonaOpcionFormulario);
            db.SaveChanges();
            Bitacora(rolPorPersonaOpcionFormulario, "D", "ROLPERSONA_OPCIONFORMULARIO");
            TempData["Type"] = "error";
            TempData["Message"] = "El registro se eliminó correctamente";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
            catch (Exception ex){ Console.WriteLine(ex.Message); }
        }
    }
}
