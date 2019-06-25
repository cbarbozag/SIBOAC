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
    public class ResetPasswordsController : BaseController<SIBOACUsuarios>
    {
        private SIBOACSecurityEntities dbSecurity = new SIBOACSecurityEntities();


        // GET: ResetPasswords
        [SessionExpire]
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list = from s in dbSecurity.SIBOACUsuarios.ToList() select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.Usuario.ToUpper().Contains(searchString.ToUpper())
                                        || s.Nombre.ToUpper().Contains(searchString.ToUpper()));
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));

            //return View(dbSecurity.SIBOACUsuarios.ToList());
        }

        //// GET: ResetPasswords/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
        //    if (sIBOACUsuarios == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sIBOACUsuarios);
        //}

        //// GET: ResetPasswords/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ResetPasswords/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Usuario,Email,Contrasena,Nombre,codigo,FechaDeActualizacionClave,Activo")] SIBOACUsuarios sIBOACUsuarios)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SIBOACUsuarios.Add(sIBOACUsuarios);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(sIBOACUsuarios);
        //}

        //// GET: ResetPasswords/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
        //    if (sIBOACUsuarios == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sIBOACUsuarios);
        //}

        //// POST: ResetPasswords/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Usuario,Email,Contrasena,Nombrecodigo,FechaDeActualizacionClave,Activo")] SIBOACUsuarios sIBOACUsuarios)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(sIBOACUsuarios).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(sIBOACUsuarios);
        //}

        // GET: ResetPasswords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = dbSecurity.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACUsuarios);
        }

        // POST: ResetPasswords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACUsuarios sIBOACUsuarios = new SIBOACUsuarios();

            var sIBOACUsuariosAntes = dbSecurity.SIBOACUsuarios.AsNoTracking().Where(d => d.Id == id).FirstOrDefault();

            //dbSecurity.SIBOACUsuarios.Find(id);

            sIBOACUsuarios = dbSecurity.SIBOACUsuarios.Find(id);
            string usuario = sIBOACUsuarios.Usuario;
            sIBOACUsuarios.Contrasena = usuario;
            sIBOACUsuarios.FechaDeActualizacionClave = DateTime.Now;
            dbSecurity.SaveChanges();

            Bitacora(sIBOACUsuarios, "U", "SIBOACUsuarios", sIBOACUsuariosAntes);
            TempData["Type"] = "success";
            TempData["Message"] = "La contraseña se reinició correctamente";
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
