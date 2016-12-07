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
    public class RolDePersonaPorVehiculoesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RolDePersonaPorVehiculoes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            ViewBag.ComboRolPersona = TempData["ComboRolPersona"] != null ? TempData["ComboRolPersona"].ToString() : "";
            var list=            
           (from r in db.RolDePersonaPorVehiculoes
            join rp in db.ROLPERSONA on r.Id equals rp.Id
            select new 
            {
                Id = r.Id,
                ActivarVehiculo = r.ActivarVehiculo,
                Estado = r.Estado,
                FechaDeInicio = r.FechaDeInicio,
                FechaDeFin = r.FechaDeFin,
                DescripcionRolPersona = rp.Descripcion
            }).ToList()
            .Select(x=> new RolDePersonaPorVehiculo
            {
                Id = x.Id,
                ActivarVehiculo = x.ActivarVehiculo,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionRolPersona = x.DescripcionRolPersona
            });
            return View(list);
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.RolDePersonaPorVehiculoes.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo rol persona" + id +
                    " ya esta registrado";
            }
            return mensaje;
        }

        // GET: RolDePersonaPorVehiculoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item =
              (from r in db.RolDePersonaPorVehiculoes
               join rp in db.ROLPERSONA on r.Id equals rp.Id
               where r.Id == id
               select new
               {
                   Id = r.Id,
                   ActivarVehiculo = r.ActivarVehiculo,
                   Estado = r.Estado,
                   FechaDeInicio = r.FechaDeInicio,
                   FechaDeFin = r.FechaDeFin,
                   DescripcionRolPersona = rp.Descripcion
               }).ToList()
              .Select(x => new RolDePersonaPorVehiculo
              {
                  Id = x.Id,
                  ActivarVehiculo = x.ActivarVehiculo,
                  Estado = x.Estado,
                  FechaDeInicio = x.FechaDeInicio,
                  FechaDeFin = x.FechaDeFin,
                  DescripcionRolPersona = x.DescripcionRolPersona
              }).SingleOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: RolDePersonaPorVehiculoes/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> itemsRolPersona = db.ROLPERSONA
            .Select(o => new SelectListItem
            {
                Value = o.Id,
                Text = o.Descripcion
            });
            TempData["ComboRolPersona"] = itemsRolPersona;
            ViewBag.ComboRolPersona = itemsRolPersona;
            return View();
        }

        // POST: RolDePersonaPorVehiculoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ActivarVehiculo,Estado,FechaDeInicio,FechaDeFin")] RolDePersonaPorVehiculo rolDePersonaPorVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.RolDePersonaPorVehiculoes.Add(rolDePersonaPorVehiculo);
                string mensaje = Verificar(rolDePersonaPorVehiculo.Id);
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
                    //IEnumerable<SelectListItem> itemsRolPersona = db.ROLPERSONA
                    //                .Select(o => new SelectListItem
                    //                {
                    //                    Value = o.Id,
                    //                    Text = o.Descripcion
                    //                });
                    //TempData["ComboRolPersona"] = itemsRolPersona;
                    //ViewBag.ComboRolPersona = itemsRolPersona;
                    
                    return View(rolDePersonaPorVehiculo);
                }
            }

           
            return View(rolDePersonaPorVehiculo);
        }

        // GET: RolDePersonaPorVehiculoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //RolDePersonaPorVehiculo rolDePersonaPorVehiculo = db.RolDePersonaPorVehiculoes.Find(id);
            var item =
            (from r in db.RolDePersonaPorVehiculoes
             join rp in db.ROLPERSONA on r.Id equals rp.Id
             where r.Id == id
             select new 
             {
                 Id = r.Id,
                 ActivarVehiculo = r.ActivarVehiculo,
                 Estado = r.Estado,
                 FechaDeInicio = r.FechaDeInicio,
                 FechaDeFin = r.FechaDeFin,
                 DescripcionRolPersona = rp.Descripcion
             }).ToList()
            .Select(x => new RolDePersonaPorVehiculo
            {
                Id = x.Id,
                ActivarVehiculo = x.ActivarVehiculo,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionRolPersona = x.DescripcionRolPersona
            }).SingleOrDefault();

            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComboRolPersona = new SelectList(db.ROLPERSONA.OrderBy(x => x.Descripcion), "Id", "Descripcion", id);

            return View(item);
        }

        // POST: RolDePersonaPorVehiculoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ActivarVehiculo,Estado,FechaDeInicio,FechaDeFin")] RolDePersonaPorVehiculo rolDePersonaPorVehiculo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolDePersonaPorVehiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rolDePersonaPorVehiculo);
        }

        // GET: RolDePersonaPorVehiculoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var item =
             (from r in db.RolDePersonaPorVehiculoes
              join rp in db.ROLPERSONA on r.Id equals rp.Id
              where r.Id == id
              select new
              {
                  Id = r.Id,
                  ActivarVehiculo = r.ActivarVehiculo,
                  Estado = r.Estado,
                  FechaDeInicio = r.FechaDeInicio,
                  FechaDeFin = r.FechaDeFin,
                  DescripcionRolPersona = rp.Descripcion
              }).ToList()
             .Select(x => new RolDePersonaPorVehiculo
             {
                 Id = x.Id,
                 ActivarVehiculo = x.ActivarVehiculo,
                 Estado = x.Estado,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 DescripcionRolPersona = x.DescripcionRolPersona
             }).SingleOrDefault();
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: RolDePersonaPorVehiculoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RolDePersonaPorVehiculo rolDePersonaPorVehiculo = db.RolDePersonaPorVehiculoes.Find(id);
            if (rolDePersonaPorVehiculo.Estado == "I")
                rolDePersonaPorVehiculo.Estado = "A";
            else
                rolDePersonaPorVehiculo.Estado = "I";
            db.SaveChanges();
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
