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
    public class OpcionFormularioPorArticuloesController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: OpcionFormularioPorArticuloes
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";


            var list =
            (from fxa in db.OPCFORMULARIOXARTICULO
             join ca in db.CATARTICULO
             on new { fxa.Id, fxa.Conducta, fxa.FechaDeInicio, fxa.FechaDeFin }
             equals new { ca.Id, ca.Conducta, ca.FechaDeInicio, ca.FechaDeFin } into ca_join
             from ca in ca_join.DefaultIfEmpty()
             join opf in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = fxa.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = opf.Id } into opf_join
             from opf in opf_join.DefaultIfEmpty()
             select new
             {
                 Id = fxa.Id,
                 Conducta = fxa.Conducta,
                 FechaDeInicio = fxa.FechaDeInicio,
                 FechaDeFin = fxa.FechaDeFin,
                 CodigoOpcionFormulario = fxa.CodigoOpcionFormulario,
                 Estado = fxa.Estado,
                 DescripcionCodigoOpcionFormulario = ca.Descripcion,
                 DescripcionCodigoCatArticulo = opf.Descripcion
             }).ToList()

            .Select(x => new OpcionFormularioPorArticulo
            {
                Id = x.Id,
                Conducta = x.Conducta,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                Estado = x.Estado,
                DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
                DescripcionCodigoCatArticulo = x.DescripcionCodigoCatArticulo

            });


            return View(list);
        }

        // GET: OpcionFormularioPorArticuloes/Details/5
        public ActionResult Details(string id, string conducta, DateTime FechaInicio, DateTime FechaFin, int? codFormulario)
        {
            if (id ==null||conducta==null||FechaInicio==null||FechaFin ==null||codFormulario==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id,conducta,FechaInicio,FechaFin,codFormulario);

            var list =
            (from fxa in db.OPCFORMULARIOXARTICULO
             join ca in db.CATARTICULO
             on new { fxa.Id, fxa.Conducta, fxa.FechaDeInicio, fxa.FechaDeFin } equals new { ca.Id, ca.Conducta, ca.FechaDeInicio, ca.FechaDeFin } into ca_join
             where fxa.Id == id && fxa.Conducta == conducta && fxa.FechaDeInicio == FechaInicio && fxa.FechaDeFin == FechaFin
             from ca in ca_join.DefaultIfEmpty()
             join opf in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = fxa.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = opf.Id } into opf_join
             where fxa.CodigoOpcionFormulario == codFormulario
             from opf in opf_join.DefaultIfEmpty()
             select new
             {
                 Id = fxa.Id,
                 Conducta = fxa.Conducta,
                 FechaDeInicio = fxa.FechaDeInicio,
                 FechaDeFin = fxa.FechaDeFin,
                 CodigoOpcionFormulario = fxa.CodigoOpcionFormulario,
                 Estado = fxa.Estado,
                 DescripcionCodigoOpcionFormulario = ca.Descripcion,
                 DescripcionCodigoCatArticulo = opf.Descripcion
             }).Take(50).ToList()

            .Select(x => new OpcionFormularioPorArticulo
            {
                Id = x.Id,
                Conducta = x.Conducta,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                Estado = x.Estado,
                DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
                DescripcionCodigoCatArticulo = x.DescripcionCodigoCatArticulo

            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: OpcionFormularioPorArticuloes/Create
        public ActionResult Create(string Id, string Conducta)
        {
            //se llenan los combos
             IEnumerable<SelectListItem> itemsOpcionFormulario = db.OPCIONFORMULARIO
              .Select(o => new SelectListItem
              {
                  Value = o.Id.ToString(),
                  Text = o.Descripcion
              });
            ViewBag.ComboOpcionFormulario = itemsOpcionFormulario;
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

        // POST: OpcionFormularioPorArticuloes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Conducta,FechaDeInicio,FechaDeFin,CodigoOpcionFormulario,Estado")] OpcionFormularioPorArticulo opcionFormularioPorArticulo)
        {
            if (ModelState.IsValid)
            {
                db.OPCFORMULARIOXARTICULO.Add(opcionFormularioPorArticulo);
                string mensaje = Verificar(opcionFormularioPorArticulo.Id,
                                            opcionFormularioPorArticulo.Conducta,
                                            opcionFormularioPorArticulo.FechaDeInicio,
                                            opcionFormularioPorArticulo.FechaDeFin,
                                            opcionFormularioPorArticulo.CodigoOpcionFormulario);
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
                                                             select new { o.Id }).ToList().Distinct(), "Id", "Id", opcionFormularioPorArticulo.Id);

                    ViewBag.ComboOpcionFormulario = new SelectList(db.OPCIONFORMULARIO.OrderBy(x => x.Descripcion), "Id", "Descripcion", opcionFormularioPorArticulo.CodigoOpcionFormulario);
                    ViewData["Conducta"] = opcionFormularioPorArticulo.Conducta;
                    ViewData["FechaDeInicio"] = opcionFormularioPorArticulo.FechaDeInicio.ToString("dd-MM-yyyy");
                    ViewData["FechaDeFin"] = opcionFormularioPorArticulo.FechaDeFin.ToString("dd-MM-yyyy");
                    return View(opcionFormularioPorArticulo);
                }
            }

            return View(opcionFormularioPorArticulo);
        }

        // GET: OpcionFormularioPorArticuloes/Edit/5
        public ActionResult Edit(string id, string conducta, DateTime FechaInicio, DateTime FechaFin, int? codFormulario)
        {
            if (id == null || conducta == null || FechaInicio == null || FechaFin == null || codFormulario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id, conducta, FechaInicio, FechaFin, codFormulario);

            var list =
            (from fxa in db.OPCFORMULARIOXARTICULO
             join ca in db.CATARTICULO
             on new { fxa.Id, fxa.Conducta, fxa.FechaDeInicio, fxa.FechaDeFin } equals new { ca.Id, ca.Conducta, ca.FechaDeInicio, ca.FechaDeFin } into ca_join
             where fxa.Id == id && fxa.Conducta == conducta && fxa.FechaDeInicio == FechaInicio && fxa.FechaDeFin == FechaFin
             from ca in ca_join.DefaultIfEmpty()
             join opf in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = fxa.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = opf.Id } into opf_join
             where fxa.CodigoOpcionFormulario == codFormulario
             from opf in opf_join.DefaultIfEmpty()
             select new
             {
                 Id = fxa.Id,
                 Conducta = fxa.Conducta,
                 FechaDeInicio = fxa.FechaDeInicio,
                 FechaDeFin = fxa.FechaDeFin,
                 CodigoOpcionFormulario = fxa.CodigoOpcionFormulario,
                 Estado = fxa.Estado,
                 DescripcionCodigoOpcionFormulario = ca.Descripcion,
                 DescripcionCodigoCatArticulo = opf.Descripcion
             }).Take(50).ToList()

            .Select(x => new OpcionFormularioPorArticulo
            {
                Id = x.Id,
                Conducta = x.Conducta,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                Estado = x.Estado,
                DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
                DescripcionCodigoCatArticulo = x.DescripcionCodigoCatArticulo

            }).SingleOrDefault();


            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboOpcionFormulario = new SelectList(db.OPCIONFORMULARIO.OrderBy(x => x.Descripcion), "Id", "Descripcion", codFormulario);

            return View(list);
        }

        // POST: OpcionFormularioPorArticuloes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Conducta,FechaDeInicio,FechaDeFin,CodigoOpcionFormulario,Estado")] OpcionFormularioPorArticulo opcionFormularioPorArticulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opcionFormularioPorArticulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(opcionFormularioPorArticulo);
        }

        // GET: OpcionFormularioPorArticuloes/Delete/5
        public ActionResult Delete(string id, string conducta, DateTime FechaInicio, DateTime FechaFin, int? codFormulario)
        {
            if (id == null || conducta == null || FechaInicio == null || FechaFin == null || codFormulario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id, conducta, FechaInicio, FechaFin, codFormulario);

            var list =
            (from fxa in db.OPCFORMULARIOXARTICULO
             join ca in db.CATARTICULO
             on new { fxa.Id, fxa.Conducta, fxa.FechaDeInicio, fxa.FechaDeFin } equals new { ca.Id, ca.Conducta, ca.FechaDeInicio, ca.FechaDeFin } into ca_join
             where fxa.Id == id && fxa.Conducta == conducta && fxa.FechaDeInicio == FechaInicio && fxa.FechaDeFin == FechaFin
             from ca in ca_join.DefaultIfEmpty()
             join opf in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = fxa.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = opf.Id } into opf_join
             where fxa.CodigoOpcionFormulario == codFormulario
             from opf in opf_join.DefaultIfEmpty()
             select new
             {
                 Id = fxa.Id,
                 Conducta = fxa.Conducta,
                 FechaDeInicio = fxa.FechaDeInicio,
                 FechaDeFin = fxa.FechaDeFin,
                 CodigoOpcionFormulario = fxa.CodigoOpcionFormulario,
                 Estado = fxa.Estado,
                 DescripcionCodigoOpcionFormulario = ca.Descripcion,
                 DescripcionCodigoCatArticulo = opf.Descripcion
             }).Take(50).ToList()

            .Select(x => new OpcionFormularioPorArticulo
            {
                Id = x.Id,
                Conducta = x.Conducta,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                Estado = x.Estado,
                DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
                DescripcionCodigoCatArticulo = x.DescripcionCodigoCatArticulo

            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }
        
        // POST: OpcionFormularioPorArticuloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id, string conducta, DateTime FechaInicio, DateTime FechaFin, int? codFormulario)
        {
            OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id, conducta, FechaInicio, FechaFin, codFormulario);
            if (opcionFormularioPorArticulo.Estado == "I")
                opcionFormularioPorArticulo.Estado = "A";
            else
                opcionFormularioPorArticulo.Estado = "I";
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: OpcionFormularioPorArticuloes/RealDelete/5
        public ActionResult RealDelete(string id, string conducta, DateTime FechaInicio, DateTime FechaFin, int? codFormulario)
        {
            if (id == null || conducta == null || FechaInicio == null || FechaFin == null || codFormulario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id, conducta, FechaInicio, FechaFin, codFormulario);

            var list =
            (from fxa in db.OPCFORMULARIOXARTICULO
             join ca in db.CATARTICULO
             on new { fxa.Id, fxa.Conducta, fxa.FechaDeInicio, fxa.FechaDeFin } equals new { ca.Id, ca.Conducta, ca.FechaDeInicio, ca.FechaDeFin } into ca_join
             where fxa.Id == id && fxa.Conducta == conducta && fxa.FechaDeInicio == FechaInicio && fxa.FechaDeFin == FechaFin
             from ca in ca_join.DefaultIfEmpty()
             join opf in db.OPCIONFORMULARIO on new { CodigoOpcionFormulario = fxa.CodigoOpcionFormulario } equals new { CodigoOpcionFormulario = opf.Id } into opf_join
             where fxa.CodigoOpcionFormulario == codFormulario
             from opf in opf_join.DefaultIfEmpty()
             select new
             {
                 Id = fxa.Id,
                 Conducta = fxa.Conducta,
                 FechaDeInicio = fxa.FechaDeInicio,
                 FechaDeFin = fxa.FechaDeFin,
                 CodigoOpcionFormulario = fxa.CodigoOpcionFormulario,
                 Estado = fxa.Estado,
                 DescripcionCodigoOpcionFormulario = ca.Descripcion,
                 DescripcionCodigoCatArticulo = opf.Descripcion
             }).Take(50).ToList()

            .Select(x => new OpcionFormularioPorArticulo
            {
                Id = x.Id,
                Conducta = x.Conducta,
                FechaDeInicio = x.FechaDeInicio,
                FechaDeFin = x.FechaDeFin,
                CodigoOpcionFormulario = x.CodigoOpcionFormulario,
                Estado = x.Estado,
                DescripcionCodigoOpcionFormulario = x.DescripcionCodigoOpcionFormulario,
                DescripcionCodigoCatArticulo = x.DescripcionCodigoCatArticulo

            }).SingleOrDefault();

            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: OpcionFormularioPorArticuloes/RealDelete/5
        [HttpPost, ActionName("RealDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult RealDeleteConfirmed(string id, string conducta, DateTime FechaInicio, DateTime FechaFin, int? codFormulario)
        {
            OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id, conducta, FechaInicio, FechaFin, codFormulario);
            db.OPCFORMULARIOXARTICULO.Remove(opcionFormularioPorArticulo);
            db.SaveChanges();
            TempData["Type"] = "error";
            TempData["Message"] = "El registro se eliminó correctamente";
            return RedirectToAction("Index");
        }

        public string Verificar(string id, string conducta, DateTime FechaInicio, DateTime FechaFin, int? codFormulario)
        {
            string mensaje = "";
            bool exist = db.OPCFORMULARIOXARTICULO.Any(x => x.Id == id
                                                        && x.Conducta == conducta
                                                        && x.FechaDeInicio == FechaInicio
                                                        && x.FechaDeFin == FechaFin
                                                        && x.CodigoOpcionFormulario == codFormulario);
            if (exist)
            {
                mensaje = "El codigo " + id +
                    ", conducta " + conducta +
                    ", FechaInicio " + FechaInicio +
                    ", Fecha Fin " + FechaFin +
                    ", Opción Formulario " + codFormulario +
                    " ya esta registrado";
            }
            return mensaje;
        }
        //[HttpPost]
        public JsonResult ObtenerConducta(string Id)
        {
            var listaconducta =
                 (from o in db.CATARTICULO
                  where o.Id == Id && o.Estado == "A"
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

        //[HttpPost]
        public JsonResult ObtenerFechaInicio(string Id, string Conducta)
        {
            var listaconducta =
                 (from o in db.CATARTICULO
                  where o.Id == Id && o.Conducta == Conducta && o.Estado == "A"
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

        //[HttpPost]
        public JsonResult ObtenerFechaFinal(string Id, string Conducta, string FechaDeInicio)
        {
            DateTime fechaDeInicio= DateTime.Now;
            if (FechaDeInicio != "" && FechaDeInicio != null)
            {
                fechaDeInicio = DateTime.Parse(FechaDeInicio);
                // FechaDeInicio = fechaDeInicio.ToString();
            }
            var listaconducta =
                 (from o in db.CATARTICULO
                  where o.Id == Id && o.Conducta == Conducta && o.Estado == "A" && o.FechaDeInicio == fechaDeInicio
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
