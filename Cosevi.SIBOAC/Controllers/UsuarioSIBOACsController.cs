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
    public class UsuarioSIBOACsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: UsuarioSIBOACs
        public ActionResult Index()
        {
            return View(db.UsuarioSIBOAC.ToList());
        }

        // GET: UsuarioSIBOACs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioSIBOAC usuarioSIBOAC = db.UsuarioSIBOAC.Find(id);
            if (usuarioSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(usuarioSIBOAC);
        }

        // GET: UsuarioSIBOACs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuarioSIBOACs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CodigoUsuario,Nombre,Estado,Clave,FechaDeCreacion,FechaDeUltimoCambio")] UsuarioSIBOAC usuarioSIBOAC)
        {
            if (ModelState.IsValid)
            {
                db.UsuarioSIBOAC.Add(usuarioSIBOAC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuarioSIBOAC);
        }

        // GET: UsuarioSIBOACs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioSIBOAC usuarioSIBOAC = db.UsuarioSIBOAC.Find(id);
            if (usuarioSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(usuarioSIBOAC);
        }

        // POST: UsuarioSIBOACs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CodigoUsuario,Nombre,Estado,Clave,FechaDeCreacion,FechaDeUltimoCambio")] UsuarioSIBOAC usuarioSIBOAC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarioSIBOAC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuarioSIBOAC);
        }

        // GET: UsuarioSIBOACs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioSIBOAC usuarioSIBOAC = db.UsuarioSIBOAC.Find(id);
            if (usuarioSIBOAC == null)
            {
                return HttpNotFound();
            }
            return View(usuarioSIBOAC);
        }

        // POST: UsuarioSIBOACs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UsuarioSIBOAC usuarioSIBOAC = db.UsuarioSIBOAC.Find(id);
            db.UsuarioSIBOAC.Remove(usuarioSIBOAC);
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
