﻿using System;
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
    public class TipoVehiculoPorCodigoPorClasesController : BaseController<TipoVehiculoPorCodigoPorClase>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoVehiculoPorCodigoPorClases
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list =
              (
                from t in db.TIPOVEHCODIGOCLASE
                join ts in db.TIPOSVEHICULOS on new { Id = t.CodigoTiposVehiculos } equals new { Id = ts.Id } into ts_join
                from ts in ts_join.DefaultIfEmpty()
                join c in db.CLASE on new { Id = t.CodigoClasePlaca } equals new { Id = c.Id } into c_join
                from c in c_join.DefaultIfEmpty()
                join cg in db.CODIGO on new { Id = t.CodigoCodigoPlaca } equals new { Id = cg.Id } into cg_join
                from cg in cg_join.DefaultIfEmpty()
                join v in db.TIPOVEH on new { Id = t.CodigoTipoVeh } equals new { Id = v.Id } into v_join
                from v in v_join.DefaultIfEmpty()
                select new
                {
                    CodigoTiposVehiculos = t.CodigoTiposVehiculos,
                    CodigoClasePlaca = t.CodigoClasePlaca,
                    CodigoCodigoPlaca = t.CodigoCodigoPlaca,
                    CodigoTipoVeh = t.CodigoTipoVeh,
                    Estado = t.Estado,
                    FechaDeInicio = t.FechaDeInicio,
                    FechaDeFin = t.FechaDeFin,
                    DescripcionCodigoTiposVehiculos = ts.Nombre,
                    DescripcionCodigoTipoVeh = v.Descripcion
                }).ToList()

               .Select(x => new TipoVehiculoPorCodigoPorClase
               {
                   CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                   CodigoClasePlaca = x.CodigoClasePlaca,
                   CodigoCodigoPlaca = x.CodigoCodigoPlaca,
                   CodigoTipoVeh = x.CodigoTipoVeh,
                   Estado = x.Estado,
                   FechaDeInicio = x.FechaDeInicio,
                   FechaDeFin = x.FechaDeFin,
                   DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                   DescripcionCodigoTipoVeh = x.DescripcionCodigoTipoVeh

               });
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
        }

        // GET: TipoVehiculoPorCodigoPorClases/Details/5
        public ActionResult Details(int? codigoTiposVehiculos, string codigoClasePlaca, string codigoCodigoPlaca, int? codigoTipoVeh)
        {
            if (codigoTiposVehiculos == null || codigoClasePlaca == null || codigoCodigoPlaca == null || codigoTipoVeh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
                (
                  from t in db.TIPOVEHCODIGOCLASE
                  join ts in db.TIPOSVEHICULOS on new { Id = t.CodigoTiposVehiculos } equals new { Id = ts.Id } into ts_join
                  where t.CodigoTiposVehiculos == codigoTiposVehiculos && t.CodigoClasePlaca == codigoClasePlaca && t.CodigoCodigoPlaca == codigoCodigoPlaca && t.CodigoTipoVeh == codigoTipoVeh
                  from ts in ts_join.DefaultIfEmpty()
                  join c in db.CLASE on new { Id = t.CodigoClasePlaca } equals new { Id = c.Id } into c_join
                  from c in c_join.DefaultIfEmpty()
                  join cg in db.CODIGO on new { Id = t.CodigoCodigoPlaca } equals new { Id = cg.Id } into cg_join
                  from cg in cg_join.DefaultIfEmpty()
                  join v in db.TIPOVEH on new { Id = t.CodigoTipoVeh } equals new { Id = v.Id } into v_join
                  from v in v_join.DefaultIfEmpty()
                  select new
                  {
                      CodigoTiposVehiculos = t.CodigoTiposVehiculos,
                      CodigoClasePlaca = t.CodigoClasePlaca,
                      CodigoCodigoPlaca = t.CodigoCodigoPlaca,
                      CodigoTipoVeh = t.CodigoTipoVeh,
                      Estado = t.Estado,
                      FechaDeInicio = t.FechaDeInicio,
                      FechaDeFin = t.FechaDeFin,
                      DescripcionCodigoTiposVehiculos = ts.Nombre,
                      DescripcionCodigoTipoVeh = v.Descripcion
                  }).ToList()
                 .Select(x => new TipoVehiculoPorCodigoPorClase
                 {
                     CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                     CodigoClasePlaca = x.CodigoClasePlaca,
                     CodigoCodigoPlaca = x.CodigoCodigoPlaca,
                     CodigoTipoVeh = x.CodigoTipoVeh,
                     Estado = x.Estado,
                     FechaDeInicio = x.FechaDeInicio,
                     FechaDeFin = x.FechaDeFin,
                     DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                     DescripcionCodigoTipoVeh = x.DescripcionCodigoTipoVeh

                 }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }
        public string Verificar(int? codigoTiposVehiculos, string codigoClasePlaca, string codigoCodigoPlaca, int? codigoTipoVeh)
        {
            string mensaje = "";
            bool exist = db.TIPOVEHCODIGOCLASE.Any(x => x.CodigoTiposVehiculos == codigoTiposVehiculos
                                                    && x.CodigoClasePlaca == codigoClasePlaca
                                                    && x.CodigoCodigoPlaca == codigoCodigoPlaca
                                                    && x.CodigoTipoVeh == codigoTipoVeh);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados:" +
                           " código de tipos vehículos " + codigoTiposVehiculos +
                           ", código clase placa" + codigoClasePlaca +
                           ", código placa " + codigoCodigoPlaca +
                           ", código tipo vehículo " + codigoTipoVeh;

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
        // GET: TipoVehiculoPorCodigoPorClases/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> itemsTiposVehiculos = db.TIPOSVEHICULOS
          .Select(o => new SelectListItem
          {
              Value = o.Id.ToString(),
              Text = o.Nombre
          });
            ViewBag.ComboTiposVehiculos = itemsTiposVehiculos;


            IEnumerable<SelectListItem> itemsCodigoPlaca = db.CODIGO
               .Select(o => new SelectListItem
               {
                   Value = o.Id.ToString(),
                   Text = o.Id.ToString()
               });
            ViewBag.ComboCodigoPlaca = itemsCodigoPlaca;

            IEnumerable<SelectListItem> itemsClasePlaca = db.CLASE
              .Select(o => new SelectListItem
              {
                  Value = o.Id.ToString(),
                  Text = o.Id.ToString()
              });
            ViewBag.ComboClasePlaca = itemsClasePlaca;


            IEnumerable<SelectListItem> itemsCodigoVehiculo = db.TIPOVEH
              .Select(o => new SelectListItem
              {
                  Value = o.Id.ToString(),
                  Text = o.Descripcion.ToString()
              });
            ViewBag.ComboCodigoVehiculo = itemsCodigoVehiculo;

            return View();
        }

        // POST: TipoVehiculoPorCodigoPorClases/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoTiposVehiculos,CodigoClasePlaca,CodigoCodigoPlaca,CodigoTipoVeh,Estado,FechaDeInicio,FechaDeFin")] TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase)
        {
            if (ModelState.IsValid)
            {
                db.TIPOVEHCODIGOCLASE.Add(tipoVehiculoPorCodigoPorClase);
                string mensaje = Verificar(tipoVehiculoPorCodigoPorClase.CodigoTiposVehiculos,
                                              tipoVehiculoPorCodigoPorClase.CodigoClasePlaca,
                                              tipoVehiculoPorCodigoPorClase.CodigoCodigoPlaca,
                                              tipoVehiculoPorCodigoPorClase.CodigoTipoVeh);
                if (mensaje == "")
                {
                    mensaje = ValidarFechas(tipoVehiculoPorCodigoPorClase.FechaDeInicio, tipoVehiculoPorCodigoPorClase.FechaDeFin);
                    if (mensaje == "")
                    {
                        db.SaveChanges();
                        Bitacora(tipoVehiculoPorCodigoPorClase, "I", "TIPOVEHCODIGOCLASE");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;
                        ViewBag.ComboTiposVehiculos = new SelectList(db.TIPOSVEHICULOS.OrderBy(x => x.Nombre), "Id", "Nombre", tipoVehiculoPorCodigoPorClase.CodigoTiposVehiculos);
                        ViewBag.ComboClasePlaca = new SelectList(db.CLASE.OrderBy(x => x.Id), "Id", "Id", tipoVehiculoPorCodigoPorClase.CodigoClasePlaca);
                        ViewBag.ComboCodigoPlaca = new SelectList(db.CODIGO.OrderBy(x => x.Id), "Id", "Id", tipoVehiculoPorCodigoPorClase.CodigoCodigoPlaca);
                        ViewBag.ComboCodigoVehiculo = new SelectList(db.TIPOVEH.OrderBy(x => x.Descripcion), "Id".ToString(), "Descripcion", tipoVehiculoPorCodigoPorClase.CodigoTipoVeh);

                        return View(tipoVehiculoPorCodigoPorClase);
                    }

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    ViewBag.ComboTiposVehiculos = new SelectList(db.TIPOSVEHICULOS.OrderBy(x => x.Nombre), "Id", "Nombre", tipoVehiculoPorCodigoPorClase.CodigoTiposVehiculos);
                    ViewBag.ComboClasePlaca = new SelectList(db.CLASE.OrderBy(x => x.Id), "Id", "Id", tipoVehiculoPorCodigoPorClase.CodigoClasePlaca);
                    ViewBag.ComboCodigoPlaca = new SelectList(db.CODIGO.OrderBy(x => x.Id), "Id", "Id", tipoVehiculoPorCodigoPorClase.CodigoCodigoPlaca);
                    ViewBag.ComboCodigoVehiculo = new SelectList(db.TIPOVEH.OrderBy(x => x.Descripcion), "Id".ToString(), "Descripcion", tipoVehiculoPorCodigoPorClase.CodigoTipoVeh);

                    return View(tipoVehiculoPorCodigoPorClase);
                }
            }

            return View(tipoVehiculoPorCodigoPorClase);
        }

        // GET: TipoVehiculoPorCodigoPorClases/Edit/5
        public ActionResult Edit(int? codigoTiposVehiculos, string codigoClasePlaca, string codigoCodigoPlaca, int? codigoTipoVeh)
        {
            if (codigoTiposVehiculos == null || codigoClasePlaca == null || codigoCodigoPlaca == null || codigoTipoVeh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase = db.TIPOVEHCODIGOCLASE.Find(codigoTipoVehic, clase, codigo, codigoVehiculo);
            var list =
              (
                from t in db.TIPOVEHCODIGOCLASE
                join ts in db.TIPOSVEHICULOS on new { Id = t.CodigoTiposVehiculos } equals new { Id = ts.Id } into ts_join
                where t.CodigoTiposVehiculos == codigoTiposVehiculos && t.CodigoClasePlaca == codigoClasePlaca && t.CodigoCodigoPlaca == codigoCodigoPlaca && t.CodigoTipoVeh == codigoTipoVeh
                from ts in ts_join.DefaultIfEmpty()
                join c in db.CLASE on new { Id = t.CodigoClasePlaca } equals new { Id = c.Id } into c_join
                from c in c_join.DefaultIfEmpty()
                join cg in db.CODIGO on new { Id = t.CodigoCodigoPlaca } equals new { Id = cg.Id } into cg_join
                from cg in cg_join.DefaultIfEmpty()
                join v in db.TIPOVEH on new { Id = t.CodigoTipoVeh } equals new { Id = v.Id } into v_join
                from v in v_join.DefaultIfEmpty()
                select new
                {
                    CodigoTiposVehiculos = t.CodigoTiposVehiculos,
                    CodigoClasePlaca = t.CodigoClasePlaca,
                    CodigoCodigoPlaca = t.CodigoCodigoPlaca,
                    CodigoTipoVeh = t.CodigoTipoVeh,
                    Estado = t.Estado,
                    FechaDeInicio = t.FechaDeInicio,
                    FechaDeFin = t.FechaDeFin,
                    DescripcionCodigoTiposVehiculos = ts.Nombre,
                    DescripcionCodigoTipoVeh = v.Descripcion
                }).ToList()
               .Select(x => new TipoVehiculoPorCodigoPorClase
               {
                   CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                   CodigoClasePlaca = x.CodigoClasePlaca,
                   CodigoCodigoPlaca = x.CodigoCodigoPlaca,
                   CodigoTipoVeh = x.CodigoTipoVeh,
                   Estado = x.Estado,
                   FechaDeInicio = x.FechaDeInicio,
                   FechaDeFin = x.FechaDeFin,
                   DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                   DescripcionCodigoTipoVeh = x.DescripcionCodigoTipoVeh

               }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComboTiposVehiculos = new SelectList(db.TIPOSVEHICULOS.OrderBy(x => x.Nombre), "Id", "Nombre", codigoTiposVehiculos);
            ViewBag.ComboClasePlaca = new SelectList(db.CLASE.OrderBy(x => x.Id), "Id", "Id", codigoClasePlaca);
            ViewBag.ComboCodigoPlaca = new SelectList(db.CODIGO.OrderBy(x => x.Id), "Id", "Id", codigoCodigoPlaca);
            ViewBag.ComboCodigoVehiculo = new SelectList(db.TIPOVEH.OrderBy(x => x.Descripcion), "Id".ToString(), "Descripcion", codigoTipoVeh);
            return View(list);
        }

        // POST: TipoVehiculoPorCodigoPorClases/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoTiposVehiculos,CodigoClasePlaca,CodigoCodigoPlaca,CodigoTipoVeh,Estado,FechaDeInicio,FechaDeFin")] TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase)
        {
            if (ModelState.IsValid)
            {
                var tipoVehiculoPorCodigoPorClaseAntes = db.TIPOVEHCODIGOCLASE.AsNoTracking().Where(d => d.CodigoTiposVehiculos == tipoVehiculoPorCodigoPorClase.CodigoTiposVehiculos
                                                    && d.CodigoClasePlaca == tipoVehiculoPorCodigoPorClase.CodigoClasePlaca
                                                    && d.CodigoCodigoPlaca == tipoVehiculoPorCodigoPorClase.CodigoCodigoPlaca
                                                    && d.CodigoTipoVeh == tipoVehiculoPorCodigoPorClase.CodigoTipoVeh).FirstOrDefault();

                db.Entry(tipoVehiculoPorCodigoPorClase).State = EntityState.Modified;
                string mensaje = ValidarFechas(tipoVehiculoPorCodigoPorClase.FechaDeInicio, tipoVehiculoPorCodigoPorClase.FechaDeFin);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(tipoVehiculoPorCodigoPorClase, "U", "TIPOVEHCODIGOCLASE", tipoVehiculoPorCodigoPorClaseAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    ViewBag.ComboTiposVehiculos = new SelectList(db.TIPOSVEHICULOS.OrderBy(x => x.Nombre), "Id", "Nombre", tipoVehiculoPorCodigoPorClase.CodigoTiposVehiculos);
                    ViewBag.ComboClasePlaca = new SelectList(db.CLASE.OrderBy(x => x.Id), "Id", "Id", tipoVehiculoPorCodigoPorClase.CodigoClasePlaca);
                    ViewBag.ComboCodigoPlaca = new SelectList(db.CODIGO.OrderBy(x => x.Id), "Id", "Id", tipoVehiculoPorCodigoPorClase.CodigoCodigoPlaca);
                    ViewBag.ComboCodigoVehiculo = new SelectList(db.TIPOVEH.OrderBy(x => x.Descripcion), "Id".ToString(), "Descripcion", tipoVehiculoPorCodigoPorClase.CodigoTipoVeh);

                    return View(tipoVehiculoPorCodigoPorClase);
                }
            }
            return View(tipoVehiculoPorCodigoPorClase);
        }

        // GET: TipoVehiculoPorCodigoPorClases/Delete/5
        public ActionResult Delete(int? codigoTiposVehiculos, string codigoClasePlaca, string codigoCodigoPlaca, int? codigoTipoVeh)
        {
            if (codigoTiposVehiculos == null || codigoClasePlaca == null || codigoCodigoPlaca == null || codigoTipoVeh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
              (
                from t in db.TIPOVEHCODIGOCLASE
                join ts in db.TIPOSVEHICULOS on new { Id = t.CodigoTiposVehiculos } equals new { Id = ts.Id } into ts_join
                where t.CodigoTiposVehiculos == codigoTiposVehiculos && t.CodigoClasePlaca == codigoClasePlaca && t.CodigoCodigoPlaca == codigoCodigoPlaca && t.CodigoTipoVeh == codigoTipoVeh
                from ts in ts_join.DefaultIfEmpty()
                join c in db.CLASE on new { Id = t.CodigoClasePlaca } equals new { Id = c.Id } into c_join
                from c in c_join.DefaultIfEmpty()
                join cg in db.CODIGO on new { Id = t.CodigoCodigoPlaca } equals new { Id = cg.Id } into cg_join
                from cg in cg_join.DefaultIfEmpty()
                join v in db.TIPOVEH on new { Id = t.CodigoTipoVeh } equals new { Id = v.Id } into v_join
                from v in v_join.DefaultIfEmpty()
                select new
                {
                    CodigoTiposVehiculos = t.CodigoTiposVehiculos,
                    CodigoClasePlaca = t.CodigoClasePlaca,
                    CodigoCodigoPlaca = t.CodigoCodigoPlaca,
                    CodigoTipoVeh = t.CodigoTipoVeh,
                    Estado = t.Estado,
                    FechaDeInicio = t.FechaDeInicio,
                    FechaDeFin = t.FechaDeFin,
                    DescripcionCodigoTiposVehiculos = ts.Nombre,
                    DescripcionCodigoTipoVeh = v.Descripcion
                }).ToList()
               .Select(x => new TipoVehiculoPorCodigoPorClase
               {
                   CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                   CodigoClasePlaca = x.CodigoClasePlaca,
                   CodigoCodigoPlaca = x.CodigoCodigoPlaca,
                   CodigoTipoVeh = x.CodigoTipoVeh,
                   Estado = x.Estado,
                   FechaDeInicio = x.FechaDeInicio,
                   FechaDeFin = x.FechaDeFin,
                   DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                   DescripcionCodigoTipoVeh = x.DescripcionCodigoTipoVeh

               }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: TipoVehiculoPorCodigoPorClases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? codigoTiposVehiculos, string codigoClasePlaca, string codigoCodigoPlaca, int? codigoTipoVeh)
        {
            TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase = db.TIPOVEHCODIGOCLASE.Find(codigoTiposVehiculos, codigoClasePlaca, codigoCodigoPlaca, codigoTipoVeh);
            TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClaseAntes = ObtenerCopia(tipoVehiculoPorCodigoPorClase);
            if (tipoVehiculoPorCodigoPorClase.Estado == "A")
                tipoVehiculoPorCodigoPorClase.Estado = "I";
            else
                tipoVehiculoPorCodigoPorClase.Estado = "A";
            db.SaveChanges();
            Bitacora(tipoVehiculoPorCodigoPorClase, "U", "TIPOVEHCODIGOCLASE", tipoVehiculoPorCodigoPorClaseAntes);
            return RedirectToAction("Index");
        }

        // GET: TipoVehiculoPorCodigoPorClases/RealDelete/5
        public ActionResult RealDelete(int? codigoTiposVehiculos, string codigoClasePlaca, string codigoCodigoPlaca, int? codigoTipoVeh)
        {
            if (codigoTiposVehiculos == null || codigoClasePlaca == null || codigoCodigoPlaca == null || codigoTipoVeh == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
              (
                from t in db.TIPOVEHCODIGOCLASE
                join ts in db.TIPOSVEHICULOS on new { Id = t.CodigoTiposVehiculos } equals new { Id = ts.Id } into ts_join
                where t.CodigoTiposVehiculos == codigoTiposVehiculos && t.CodigoClasePlaca == codigoClasePlaca && t.CodigoCodigoPlaca == codigoCodigoPlaca && t.CodigoTipoVeh == codigoTipoVeh
                from ts in ts_join.DefaultIfEmpty()
                join c in db.CLASE on new { Id = t.CodigoClasePlaca } equals new { Id = c.Id } into c_join
                from c in c_join.DefaultIfEmpty()
                join cg in db.CODIGO on new { Id = t.CodigoCodigoPlaca } equals new { Id = cg.Id } into cg_join
                from cg in cg_join.DefaultIfEmpty()
                join v in db.TIPOVEH on new { Id = t.CodigoTipoVeh } equals new { Id = v.Id } into v_join
                from v in v_join.DefaultIfEmpty()
                select new
                {
                    CodigoTiposVehiculos = t.CodigoTiposVehiculos,
                    CodigoClasePlaca = t.CodigoClasePlaca,
                    CodigoCodigoPlaca = t.CodigoCodigoPlaca,
                    CodigoTipoVeh = t.CodigoTipoVeh,
                    Estado = t.Estado,
                    FechaDeInicio = t.FechaDeInicio,
                    FechaDeFin = t.FechaDeFin,
                    DescripcionCodigoTiposVehiculos = ts.Nombre,
                    DescripcionCodigoTipoVeh = v.Descripcion
                }).ToList()
               .Select(x => new TipoVehiculoPorCodigoPorClase
               {
                   CodigoTiposVehiculos = x.CodigoTiposVehiculos,
                   CodigoClasePlaca = x.CodigoClasePlaca,
                   CodigoCodigoPlaca = x.CodigoCodigoPlaca,
                   CodigoTipoVeh = x.CodigoTipoVeh,
                   Estado = x.Estado,
                   FechaDeInicio = x.FechaDeInicio,
                   FechaDeFin = x.FechaDeFin,
                   DescripcionCodigoTiposVehiculos = x.DescripcionCodigoTiposVehiculos,
                   DescripcionCodigoTipoVeh = x.DescripcionCodigoTipoVeh

               }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: TipoVehiculoPorCodigoPorClases/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(int? codigoTiposVehiculos, string codigoClasePlaca, string codigoCodigoPlaca, int? codigoTipoVeh)
        {
            TipoVehiculoPorCodigoPorClase tipoVehiculoPorCodigoPorClase = db.TIPOVEHCODIGOCLASE.Find(codigoTiposVehiculos, codigoClasePlaca, codigoCodigoPlaca, codigoTipoVeh);
            db.TIPOVEHCODIGOCLASE.Remove(tipoVehiculoPorCodigoPorClase);
            db.SaveChanges();
            Bitacora(tipoVehiculoPorCodigoPorClase, "D", "TIPOVEHCODIGOCLASE");
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
