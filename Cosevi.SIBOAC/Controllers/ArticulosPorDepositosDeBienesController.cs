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

            IEnumerable<SelectListItem> itemsOpcionFormularios = db.OPCIONFORMULARIO
             .Select(o => new SelectListItem
             {
                 Value = o.Id.ToString(),
                 Text = o.Descripcion
             });
            ViewBag.ComboOpcionFormulario = itemsOpcionFormularios;

            IEnumerable<SelectListItem> itemsCatArticulos = db.CATARTICULO
            .Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Descripcion
            });
            ViewBag.ComboArticulos = itemsCatArticulos;
            return View();
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
                db.SaveChanges();
                return RedirectToAction("Index");
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
            db.ARTICULOSXDEPOSITOSBIENES.Remove(articulosPorDepositosDeBienes);
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
    }
}
