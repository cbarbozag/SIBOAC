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
    public class DispositivoPorRolPersonasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DispositivoPorRolPersonas
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list =
              (
                from drp in db.DISPXROLPERSONA
                join rp in db.ROLPERSONA on new { CodigoRolPersona = drp.CodigoRolPersona } equals new { CodigoRolPersona = rp.Id } into rp_join
                from rp in rp_join.DefaultIfEmpty()
                join di in db.Dispositivoes1 on new { CodigoDispositivo = drp.CodigoDispositivo } equals new { CodigoDispositivo = di.Id } into di_join
                from di in di_join.DefaultIfEmpty()
                select new
                {
                    CodigoRolPersona =  drp.CodigoRolPersona,
                    CodigoDispositivo=  drp.CodigoDispositivo,
                    DescripcionRolPersona = rp.Descripcion,
                    DescripcionDispositivo = di.Descripcion
                }).ToList()
               .Select(x => new DispositivoPorRolPersona
               {
                   CodigoRolPersona = x.CodigoRolPersona,
                   CodigoDispositivo = x.CodigoDispositivo,
                   DescripcionRolPersona = x.DescripcionRolPersona,
                   DescripcionDispositivo = x.DescripcionDispositivo

               });
            return View(list);
        }

        // GET: DispositivoPorRolPersonas/Details/5
        public ActionResult Details(string CodRol, int? CodDisp)
        {
            if (CodRol == null|| CodDisp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
           (
                from drp in db.DISPXROLPERSONA
                join rp in db.ROLPERSONA on new { CodigoRolPersona = drp.CodigoRolPersona } equals new { CodigoRolPersona = rp.Id } into rp_join
                where drp.CodigoRolPersona == CodRol && drp.CodigoDispositivo == CodDisp
                from rp in rp_join.DefaultIfEmpty()
                join di in db.Dispositivoes1 on new { CodigoDispositivo = drp.CodigoDispositivo } equals new { CodigoDispositivo = di.Id } into di_join
                from di in di_join.DefaultIfEmpty()
                select new
                {
                    CodigoRolPersona = drp.CodigoRolPersona,
                    CodigoDispositivo = drp.CodigoDispositivo,
                    DescripcionRolPersona = rp.Descripcion,
                    DescripcionDispositivo = di.Descripcion
                }).ToList()
            .Select(x => new DispositivoPorRolPersona
            {
                CodigoRolPersona = x.CodigoRolPersona,
                CodigoDispositivo = x.CodigoDispositivo,
                DescripcionRolPersona = x.DescripcionRolPersona,
                DescripcionDispositivo = x.DescripcionDispositivo
            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: DispositivoPorRolPersonas/Create
        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsRol = db.ROLPERSONA
              .Select(o => new SelectListItem
              {
                  Value = o.Id,
                  Text = o.Descripcion
              });
            ViewBag.ComboRolPersona = itemsRol;

            IEnumerable<SelectListItem> itemsDispositivos = db.Dispositivoes1
            .Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Descripcion
            });
            ViewBag.ComboDispositivo = itemsDispositivos;
            return View();
        }

        // POST: DispositivoPorRolPersonas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoRolPersona,CodigoDispositivo")] DispositivoPorRolPersona dispositivoPorRolPersona)
        {
            if (ModelState.IsValid)
            {
                db.DISPXROLPERSONA.Add(dispositivoPorRolPersona);
                string mensaje = Verificar(dispositivoPorRolPersona.CodigoRolPersona,
                                            dispositivoPorRolPersona.CodigoDispositivo);
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
                    return View(dispositivoPorRolPersona);
                }
            }

            return View(dispositivoPorRolPersona);
        }

        // GET: DispositivoPorRolPersonas/Edit/5
        public ActionResult Edit(string CodRol, int? CodDisp)
        {
            if (CodRol == null|| CodDisp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
          (
               from drp in db.DISPXROLPERSONA
               join rp in db.ROLPERSONA on new { CodigoRolPersona = drp.CodigoRolPersona } equals new { CodigoRolPersona = rp.Id } into rp_join
               where drp.CodigoRolPersona == CodRol && drp.CodigoDispositivo == CodDisp
               from rp in rp_join.DefaultIfEmpty()
               join di in db.Dispositivoes1 on new { CodigoDispositivo = drp.CodigoDispositivo } equals new { CodigoDispositivo = di.Id } into di_join
               from di in di_join.DefaultIfEmpty()
               select new
               {
                   CodigoRolPersona = drp.CodigoRolPersona,
                   CodigoDispositivo = drp.CodigoDispositivo,
                   DescripcionRolPersona = rp.Descripcion,
                   DescripcionDispositivo = di.Descripcion
               }).ToList()
           .Select(x => new DispositivoPorRolPersona
           {
               CodigoRolPersona = x.CodigoRolPersona,
               CodigoDispositivo = x.CodigoDispositivo,
               DescripcionRolPersona = x.DescripcionRolPersona,
               DescripcionDispositivo = x.DescripcionDispositivo
           }).SingleOrDefault();
            ViewBag.ComboRolPersona = new SelectList(db.ROLPERSONA.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodRol.ToString());
            ViewBag.ComboDispositivo = new SelectList(db.Dispositivoes1.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodDisp);


            return View(list);
        }

        // POST: DispositivoPorRolPersonas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoRolPersona,CodigoDispositivo")] DispositivoPorRolPersona dispositivoPorRolPersona)
        {
            if (ModelState.IsValid)
            {
                dispositivoPorRolPersona.CodigoRolPersona = dispositivoPorRolPersona.CodigoRolPersona.Trim();
                db.Entry(dispositivoPorRolPersona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dispositivoPorRolPersona);
        }


        // GET: DispositivoPorRolPersonas/RealDelete/5
        public ActionResult RealDelete(string CodRol, int? CodDisp)
        {
            if ( (CodRol == null) || (CodDisp == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivoPorRolPersona dispositivo = db.DISPXROLPERSONA.Find(CodRol, CodDisp);
            if (dispositivo == null)
            {
                return HttpNotFound();
            }
            return View(dispositivo);
        }

        // POST: DispositivoPorRolPersonas/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string CodRol, int? CodDisp)
        {
            DispositivoPorRolPersona dispositivo = db.DISPXROLPERSONA.Find(CodRol, CodDisp);
            db.DISPXROLPERSONA.Remove(dispositivo);
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

        public string Verificar(string CodRol, int CodDisp)
        {
            string mensaje = "";
            bool exist = db.DISPXROLPERSONA.Any(x => x.CodigoRolPersona == CodRol
                                                    && x.CodigoDispositivo == CodDisp);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados:" +
                           " código de Rol Persona" + CodRol +
                           ", código de Dispositivo" + CodDisp;

            }
            return mensaje;
        }
    }
}
