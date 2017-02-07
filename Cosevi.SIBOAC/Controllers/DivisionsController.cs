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
    public class DivisionsController : BaseController<Division>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Divisions
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list =
              (
                from d in db.DIVISION
                join c in db.CANTON on new { IdCanton = d.IdCanton } equals new { IdCanton = c.Id } into c_join
                from c in c_join.DefaultIfEmpty()
                join o in db.OficinaParaImpugnars on new { Id = d.CodigoOficinaImpugna } equals new { Id = o.Id } into o_join
                from o in o_join.DefaultIfEmpty()
                select new
                {
                    IdCanton = d.IdCanton,
                    CodigoOficinaImpugna = d.CodigoOficinaImpugna,
                    Estado = d.Estado,
                    FechaDeInicio= d.FechaDeInicio,
                    FechaDeFin = d.FechaDeFin,
                    DescripcionCanton = c.Descripcion,
                    DescripcionOficina = o.Descripcion
                }).ToList()
               .Select(x => new Division
               {

                   IdCanton = x.IdCanton,
                   CodigoOficinaImpugna = x.CodigoOficinaImpugna,
                   Estado = x.Estado,
                   FechaDeInicio = x.FechaDeInicio,
                   FechaDeFin = x.FechaDeFin,
                   DescripcionCanton = x.DescripcionCanton,
                   DescripcionOficina = x.DescripcionOficina

               });
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
        }

        // GET: Divisions/Details/5
        public ActionResult Details(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton== null||OficinaImpugna == null|| FechaInicio== null || FechaFin ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
            (  from d in db.DIVISION
                  join c in db.CANTON on new { IdCanton = d.IdCanton } equals new { IdCanton = c.Id } into c_join
                  where d.IdCanton == canton && d.CodigoOficinaImpugna == OficinaImpugna && d.FechaDeInicio == FechaInicio && d.FechaDeFin == FechaFin
                  from c in c_join.DefaultIfEmpty()
                  join o in db.OficinaParaImpugnars on new { Id = d.CodigoOficinaImpugna } equals new { Id = o.Id } into o_join
                  from o in o_join.DefaultIfEmpty()
                  select new
                  {
                      IdCanton = d.IdCanton,
                      CodigoOficinaImpugna = d.CodigoOficinaImpugna,
                      Estado = d.Estado,
                      FechaDeInicio = d.FechaDeInicio,
                      FechaDeFin = d.FechaDeFin,
                      DescripcionCanton = c.Descripcion,
                      DescripcionOficina = o.Descripcion
                  }).ToList()
                 .Select(x => new Division
                  {

                           IdCanton = x.IdCanton,
                           CodigoOficinaImpugna = x.CodigoOficinaImpugna,
                           Estado = x.Estado,
                           FechaDeInicio = x.FechaDeInicio,
                           FechaDeFin = x.FechaDeFin,
                           DescripcionCanton = x.DescripcionCanton,
                           DescripcionOficina = x.DescripcionOficina
                 }).SingleOrDefault();

                if (list == null)
                {
                   return HttpNotFound();
                }
                return View(list);
        }

        // GET: Divisions/Create
        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsCanton = db.CANTON
              .Select(o => new SelectListItem
              {
                  Value = o.Id.ToString(),
                  Text = o.Descripcion

              });


            ViewBag.ComboCanton = itemsCanton;

            IEnumerable<SelectListItem> itemsOficina = db.OficinaParaImpugnars
          .Select(o => new SelectListItem
          {
              Value = o.Id.ToString(),
              Text = o.Descripcion

          });

            ViewBag.ComboOficina = itemsOficina;
            return View();
        }

        public string Verificar(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            string mensaje = "";
            bool exist = db.DIVISION.Any(x => x.IdCanton == canton
                                              && x.CodigoOficinaImpugna == OficinaImpugna
                                              &&x.FechaDeInicio == FechaInicio
                                              &&x.FechaDeFin == FechaFin);
            if (exist)
            {
                mensaje = "El codigo cantón " + canton +
                           ", código Oficina impugna " + OficinaImpugna +
                           ", fecha inicio " + FechaInicio +
                           ", fecha fin " + FechaFin +
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

        // POST: Divisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCanton,CodigoOficinaImpugna,Estado,FechaDeInicio,FechaDeFin")] Division division)
        {
            if (ModelState.IsValid)
            {
                db.DIVISION.Add(division);
                string mensaje = Verificar(division.IdCanton, division.CodigoOficinaImpugna, division.FechaDeInicio, division.FechaDeFin);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(division.FechaDeInicio, division.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(division, "I", "DIVISION");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        IEnumerable<SelectListItem> itemsCanton = db.CANTON
                        .Select(o => new SelectListItem
                        {
                            Value = o.Id.ToString(),
                            Text = o.Descripcion

                        });
                        ViewBag.ComboCanton = itemsCanton;

                        IEnumerable<SelectListItem> itemsOficina = db.OficinaParaImpugnars
                        .Select(o => new SelectListItem
                        {
                            Value = o.Id.ToString(),
                            Text = o.Descripcion

                        });
                        ViewBag.ComboOficina = itemsOficina;
                        return View(division);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsCanton = db.CANTON
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.Descripcion

                    });
                    ViewBag.ComboCanton = itemsCanton;

                    IEnumerable<SelectListItem> itemsOficina = db.OficinaParaImpugnars
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.Descripcion

                    });
                    ViewBag.ComboOficina = itemsOficina;
                    return View(division);
                }
            }

            return View(division);
        }

        // GET: Divisions/Edit/5
        public ActionResult Edit(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton == null || OficinaImpugna == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var list =
           (from d in db.DIVISION
            join c in db.CANTON on new { IdCanton = d.IdCanton } equals new { IdCanton = c.Id } into c_join
            where d.IdCanton == canton && d.CodigoOficinaImpugna == OficinaImpugna && d.FechaDeInicio == FechaInicio && d.FechaDeFin == FechaFin
            from c in c_join.DefaultIfEmpty()
            join o in db.OficinaParaImpugnars on new { Id = d.CodigoOficinaImpugna } equals new { Id = o.Id } into o_join
            from o in o_join.DefaultIfEmpty()
            select new
            {
                IdCanton = d.IdCanton,
                CodigoOficinaImpugna = d.CodigoOficinaImpugna,
                Estado = d.Estado,
                FechaDeInicio = d.FechaDeInicio,
                FechaDeFin = d.FechaDeFin,
                DescripcionCanton = c.Descripcion,
                DescripcionOficina = o.Descripcion
            }).ToList()
                .Select(x => new Division
                {

                    IdCanton = x.IdCanton,
                    CodigoOficinaImpugna = x.CodigoOficinaImpugna,
                    Estado = x.Estado,
                    FechaDeInicio = x.FechaDeInicio,
                    FechaDeFin = x.FechaDeFin,
                    DescripcionCanton = x.DescripcionCanton,
                    DescripcionOficina = x.DescripcionOficina
                }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboCanton = new SelectList(db.CANTON.OrderBy(x => x.Descripcion), "Id", "Descripcion", canton);
            ViewBag.ComboOficina = new SelectList(db.OficinaParaImpugnars.OrderBy(x => x.Descripcion), "Id", "Descripcion", OficinaImpugna);

            list.CodigoOficinaImpugna = list.CodigoOficinaImpugna.Trim();

            return View(list);
        }

        // POST: Divisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCanton,CodigoOficinaImpugna,Estado,FechaDeInicio,FechaDeFin")] Division division)
        {
            if (ModelState.IsValid)
            {
                var divisionAntes = db.DIVISION.AsNoTracking().Where(d => d.IdCanton == division.IdCanton &&
                                                                                    d.CodigoOficinaImpugna == division.CodigoOficinaImpugna).FirstOrDefault();
                db.Entry(division).State = EntityState.Modified;
                string mensaje = ValidarFechas(division.FechaDeInicio, division.FechaDeFin);

                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(division, "U", "DIVISION", divisionAntes);
                    TempData["Type"] = "success";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsCanton = db.CANTON
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.Descripcion

                    });
                    ViewBag.ComboCanton = itemsCanton;

                    IEnumerable<SelectListItem> itemsOficina = db.OficinaParaImpugnars
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.ToString(),
                        Text = o.Descripcion

                    });
                    ViewBag.ComboOficina = itemsOficina;
                    return View(division);
                }
            }
            return View(division);
        }

        // GET: Divisions/Delete/5
        public ActionResult Delete(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton == null || OficinaImpugna == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
          (from d in db.DIVISION
           join c in db.CANTON on new { IdCanton = d.IdCanton } equals new { IdCanton = c.Id } into c_join
           where d.IdCanton == canton && d.CodigoOficinaImpugna == OficinaImpugna && d.FechaDeInicio == FechaInicio && d.FechaDeFin == FechaFin
           from c in c_join.DefaultIfEmpty()
           join o in db.OficinaParaImpugnars on new { Id = d.CodigoOficinaImpugna } equals new { Id = o.Id } into o_join
           from o in o_join.DefaultIfEmpty()
           select new
           {
               IdCanton = d.IdCanton,
               CodigoOficinaImpugna = d.CodigoOficinaImpugna,
               Estado = d.Estado,
               FechaDeInicio = d.FechaDeInicio,
               FechaDeFin = d.FechaDeFin,
               DescripcionCanton = c.Descripcion,
               DescripcionOficina = o.Descripcion
           }).ToList()
               .Select(x => new Division
               {

                   IdCanton = x.IdCanton,
                   CodigoOficinaImpugna = x.CodigoOficinaImpugna,
                   Estado = x.Estado,
                   FechaDeInicio = x.FechaDeInicio,
                   FechaDeFin = x.FechaDeFin,
                   DescripcionCanton = x.DescripcionCanton,
                   DescripcionOficina = x.DescripcionOficina
               }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            Division divisionAntes = ObtenerCopia(division);

            if (division.Estado =="A")
                division.Estado = "I";
            else
                division.Estado = "A";
            db.SaveChanges();
            Bitacora(division, "U", "DIVISION", divisionAntes);    
            return RedirectToAction("Index");
        }



        // GET: Divisions/RealDelete/5
        public ActionResult RealDelete(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            if (canton == null || OficinaImpugna == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
          (from d in db.DIVISION
           join c in db.CANTON on new { IdCanton = d.IdCanton } equals new { IdCanton = c.Id } into c_join
           where d.IdCanton == canton && d.CodigoOficinaImpugna == OficinaImpugna && d.FechaDeInicio == FechaInicio && d.FechaDeFin == FechaFin
           from c in c_join.DefaultIfEmpty()
           join o in db.OficinaParaImpugnars on new { Id = d.CodigoOficinaImpugna } equals new { Id = o.Id } into o_join
           from o in o_join.DefaultIfEmpty()
           select new
           {
               IdCanton = d.IdCanton,
               CodigoOficinaImpugna = d.CodigoOficinaImpugna,
               Estado = d.Estado,
               FechaDeInicio = d.FechaDeInicio,
               FechaDeFin = d.FechaDeFin,
               DescripcionCanton = c.Descripcion,
               DescripcionOficina = o.Descripcion
           }).ToList()
               .Select(x => new Division
               {

                   IdCanton = x.IdCanton,
                   CodigoOficinaImpugna = x.CodigoOficinaImpugna,
                   Estado = x.Estado,
                   FechaDeInicio = x.FechaDeInicio,
                   FechaDeFin = x.FechaDeFin,
                   DescripcionCanton = x.DescripcionCanton,
                   DescripcionOficina = x.DescripcionOficina
               }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: Divisions/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int? canton, string OficinaImpugna, DateTime FechaInicio, DateTime FechaFin)
        {
            Division division = db.DIVISION.Find(canton, OficinaImpugna, FechaInicio, FechaFin);
            db.DIVISION.Remove(division);
            db.SaveChanges();
            Bitacora(division, "U", "DIVISION");
            TempData["Type"] = "error";
            TempData["Message"] = "El registro se eliminó correctamente";
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
