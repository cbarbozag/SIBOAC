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

namespace Cosevi.SIBOAC.Controllers
{
    public class MantenimientoUsuariosController : Controller
    {
        private SIBOACSecurityEntities db = new SIBOACSecurityEntities();

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
                activo = rol.Activo

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
                Activo = x.activo

            });
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
        }


        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = db.SIBOACUsuarios.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
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
        public ActionResult Create([Bind(Include = " Id, Nombre, Usuario, Contrasena, Email, codigo, Activo ")] SIBOACUsuarios sIBOACUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.SIBOACUsuarios.Add(sIBOACUsuarios);
                string mensaje = Verificar(sIBOACUsuarios.Id);
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
                    return View(sIBOACUsuarios);
                }
            }

            return View(sIBOACUsuarios);
        }

        // GET: MantenimientoUsuarios/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: MantenimientoUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Contrasena,Nombre,Usuario")] SIBOACUsuarios sIBOACUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sIBOACUsuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
            if (sIBOACUsuarios.Activo == false)

                sIBOACUsuarios.Activo = true;
            else
                sIBOACUsuarios.Activo = false;
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
