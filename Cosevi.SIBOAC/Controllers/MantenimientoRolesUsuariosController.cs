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
    public class MantenimientoRolesUsuariosController : Controller
    {
        private SIBOACSecurityEntities db = new SIBOACSecurityEntities();

        // GET: MantenimientoRolesUsuarios
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list =
                (
            from usu in db.SIBOACUsuarios
            join ru in db.SIBOACRolesDeUsuarios on new { IdUsuario = usu.Id } equals new { IdUsuario = ru.IdUsuario }
            join rol in db.SIBOACRoles on new { Id = ru.IdRol } equals new { Id = rol.Id }
            select new
            {
                Id = ru.Id,
                NombreUsuario = usu.Nombre,
                //Usuario = usu.Usuario,
                //Email = usu.Email,
                //Codigo = usu.codigo,
                //Fecha = usu.FechaDeActualizacionClave,
                //contrasena = usu.Contrasena,
                Roles = rol.Nombre
                //activo = rol.Activo

            }).ToList()
            .Select(x => new SIBOACRolesDeUsuarios
            {
                Id = x.Id,
                NombreUsuario = x.NombreUsuario,
                //Usuario = x.Usuario,
                //Email = x.Email,
                //codigo = x.Codigo,
                //FechaDeActualizacionClave = x.Fecha,
                Roles = x.Roles
                //Contrasena = x.contrasena,
                //Activo = x.activo

            });

            return View(list);
        }

        // GET: MantenimientoRolesUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACRolesDeUsuarios sIBOACRolesDeUsuarios = db.SIBOACRolesDeUsuarios.Find(id);
            if (sIBOACRolesDeUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACRolesDeUsuarios);
        }

        // GET: MantenimientoRolesUsuarios/Create
        public ActionResult Create()
        {
            ViewBag.IdRol = new SelectList(db.SIBOACRoles, "Id", "Nombre");
            //ViewBag.IdUsuario = new SelectList(db.SIBOACUsuarios, "Id", "IdUsuario");


            var list =
                (from usu in db.SIBOACUsuarios
                 where (from rol in db.SIBOACRolesDeUsuarios

                         select new
                         { rol.IdUsuario }).Contains(new
                         { IdUsuario = usu.Id })

                 select new
                 {
                     Nombre = usu.Nombre,
                     Id = usu.Id

                 }).ToList()

                .Select(x => new SIBOACRolesDeUsuarios
                {
                    NombreUsuario = x.Nombre,
                    IdUsuario = x.Id
                });

            ViewBag.ComboDeUsuariosSinRol = new SelectList(list, "IdUsuario", "NombreUsuario");
            return View();
        }


        // POST: MantenimientoRolesUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdUsuario,IdRol")] SIBOACRolesDeUsuarios sIBOACRolesDeUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.SIBOACRolesDeUsuarios.Add(sIBOACRolesDeUsuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRol = new SelectList(db.SIBOACRoles, "Id", "Nombre", sIBOACRolesDeUsuarios.IdRol);
            //ViewBag.ComboDeUsuariosSinRol = new SelectList(db.SIBOACUsuarios, "Id", "Usuario", sIBOACRolesDeUsuarios.IdUsuario);
            return View(sIBOACRolesDeUsuarios);
        }

        // GET: MantenimientoRolesUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACRolesDeUsuarios sIBOACRolesDeUsuarios = db.SIBOACRolesDeUsuarios.Find(id);
            if (sIBOACRolesDeUsuarios == null)
            {
                return HttpNotFound();
            }

            ViewBag.IdRol = new SelectList(db.SIBOACRoles, "Id", "Nombre", sIBOACRolesDeUsuarios.IdRol);
            //ViewBag.IdUsuario = new SelectList(db.SIBOACUsuarios, "Id", "Usuario", sIBOACRolesDeUsuarios.IdUsuario);
            return View(sIBOACRolesDeUsuarios);
        }

        // POST: MantenimientoRolesUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdUsuario,IdRol")] SIBOACRolesDeUsuarios sIBOACRolesDeUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sIBOACRolesDeUsuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
      
            ViewBag.IdRol = new SelectList(db.SIBOACRoles, "Id", "Nombre", sIBOACRolesDeUsuarios.IdRol);
            //ViewBag.IdUsuario = new SelectList(db.SIBOACUsuarios, "Id", "Usuario", sIBOACRolesDeUsuarios.IdUsuario);
            return View(sIBOACRolesDeUsuarios);
        }

        // GET: MantenimientoRolesUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SIBOACRolesDeUsuarios sIBOACRolesDeUsuarios = db.SIBOACRolesDeUsuarios.Find(id);
            if (sIBOACRolesDeUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACRolesDeUsuarios);
        }

        // POST: MantenimientoRolesUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACRolesDeUsuarios sIBOACRolesDeUsuarios = db.SIBOACRolesDeUsuarios.Find(id);
            db.SIBOACRolesDeUsuarios.Remove(sIBOACRolesDeUsuarios);
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
