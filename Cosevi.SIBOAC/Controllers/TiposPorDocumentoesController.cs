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
    public class TiposPorDocumentoesController : BaseController<TiposPorDocumento>
    {
        // GET: TiposPorDocumentoes
        [SessionExpire]
        public ActionResult Index(int? page)
        {

            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list =
           (from td in db.TIPOSXDOCUMENTO
            join d in db.TIPODOCUMENTO on new { Id = td.CodigoTipoDocumento } equals new { Id = d.Id } into d_join
            from d in d_join.DefaultIfEmpty()
            join i in db.TIPO_IDENTIFICACION on new { Id = td.CodigoTipoDeIdentificacion } equals new { Id = i.Id } into i_join
            from i in i_join.DefaultIfEmpty()
            select new
            {
                CodigoTipoDocumento = td.CodigoTipoDocumento,
                CodigoTipoDeIdentificacion = td.CodigoTipoDeIdentificacion,
                Estado = td.Estado,
                FechaDeInicio = td.FechaDeInicio,
                FechaDeFin = td.FechaDeFin,
                DescripcionCodigoTipoDocumento = d.Descripcion,
                DescripcionCodigoTipoDeIdentificacion = i.Descripcion
            }).ToList()
            .Select(x => new TiposPorDocumento
            {
                CodigoTipoDocumento = x.CodigoTipoDocumento,
                CodigoTipoDeIdentificacion = x.CodigoTipoDeIdentificacion,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoTipoDocumento = x.DescripcionCodigoTipoDocumento,
                DescripcionCodigoTipoDeIdentificacion = x.DescripcionCodigoTipoDeIdentificacion
            });
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
        }

        // GET: TiposPorDocumentoes/Details/5
        public ActionResult Details(string codigod,string codigot)
        {
            if (codigod== null || codigot == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod,codigot);


            var list =
          (from td in db.TIPOSXDOCUMENTO
           join d in db.TIPODOCUMENTO on new { Id = td.CodigoTipoDocumento } equals new { Id = d.Id } into d_join
           where td.CodigoTipoDocumento == codigod
           from d in d_join.DefaultIfEmpty()
           join i in db.TIPO_IDENTIFICACION on new { Id = td.CodigoTipoDeIdentificacion } equals new { Id = i.Id } into i_join
           where td.CodigoTipoDeIdentificacion == codigot
           from i in i_join.DefaultIfEmpty()
           select new
           {
               CodigoTipoDocumento = td.CodigoTipoDocumento,
               CodigoTipoDeIdentificacion = td.CodigoTipoDeIdentificacion,
               Estado = td.Estado,
               FechaDeInicio = td.FechaDeInicio,
               FechaDeFin = td.FechaDeFin,
               DescripcionCodigoTipoDocumento = d.Descripcion,
               DescripcionCodigoTipoDeIdentificacion = i.Descripcion
           }).ToList()


           .Select(x => new TiposPorDocumento
           {
               CodigoTipoDocumento = x.CodigoTipoDocumento,
               CodigoTipoDeIdentificacion = x.CodigoTipoDeIdentificacion,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionCodigoTipoDocumento = x.DescripcionCodigoTipoDocumento,
               DescripcionCodigoTipoDeIdentificacion = x.DescripcionCodigoTipoDeIdentificacion
           }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: TiposPorDocumentoes/Create
        public ActionResult Create()
        {

            //se llenan los combos
            IEnumerable<SelectListItem> itemsTipoDocumento = db.TIPODOCUMENTO
              .Select(o => new SelectListItem
              {
                  Value = o.Id,
                  Text = o.Descripcion
              });
            ViewBag.ComboTipoDocumento = itemsTipoDocumento;

            IEnumerable<SelectListItem> itemsTipoIdentificacion = db.TIPO_IDENTIFICACION
             .Select(c => new SelectListItem
             {
                 Value = c.Id.ToString(),
                 Text = c.Descripcion
             });
            ViewBag.ComboTipoIdentificacion = itemsTipoIdentificacion;
            return View();
        }

        // POST: TiposPorDocumentoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTipoDocumento,CodigoTipoDeIdentificacion,Estado,FechaDeInicio,FechaDeFin")] TiposPorDocumento tiposPorDocumento)
        {
            if (ModelState.IsValid)
            {
                db.TIPOSXDOCUMENTO.Add(tiposPorDocumento);
                string mensaje = Verificar(tiposPorDocumento.CodigoTipoDocumento,tiposPorDocumento.CodigoTipoDeIdentificacion);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(tiposPorDocumento.FechaDeInicio, tiposPorDocumento.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(tiposPorDocumento, "I", "TIPOSXDOCUMENTO");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        IEnumerable<SelectListItem> itemsTipoDocumento = db.TIPODOCUMENTO
                            .Select(o => new SelectListItem
                            {
                                Value = o.Id,
                                Text = o.Descripcion
                            });
                        ViewBag.ComboTipoDocumento = itemsTipoDocumento;

                        IEnumerable<SelectListItem> itemsTipoIdentificacion = db.TIPO_IDENTIFICACION
                         .Select(c => new SelectListItem
                         {
                             Value = c.Id.ToString(),
                             Text = c.Descripcion
                         });
                        ViewBag.ComboTipoIdentificacion = itemsTipoIdentificacion;
                        return View(tiposPorDocumento);
                    }
                }            
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsTipoDocumento = db.TIPODOCUMENTO
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id,
                        Text = o.Descripcion
                    });
                    ViewBag.ComboTipoDocumento = itemsTipoDocumento;

                    IEnumerable<SelectListItem> itemsTipoIdentificacion = db.TIPO_IDENTIFICACION
                     .Select(c => new SelectListItem
                     {
                         Value = c.Id.ToString(),
                         Text = c.Descripcion
                     });
                    ViewBag.ComboTipoIdentificacion = itemsTipoIdentificacion;
                    return View(tiposPorDocumento);
                }
           }

            return View(tiposPorDocumento);
        }

        // GET: TiposPorDocumentoes/Edit/5
        public ActionResult Edit(string codigod, string codigot)
        {
            if (codigod == null || codigot == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod, codigot);


            var list =
                (from td in db.TIPOSXDOCUMENTO
                 join d in db.TIPODOCUMENTO on new { Id = td.CodigoTipoDocumento } equals new { Id = d.Id } into d_join
                 where td.CodigoTipoDocumento == codigod
                 from d in d_join.DefaultIfEmpty()
                 join i in db.TIPO_IDENTIFICACION on new { Id = td.CodigoTipoDeIdentificacion } equals new { Id = i.Id } into i_join
                 where td.CodigoTipoDeIdentificacion == codigot
                 from i in i_join.DefaultIfEmpty()
                 select new
                 {
                     CodigoTipoDocumento = td.CodigoTipoDocumento,
                     CodigoTipoDeIdentificacion = td.CodigoTipoDeIdentificacion,
                     Estado = td.Estado,
                     FechaDeInicio = td.FechaDeInicio,
                     FechaDeFin = td.FechaDeFin,
                     DescripcionCodigoTipoDocumento = d.Descripcion,
                     DescripcionCodigoTipoDeIdentificacion = i.Descripcion
                 }).ToList()


          .Select(x => new TiposPorDocumento
          {
              CodigoTipoDocumento = x.CodigoTipoDocumento,
              CodigoTipoDeIdentificacion = x.CodigoTipoDeIdentificacion,
              Estado = x.Estado,
              FechaDeInicio = x.FechaDeInicio,
              FechaDeFin = x.FechaDeFin,
              DescripcionCodigoTipoDocumento = x.DescripcionCodigoTipoDocumento,
              DescripcionCodigoTipoDeIdentificacion = x.DescripcionCodigoTipoDeIdentificacion
          }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboTipoDocumento = new SelectList(db.TIPODOCUMENTO.OrderBy(x => x.Descripcion), "Id", "Descripcion", codigod);
            ViewBag.ComboTipoIdentificacion = new SelectList(db.TIPO_IDENTIFICACION.OrderBy(x => x.Descripcion), "Id", "Descripcion", codigot);


            return View(list);
        }

        // POST: TiposPorDocumentoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTipoDocumento,CodigoTipoDeIdentificacion,Estado,FechaDeInicio,FechaDeFin")] TiposPorDocumento tiposPorDocumento)
        {
            if (ModelState.IsValid)
            {
                var tiposPorDocumentoAntes = db.TIPOSXDOCUMENTO.AsNoTracking().Where(d => d.CodigoTipoDocumento == tiposPorDocumento.CodigoTipoDocumento 
                                                                                   && d.CodigoTipoDeIdentificacion== tiposPorDocumento.CodigoTipoDeIdentificacion).FirstOrDefault();

                db.Entry(tiposPorDocumento).State = EntityState.Modified;
                string mensaje = ValidarFechas(tiposPorDocumento.FechaDeInicio, tiposPorDocumento.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(tiposPorDocumento, "U", "TIPOSXDOCUMENTO", tiposPorDocumentoAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    ViewBag.ComboTipoDocumento = new SelectList(db.TIPODOCUMENTO.OrderBy(x => x.Descripcion), "Id", "Descripcion", tiposPorDocumento.CodigoTipoDocumento);
                    ViewBag.ComboTipoIdentificacion = new SelectList(db.TIPO_IDENTIFICACION.OrderBy(x => x.Descripcion), "Id", "Descripcion", tiposPorDocumento.CodigoTipoDeIdentificacion);


                    return View(tiposPorDocumento);
                }
            }
            return View(tiposPorDocumento);
        }

        // GET: TiposPorDocumentoes/Delete/5
        public ActionResult Delete(string codigod, string codigot)
        {
            if (codigod == null || codigot == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod, codigot);


            var list =
                (from td in db.TIPOSXDOCUMENTO
                 join d in db.TIPODOCUMENTO on new { Id = td.CodigoTipoDocumento } equals new { Id = d.Id } into d_join
                 where td.CodigoTipoDocumento == codigod
                 from d in d_join.DefaultIfEmpty()
                 join i in db.TIPO_IDENTIFICACION on new { Id = td.CodigoTipoDeIdentificacion } equals new { Id = i.Id } into i_join
                 where td.CodigoTipoDeIdentificacion == codigot
                 from i in i_join.DefaultIfEmpty()
                 select new
                 {
                     CodigoTipoDocumento = td.CodigoTipoDocumento,
                     CodigoTipoDeIdentificacion = td.CodigoTipoDeIdentificacion,
                     Estado = td.Estado,
                     FechaDeInicio = td.FechaDeInicio,
                     FechaDeFin = td.FechaDeFin,
                     DescripcionCodigoTipoDocumento = d.Descripcion,
                     DescripcionCodigoTipoDeIdentificacion = i.Descripcion
                 }).ToList()


          .Select(x => new TiposPorDocumento
          {
              CodigoTipoDocumento = x.CodigoTipoDocumento,
              CodigoTipoDeIdentificacion = x.CodigoTipoDeIdentificacion,
              Estado = x.Estado,
              FechaDeInicio = x.FechaDeInicio,
              FechaDeFin = x.FechaDeFin,
              DescripcionCodigoTipoDocumento = x.DescripcionCodigoTipoDocumento,
              DescripcionCodigoTipoDeIdentificacion = x.DescripcionCodigoTipoDeIdentificacion
          }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: TiposPorDocumentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string codigod, string codigot)
        {
            TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod, codigot);
            TiposPorDocumento tiposPorDocumentoAntes = ObtenerCopia(tiposPorDocumento);
            if (tiposPorDocumento.Estado == "A")
                tiposPorDocumento.Estado = "I";
            else
                tiposPorDocumento.Estado = "A";
            db.SaveChanges();
            Bitacora(tiposPorDocumento, "U", "TIPOSXDOCUMENTO", tiposPorDocumentoAntes);
            return RedirectToAction("Index");
        }

        // GET: TiposPorDocumentoes/RealDelete/5
        public ActionResult RealDelete(string codigod, string codigot)
        {
            if (codigod == null || codigot == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod, codigot);


            var list =
                (from td in db.TIPOSXDOCUMENTO
                 join d in db.TIPODOCUMENTO on new { Id = td.CodigoTipoDocumento } equals new { Id = d.Id } into d_join
                 where td.CodigoTipoDocumento == codigod
                 from d in d_join.DefaultIfEmpty()
                 join i in db.TIPO_IDENTIFICACION on new { Id = td.CodigoTipoDeIdentificacion } equals new { Id = i.Id } into i_join
                 where td.CodigoTipoDeIdentificacion == codigot
                 from i in i_join.DefaultIfEmpty()
                 select new
                 {
                     CodigoTipoDocumento = td.CodigoTipoDocumento,
                     CodigoTipoDeIdentificacion = td.CodigoTipoDeIdentificacion,
                     Estado = td.Estado,
                     FechaDeInicio = td.FechaDeInicio,
                     FechaDeFin = td.FechaDeFin,
                     DescripcionCodigoTipoDocumento = d.Descripcion,
                     DescripcionCodigoTipoDeIdentificacion = i.Descripcion
                 }).ToList()


          .Select(x => new TiposPorDocumento
          {
              CodigoTipoDocumento = x.CodigoTipoDocumento,
              CodigoTipoDeIdentificacion = x.CodigoTipoDeIdentificacion,
              Estado = x.Estado,
              FechaDeInicio = x.FechaDeInicio,
              FechaDeFin = x.FechaDeFin,
              DescripcionCodigoTipoDocumento = x.DescripcionCodigoTipoDocumento,
              DescripcionCodigoTipoDeIdentificacion = x.DescripcionCodigoTipoDeIdentificacion
          }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: TiposPorDocumentoes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string codigod, string codigot)
        {
            TiposPorDocumento tiposPorDocumento = db.TIPOSXDOCUMENTO.Find(codigod, codigot);
            db.TIPOSXDOCUMENTO.Remove(tiposPorDocumento);
            db.SaveChanges();
            Bitacora(tiposPorDocumento, "D", "TIPOSXDOCUMENTO");
            TempData["Type"] = "error";
            TempData["Message"] = "El registro se eliminó correctamente";
            return RedirectToAction("Index");
        }
        public string ValidarFechas(DateTime FechaIni, DateTime FechaFin)
        {
            if (FechaIni.CompareTo(FechaFin) == 1)
            {
                return "La fecha de inicio no puede ser mayor que la fecha fin";
            }
            return "";
        }
        public string Verificar(string codigod,string codigot)
        {
            string mensaje = "";
            bool exist = db.TIPOSXDOCUMENTO.Any(x => x.CodigoTipoDeIdentificacion == codigot && x.CodigoTipoDocumento == codigod);
            if (exist)
            {
                mensaje = "El codigo tipo de identificación " + codigot + ", codigo tipo documento ="+ codigod+" ya esta registrado";
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
