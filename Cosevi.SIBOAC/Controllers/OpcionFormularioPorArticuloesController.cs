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
            return View(db.OPCFORMULARIOXARTICULO.ToList());
        }

        // GET: OpcionFormularioPorArticuloes/Details/5
        public ActionResult Details(string id, string conducta, DateTime FechaInicio, DateTime FechaFin, int? codFormulario)
        {
            if (id ==null||conducta==null||FechaInicio==null||FechaFin ==null||codFormulario==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id,conducta,FechaInicio,FechaFin,codFormulario);
            if (opcionFormularioPorArticulo == null)
            {
                return HttpNotFound();
            }
            return View(opcionFormularioPorArticulo);
        }

        // GET: OpcionFormularioPorArticuloes/Create
        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsOpcionFormulario = db.OPCIONFORMULARIO
              .Select(o => new SelectListItem
              {
                  Value = o.Id.ToString(),
                  Text = o.Descripcion
              });
            ViewBag.ComboOpcionFormulario = itemsOpcionFormulario;
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
            OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id, conducta, FechaInicio, FechaFin, codFormulario);
            if (opcionFormularioPorArticulo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComboOpcionFormulario = new SelectList(db.OPCIONFORMULARIO.OrderBy(x => x.Descripcion), "Id", "Descripcion", codFormulario);

            return View(opcionFormularioPorArticulo);
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
            OpcionFormularioPorArticulo opcionFormularioPorArticulo = db.OPCFORMULARIOXARTICULO.Find(id, conducta, FechaInicio, FechaFin, codFormulario);
            if (opcionFormularioPorArticulo == null)
            {
                return HttpNotFound();
            }
            return View(opcionFormularioPorArticulo);
        }
        public string Verificar(string id, string conducta, DateTime FechaInicio, DateTime FechaFin, int? codFormulario)
        {
            string mensaje = "";
            bool exist = db.OPCFORMULARIOXARTICULO.Any(x => x.Id == id
                                                        &&x.Conducta==conducta
                                                        &&x.FechaDeInicio==FechaInicio
                                                        &&x.FechaDeFin== FechaFin
                                                        &&x.CodigoOpcionFormulario ==codFormulario);
            if (exist)
            {
                mensaje = "El codigo " + id +
                    ", conducta "+conducta+
                    ", FechaInicio "+ FechaInicio+
                    ", Fecha Fin "+ FechaFin+
                    ", Opción Formulario "+ codFormulario+
                    " ya esta registrado";
            }
            return mensaje;
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
