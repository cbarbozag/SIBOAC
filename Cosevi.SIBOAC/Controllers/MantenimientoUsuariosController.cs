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
using System.Web.Security;

namespace Cosevi.SIBOAC.Controllers
{
    public class MantenimientoUsuariosController : BaseController<SIBOACUsuarios>
    {
        

        // GET: MantenimientoUsuarios
        public ActionResult Index(int ? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list =
                (
            from usu in db.SIBOACUsuarios
            join rol in db.SIBOACRoles on new { Id = usu.Id } equals new { Id = rol.Id }
            select new
            {
                Id = usu.Id,
                Nombre = usu.Nombre,
                Usuario = usu.Usuario,
                Email = usu.Email,
                Codigo = usu.codigo,
                Fecha = usu.FechaDeActualizacionClave,
                contrasena = usu.Contrasena,
                Roles = rol.Nombre,
                Activo = usu.Activo

            }).ToList()
            .Select(x => new SIBOACUsuarios
            {
                Id = x.Id,
                Nombre = x.Nombre,
                Usuario = x.Usuario,
                Email = x.Email,
                codigo = x.Codigo,
                FechaDeActualizacionClave = x.Fecha,
                Roles = x.Roles,
                Contrasena = x.contrasena,
                Activo = x.Activo

            });
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(db.SIBOACUsuarios.ToList().ToPagedList(pageNumber, pageSize));            
            //return View(db.SIBOACUsuarios.ToList());
        }


        public string Verificar(String usuario)
        {
            string mensaje = "";
            bool exist = db.SIBOACUsuarios.Any(x => x.Usuario == usuario);
            if (exist)
            {
                mensaje = "El usuario " + usuario + " ya existe";
            }
            return mensaje;
        }


        // GET: MantenimientoUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACUsuarios);
        }

        // GET: MantenimientoUsuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MantenimientoUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = " Id, Nombre, Usuario, Contrasena, Email, codigo,FechaDeActualizacionClave, Activo ")] SIBOACUsuarios sIBOACUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.SIBOACUsuarios.Add(sIBOACUsuarios);
                string mensaje = Verificar(sIBOACUsuarios.Usuario.ToString());
                if (mensaje == "")
                {
                    sIBOACUsuarios.FechaDeActualizacionClave = DateTime.Now;
                    db.SaveChanges();
                    Bitacora(sIBOACUsuarios, "I", "SIBOACUsuarios");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(sIBOACUsuarios);
                }
            }

            return View(sIBOACUsuarios);
        }

        // GET: MantenimientoUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            var usuario = User.Identity.Name;
            string mensaje = "No puede editar el usuario " + usuario;

                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }

            if (sIBOACUsuarios.Usuario != usuario)
            {
                return View(sIBOACUsuarios);
            }
            else
            {
                //ViewBag.Type = "warning";
                //ViewBag.Message = mensaje;
                TempData["Type"] = "warning";
                TempData["Message"] = mensaje;
                return RedirectToAction("Index");
            }
        }

        // POST: MantenimientoUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Usuario, Email, Contrasena, Nombre, codigo, FechaDeActualizacionClave, Activo")] SIBOACUsuarios sIBOACUsuarios)
        {
                if (ModelState.IsValid)
                {
                    var sIBOACUsuariosAntes = db.SIBOACUsuarios.AsNoTracking().Where(d => d.Id == sIBOACUsuarios.Id).FirstOrDefault();
                    sIBOACUsuarios.FechaDeActualizacionClave = DateTime.Now;
                    sIBOACUsuarios.Contrasena = sIBOACUsuariosAntes.Contrasena;
                    sIBOACUsuarios.Activo = sIBOACUsuariosAntes.Activo;
                    db.Entry(sIBOACUsuarios).State = EntityState.Modified;

                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges

                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
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

                    Bitacora(sIBOACUsuarios, "U", "SIBOACUsuarios", sIBOACUsuariosAntes);
                    return RedirectToAction("Index");
                }

            ViewBag.IdUsuario = new SelectList(db.SIBOACUsuarios, "Id", "Nombre", sIBOACUsuarios.Id);
            return View(sIBOACUsuarios);
        }

        // GET: MantenimientoUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACUsuarios);
        }

        // POST: MantenimientoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            SIBOACUsuarios sIBOACUsuariosAntes = ObtenerCopia(sIBOACUsuarios);
            if (sIBOACUsuarios.Activo == false)

                sIBOACUsuarios.Activo = true;
            else
                sIBOACUsuarios.Activo = false;
            db.SaveChanges();
            Bitacora(sIBOACUsuarios, "U", "SIBOACUsuarios", sIBOACUsuariosAntes);
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
