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
    public class ArticulosPorDepositosDeBienesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: ArticulosPorDepositosDeBienes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list =
            (from a in db.ARTICULOSXDEPOSITOSBIENES
             join d in db.DEPOSITOBIENES on new { CodigoDepositosBienes = a.CodigoDepositosBienes } equals new { CodigoDepositosBienes = d.Id } into d_join
             from d in d_join.DefaultIfEmpty()
             join o in db.OPCIONFORMULARIO on new { Id = a.CodigoOpcionFormulario } equals new { Id = o.Id } into o_join
             from o in o_join.DefaultIfEmpty()
             join c in db.CATARTICULO
                   on new { a.CodigoArticulo, a.Conducta, a.FechaDeInicio, a.FechaDeFin }
               equals new { CodigoArticulo = c.Id, c.Conducta, c.FechaDeInicio, c.FechaDeFin } into c_join
             from c in c_join.DefaultIfEmpty()
             select new
             {
                 CodigoDepositosBienes = a.CodigoDepositosBienes,
                 CodigoOpcionFormulario = a.CodigoOpcionFormulario,
                 CodigoArticulo = a.CodigoArticulo,
                 Conducta = a.Conducta,
                 FechaDeInicio = a.FechaDeInicio,
                 FechaDeFin = a.FechaDeFin,
                 Estado = a.Estado,
                 DescripcionDepositosBienes = d.Descripcion,
                 DescripcionCodigoFormulario = o.Descripcion,
                 DescripcionArticulo = c.Descripcion
             }).ToList()

             .Select(x => new ArticulosPorDepositosDeBienes
             {
                 CodigoDepositosBienes = x.CodigoDepositosBienes,
                 CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                 CodigoArticulo = x.CodigoArticulo,
                 Conducta = x.Conducta,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 Estado = x.Estado,
                 DescripcionDepositosBienes = x.DescripcionDepositosBienes,
                 DescripcionCodigoFormulario = x.DescripcionCodigoFormulario,
                 DescripcionArticulo = x.DescripcionArticulo

             }).OrderBy(x => (x.CodigoDepositosBienes));

            return View(list);
        }

        // GET: ArticulosPorDepositosDeBienes/Details/5
        public ActionResult Details(int? CodDepositoBienes, int? CodFormulario, string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin)
        {
            if (CodDepositoBienes == null || CodFormulario == null || CodArticulo == null || Conducta == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ArticulosPorDepositosDeBienes articulosPorDepositosDeBienes = db.ARTICULOSXDEPOSITOSBIENES.Find(CodDepositoBienes,  CodFormulario,  CodArticulo, Conducta, FechaInicio, FechaFin);


            var list =
            (from a in db.ARTICULOSXDEPOSITOSBIENES
             join d in db.DEPOSITOBIENES on new { CodigoDepositosBienes = a.CodigoDepositosBienes } equals new { CodigoDepositosBienes = d.Id } into d_join
             where a.CodigoDepositosBienes == CodDepositoBienes
             from d in d_join.DefaultIfEmpty()
             join o in db.OPCIONFORMULARIO on new { Id = a.CodigoOpcionFormulario } equals new { Id = o.Id } into o_join
             where a.CodigoOpcionFormulario == CodFormulario
             from o in o_join.DefaultIfEmpty()
             join c in db.CATARTICULO on new { a.CodigoArticulo, a.Conducta, a.FechaDeInicio, a.FechaDeFin } equals new { CodigoArticulo = c.Id, c.Conducta, c.FechaDeInicio, c.FechaDeFin } into c_join
             where a.CodigoArticulo == CodArticulo && a.Conducta == Conducta && a.FechaDeInicio == FechaInicio && a.FechaDeFin == FechaFin
             from c in c_join.DefaultIfEmpty()
             select new
             {
                 a.CodigoDepositosBienes,
                 a.CodigoOpcionFormulario,
                 a.CodigoArticulo,
                 a.Conducta,
                 a.FechaDeInicio,
                 a.FechaDeFin,
                 a.Estado,
                 DescripcionDepositosBienes = d.Descripcion,
                 DescripcionCodigoFormulario = o.Descripcion,
                 DescripcionArticulo = c.Descripcion
             }).ToList()

             .Select(x => new ArticulosPorDepositosDeBienes
             {
                 CodigoDepositosBienes = x.CodigoDepositosBienes,
                 CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                 CodigoArticulo = x.CodigoArticulo,
                 Conducta = x.Conducta,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 Estado = x.Estado,
                 DescripcionDepositosBienes = x.DescripcionDepositosBienes,
                 DescripcionCodigoFormulario = x.DescripcionCodigoFormulario,
                 DescripcionArticulo = x.DescripcionArticulo

             }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: ArticulosPorDepositosDeBienes/Create

        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsDepositosBienes = db.DEPOSITOBIENES
              .Select(o => new SelectListItem
              {
                  Value = o.Id.ToString(),
                  Text = o.Descripcion
              });


            ViewBag.ComboDepositosBienes = itemsDepositosBienes;

            IEnumerable<SelectListItem> itemsOpcionFormularios = db.OPCIONFORMULARIO.Where(a => a.Estado == "A")
             .Select(o => new SelectListItem
             {
                 Value = o.Id.ToString(),
                 Text = o.Descripcion
             });
            ViewBag.ComboOpcionFormulario = itemsOpcionFormularios;


            IEnumerable<SelectListItem> itemsCatArticulos = (from o in db.CATARTICULO
                                                             where o.Estado == "A"
                                                             select new  { o.Id }).ToList().Distinct()          
                                                            .Select(o => new SelectListItem
                                                            {
                                                                Value = o.Id.ToString(),
                                                                Text = o.Id.ToString()
                                                            });
                                                            ViewBag.ComboArticulos = itemsCatArticulos;

            return View();
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
        // POST: ArticulosPorDepositosDeBienes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoDepositosBienes,CodigoOpcionFormulario,CodigoArticulo,Conducta,FechaDeInicio,FechaDeFin,Estado")] ArticulosPorDepositosDeBienes articulosPorDepositosDeBienes)
        {
            if (ModelState.IsValid)
            {
                db.ARTICULOSXDEPOSITOSBIENES.Add(articulosPorDepositosDeBienes);
                string mensaje = Verificar(articulosPorDepositosDeBienes.CodigoDepositosBienes,
                                           articulosPorDepositosDeBienes.CodigoOpcionFormulario,
                                           articulosPorDepositosDeBienes.CodigoArticulo,
                                           articulosPorDepositosDeBienes.Conducta,
                                           articulosPorDepositosDeBienes.FechaDeInicio,
                                           articulosPorDepositosDeBienes.FechaDeFin);
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

                    ViewBag.ComboArticulos = null;
                    ViewBag.ComboArticulos = new SelectList((from o in db.CATARTICULO
                                                             where o.Estado == "A"
                                                             select new { o.Id }).ToList().Distinct(), "Id", "Id", articulosPorDepositosDeBienes.CodigoArticulo);

                    ViewBag.ComboDepositosBienes = new SelectList((from o in db.DEPOSITOBIENES
                                                                   select new { o.Id,o.Descripcion }).ToList().Distinct(), "Id", "Descripcion", articulosPorDepositosDeBienes.CodigoDepositosBienes);

                    ViewBag.ComboOpcionFormulario = new SelectList((from o in db.OPCIONFORMULARIO
                                                                    select new { o.Id, o.Descripcion }).ToList().Distinct(), "Id", "Descripcion", articulosPorDepositosDeBienes.CodigoOpcionFormulario);


                    ViewData["Conducta"] = articulosPorDepositosDeBienes.Conducta;
                    ViewData["FechaDeInicio"] = articulosPorDepositosDeBienes.FechaDeInicio.ToString("dd-MM-yyyy");
                    ViewData["FechaDeFin"] = articulosPorDepositosDeBienes.FechaDeFin.ToString("dd-MM-yyyy");
                    return View(articulosPorDepositosDeBienes);
                }
            }

            return View(articulosPorDepositosDeBienes);
        }

        // GET: ArticulosPorDepositosDeBienes/Edit/5
        public ActionResult Edit(int? CodDepositoBienes, int? CodFormulario, string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin)
        {
            if (CodDepositoBienes == null || CodFormulario == null || CodArticulo == null || Conducta == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ArticulosPorDepositosDeBienes articulosPorDepositosDeBienes = db.ARTICULOSXDEPOSITOSBIENES.Find(CodDepositoBienes, CodFormulario, CodArticulo, Conducta, FechaInicio, FechaFin);

            var list =
            (from a in db.ARTICULOSXDEPOSITOSBIENES
             join d in db.DEPOSITOBIENES on new { CodigoDepositosBienes = a.CodigoDepositosBienes } equals new { CodigoDepositosBienes = d.Id } into d_join
             where a.CodigoDepositosBienes == CodDepositoBienes
             from d in d_join.DefaultIfEmpty()
             join o in db.OPCIONFORMULARIO on new { Id = a.CodigoOpcionFormulario } equals new { Id = o.Id } into o_join
             where a.CodigoOpcionFormulario == CodFormulario
             from o in o_join.DefaultIfEmpty()
             join c in db.CATARTICULO on new { a.CodigoArticulo, a.Conducta, a.FechaDeInicio, a.FechaDeFin } equals new { CodigoArticulo = c.Id, c.Conducta, c.FechaDeInicio, c.FechaDeFin } into c_join
             where a.CodigoArticulo == CodArticulo && a.Conducta == Conducta && a.FechaDeInicio == FechaInicio && a.FechaDeFin == FechaFin
             from c in c_join.DefaultIfEmpty()
             select new
             {
                 CodigoDepositosBienes = a.CodigoDepositosBienes,
                 CodigoOpcionFormulario = a.CodigoOpcionFormulario,
                 CodigoArticulo = a.CodigoArticulo,
                 Conducta = a.Conducta,
                 FechaDeInicio = a.FechaDeInicio,
                 FechaDeFin = a.FechaDeFin,
                 Estado = a.Estado,
                 DescripcionDepositosBienes = d.Descripcion,
                 DescripcionCodigoFormulario = o.Descripcion,
                 DescripcionArticulo = c.Descripcion
             }).ToList()

             .Select(x => new ArticulosPorDepositosDeBienes
             {
                 CodigoDepositosBienes = x.CodigoDepositosBienes,
                 CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                 CodigoArticulo = x.CodigoArticulo,
                 Conducta = x.Conducta,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 Estado = x.Estado,
                 DescripcionDepositosBienes = x.DescripcionDepositosBienes,
                 DescripcionCodigoFormulario = x.DescripcionCodigoFormulario,
                 DescripcionArticulo = x.DescripcionArticulo

             }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboDepositosBienes = new SelectList(db.DEPOSITOBIENES.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodDepositoBienes);
            ViewBag.ComboOpcionFormulario = new SelectList(db.OPCIONFORMULARIO.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodFormulario);
            ViewBag.ComboArticulos = new SelectList(db.CATARTICULO.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodArticulo);

            return View(list);
        }

        // POST: ArticulosPorDepositosDeBienes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoDepositosBienes,CodigoOpcionFormulario,CodigoArticulo,Conducta,FechaDeInicio,FechaDeFin,Estado")] ArticulosPorDepositosDeBienes articulosPorDepositosDeBienes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articulosPorDepositosDeBienes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(articulosPorDepositosDeBienes);
        }

        // GET: ArticulosPorDepositosDeBienes/Delete/5
        public ActionResult Delete(int? CodDepositoBienes, int? CodFormulario, string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin)
        {
            if (CodDepositoBienes == null || CodFormulario == null || CodArticulo == null || Conducta == null || FechaInicio == null || FechaFin == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ArticulosPorDepositosDeBienes articulosPorDepositosDeBienes = db.ARTICULOSXDEPOSITOSBIENES.Find(CodDepositoBienes, CodFormulario, CodArticulo, Conducta, FechaInicio, FechaFin);

            var list =
            (from a in db.ARTICULOSXDEPOSITOSBIENES
             join d in db.DEPOSITOBIENES on new { CodigoDepositosBienes = a.CodigoDepositosBienes } equals new { CodigoDepositosBienes = d.Id } into d_join
             where a.CodigoDepositosBienes == CodDepositoBienes
             from d in d_join.DefaultIfEmpty()
             join o in db.OPCIONFORMULARIO on new { Id = a.CodigoOpcionFormulario } equals new { Id = o.Id } into o_join
             where a.CodigoOpcionFormulario == CodFormulario
             from o in o_join.DefaultIfEmpty()
             join c in db.CATARTICULO on new { a.CodigoArticulo, a.Conducta, a.FechaDeInicio, a.FechaDeFin } equals new { CodigoArticulo = c.Id, c.Conducta, c.FechaDeInicio, c.FechaDeFin } into c_join
             where a.CodigoArticulo == CodArticulo && a.Conducta == Conducta && a.FechaDeInicio == FechaInicio && a.FechaDeFin == FechaFin
             from c in c_join.DefaultIfEmpty()
             select new
             {
                 a.CodigoDepositosBienes,
                 a.CodigoOpcionFormulario,
                 a.CodigoArticulo,
                 a.Conducta,
                 a.FechaDeInicio,
                 a.FechaDeFin,
                 a.Estado,
                 DescripcionDepositosBienes = d.Descripcion,
                 DescripcionCodigoFormulario = o.Descripcion,
                 DescripcionArticulo = c.Descripcion
             }).ToList()

             .Select(x => new ArticulosPorDepositosDeBienes
             {
                 CodigoDepositosBienes = x.CodigoDepositosBienes,
                 CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                 CodigoArticulo = x.CodigoArticulo,
                 Conducta = x.Conducta,
                 FechaDeInicio = x.FechaDeInicio,
                 FechaDeFin = x.FechaDeFin,
                 Estado = x.Estado,
                 DescripcionDepositosBienes = x.DescripcionDepositosBienes,
                 DescripcionCodigoFormulario = x.DescripcionCodigoFormulario,
                 DescripcionArticulo = x.DescripcionArticulo

             }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: ArticulosPorDepositosDeBienes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? CodDepositoBienes, int? CodFormulario, string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin)
        {
            ArticulosPorDepositosDeBienes articulosPorDepositosDeBienes = db.ARTICULOSXDEPOSITOSBIENES.Find(CodDepositoBienes, CodFormulario, CodArticulo, Conducta, FechaInicio, FechaFin);
            if (articulosPorDepositosDeBienes.Estado == "A")
                articulosPorDepositosDeBienes.Estado = "I";
            else
                articulosPorDepositosDeBienes.Estado = "A";
            db.SaveChanges();
            return RedirectToAction("Index");
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
                
            return Json(listaconducta,JsonRequestBehavior.AllowGet);
        }
   
        public JsonResult ObtenerFechaInicio(string CodigoArticulo,string Conducta)
        {
            var listaconducta =
                 (from o in db.CATARTICULO
                  where o.Id == CodigoArticulo && o.Conducta == Conducta && o.Estado=="A"
                  select new
                  {
                      fecha = o.FechaDeInicio.ToString()
                    
                  }).ToList().Distinct()
            .Select(x => new ClaseFechaInicio
            {
                Id = DateTime.Parse(x.fecha).ToString("dd-MM-yyyy"),
                Nombre = DateTime.Parse(x.fecha).ToString("dd-MM-yyyy")
            }).Distinct();

            return Json(listaconducta, JsonRequestBehavior.AllowGet);
        }

      
        public JsonResult ObtenerFechaFinal(string CodigoArticulo, string Conducta,string FechaDeInicio)
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
                  }).ToList().Distinct()
            .Select(x => new ClaseFechaFinal
            {
                Id = DateTime.Parse(x.fecha).ToString("dd-MM-yyyy"),
                Nombre = DateTime.Parse(x.fecha).ToString("dd-MM-yyyy")
            }).Distinct();

            return Json(listaconducta, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string Verificar(int? CodDepositoBienes, int? CodFormulario, string CodArticulo, string Conducta, DateTime FechaInicio, DateTime FechaFin)
        {
            string mensaje = "";
            bool exist = db.ARTICULOSXDEPOSITOSBIENES.Any(x => x.CodigoDepositosBienes == CodDepositoBienes
                                                    && x.CodigoOpcionFormulario == CodFormulario
                                                    && x.CodigoArticulo == CodArticulo
                                                    && x.Conducta == Conducta
                                                    && x.FechaDeInicio == FechaInicio
                                                    && x.FechaDeFin == FechaFin);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados: código de depósito de bienes" + CodDepositoBienes +
                           ", código opción formulario" + CodFormulario +
                           ", código árticulo" + CodArticulo +
                           ", conducta " + Conducta +
                           ", Fecha Inicio " + FechaInicio +
                           ", fecha Fin " + FechaFin;

            }
            return mensaje;
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

    }
}
