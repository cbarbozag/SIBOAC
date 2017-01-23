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
    public class ValidarCarroceriasController : BaseController<ValidarCarroceria>
    {
        // GET: ValidarCarrocerias
        [SessionExpire]
        public ActionResult Index(int? page)
        {

            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";


            var list =
           (from vc in db.VALIDARCARROCERIA
            join tidv in db.TIPOIDEVEHICULO on new { CodigoTipoIdVehiculo = vc.CodigoTipoIdVehiculo } equals new { CodigoTipoIdVehiculo = tidv.Id } into tidv_join
            from tidv in tidv_join.DefaultIfEmpty()
            join tv in db.TIPOSVEHICULOS on new { CodigoTiposVehiculos = vc.CodigoTiposVehiculos } equals new { CodigoTiposVehiculos = tv.Id } into tv_join
            from tv in tv_join.DefaultIfEmpty()
            join ca in db.CARROCERIA on new { CodigoCarroceria = (int)vc.CodigoCarroceria } equals new { CodigoCarroceria = ca.Id } into ca_join
            from ca in ca_join.DefaultIfEmpty()
            select new
            {
                vc.CodigoTipoIdVehiculo,
                vc.CodigoTiposVehiculos,
                vc.CodigoCarroceria,
                vc.Estado,
                vc.FechaDeInicio,
                vc.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = tidv.Descripcion,
                DescripcionCodigoTiposVehiculos = tv.Nombre,
                DescripcionCodigoCarroceria = ca.Descripcion

            }).ToList()
            .Select(x=> new ValidarCarroceria
            {
                CodigoTipoIdVehiculo = x.CodigoTipoIdVehiculo,
                CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                CodigoCarroceria = x.CodigoCarroceria,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = x.DescripcionCodigoTipoIdVehiculo,
                DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                DescripcionCodigoCarroceria = x.DescripcionCodigoCarroceria

            });
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            

        }

        public string Verificar(string CodigoTipoIdentificacion, int? CodigoTipoVehiculo, int? CodigoCarroceria)
        {
            string mensaje = "";
            bool exist = db.VALIDARCARROCERIA.Any(x => x.CodigoTipoIdVehiculo == CodigoTipoIdentificacion
                                          && x.CodigoTiposVehiculos == CodigoTipoVehiculo
                                          && x.CodigoCarroceria == CodigoCarroceria);
            if (exist)
            {
                mensaje = "El codigo de identificación" + CodigoTipoIdentificacion +
                    ",codigo de tipo vehiculo " + CodigoTipoVehiculo +
                    ", y el codigo de carroceria " + CodigoCarroceria + " ya estan registrado";
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
        // GET: ValidarCarrocerias/Details/5        

        public ActionResult Details(string CodigoTipoIdentificacion, int? CodigoTipoVehiculo, int? CodigoCarroceria)
        {
            if (CodigoTipoIdentificacion == null || CodigoTipoVehiculo == null || CodigoCarroceria == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var list =
           (from vc in db.VALIDARCARROCERIA
            join tidv in db.TIPOIDEVEHICULO on new { CodigoTipoIdVehiculo = vc.CodigoTipoIdVehiculo } equals new { CodigoTipoIdVehiculo = tidv.Id } into tidv_join
            where vc.CodigoTipoIdVehiculo == CodigoTipoIdentificacion
            from tidv in tidv_join.DefaultIfEmpty()
            join tv in db.TIPOSVEHICULOS on new { CodigoTiposVehiculos = vc.CodigoTiposVehiculos } equals new { CodigoTiposVehiculos = tv.Id } into tv_join
            where vc.CodigoTiposVehiculos == CodigoTipoVehiculo
            from tv in tv_join.DefaultIfEmpty()
            join ca in db.CARROCERIA on new { CodigoCarroceria = (int)vc.CodigoCarroceria } equals new { CodigoCarroceria = ca.Id } into ca_join
            where vc.CodigoCarroceria == CodigoCarroceria
            from ca in ca_join.DefaultIfEmpty()

            select new
            {
                vc.CodigoTipoIdVehiculo,
                vc.CodigoTiposVehiculos,
                vc.CodigoCarroceria,
                vc.Estado,
                vc.FechaDeInicio,
                vc.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = tidv.Descripcion,
                DescripcionCodigoTiposVehiculos = tv.Nombre,
                DescripcionCodigoCarroceria = ca.Descripcion

            }).ToList()
            .Select(x => new ValidarCarroceria
            {
                CodigoTipoIdVehiculo = x.CodigoTipoIdVehiculo,
                CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                CodigoCarroceria = x.CodigoCarroceria,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = x.DescripcionCodigoTipoIdVehiculo,
                DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                DescripcionCodigoCarroceria = x.DescripcionCodigoCarroceria

            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: ValidarCarrocerias/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> itemsTipoIdentificacion = db.TIPOIDEVEHICULO
             .Select(o => new SelectListItem
             {
                 Value = o.Id,
                 Text = o.Descripcion
             });
            ViewBag.ComboTipoIdVehiculo = itemsTipoIdentificacion;

            IEnumerable<SelectListItem> itemsTiposVehiculos = db.TIPOSVEHICULOS
            .Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Nombre
            });
            ViewBag.ComboTiposVehiculos = itemsTiposVehiculos;


            IEnumerable<SelectListItem> itemsCarroceria = db.CARROCERIA
             .Select(o => new SelectListItem
             {
                 Value = o.Id.ToString(),
                 Text = o.Descripcion
             });
            ViewBag.ComboCarroceria = itemsCarroceria;

            return View();
        }

        // POST: ValidarCarrocerias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTipoIdVehiculo,CodigoTiposVehiculos,CodigoCarroceria,Estado,FechaDeInicio,FechaDeFin")] ValidarCarroceria validarCarroceria)
        {
            if (ModelState.IsValid)
            {
                db.VALIDARCARROCERIA.Add(validarCarroceria);
                string mensaje = Verificar(validarCarroceria.CodigoTipoIdVehiculo,
                                            validarCarroceria.CodigoTiposVehiculos,
                                            validarCarroceria.CodigoCarroceria);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(validarCarroceria.FechaDeInicio, validarCarroceria.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(validarCarroceria, "I", "VALIDARCORROCERIA");

                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        return View(validarCarroceria);
                    }
                 
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(validarCarroceria);
                }
            }

            return View(validarCarroceria);
        }

        // GET: ValidarCarrocerias/Edit/5
        public ActionResult Edit(string CodigoTipoIdentificacion, int? CodigoTipoVehiculo, int? CodigoCarroceria)
        {
            if (CodigoTipoIdentificacion == null || CodigoTipoVehiculo == null || CodigoCarroceria == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ValidarCarroceria validarCarroceria = db.VALIDARCARROCERIA.Find(CodigoTipoIdentificacion, CodigoTipoVehiculo, CodigoCarroceria);

            var list =
           (from vc in db.VALIDARCARROCERIA
            join tidv in db.TIPOIDEVEHICULO on new { CodigoTipoIdVehiculo = vc.CodigoTipoIdVehiculo } equals new { CodigoTipoIdVehiculo = tidv.Id } into tidv_join
            where vc.CodigoTipoIdVehiculo == CodigoTipoIdentificacion
            from tidv in tidv_join.DefaultIfEmpty()
            join tv in db.TIPOSVEHICULOS on new { CodigoTiposVehiculos = vc.CodigoTiposVehiculos } equals new { CodigoTiposVehiculos = tv.Id } into tv_join
            where vc.CodigoTiposVehiculos == CodigoTipoVehiculo
            from tv in tv_join.DefaultIfEmpty()
            join ca in db.CARROCERIA on new { CodigoCarroceria = (int)vc.CodigoCarroceria } equals new { CodigoCarroceria = ca.Id } into ca_join
            where vc.CodigoCarroceria == CodigoCarroceria
            from ca in ca_join.DefaultIfEmpty()

            select new
            {
                vc.CodigoTipoIdVehiculo,
                vc.CodigoTiposVehiculos,
                vc.CodigoCarroceria,
                vc.Estado,
                vc.FechaDeInicio,
                vc.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = tidv.Descripcion,
                DescripcionCodigoTiposVehiculos = tv.Nombre,
                DescripcionCodigoCarroceria = ca.Descripcion

            }).ToList()
            .Select(x => new ValidarCarroceria
            {
                CodigoTipoIdVehiculo = x.CodigoTipoIdVehiculo,
                CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                CodigoCarroceria = x.CodigoCarroceria,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = x.DescripcionCodigoTipoIdVehiculo,
                DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                DescripcionCodigoCarroceria = x.DescripcionCodigoCarroceria

            }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboTipoIdVehiculo = new SelectList(db.TIPOIDEVEHICULO.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodigoTipoIdentificacion);
            ViewBag.ComboTiposVehiculos = new SelectList(db.TIPOSVEHICULOS.OrderBy(x => x.Nombre), "Id", "Nombre", CodigoTipoVehiculo);
            ViewBag.ComboCarroceria = new SelectList(db.CARROCERIA.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodigoCarroceria);
            return View(list);
        }

        // POST: ValidarCarrocerias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTipoIdVehiculo,CodigoTiposVehiculos,CodigoCarroceria,Estado,FechaDeInicio,FechaDeFin")] ValidarCarroceria validarCarroceria)
        {
            if (ModelState.IsValid)
            {
                var validarCorroceriaAntes = db.VALIDARCARROCERIA.AsNoTracking().Where(d => d.CodigoTipoIdVehiculo == validarCarroceria.CodigoTipoIdVehiculo
                                          && d.CodigoTiposVehiculos == validarCarroceria.CodigoTiposVehiculos
                                          && d.CodigoCarroceria == validarCarroceria.CodigoCarroceria).FirstOrDefault();

                db.Entry(validarCarroceria).State = EntityState.Modified;
                string mensaje = ValidarFechas(validarCarroceria.FechaDeInicio, validarCarroceria.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(validarCarroceria, "U", "VALIDARCARROCERIA", validarCorroceriaAntes);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(validarCorroceriaAntes);
                }
            }
            return View(validarCarroceria);
        }

        // GET: ValidarCarrocerias/Delete/5
        public ActionResult Delete(string CodigoTipoIdentificacion, int? CodigoTipoVehiculo, int? CodigoCarroceria)
        {
            if (CodigoTipoIdentificacion == null|| CodigoTipoVehiculo == null || CodigoCarroceria == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            //ValidarCarroceria validarCarroceria = db.VALIDARCARROCERIA.Find(CodigoTipoIdentificacion, CodigoTipoVehiculo, CodigoCarroceria);


            var list =
           (from vc in db.VALIDARCARROCERIA
            join tidv in db.TIPOIDEVEHICULO on new { CodigoTipoIdVehiculo = vc.CodigoTipoIdVehiculo } equals new { CodigoTipoIdVehiculo = tidv.Id } into tidv_join
            where vc.CodigoTipoIdVehiculo == CodigoTipoIdentificacion
            from tidv in tidv_join.DefaultIfEmpty()
            join tv in db.TIPOVEH on new { CodigoTiposVehiculos = vc.CodigoTiposVehiculos } equals new { CodigoTiposVehiculos = tv.Id } into tv_join
            where vc.CodigoTiposVehiculos == CodigoTipoVehiculo
            from tv in tv_join.DefaultIfEmpty()
            join ca in db.CARROCERIA on new { CodigoCarroceria = (int)vc.CodigoCarroceria } equals new { CodigoCarroceria = ca.Id } into ca_join
            where vc.CodigoCarroceria == CodigoCarroceria
            from ca in ca_join.DefaultIfEmpty()

            select new
            {
                vc.CodigoTipoIdVehiculo,
                vc.CodigoTiposVehiculos,
                vc.CodigoCarroceria,
                vc.Estado,
                vc.FechaDeInicio,
                vc.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = tidv.Descripcion,
                DescripcionCodigoTiposVehiculos = tv.Descripcion,
                DescripcionCodigoCarroceria = ca.Descripcion

            }).ToList()
            .Select(x => new ValidarCarroceria
            {
                CodigoTipoIdVehiculo = x.CodigoTipoIdVehiculo,
                CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                CodigoCarroceria = x.CodigoCarroceria,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = x.DescripcionCodigoTipoIdVehiculo,
                DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                DescripcionCodigoCarroceria = x.DescripcionCodigoCarroceria

            }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: ValidarCarrocerias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string CodigoTipoIdentificacion, int CodigoTipoVehiculo, int CodigoCarroceria)
        {
            ValidarCarroceria validarCarroceria = db.VALIDARCARROCERIA.Find(CodigoTipoIdentificacion, CodigoTipoVehiculo, CodigoCarroceria);
            ValidarCarroceria validarCarroceriaAntes = ObtenerCopia(validarCarroceria);
            if (validarCarroceria.Estado == "I")
                validarCarroceria.Estado = "A";
            else
                validarCarroceria.Estado = "I";
            db.SaveChanges();
            Bitacora(validarCarroceria, "U", "VALIDARCARROCERIA", validarCarroceriaAntes);
            return RedirectToAction("Index");
        }

        // GET: ValidarCarrocerias/RealDelete/5
        public ActionResult RealDelete(string CodigoTipoIdentificacion, int? CodigoTipoVehiculo, int? CodigoCarroceria)
        {
            if (CodigoTipoIdentificacion == null || CodigoTipoVehiculo == null || CodigoCarroceria == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var list =
           (from vc in db.VALIDARCARROCERIA
            join tidv in db.TIPOIDEVEHICULO on new { CodigoTipoIdVehiculo = vc.CodigoTipoIdVehiculo } equals new { CodigoTipoIdVehiculo = tidv.Id } into tidv_join
            where vc.CodigoTipoIdVehiculo == CodigoTipoIdentificacion
            from tidv in tidv_join.DefaultIfEmpty()
            join tv in db.TIPOVEH on new { CodigoTiposVehiculos = vc.CodigoTiposVehiculos } equals new { CodigoTiposVehiculos = tv.Id } into tv_join
            where vc.CodigoTiposVehiculos == CodigoTipoVehiculo
            from tv in tv_join.DefaultIfEmpty()
            join ca in db.CARROCERIA on new { CodigoCarroceria = (int)vc.CodigoCarroceria } equals new { CodigoCarroceria = ca.Id } into ca_join
            where vc.CodigoCarroceria == CodigoCarroceria
            from ca in ca_join.DefaultIfEmpty()

            select new
            {
                vc.CodigoTipoIdVehiculo,
                vc.CodigoTiposVehiculos,
                vc.CodigoCarroceria,
                vc.Estado,
                vc.FechaDeInicio,
                vc.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = tidv.Descripcion,
                DescripcionCodigoTiposVehiculos = tv.Descripcion,
                DescripcionCodigoCarroceria = ca.Descripcion

            }).ToList()
            .Select(x => new ValidarCarroceria
            {
                CodigoTipoIdVehiculo = x.CodigoTipoIdVehiculo,
                CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                CodigoCarroceria = x.CodigoCarroceria,
                Estado = x.Estado,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                DescripcionCodigoTipoIdVehiculo = x.DescripcionCodigoTipoIdVehiculo,
                DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                DescripcionCodigoCarroceria = x.DescripcionCodigoCarroceria

            }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: ValidarCarrocerias/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string CodigoTipoIdentificacion, int CodigoTipoVehiculo, int CodigoCarroceria)
        {
            ValidarCarroceria validarCarroceria = db.VALIDARCARROCERIA.Find(CodigoTipoIdentificacion, CodigoTipoVehiculo, CodigoCarroceria);
            db.VALIDARCARROCERIA.Remove(validarCarroceria);
            db.SaveChanges();
            Bitacora(validarCarroceria, "D", "VALIDARCARROCERIA");
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
