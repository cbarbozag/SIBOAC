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
    public class RolDePersonaPorTipoDeIdentificacionDeVehiculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.ROLPERSONAXTIPOIDEVEHICULO.ToList());
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Details/5
        public ActionResult Details(string CodRol, string CodVeh)
        {
            if (CodRol == null|| CodVeh ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(CodRol,CodVeh);
            if (rolDePersonaPorTipoDeIdentificacionDeVehiculo == null)
            {
                return HttpNotFound();
            }
            rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo = rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo.Trim();
            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
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

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
                }
            }

            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Edit/5
        public ActionResult Edit(string codRol, string CodVeh)
       {
            if (codRol == null||CodVeh ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(codRol, CodVeh);
            if (rolDePersonaPorTipoDeIdentificacionDeVehiculo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComboRolPersona = new SelectList(db.ROLPERSONA.OrderBy(x => x.Descripcion), "Id", "Descripcion",codRol);
            ViewBag.ComboTipoVeh = new SelectList(db.TIPOVEH.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodVeh.ToString().Trim());

            rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo = rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo.Trim();
            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
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
                 db.Entry(rolDePersonaPorTipoDeIdentificacionDeVehiculo).State = EntityState.Modified;
                 db.SaveChanges();
                 return RedirectToAction("Index");
             }
             return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
                   ViewBag.codRol = new SelectList(db.ROLPERSONA, "Id", "Descripcion", rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeRol);
            ViewBag.CodVeh = new SelectList(db.TIPOVEH, "Id", "Descripcion", rolDePersonaPorTipoDeIdentificacionDeVehiculo.CodigoDeIdentificacionVehiculo);
                    
        }

        // GET: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Delete/5
        public ActionResult Delete(string codRol, string CodVeh)
        {
            if (codRol ==null || CodVeh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(codRol, CodVeh);
            if (rolDePersonaPorTipoDeIdentificacionDeVehiculo == null)
            {
                return HttpNotFound();
            }
            
            return View(rolDePersonaPorTipoDeIdentificacionDeVehiculo);
        }

        // POST: RolDePersonaPorTipoDeIdentificacionDeVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codRol, string CodVeh)
        {
            RolDePersonaPorTipoDeIdentificacionDeVehiculo rolDePersonaPorTipoDeIdentificacionDeVehiculo = db.ROLPERSONAXTIPOIDEVEHICULO.Find(codRol,CodVeh);
            if (rolDePersonaPorTipoDeIdentificacionDeVehiculo.Estado == "I")
                rolDePersonaPorTipoDeIdentificacionDeVehiculo.Estado = "A";
            else
                rolDePersonaPorTipoDeIdentificacionDeVehiculo.Estado = "I";
            db.SaveChanges();
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
