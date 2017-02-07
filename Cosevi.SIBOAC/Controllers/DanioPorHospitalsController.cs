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
    public class DanioPorHospitalsController : BaseController<DanioPorHospital>

    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DanioPorHospitals
        [SessionExpire]
        public ActionResult Index(int ? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            //var sCanton = db.CANTON.ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            

            var list =
            (from dh in db.DAÑOXHOSPITAL
             join ho in db.HOSPITAL on new { IdHospital = dh.IdHospital } equals new { IdHospital = ho.Id } into ho_join
             from ho in ho_join.DefaultIfEmpty()
             join da in db.DAÑO on new { IdDanio = dh.IdDanio } equals new { IdDanio = da.Id } into da_join
             from da in da_join.DefaultIfEmpty()
             select new
             {
                 IdHospital = dh.IdHospital,
                 IdDanio = dh.IdDanio,
                 Estado = dh.Estado,
                 FechaDeInicio = dh.FechaDeInicio,
                 FechaDeFin = dh.FechaDeFin,
                 DescripcionHospital = ho.Descripcion,
                 DescripcionDanio = da.Descripcion
             }).ToList()

            .Select(x => new DanioPorHospital
             {
                 IdHospital = x.IdHospital,
                 IdDanio = x.IdDanio,
                 Estado = x.Estado,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 DescripcionHospital = x.DescripcionHospital,
                 DescripcionDanio = x.DescripcionDanio
            });

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: DanioPorHospitals/Details/5
        public ActionResult Details(string IdHospital, int? IdDanio)
        {
            if (IdHospital == null|| IdDanio == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital, IdDanio);

            var list =
            (from dh in db.DAÑOXHOSPITAL
             join ho in db.HOSPITAL on new { IdHospital = dh.IdHospital } equals new { IdHospital = ho.Id } into ho_join
             where dh.IdHospital == IdHospital
             from ho in ho_join.DefaultIfEmpty()
             join da in db.DAÑO on new { IdDanio = dh.IdDanio } equals new { IdDanio = da.Id } into da_join
             where dh.IdDanio == IdDanio
             from da in da_join.DefaultIfEmpty()
             select new
             {
                 IdHospital = dh.IdHospital,
                 IdDanio = dh.IdDanio,
                 Estado = dh.Estado,
                 FechaDeInicio = dh.FechaDeInicio,
                 FechaDeFin = dh.FechaDeFin,
                 DescripcionHospital = ho.Descripcion,
                 DescripcionDanio = da.Descripcion
             }).ToList()

            .Select(x => new DanioPorHospital
            {
                IdHospital = x.IdHospital,
                IdDanio = x.IdDanio,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionHospital = x.DescripcionHospital,
                DescripcionDanio = x.DescripcionDanio

            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: DanioPorHospitals/Create
        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsHospital = db.HOSPITAL
              .Select(o => new SelectListItem
              {
                  Value = o.Id,
                  Text = o.Descripcion
              });
            ViewBag.ComboHospital = itemsHospital;
            IEnumerable<SelectListItem> itemsDannio = db.DAÑO
             .Select(c => new SelectListItem
             {
                 Value = c.Id.ToString(),
                 Text = c.Descripcion
             });
            ViewBag.ComboDannio = itemsDannio;
            return View();
        }

        // POST: DanioPorHospitals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdHospital,IdDanio,Estado,FechaDeInicio,FechaDeFin")] DanioPorHospital danioPorHospital)
        {
            if (ModelState.IsValid)
            {
                db.DAÑOXHOSPITAL.Add(danioPorHospital);
                string mensaje = Verificar(danioPorHospital.IdHospital,
                                               danioPorHospital.IdDanio);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(danioPorHospital.FechaDeInicio, danioPorHospital.FechaDeFin);

                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(danioPorHospital, "I", "DAÑOXHOSPITAL");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        IEnumerable<SelectListItem> itemsHospital = db.HOSPITAL.Select(o => new SelectListItem
                        {
                            Value = o.Id,
                            Text = o.Descripcion
                        });
                        ViewBag.ComboHospital = itemsHospital;
                        IEnumerable<SelectListItem> itemsDannio = db.DAÑO.Select(c => new SelectListItem
                        {
                            Value = c.Id.ToString(),
                            Text = c.Descripcion
                        });
                        ViewBag.ComboDannio = itemsDannio;
                        return View(danioPorHospital);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsHospital = db.HOSPITAL.Select(o => new SelectListItem
                    {
                        Value = o.Id,
                        Text = o.Descripcion
                    });
                    ViewBag.ComboHospital = itemsHospital;
                    IEnumerable<SelectListItem> itemsDannio = db.DAÑO.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Descripcion
                    });
                    ViewBag.ComboDannio = itemsDannio;
                    return View(danioPorHospital);
                }
            }

            return View(danioPorHospital);
        }

        // GET: DanioPorHospitals/Edit/5
        public ActionResult Edit(string IdHospital, int? IdDanio)
        {
            if (IdHospital == null || IdDanio == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital, IdDanio);


            var list =
            (from dh in db.DAÑOXHOSPITAL
             join ho in db.HOSPITAL on new { IdHospital = dh.IdHospital } equals new { IdHospital = ho.Id } into ho_join
             where dh.IdHospital == IdHospital
             from ho in ho_join.DefaultIfEmpty()
             join da in db.DAÑO on new { IdDanio = dh.IdDanio } equals new { IdDanio = da.Id } into da_join
             where dh.IdDanio == IdDanio
             from da in da_join.DefaultIfEmpty()
             select new
             {
                 IdHospital = dh.IdHospital,
                 IdDanio = dh.IdDanio,
                 Estado = dh.Estado,
                 FechaDeInicio = dh.FechaDeInicio,
                 FechaDeFin = dh.FechaDeFin,
                 DescripcionHospital = ho.Descripcion,
                 DescripcionDanio = da.Descripcion
             }).ToList()

            .Select(x => new DanioPorHospital
            {
                IdHospital = x.IdHospital,
                IdDanio = x.IdDanio,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionHospital = x.DescripcionHospital,
                DescripcionDanio = x.DescripcionDanio

            }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboHospital = new SelectList(db.HOSPITAL.OrderBy(x => x.Descripcion), "Id", "Descripcion", IdHospital);
            ViewBag.ComboDannio = new SelectList(db.DAÑO.OrderBy(x => x.Descripcion), "Id", "Descripcion", IdDanio/*.ToString().Trim()*/);

            return View(list);
        }

        // POST: DanioPorHospitals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdHospital,IdDanio,Estado,FechaDeInicio,FechaDeFin")] DanioPorHospital danioPorHospital)
        {
            if (ModelState.IsValid)
            {
                var danioPorHospitalAntes = db.DAÑOXHOSPITAL.AsNoTracking().Where(d => d.IdHospital == danioPorHospital.IdHospital &&
                                                                                    d.IdDanio == danioPorHospital.IdDanio).FirstOrDefault();

                db.Entry(danioPorHospital).State = EntityState.Modified;
                string mensaje = ValidarFechas(danioPorHospital.FechaDeInicio, danioPorHospital.FechaDeFin);

                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(danioPorHospital, "U", "DAÑOXHOSPITAL", danioPorHospitalAntes);
                    TempData["Type"] = "success";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsHospital = db.HOSPITAL.Select(o => new SelectListItem
                    {
                        Value = o.Id,
                        Text = o.Descripcion
                    });
                    ViewBag.ComboHospital = itemsHospital;
                    IEnumerable<SelectListItem> itemsDannio = db.DAÑO.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Descripcion
                    });
                    ViewBag.ComboDannio = itemsDannio;
                    return View(danioPorHospital);
                }
            }
            return View(danioPorHospital);
        }

        // GET: DanioPorHospitals/Delete/5
        public ActionResult Delete(string IdHospital, int? IdDanio)
        {
            if (IdHospital == null|| IdDanio ==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital, IdDanio);

            var list =
            (from dh in db.DAÑOXHOSPITAL
            join ho in db.HOSPITAL on new { IdHospital = dh.IdHospital } equals new { IdHospital = ho.Id } into ho_join
            where dh.IdHospital == IdHospital
            from ho in ho_join.DefaultIfEmpty()
            join da in db.DAÑO on new { IdDanio = dh.IdDanio } equals new { IdDanio = da.Id } into da_join
            where dh.IdDanio == IdDanio
            from da in da_join.DefaultIfEmpty()
            select new
            {
                IdHospital = dh.IdHospital,
                IdDanio = dh.IdDanio,
                Estado = dh.Estado,
                FechaDeInicio = dh.FechaDeInicio,
                FechaDeFin = dh.FechaDeFin,
                DescripcionHospital = ho.Descripcion,
                DescripcionDanio = da.Descripcion
            }).ToList()

           .Select(x => new DanioPorHospital
           {
               IdHospital = x.IdHospital,
               IdDanio = x.IdDanio,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionHospital = x.DescripcionHospital,
               DescripcionDanio = x.DescripcionDanio

           }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: DanioPorHospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string IdHospital, int IdDanio)
        {
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital,IdDanio);
            DanioPorHospital danioPorHospitalAntes = ObtenerCopia(danioPorHospital);

            if (danioPorHospital.Estado == "A")
                danioPorHospital.Estado = "I";
            else
                danioPorHospital.Estado = "A";
                db.SaveChanges();
                Bitacora(danioPorHospital, "U", "DAÑOXHOSPITAL", danioPorHospitalAntes);
                return RedirectToAction("Index");
        }



        // GET: DanioPorHospitals/RealDelete/5
        public ActionResult RealDelete(string IdHospital, int? IdDanio)
        {
            if (IdHospital == null || IdDanio == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital, IdDanio);

            var list =
            (from dh in db.DAÑOXHOSPITAL
             join ho in db.HOSPITAL on new { IdHospital = dh.IdHospital } equals new { IdHospital = ho.Id } into ho_join
             where dh.IdHospital == IdHospital
             from ho in ho_join.DefaultIfEmpty()
             join da in db.DAÑO on new { IdDanio = dh.IdDanio } equals new { IdDanio = da.Id } into da_join
             where dh.IdDanio == IdDanio
             from da in da_join.DefaultIfEmpty()
             select new
             {
                 IdHospital = dh.IdHospital,
                 IdDanio = dh.IdDanio,
                 Estado = dh.Estado,
                 FechaDeInicio = dh.FechaDeInicio,
                 FechaDeFin = dh.FechaDeFin,
                 DescripcionHospital = ho.Descripcion,
                 DescripcionDanio = da.Descripcion
             }).ToList()

           .Select(x => new DanioPorHospital
           {
               IdHospital = x.IdHospital,
               IdDanio = x.IdDanio,
               Estado = x.Estado,
               FechaDeInicio = x.FechaDeInicio,
               FechaDeFin = x.FechaDeFin,
               DescripcionHospital = x.DescripcionHospital,
               DescripcionDanio = x.DescripcionDanio

           }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: DanioPorHospitals/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string IdHospital, int IdDanio)
        {
            DanioPorHospital danioPorHospital = db.DAÑOXHOSPITAL.Find(IdHospital, IdDanio);
            db.DAÑOXHOSPITAL.Remove(danioPorHospital);
            db.SaveChanges();
            Bitacora(danioPorHospital, "D", "DAÑOXHOSPITAL");
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

        public string Verificar(string IdHospital, int IdDanio)
        {
            string mensaje = "";
            bool exist = db.DAÑOXHOSPITAL.Any(x => x.IdHospital == IdHospital
                                                    && x.IdDanio == IdDanio);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrado:" +
                           " código de Hospital " + IdHospital +
                           ", código Daño " + IdDanio;

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
    }
}
