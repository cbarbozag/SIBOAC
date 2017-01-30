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
    public class DetallePorTipoSenialsController : BaseController<DetallePorTipoSenial>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DetallePorTipoSenials
        [SessionExpire]
        public ActionResult Index(int ? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list =
              (
                   from dts in db.DETALLETIPOSEÑAL
                   join tse in db.TIPOSEÑALEXISTE on new { CodigoTipoSenial = dts.CodigoTipoSenial } equals new { CodigoTipoSenial = tse.Id } into tse_join
                   from tse in tse_join.DefaultIfEmpty()
                   select new
                    {
                        CodigoTipoSenial = dts.CodigoTipoSenial,
                        Id= dts.Id,
                        Descripcion = dts.Descripcion,
                        Estado = dts.Estado,
                        FechaDeInicio= dts.FechaDeInicio,
                        FechaDeFin = dts.FechaDeFin,                     
                        DescripcionCodigoTipoSenial = tse.Descripcion
                    }).ToList()
                   .Select(x => new DetallePorTipoSenial
                   {
                       CodigoTipoSenial = x.CodigoTipoSenial,
                       Id = x.Id,
                       Descripcion = x.Descripcion,
                       Estado = x.Estado,
                       FechaDeInicio = x.FechaDeInicio,
                       FechaDeFin = x.FechaDeFin,                    
                       DescripcionCodigoTipoSenial = x.DescripcionCodigoTipoSenial

                   });
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
        }

        // GET: DetallePorTipoSenials/Details/5
        public ActionResult Details(string codsenex, string codigose)
        {
            if (codsenex == null || codigose == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
            (
                from dts in db.DETALLETIPOSEÑAL
                 join tse in db.TIPOSEÑALEXISTE on new { CodigoTipoSenial = dts.CodigoTipoSenial } equals new { CodigoTipoSenial = tse.Id } into tse_join
                 where dts.Id == codigose && dts.CodigoTipoSenial == codsenex
                 from tse in tse_join.DefaultIfEmpty()           
                  select new
                  {
                      CodigoTipoSenial = dts.CodigoTipoSenial,
                      Id = dts.Id,
                      Descripcion = dts.Descripcion,
                      Estado = dts.Estado,
                      FechaDeInicio = dts.FechaDeInicio,
                      FechaDeFin = dts.FechaDeFin,                      
                      DescripcionCodigoTipoSenial = tse.Descripcion
                  }).ToList()
                 .Select(x => new DetallePorTipoSenial
                 {
                     CodigoTipoSenial = x.CodigoTipoSenial,
                     Id = x.Id,
                     Descripcion = x.Descripcion,
                     Estado = x.Estado,
                     FechaDeInicio = x.FechaDeInicio,
                     FechaDeFin = x.FechaDeFin,
                     DescripcionCodigoTipoSenial = x.DescripcionCodigoTipoSenial

                 }).SingleOrDefault();
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: DetallePorTipoSenials/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> itemsTipoSenial = db.TIPOSEÑALEXISTE
            .Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Descripcion
            });
            ViewBag.ComboTipoSenialExiste = itemsTipoSenial;
            
            return View();
        }

        // POST: DetallePorTipoSenials/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTipoSenial,Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoSenial detallePorTipoSenial)
        {
            if (ModelState.IsValid)
            {
                db.DETALLETIPOSEÑAL.Add(detallePorTipoSenial);
                string mensaje = Verificar(detallePorTipoSenial.CodigoTipoSenial,
                                          detallePorTipoSenial.Id);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(detallePorTipoSenial.FechaDeInicio, detallePorTipoSenial.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(detallePorTipoSenial, "I", "DETALLETIPOSEÑAL");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        IEnumerable<SelectListItem> itemsTipoSenial = db.TIPOSEÑALEXISTE.Select(o => new SelectListItem
                        {
                            Value = o.Id.ToString(),
                            Text = o.Descripcion
                        });
                        ViewBag.ComboTipoSenialExiste = itemsTipoSenial;
                        return View(detallePorTipoSenial);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsTipoSenial = db.TIPOSEÑALEXISTE.Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.Descripcion
                    });
                    ViewBag.ComboTipoSenialExiste = itemsTipoSenial;
                    return View(detallePorTipoSenial);
                }
            }

            return View(detallePorTipoSenial);
        }

        // GET: DetallePorTipoSenials/Edit/5
        public ActionResult Edit(string codsenex, string codigose)
        {
            if (codsenex == null || codigose == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
         (
                from dts in db.DETALLETIPOSEÑAL
                join tse in db.TIPOSEÑALEXISTE on new { CodigoTipoSenial = dts.CodigoTipoSenial } equals new { CodigoTipoSenial = tse.Id } into tse_join
                where dts.Id == codigose && dts.CodigoTipoSenial == codsenex
                from tse in tse_join.DefaultIfEmpty()
                select new
                {
                    CodigoTipoSenial = dts.CodigoTipoSenial,
                    Id = dts.Id,
                    Descripcion = dts.Descripcion,
                    Estado = dts.Estado,
                    FechaDeInicio = dts.FechaDeInicio,
                    FechaDeFin = dts.FechaDeFin,
                    DescripcionCodigoTipoSenial = tse.Descripcion
                }).ToList()
                 .Select(x => new DetallePorTipoSenial
                 {
                     CodigoTipoSenial = x.CodigoTipoSenial,
                     Id = x.Id,
                     Descripcion = x.Descripcion,
                     Estado = x.Estado,
                     FechaDeInicio = x.FechaDeInicio,
                     FechaDeFin = x.FechaDeFin,
                     DescripcionCodigoTipoSenial = x.DescripcionCodigoTipoSenial

                 }).SingleOrDefault();
            if (list == null)
            {
                return HttpNotFound();
            }
          
            ViewBag.ComboTipoSenialExiste = new SelectList(db.TIPOSEÑALEXISTE.OrderBy(x => x.Descripcion), "Id", "Descripcion", codsenex);

            return View(list);
        }

        // POST: DetallePorTipoSenials/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTipoSenial,Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] DetallePorTipoSenial detallePorTipoSenial)
        {
            if (ModelState.IsValid)
            {
                var detallePorTipoSenialAntes = db.DETALLETIPOSEÑAL.AsNoTracking().Where(d => d.CodigoTipoSenial == detallePorTipoSenial.CodigoTipoSenial &&
                                                                                    d.Id == detallePorTipoSenial.Id).FirstOrDefault();
                string mensaje = ValidarFechas(detallePorTipoSenial.FechaDeInicio, detallePorTipoSenial.FechaDeFin);

                if (mensaje == "")
                {
                    db.Entry(detallePorTipoSenial).State = EntityState.Modified;
                    db.SaveChanges();
                    Bitacora(detallePorTipoSenial, "U", "DETALLETIPOSEÑAL", detallePorTipoSenialAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsTipoSenial = db.TIPOSEÑALEXISTE.Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.Descripcion
                    });
                    ViewBag.ComboTipoSenialExiste = itemsTipoSenial;
                    return View(detallePorTipoSenial);
                }
            }
            return View(detallePorTipoSenial);
        }

        // GET: DetallePorTipoSenials/Delete/5
        public ActionResult Delete(string codsenex, string codigose)
        {
            if (codsenex == null || codigose == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
          (
               from dts in db.DETALLETIPOSEÑAL
               join se in db.SEÑALAMIENTO on new { CodigoTipoSenial = dts.CodigoTipoSenial } equals new { CodigoTipoSenial = se.Id.ToString() } into se_join
               where dts.Id == codigose && dts.CodigoTipoSenial == codsenex
               from se in se_join.DefaultIfEmpty()
               join tse in db.TIPOSEÑALEXISTE on dts.Id equals tse.Id into tse_join
               from tse in tse_join.DefaultIfEmpty()
               select new
                {
                       CodigoTipoSenial = dts.CodigoTipoSenial,
                       Id = dts.Id,
                       Descripcion = dts.Descripcion,
                       Estado = dts.Estado,
                       FechaDeInicio = dts.FechaDeInicio,
                       FechaDeFin = dts.FechaDeFin,
                       DescripcionSenalamiento = se.Descripcion,
                       DescripcionCodigoTipoSenial = tse.Descripcion
                }).ToList()
                .Select(x => new DetallePorTipoSenial
                 {
                      CodigoTipoSenial = x.CodigoTipoSenial,
                      Id = x.Id,
                      Descripcion = x.Descripcion,
                      Estado = x.Estado,
                      FechaDeInicio = x.FechaDeInicio,
                      FechaDeFin = x.FechaDeFin,
                      DescripcionSenalamiento = x.DescripcionSenalamiento,
                      DescripcionCodigoTipoSenial = x.DescripcionCodigoTipoSenial

                  }).SingleOrDefault();
                    if (list == null)
                    {
                        return HttpNotFound();
                    }

            return View(list);
        }

        // POST: DetallePorTipoSenials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codsenex, string codigose)
        {
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codsenex, codigose);
            DetallePorTipoSenial detallePorTipoSenialAntes = ObtenerCopia(detallePorTipoSenial);

            if (detallePorTipoSenial.Estado == "A")
                detallePorTipoSenial.Estado = "I";
            else
                detallePorTipoSenial.Estado = "A";
            db.SaveChanges();
            Bitacora(detallePorTipoSenial, "U", "DETALLETIPOSEÑAL", detallePorTipoSenialAntes);
            return RedirectToAction("Index");
        }

        // GET: DetallePorTipoSenials/RealDelete/5
        public ActionResult RealDelete(string codsenex, string codigose)
        {
            if (codsenex == null || codigose == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
          (
               from dts in db.DETALLETIPOSEÑAL
               join se in db.SEÑALAMIENTO on new { CodigoTipoSenial = dts.CodigoTipoSenial } equals new { CodigoTipoSenial = se.Id.ToString() } into se_join
               where dts.Id == codigose && dts.CodigoTipoSenial == codsenex
               from se in se_join.DefaultIfEmpty()
               join tse in db.TIPOSEÑALEXISTE on dts.Id equals tse.Id into tse_join
               from tse in tse_join.DefaultIfEmpty()
               select new
               {
                   CodigoTipoSenial = dts.CodigoTipoSenial,
                   Id = dts.Id,
                   Descripcion = dts.Descripcion,
                   Estado = dts.Estado,
                   FechaDeInicio = dts.FechaDeInicio,
                   FechaDeFin = dts.FechaDeFin,
                   DescripcionSenalamiento = se.Descripcion,
                   DescripcionCodigoTipoSenial = tse.Descripcion
               }).ToList()
                .Select(x => new DetallePorTipoSenial
                {
                    CodigoTipoSenial = x.CodigoTipoSenial,
                    Id = x.Id,
                    Descripcion = x.Descripcion,
                    Estado = x.Estado,
                    FechaDeInicio = x.FechaDeInicio,
                    FechaDeFin = x.FechaDeFin,
                    DescripcionSenalamiento = x.DescripcionSenalamiento,
                    DescripcionCodigoTipoSenial = x.DescripcionCodigoTipoSenial

                }).SingleOrDefault();
            if (list == null)
            {
                return HttpNotFound();
            }

            return View(list);
        }

        // POST: DetallePorTipoSenials/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string codsenex, string codigose)
        {
            DetallePorTipoSenial detallePorTipoSenial = db.DETALLETIPOSEÑAL.Find(codsenex, codigose);
            db.DETALLETIPOSEÑAL.Remove(detallePorTipoSenial);
            db.SaveChanges();
            Bitacora(detallePorTipoSenial, "D", "DETALLETIPOSEÑAL");
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
        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
        }
        public string Verificar(string codsenex, string codigose)
        {
            string mensaje = "";
            bool exist = db.DETALLETIPOSEÑAL.Any(x => x.CodigoTipoSenial == codsenex
                                                    && x.Id == codigose);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados:" +
                           " código de Tipo Señal" + codsenex +
                           ", código" + codigose;

            }
            return mensaje;
        }
    }
}
