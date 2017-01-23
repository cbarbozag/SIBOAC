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
    public class RolDePersonaPorTipoDeIdentificacionDeVehiculoesController : BaseController<RolDePersonaPorTipoDeIdentificacionDeVehiculo>
    {


        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list =

            (from ro in db.ROLPERSONAXTIPOIDEVEHICULO
             join r in db.ROLPERSONA on new { Id = ro.CodigoDeRol.Trim() } equals new { Id = r.Id.Trim() } into r_join
             from r in r_join.DefaultIfEmpty()
             join t in db.TIPOVEH on new {Id = ro.CodigoDeIdentificacionVehiculo.Trim()} equals new { Id = t.Id.ToString()} into t_join
             from t in t_join.DefaultIfEmpty()
             select new
             {
                 CodigoDeRol = ro.CodigoDeRol,
                 CodigoDeIdentificacionVehiculo = ro.CodigoDeIdentificacionVehiculo,
                 Estado = ro.Estado,
                 FechaDeInicio = ro.FechaDeInicio,
                 FechaDeFin = ro.FechaDeFin,
                 DescripcionRol = r.Descripcion,
                 DescripcionTipoVehiculo = t.Descripcion
             }).ToList()

            .Select(x => new RolDePersonaPorTipoDeIdentificacionDeVehiculo
            {
                CodigoDeRol = x.CodigoDeRol,
                CodigoDeIdentificacionVehiculo = x.CodigoDeIdentificacionVehiculo,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionRol = x.DescripcionRol,
                DescripcionTipoVehiculo = x.DescripcionTipoVehiculo

            });                    
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Details/5
        public ActionResult Details(string CodRol, string CodVeh)
        {
            if (CodRol == null|| CodVeh ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(CodRol,CodVeh);

            var list =
           (from ro in db.ROLPERSONAXTIPOIDEVEHICULO
            join r in db.ROLPERSONA on new { Id = ro.CodigoDeRol.Trim() } equals new { Id = r.Id.Trim() } into r_join
            where ro.CodigoDeRol == CodRol
            from r in r_join.DefaultIfEmpty()
            join t in db.TIPOVEH on new { Id = ro.CodigoDeIdentificacionVehiculo.Trim() } equals new { Id = t.Id.ToString() } into t_join
            where ro.CodigoDeIdentificacionVehiculo == CodVeh
            from t in t_join.DefaultIfEmpty()
            select new
            {
                CodigoDeRol = ro.CodigoDeRol,
                CodigoDeIdentificacionVehiculo = ro.CodigoDeIdentificacionVehiculo,
                Estado = ro.Estado,
                FechaDeInicio = ro.FechaDeInicio,
                FechaDeFin = ro.FechaDeFin,
                DescripcionRol = r.Descripcion,
                DescripcionTipoVehiculo = t.Descripcion
            }).ToList()

           .Select(x => new RolDePersonaPorTipoDeIdentificacionDeVehiculo
           {
               CodigoDeRol = x.CodigoDeRol,
               CodigoDeIdentificacionVehiculo = x.CodigoDeIdentificacionVehiculo,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionRol = x.DescripcionRol,
               DescripcionTipoVehiculo = x.DescripcionTipoVehiculo

           }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }

            return View(list);
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Create
        public ActionResult Create()
        {
            var db = new PC_HH_AndroidEntities();
            //se llenan los combos
            IEnumerable<SelectListItem> itemsRol = db.ROLPERSONA
              .Select(o => new SelectListItem
              {
                  Value = o.Id,
                  Text = o.Descripcion
              });
            ViewBag.ComboRolPersona = itemsRol;
            IEnumerable<SelectListItem> itemsTipoVeh = db.TIPOVEH
             .Select(c => new SelectListItem
             {
                 Value = c.Id.ToString(),
                 Text = c.Descripcion
             });
                ViewBag.ComboTipoVeh = itemsTipoVeh;
            return View();
        }

        
        // POST: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoDeRol,CodigoDeIdentificacionVehiculo,Estado,FechaDeInicio,FechaDeFin")] RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.ROLPERSONAXTIPOIDEVEHICULO.Add(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
                string mensaje = Verificar(rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeRol,
                                         rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(rolDePersonaPorTipoDeIdentificacionDeVehiculo, "I", "ROLPERSONAXTIPOIDEVEHICULO");
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
                        Value = o.Id,
                        Text = o.Descripcion
                    });
                    ViewBag.ComboRolPersona = itemsRol;
                    IEnumerable<SelectListItem> itemsTipoVeh = db.TIPOVEH
                     .Select(c => new SelectListItem
                     {
                         Value = c.Id.ToString(),
                         Text = c.Descripcion
                     });
                    ViewBag.ComboTipoVeh = itemsTipoVeh;
                    return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
                }
            }

            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Edit/5
        public ActionResult Edit(string CodRol, string CodVeh)
       {
            if (CodRol == null||CodVeh ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(codRol, CodVeh);


            var list =
            (from ro in db.ROLPERSONAXTIPOIDEVEHICULO
            join r in db.ROLPERSONA on new { Id = ro.CodigoDeRol.Trim() } equals new { Id = r.Id.Trim() } into r_join
            where ro.CodigoDeRol == CodRol
             from r in r_join.DefaultIfEmpty()
            join t in db.TIPOVEH on new { Id = ro.CodigoDeIdentificacionVehiculo.Trim() } equals new { Id = t.Id.ToString() } into t_join
            where ro.CodigoDeIdentificacionVehiculo == CodVeh
            from t in t_join.DefaultIfEmpty()
            select new
            {
                CodigoDeRol = ro.CodigoDeRol,
                CodigoDeIdentificacionVehiculo = ro.CodigoDeIdentificacionVehiculo,
                Estado = ro.Estado,
                FechaDeInicio = ro.FechaDeInicio,
                FechaDeFin = ro.FechaDeFin,
                DescripcionRol = r.Descripcion,
                DescripcionTipoVehiculo = t.Descripcion
            }).ToList()

           .Select(x => new RolDePersonaPorTipoDeIdentificacionDeVehiculo
           {
               CodigoDeRol = x.CodigoDeRol,
               CodigoDeIdentificacionVehiculo = x.CodigoDeIdentificacionVehiculo,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionRol = x.DescripcionRol,
               DescripcionTipoVehiculo = x.DescripcionTipoVehiculo
           }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComboRolPersona = new SelectList(db.ROLPERSONA.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodRol);
            ViewBag.ComboTipoVeh = new SelectList(db.TIPOVEH.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodVeh.ToString().Trim());

            return View(list);
        }

        // POST: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoDeRol,CodigoDeIdentificacionVehiculo,Estado,FechaDeInicio,FechaDeFin")] RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo)
        {
             if (ModelState.IsValid)
             {
                var rolDePersonaPorTipoDeIdentificacionDeVehiculoAntes = db.ROLPERSONAXTIPOIDEVEHICULO.AsNoTracking().Where(d => d.CodigoDeIdentificacionVehiculo == rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo && d.CodigoDeRol== rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeRol).FirstOrDefault();
                db.Entry(rolDePersonaPorTipoDeIdentificacionDeVehiculo).State = EntityState.Modified;
                 db.SaveChanges();
                Bitacora(rolDePersonaPorTipoDeIdentificacionDeVehiculo, "U", "ROLPERSONAXTIPOIDEVEHICULO", rolDePersonaPorTipoDeIdentificacionDeVehiculoAntes);
                return RedirectToAction("Index");
             }

            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
            ViewBag.CodRol = new SelectList(db.ROLPERSONA, "Id", "Descripcion", rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeRol);
            ViewBag.CodVeh = new SelectList(db.TIPOVEH, "Id", "Descripcion", rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo);
                    
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Delete/5
        public ActionResult Delete(string CodRol, string CodVeh)
        {
            if (CodRol == null || CodVeh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(codRol, CodVeh);


            var list =
            (from ro in db.ROLPERSONAXTIPOIDEVEHICULO
             join r in db.ROLPERSONA on new { Id = ro.CodigoDeRol.Trim() } equals new { Id = r.Id.Trim() } into r_join
             where ro.CodigoDeRol == CodRol
             from r in r_join.DefaultIfEmpty()
             join t in db.TIPOVEH on new { Id = ro.CodigoDeIdentificacionVehiculo.Trim() } equals new { Id = t.Id.ToString() } into t_join
             where ro.CodigoDeIdentificacionVehiculo == CodVeh
             from t in t_join.DefaultIfEmpty()
             select new
             {
                 CodigoDeRol = ro.CodigoDeRol,
                 CodigoDeIdentificacionVehiculo = ro.CodigoDeIdentificacionVehiculo,
                 Estado = ro.Estado,
                 FechaDeInicio = ro.FechaDeInicio,
                 FechaDeFin = ro.FechaDeFin,
                 DescripcionRol = r.Descripcion,
                 DescripcionTipoVehiculo = t.Descripcion
             }).ToList()

           .Select(x => new RolDePersonaPorTipoDeIdentificacionDeVehiculo
           {
               CodigoDeRol = x.CodigoDeRol,
               CodigoDeIdentificacionVehiculo = x.CodigoDeIdentificacionVehiculo,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionRol = x.DescripcionRol,
               DescripcionTipoVehiculo = x.DescripcionTipoVehiculo
           }).SingleOrDefault();



            if (list == null)
            {
                return HttpNotFound();
            }
            
            return View(list);
        }

        // POST: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string CodRol, string CodVeh)
        {
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(CodRol, CodVeh);
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculoAntes = ObtenerCopia(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
            if (rolDePersonaPorTipoDeIdentificacionDeVehiculo.Estado == "I")
                rolDePersonaPorTipoDeIdentificacionDeVehiculo.Estado = "A";
            else
                rolDePersonaPorTipoDeIdentificacionDeVehiculo.Estado = "I";
            db.SaveChanges();
            Bitacora(rolDePersonaPorTipoDeIdentificacionDeVehiculo, "U", "ROLPERSONAXTIPOIDEVEHICULO", rolDePersonaPorTipoDeIdentificacionDeVehiculoAntes);
            return RedirectToAction("Index");
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/RealDelete/5
        public ActionResult RealDelete(string CodRol, string CodVeh)
        {
            if (CodRol == null || CodVeh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(codRol, CodVeh);


            var list =
            (from ro in db.ROLPERSONAXTIPOIDEVEHICULO
             join r in db.ROLPERSONA on new { Id = ro.CodigoDeRol.Trim() } equals new { Id = r.Id.Trim() } into r_join
             where ro.CodigoDeRol == CodRol
             from r in r_join.DefaultIfEmpty()
             join t in db.TIPOVEH on new { Id = ro.CodigoDeIdentificacionVehiculo.Trim() } equals new { Id = t.Id.ToString() } into t_join
             where ro.CodigoDeIdentificacionVehiculo == CodVeh
             from t in t_join.DefaultIfEmpty()
             select new
             {
                 CodigoDeRol = ro.CodigoDeRol,
                 CodigoDeIdentificacionVehiculo = ro.CodigoDeIdentificacionVehiculo,
                 Estado = ro.Estado,
                 FechaDeInicio = ro.FechaDeInicio,
                 FechaDeFin = ro.FechaDeFin,
                 DescripcionRol = r.Descripcion,
                 DescripcionTipoVehiculo = t.Descripcion
             }).ToList()

           .Select(x => new RolDePersonaPorTipoDeIdentificacionDeVehiculo
           {
               CodigoDeRol = x.CodigoDeRol,
               CodigoDeIdentificacionVehiculo = x.CodigoDeIdentificacionVehiculo,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionRol = x.DescripcionRol,
               DescripcionTipoVehiculo = x.DescripcionTipoVehiculo
           }).SingleOrDefault();



            if (list == null)
            {
                return HttpNotFound();
            }

            return View(list);
        }

        // POST: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Delete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string CodRol, string CodVeh)
        {
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(CodRol, CodVeh);
            db.ROLPERSONAXTIPOIDEVEHICULO.Remove(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
            db.SaveChanges();
            Bitacora(rolDePersonaPorTipoDeIdentificacionDeVehiculo, "D", "ROLPERSONAXTIPOIDEVEHICULO");
            TempData["Type"] = "error";
            TempData["Message"] = "El registro se eliminó correctamente";
            return RedirectToAction("Index");
        }

        public string Verificar(string codRol, string CodVeh)
        {
            string mensaje = "";
            bool exist = db.ROLPERSONAXTIPOIDEVEHICULO.Any(x => x.CodigoDeRol == codRol
                                                        && x.CodigoDeIdentificacionVehiculo == CodVeh);
            if (exist)
            {
                mensaje = "El codigo rol persona" + codRol +
                    ", código tipo de identificación vehiculo " + CodVeh +
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
