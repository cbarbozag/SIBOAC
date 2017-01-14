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
    public class ResetPasswordsController : BaseController<SIBOACUsuarios>
    {
        

        // GET: ResetPasswords
        public ActionResult Index()
        {
            return View(db.SIBOACUsuarios.ToList());
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
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
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
            SIBOACUsuarios sIBOACUsuarios = db.SIBOACUsuarios.Find(id);
            SIBOACUsuarios sIBOACUsuariosAntes = ObtenerCopia(sIBOACUsuarios);
            var usuario = sIBOACUsuarios.Usuario;
            sIBOACUsuarios.Contrasena = usuario;
            sIBOACUsuarios.FechaDeActualizacionClave = DateTime.Now;
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
