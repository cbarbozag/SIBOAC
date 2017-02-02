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
    public class ARTICULO_ESPECIFICOController : BaseController<ARTICULO_ESPECIFICO>
    {
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        public string Verificar(string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin)
        {
            string mensaje = "";
            bool exist = db.ARTICULO_ESPECIFICO.Any(x => x.codigo == CodArticulo
                                                    && x.conducta == Conducta
                                                    && x.fecha_inicio == FechaInicio
                                                    && x.fecha_final == FechaFin);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados: código de artículos específicos " + CodArticulo +
                           ", Conducta " + Conducta +
                           ", Fecha inicial " + FechaInicio +
                           ", Fecha final " + FechaFin;
            }
            return mensaje;
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


        public JsonResult ObtenerArticuloRetiroTemporal(string FechaDeInicio, string FechaDeFin, string Conducta)
        {
            DateTime fechaDeInicio = DateTime.Now;
            DateTime fechaDeFin = DateTime.Now;
            if (FechaDeInicio != "" && FechaDeInicio != null)
            {
                fechaDeInicio = DateTime.Parse(FechaDeInicio);
                fechaDeFin = DateTime.Parse(FechaDeFin);
                // FechaDeInicio = fechaDeInicio.ToString();
            }

            var listaArticuloRetiroTemporal =
                 (from o in db.CATARTICULO
                  where o.FechaDeInicio == fechaDeInicio && o.FechaDeFin == fechaDeFin && o.Estado == "A" && o.Conducta == "1"
                  select new
                  {

                      o.Id

                  }).ToList()
            .Select(x => new ClaseArticuloRetiroTemporal
            {
                Id = x.Id,
                Nombre = x.Id

            }).Distinct();

            return Json(listaArticuloRetiroTemporal, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ObtenerArticuloInmovilizacion(string FechaDeInicio, string FechaDeFin, string Conducta)
        {
            DateTime fechaDeInicio = DateTime.Now;
            DateTime fechaDeFin = DateTime.Now;
            if (FechaDeInicio != "" && FechaDeInicio != null)
            {
                fechaDeInicio = DateTime.Parse(FechaDeInicio);
                fechaDeFin = DateTime.Parse(FechaDeFin);
                // FechaDeInicio = fechaDeInicio.ToString();
            }

            var listaArticuloInmovilizacion =
                 (from o in db.CATARTICULO
                  where o.FechaDeInicio == fechaDeInicio && o.FechaDeFin == fechaDeFin && o.Estado == "A" && o.Conducta == "1"
                  select new
                  {

                      o.Id

                  }).ToList()
            .Select(x => new ClaseArticuloInmovilizacion
            {
                Id = x.Id,
                Nombre = x.Id

            }).Distinct();

            return Json(listaArticuloInmovilizacion, JsonRequestBehavior.AllowGet);
        }



        public JsonResult ObtenerConducta(string CodigoArticulo)
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
                //Id = Convert.ToString(x.fecha),
                //Nombre = Convert.ToString(x.fecha)

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
                // FechaDeInicio = fechaDeInicio.ToString();
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

        // GET: ARTICULO_ESPECIFICO
        public ActionResult Index()
        {
            return View(db.ARTICULO_ESPECIFICO.ToList());
        }

        // GET: ARTICULO_ESPECIFICO/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARTICULO_ESPECIFICO aRTICULO_ESPECIFICO = db.ARTICULO_ESPECIFICO.Find(id);
            if (aRTICULO_ESPECIFICO == null)
            {
                return HttpNotFound();
            }
            return View(aRTICULO_ESPECIFICO);
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
        public ActionResult Create([Bind(Include = "codigo,conducta,fecha_inicio,fecha_final,estado,codigo_retiro_temporal,codigo_inmovilizacion,observacion_noaplicacion")] ARTICULO_ESPECIFICO aRTICULO_ESPECIFICO)
        {
            if (ModelState.IsValid)
            {

                db.ARTICULO_ESPECIFICO.Add(aRTICULO_ESPECIFICO);
                string mensaje = Verificar(aRTICULO_ESPECIFICO.codigo,
                                           aRTICULO_ESPECIFICO.conducta,
                                           aRTICULO_ESPECIFICO.fecha_inicio.Value,
                                           aRTICULO_ESPECIFICO.fecha_final.Value);


                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(aRTICULO_ESPECIFICO, "I", "ARTICULO_ESPECIFICO");
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
                                                             select new { o.Id }).ToList().Distinct(), "Id", "Id", aRTICULO_ESPECIFICO.codigo);

                    //ViewBag.ComboConducta = new SelectList((from o in db.CATARTICULO
                    //                                               select new { o.Conducta }).ToList().Distinct(), "Id", "conducta", aRTICULO_ESPECIFICO.conducta);

                    //ViewBag.ComboFechaInicio = new SelectList((from o in db.CATARTICULO
                    //                                           select new { o.FechaDeInicio }).ToList().Distinct(), "Id", "fecha_inicio", aRTICULO_ESPECIFICO.fecha_inicio);


                    ViewData["Conducta"] = aRTICULO_ESPECIFICO.conducta;
                    ViewData["FechaDeInicio"] = aRTICULO_ESPECIFICO.fecha_inicio.Value.ToString("dd-MM-yyyy");
                    ViewData["FechaDeFin"] = aRTICULO_ESPECIFICO.fecha_final.Value.ToString("dd-MM-yyyy");
                    ViewData["CodRetTemp"] = aRTICULO_ESPECIFICO.codigo_retiro_temporal;
                    return View(aRTICULO_ESPECIFICO);
                }

            }

            return View(aRTICULO_ESPECIFICO);
        }

        // GET: ARTICULO_ESPECIFICO/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARTICULO_ESPECIFICO aRTICULO_ESPECIFICO = db.ARTICULO_ESPECIFICO.Find(id);
            if (aRTICULO_ESPECIFICO == null)
            {
                return HttpNotFound();
            }
            return View(aRTICULO_ESPECIFICO);
        }

        // POST: ARTICULO_ESPECIFICO/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "codigo,conducta,fecha_inicio,fecha_final,estado,codigo_retiro_temporal,codigo_inmovilizacion,observacion_noaplicacion")] ARTICULO_ESPECIFICO aRTICULO_ESPECIFICO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aRTICULO_ESPECIFICO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aRTICULO_ESPECIFICO);
        }

        // GET: ARTICULO_ESPECIFICO/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ARTICULO_ESPECIFICO aRTICULO_ESPECIFICO = db.ARTICULO_ESPECIFICO.Find(id);
            if (aRTICULO_ESPECIFICO == null)
            {
                return HttpNotFound();
            }
            return View(aRTICULO_ESPECIFICO);
        }

        // POST: ARTICULO_ESPECIFICO/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ARTICULO_ESPECIFICO aRTICULO_ESPECIFICO = db.ARTICULO_ESPECIFICO.Find(id);
            db.ARTICULO_ESPECIFICO.Remove(aRTICULO_ESPECIFICO);
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
