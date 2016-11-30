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
            return View(db.ARTICULOSXDEPOSITOSBIENES.ToList());
        }

        // GET: ArticulosPorDepositosDeBienes/Details/5
        public ActionResult Details(int? CodDepositoBienes,int? CodFormulario, string CodArticulo,string Conducta, DateTime FechaInicio, DateTime FechaFin )
        {
            if (CodDepositoBienes == null|| CodFormulario == null || CodArticulo ==null || Conducta == null || FechaInicio == null || FechaFin == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticulosPorDepositosDeBienes articulosPorDepositosDeBienes = db.ARTICULOSXDEPOSITOSBIENES.Find(CodDepositoBienes,  CodFormulario,  CodArticulo, Conducta, FechaInicio, FechaFin);
            if (articulosPorDepositosDeBienes == null)
            {
                return HttpNotFound();
            }
            return View(articulosPorDepositosDeBienes);
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


            IEnumerable<SelectListItem> itemsCatArticulos = db.CATARTICULO.Where(a => a.Estado == "A")
            .Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Id.ToString() + " | " + o.Conducta + " | " + o.FechaDeInicio.ToString() + " | " + o.FechaDeFin
            });
            ViewBag.ComboArticulos = itemsCatArticulos;
        
            return View();
        }
      


        public List<CatalogoDeArticulos> ListCatalogoArticulos()
        {

          
            var list =
              (from c in db.CATARTICULO
               where c.Estado =="A"
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
            ArticulosPorDepositosDeBienes articulosPorDepositosDeBienes = db.ARTICULOSXDEPOSITOSBIENES.Find(CodDepositoBienes, CodFormulario, CodArticulo, Conducta, FechaInicio, FechaFin);
            if (articulosPorDepositosDeBienes == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboDepositosBienes = new SelectList(db.DEPOSITOBIENES.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodDepositoBienes);
            ViewBag.ComboOpcionFormulario = new SelectList(db.OPCIONFORMULARIO.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodFormulario);
            ViewBag.ComboArticulos = new SelectList(db.CATARTICULO.OrderBy(x => x.Descripcion), "Id", "Descripcion", CodArticulo);

            return View(articulosPorDepositosDeBienes);
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
            ArticulosPorDepositosDeBienes articulosPorDepositosDeBienes = db.ARTICULOSXDEPOSITOSBIENES.Find(CodDepositoBienes, CodFormulario, CodArticulo, Conducta, FechaInicio, FechaFin);
            if (articulosPorDepositosDeBienes == null)
            {
                return HttpNotFound();
            }
            return View(articulosPorDepositosDeBienes);
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
                                                    &&x.Conducta == Conducta
                                                    &&x.FechaDeInicio == FechaInicio
                                                    &&x.FechaDeFin == FechaFin);
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
    }
}
