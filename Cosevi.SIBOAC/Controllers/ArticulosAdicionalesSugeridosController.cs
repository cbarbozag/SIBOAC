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
using System.Data.Entity.Validation;

namespace Cosevi.SIBOAC.Controllers
{
    public class ArticulosAdicionalesSugeridosController : BaseController<Articulos_Adicionales_Sugeridos>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();



        // GET: ARTICULO_ESPECIFICO
        [SessionExpire]
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list =
            (from a in db.Articulos_Adicionales_Sugeridos
             join c in db.CATARTICULO
                   on new { CodigoArticulo = a.Codigo, Conducta = a.Conducta, fechaInicio = a.Fecha_inicio, fechaFinal = a.Fecha_final }
               equals new { CodigoArticulo = c.Id, Conducta = c.Conducta, fechaInicio = c.FechaDeInicio, fechaFinal = c.FechaDeFin } into c_join
             from c in c_join.DefaultIfEmpty()
             select new
             {
                 CodigoArticulo = a.Codigo,
                 Conducta = a.Conducta,
                 FechaDeInicio = a.Fecha_inicio,
                 FechaDeFin = a.Fecha_final,
                 Estado = a.Estado,
                 CodigoRetiroTemporal = a.Retiro_temporal,
                 CodigoInmovilizacion = a.Inmovilizacion
             }).ToList()

