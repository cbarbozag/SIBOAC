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
    public class RolPorPersonaOpcionFormulariosController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RolPorPersonaOpcionFormularios
        public ActionResult Index()
        {
            return View(db.ROLPERSONA_OPCIONFORMULARIO.ToList());
        }

        // GET: RolPorPersonaOpcionFormularios/Details/5
        public ActionResult Details(int? codRol, int? codFormulario)
        {
            if (codRol == null|| codFormulario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);
            if (rolPorPersonaOpcionFormulario == null)
            {
                return HttpNotFound();
            }
            return View(rolPorPersonaOpcionFormulario);
        }

        // GET: RolPorPersonaOpcionFormularios/Create
        public ActionResult Create()
        {

            IEnumerable<SelectListItem> itemsRol = db.ROLPERSONA
             .Select(o => new SelectListItem
             {
                 Value = o.Id,
                 Text = o.Descripcion
             });
            ViewBag.ComboRolPersona = itemsRol;

            IEnumerable<SelectListItem> itemsOpcionFormulario = db.OPCIONFORMULARIO
            .Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Descripcion
            });
            ViewBag.ComboOpcionFormulario = itemsOpcionFormulario;
           
            return View();
        }

        // POST: RolPorPersonaOpcionFormularios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoRolPersona,CodigoOpcionFormulario,Estado,FechaDeInicio,FechaDeFin")] RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario)
        {
            if (ModelState.IsValid)
            {
                db.ROLPERSONA_OPCIONFORMULARIO.Add(rolPorPersonaOpcionFormulario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rolPorPersonaOpcionFormulario);
        }

        // GET: RolPorPersonaOpcionFormularios/Edit/5
        public ActionResult Edit(int? codRol, int codFormulario)
        {
            if (codRol == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);
            if (rolPorPersonaOpcionFormulario == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboRolPersona = new SelectList(db.ROLPERSONA.OrderBy(x => x.Descripcion), "Id", "Descripcion", codRol);
            ViewBag.ComboOpcionFormulario = new SelectList(db.OPCIONFORMULARIO.OrderBy(x => x.Descripcion), "Id", "Descripcion", codFormulario);

            return View(rolPorPersonaOpcionFormulario);
        }

        // POST: RolPorPersonaOpcionFormularios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoRolPersona,CodigoOpcionFormulario,Estado,FechaDeInicio,FechaDeFin")] RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rolPorPersonaOpcionFormulario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rolPorPersonaOpcionFormulario);
        }

        // GET: RolPorPersonaOpcionFormularios/Delete/5
        public ActionResult Delete(int? codRol, int? codFormulario)
        {
            if (codRol == null|| codFormulario==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);
            if (rolPorPersonaOpcionFormulario == null)
            {
                return HttpNotFound();
            }
            return View(rolPorPersonaOpcionFormulario);
        }

        // POST: RolPorPersonaOpcionFormularios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int codRol, int codFormulario)
        {
            RolPorPersonaOpcionFormulario rolPorPersonaOpcionFormulario = db.ROLPERSONA_OPCIONFORMULARIO.Find(codRol, codFormulario);
            if (rolPorPersonaOpcionFormulario.Estado == "I")
                rolPorPersonaOpcionFormulario.Estado = "A";
            else
                rolPorPersonaOpcionFormulario.Estado = "I";
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