             .Select(x => new Articulos_Adicionales_Sugeridos
             {
                 Codigo = x.CodigoArticulo,
                 Conducta = x.Conducta,
                 Fecha_inicio = x.FechaDeInicio,
                 Fecha_final = x.FechaDeFin,
                 Estado = x.Estado,
                 Retiro_temporal = x.CodigoRetiroTemporal,
                 Inmovilizacion = x.CodigoInmovilizacion

             }).OrderBy(x => (x.Codigo));

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        // GET: ARTICULO_ESPECIFICO/Details/5
        public ActionResult Details(string CodArticulo, string Conducta, DateTime? FechaInicio, DateTime? FechaFin)
        {
            if (CodArticulo == null || Conducta == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulos_Adicionales_Sugeridos Articulos_Adicionales_Sugeridos = db.Articulos_Adicionales_Sugeridos.Find(CodArticulo, Conducta, FechaInicio, FechaFin);
            if (Articulos_Adicionales_Sugeridos == null)
            {
                return HttpNotFound();
            }
            return View(Articulos_Adicionales_Sugeridos);
        }

        // GET: ARTICULO_ESPECIFICO/Create
        public ActionResult Create()
        {

            IEnumerable<SelectListItem> itemsCatArticulos = (from o in db.CATARTICULO
                                                             where o.Estado == "A"
                                                             select new { o.Id }).ToList().Distinct()
                                                          .Select(o => new SelectListItem
                                                          {
                                                              Value = o.Id.ToString(),
                                                              Text = o.Id.ToString()
                                                          });
            ViewBag.ComboArticulos = itemsCatArticulos;
            return View();
        }

        // POST: ARTICULO_ESPECIFICO/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "codigo, conducta, fecha_inicio, fecha_final, estado, codigo_retiro_temporal, codigo_inmovilizacion")] Articulos_Adicionales_Sugeridos Articulos_Adicionales_Sugeridos)
        {
            if (ModelState.IsValid)
            {

                db.Articulos_Adicionales_Sugeridos.Add(Articulos_Adicionales_Sugeridos);

                string mensajeNull = validarNulos(Articulos_Adicionales_Sugeridos.Retiro_temporal,
                                                  Articulos_Adicionales_Sugeridos.Inmovilizacion);
                if (mensajeNull == "")
                {

                    string mensaje = Verificar(Articulos_Adicionales_Sugeridos.Codigo,
                                               Articulos_Adicionales_Sugeridos.Conducta,
                                               Articulos_Adicionales_Sugeridos.Fecha_inicio,
                                               Articulos_Adicionales_Sugeridos.Fecha_final,
                                               Articulos_Adicionales_Sugeridos.Retiro_temporal,
                                               Articulos_Adicionales_Sugeridos.Inmovilizacion
                                               );


                    if (mensaje == "")
                    {
                        if (Articulos_Adicionales_Sugeridos.Retiro_temporal == "0")
                        {
                            Articulos_Adicionales_Sugeridos.Retiro_temporal = "";
                        }

                        if (Articulos_Adicionales_Sugeridos.Inmovilizacion == "0")
                        {
                            Articulos_Adicionales_Sugeridos.Inmovilizacion = "";
                        }

                        db.SaveChanges();
                        Bitacora(Articulos_Adicionales_Sugeridos, "I", "Articulos_Adicionales_Sugeridos");
                        TempData["Type"] = "success";
                        TempData["Message"] = "El registro se realizó correctamente";
                        return RedirectToAction("Index");

                    }

                    else
                    {
                        ViewBag.Type = "warning";
                        ViewBag.Message = mensaje;

                        ViewBag.ComboArticulos = null;
                        ViewBag.ComboArticulos = new SelectList((from o in db.CATARTICULO
                                                                 where o.Estado == "A"
                                                                 select new { o.Id }).ToList().Distinct(), "Id", "Id", Articulos_Adicionales_Sugeridos.Codigo);

                        ViewData["conducta"] = Articulos_Adicionales_Sugeridos.Conducta;
                        ViewData["fecha_inicio"] = Articulos_Adicionales_Sugeridos.Fecha_inicio.ToString("dd-MM-yyyy");
                        ViewData["fecha_final"] = Articulos_Adicionales_Sugeridos.Fecha_final.ToString("dd-MM-yyyy");
                        ViewData["codigo_retiro_temporal"] = Articulos_Adicionales_Sugeridos.Retiro_temporal;
                        return View(Articulos_Adicionales_Sugeridos);
                    }
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensajeNull;

                    ViewBag.ComboArticulos = null;
                    ViewBag.ComboArticulos = new SelectList((from o in db.CATARTICULO
                                                             where o.Estado == "A"
                                                             select new { o.Id }).ToList().Distinct(), "Id", "Id", Articulos_Adicionales_Sugeridos.Codigo);

                    ViewData["conducta"] = Articulos_Adicionales_Sugeridos.Conducta;
                    ViewData["fecha_inicio"] = Articulos_Adicionales_Sugeridos.Fecha_inicio.ToString("dd-MM-yyyy");
                    ViewData["fecha_final"] = Articulos_Adicionales_Sugeridos.Fecha_final.ToString("dd-MM-yyyy");
                    ViewData["codigo_retiro_temporal"] = Articulos_Adicionales_Sugeridos.Retiro_temporal;
                    return View(Articulos_Adicionales_Sugeridos);
                }

            }

            return View(Articulos_Adicionales_Sugeridos);
        }

        // GET: ARTICULO_ESPECIFICO/Edit/5
        public ActionResult Edit(string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin)
        {
            if (CodArticulo == null || Conducta == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<SelectListItem> itemsCatArticulos = (from o in db.CATARTICULO
                                                             where o.Id == CodArticulo
                                                             select new { o.Id }).ToList().Distinct()
                                                          .Select(o => new SelectListItem
                                                          {
                                                              Value = o.Id.ToString(),
                                                              Text = o.Id.ToString()
                                                          });
            ViewBag.ComboArticulos = itemsCatArticulos;
            //ViewBag.ComboArticulos = new SelectList(db.CATARTICULO.OrderBy(x => x.Id), "Id", "Id", CodArticulo);
            //ViewBag.ComboConducta = new SelectList(db.CATARTICULO.OrderBy(x => x.Conducta).Distinct(), "Id", "conducta", Conducta);
            return View();
        }

        // POST: ARTICULO_ESPECIFICO/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codigo,conducta,fecha_inicio,fecha_final,estado,codigo_retiro_temporal,codigo_inmovilizacion")] Articulos_Adicionales_Sugeridos Articulos_Adicionales_Sugeridos)
        {
            if (ModelState.IsValid)
            {
                db.Articulos_Adicionales_Sugeridos.Add(Articulos_Adicionales_Sugeridos);

                    var Articulos_Adicionales_SugeridosAntes = db.Articulos_Adicionales_Sugeridos.AsNoTracking().Where(d => d.Codigo == Articulos_Adicionales_Sugeridos.Codigo &&
                                                                          d.Fecha_inicio == Articulos_Adicionales_Sugeridos.Fecha_inicio &&
                                                                          d.Fecha_final == Articulos_Adicionales_Sugeridos.Fecha_final).FirstOrDefault();
                    

                bool saveFailed;
                    do
                    {
                        saveFailed = false;
                        try
                    {
                        string mensaje = Verificar(Articulos_Adicionales_Sugeridos.Codigo,
                                               Articulos_Adicionales_Sugeridos.Conducta,
                                               Articulos_Adicionales_Sugeridos.Fecha_inicio,
                                               Articulos_Adicionales_Sugeridos.Fecha_final,
                                               Articulos_Adicionales_Sugeridos.Retiro_temporal,
                                               Articulos_Adicionales_Sugeridos.Inmovilizacion
                                               );
                        if(mensaje == "")
                        {
                            if (Articulos_Adicionales_Sugeridos.Retiro_temporal == "0")
                            {
                                Articulos_Adicionales_Sugeridos.Retiro_temporal = null;
                            }

                            if (Articulos_Adicionales_Sugeridos.Inmovilizacion == "0")
                            {
                                Articulos_Adicionales_Sugeridos.Inmovilizacion = null;
                            }

                            db.Entry(Articulos_Adicionales_Sugeridos).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.Type = "warning";
                            ViewBag.Message = mensaje;

                            return View(Articulos_Adicionales_Sugeridos);
                        }

                    }
                    catch (DbEntityValidationException e)
                    {
                            saveFailed = true;
                            foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    } while (saveFailed);

                    Bitacora(Articulos_Adicionales_Sugeridos, "U", "Artículos_Adicionales_Sugeridos", Articulos_Adicionales_SugeridosAntes);
                    return RedirectToAction("Index");
                }

            return View(Articulos_Adicionales_Sugeridos);
        }

        // GET: ARTICULO_ESPECIFICO/Delete/5
        public ActionResult Delete(string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin)
        {
            if (CodArticulo == null || Conducta == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulos_Adicionales_Sugeridos Articulos_Adicionales_Sugeridos = db.Articulos_Adicionales_Sugeridos.Find(CodArticulo, Conducta, FechaInicio, FechaFin);
            if (Articulos_Adicionales_Sugeridos == null)
            {
                return HttpNotFound();
            }
            return View(Articulos_Adicionales_Sugeridos);
        }

        // POST: ARTICULO_ESPECIFICO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin)
        {
            Articulos_Adicionales_Sugeridos Articulos_Adicionales_Sugeridos = db.Articulos_Adicionales_Sugeridos.Find(CodArticulo, Conducta, FechaInicio, FechaFin);
            Articulos_Adicionales_Sugeridos Articulos_Adicionales_SugeridosAntes = ObtenerCopia(Articulos_Adicionales_Sugeridos);
            if (Articulos_Adicionales_Sugeridos.Estado == "A")
                Articulos_Adicionales_Sugeridos.Estado = "I";
            else
                Articulos_Adicionales_Sugeridos.Estado = "A";
            db.SaveChanges();
            Bitacora(Articulos_Adicionales_Sugeridos, "U", "Artículos_Adicionales_Sugeridos", Articulos_Adicionales_SugeridosAntes);
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





        public string Verificar(string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin, string codigoRetiroTemporal, string codigoInmovilizacion)
        {
            string mensaje = "";
            bool exist = db.Articulos_Adicionales_Sugeridos.Any(x => x.Codigo == CodArticulo
                                                    && x.Conducta == Conducta
                                                    && x.Fecha_inicio == FechaInicio
                                                    && x.Fecha_final == FechaFin
                                                    && x.Retiro_temporal == codigoRetiroTemporal
                                                    && x.Inmovilizacion == codigoInmovilizacion);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados: código de artículos específicos " + CodArticulo +
                           ", Conducta " + Conducta +
                           ", Fecha inicial " + FechaInicio +
                           ", Fecha final " + FechaFin +
                           ", Código de Retiro Temporal " + codigoRetiroTemporal +
                           ", Código de Inmovilización " + codigoInmovilizacion;
            }
            return mensaje;
        }




        public string validarNulos(string codigoRetiroTemporal, string codigoInmovilizacion)
        {
            string mensajeValidaNull = "";

            /*bool exist = db.ARTICULO_ESPECIFICO.Any(x => x.codigo_retiro_temporal == codigoRetiroTemporal
                                                    && x.codigo_inmovilizacion == codigoInmovilizacion
                                                    && x.observacion_noaplicacion == observacionNoAplicacion);*/

            if (codigoRetiroTemporal == "0" && codigoInmovilizacion == "0")
            {
                mensajeValidaNull = " Debe indicar una observación ";
            }

            return mensajeValidaNull;
            
        }


        public List<CatalogoDeArticulos> ListCatalogoArticulos()
        {
            var list =
              (from c in db.CATARTICULO
               where c.Estado == "A"
               select new
               {
                   CodArticulo = c.Id,
                   Conducta = c.Conducta,
                   estado = c.Estado,
                   fecha_inicio = c.FechaDeInicio,
                   fecha_final = c.FechaDeFin,
               }).ToList()
               .Select(x => new CatalogoDeArticulos
               {
                   Id = x.CodArticulo,
                   Conducta = x.Conducta,
                   Estado = x.estado,
                   FechaDeInicio = x.fecha_inicio,
                   FechaDeFin = x.fecha_final,
               });


            return list.ToList();
        }



        public class ClaseConducta
        {
            public string Id { get; set; }
            public string Nombre { get; set; }

        }
        public class ClaseFechaInicio
        {
            public string Id { get; set; }
            public string Nombre { get; set; }

        }

        public class ClaseFechaFinal
        {
            public string Id { get; set; }
            public string Nombre { get; set; }

        }

        public class ClaseArticuloRetiroTemporal
        {
            public string Id { get; set; }
            public string Nombre { get; set; }

        }

        public class ClaseArticuloInmovilizacion
        {
            public string Id { get; set; }
            public string Nombre { get; set; }

        }





        public JsonResult ObtenerConducta(string CodigoArticulo)
        {

            var listaconducta =
                 (from o in db.Articulos_Adicionales_Sugeridos
                  where o.Codigo == CodigoArticulo && o.Estado == "A"
                  select new
                  {
                      o.Conducta

                  }).ToList().Distinct()

            .Select(x => new ClaseConducta
            {
                Id = x.Conducta,
                Nombre = x.Conducta
            }).Distinct();

            return Json(listaconducta, JsonRequestBehavior.AllowGet);

        }





        public JsonResult ObtenerConductaCreate(string CodigoArticulo)
        {
            var listaconducta =
                 (from o in db.CATARTICULO
                  where o.Id == CodigoArticulo && o.Estado == "A"
                  select new
                  {

                      o.Conducta
                  }).ToList().Distinct()
            .Select(x => new ClaseConducta
            {
                Id = x.Conducta,
                Nombre = x.Conducta
            }).Distinct();

            return Json(listaconducta, JsonRequestBehavior.AllowGet);
        }





        public JsonResult ObtenerFechaInicio(string CodigoArticulo, string Conducta)
        {
            var listaconducta =
                 (from o in db.CATARTICULO
                  where o.Id == CodigoArticulo && o.Conducta == Conducta && o.Estado == "A"
                  select new
                  {
                      //fecha = o.FechaDeInicio
                      fecha = o.FechaDeInicio.ToString()


                  }).ToList().Distinct()
            .Select(x => new ClaseFechaInicio
            {
                // Id = Convert.ToString(x.fecha),
                // Nombre = Convert.ToString(x.fecha)

                Id = DateTime.Parse(x.fecha).ToString("dd-MM-yyyy"),
                Nombre = DateTime.Parse(x.fecha).ToString("dd-MM-yyyy")

            }).Distinct();

            return Json(listaconducta, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ObtenerFechaFinal(string CodigoArticulo, string Conducta, string FechaDeInicio)
            {
            DateTime fechaDeInicio = DateTime.Now;
            if (FechaDeInicio != "" && FechaDeInicio != null)
            {
                fechaDeInicio = DateTime.Parse(FechaDeInicio);
                //FechaDeInicio = fechaDeInicio.ToString();
            }
            var listaconducta =
                 (from o in db.CATARTICULO
                  where o.Id == CodigoArticulo && o.Conducta == Conducta && o.Estado == "A" && o.FechaDeInicio == fechaDeInicio
                  select new
                  {
                      fecha = o.FechaDeFin.ToString()
                      //fecha = o.FechaDeFin
                  }).ToList().Distinct()
            .Select(x => new ClaseFechaFinal
            {
                Id = DateTime.Parse(x.fecha).ToString("dd-MM-yyyy"),
                Nombre = DateTime.Parse(x.fecha).ToString("dd-MM-yyyy")

                //Id = Convert.ToString(x.fecha),
                //Nombre = Convert.ToString(x.fecha)

            }).Distinct();

            return Json(listaconducta, JsonRequestBehavior.AllowGet);
        }




        public JsonResult ObtenerArticuloRetiroTemporal(string CodigoArticulo, string Conducta, string FechaDeInicio, string FechaDeFin)
        {
            //DateTime fechaDeInicio = DateTime.Now;
            //DateTime fechaDeFin = DateTime.Now;
            //if (FechaDeInicio != "" && FechaDeInicio != null)
            //{
            //    fechaDeInicio = DateTime.Parse(FechaDeInicio);
            //    fechaDeFin = DateTime.Parse(FechaDeFin);
            //    // FechaDeInicio = fechaDeInicio.ToString();
            //}

            //var listaArticuloRetiroTemporal =
            //     (from o in db.CATARTICULO
            //      where o.Estado == "A" && o.FechaDeInicio == fechaDeInicio && o.FechaDeFin == fechaDeFin && o.Conducta == Conducta
            //      select new
            //      {

            //          o.Id

            //      }).ToList()
            //.Select(x => new ClaseArticuloRetiroTemporal
            //{
            //    Id = x.Id,
            //    Nombre = x.Id

            //}).Distinct();

            var listaArticuloRetiroTemporal =
              (from c in db.CATARTICULO
               where c.Estado == "A"
               select new
               {
                   c.Id
               }).ToList()
               .Select(x => new ClaseArticuloRetiroTemporal
               {
                   Id = x.Id,
                   Nombre = x.Id

               }).Distinct();

            return Json(listaArticuloRetiroTemporal, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ObtenerArticuloInmovilizacion(string CodigoArticulo, string Conducta, string FechaDeInicio, string FechaDeFin)
        {
            //DateTime fechaDeInicio = DateTime.Now;
            //DateTime fechaDeFin = DateTime.Now;
            //if (FechaDeInicio != "" && FechaDeInicio != null)
            //{
            //    fechaDeInicio = DateTime.Parse(FechaDeInicio);
            //    fechaDeFin = DateTime.Parse(FechaDeFin);
            //    // FechaDeInicio = fechaDeInicio.ToString();
            //}

            //var listaArticuloInmovilizacion =
            //     (from o in db.CATARTICULO
            //      where o.FechaDeInicio == fechaDeInicio && o.FechaDeFin == fechaDeFin && o.Estado == "A" && o.Conducta == Conducta
            //      select new
            //      {

            //          o.Id

            //      }).ToList()
            //.Select(x => new ClaseArticuloInmovilizacion
            //{
            //    Id = x.Id,
            //    Nombre = x.Id

            //}).Distinct();

            //return Json(listaArticuloInmovilizacion, JsonRequestBehavior.AllowGet);

            var listaArticuloInmovilizacion =
              (from c in db.CATARTICULO
               where c.Estado == "A"
               select new
               {
                   c.Id
               }).ToList()
               .Select(x => new ClaseArticuloInmovilizacion
               {
                   Id = x.Id,
                   Nombre = x.Id
               }).Distinct();

            return Json(listaArticuloInmovilizacion, JsonRequestBehavior.AllowGet);

        }
    }
}
