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
    public class MantenimientoRolesController : BaseController<SIBOACRoles>
    {
        private SIBOACSecurityEntities dbs = new SIBOACSecurityEntities();

        // GET: MantenimientoRoles
        [SessionExpire]
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            return View(dbs.SIBOACRoles.ToList());
        }

        public string Verificar(int id)
        {
            string mensaje = "";
            bool exist = dbs.SIBOACRoles.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: MantenimientoRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACRoles sIBOACRoles = dbs.SIBOACRoles.Find(id);
            if (sIBOACRoles == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACRoles);
        }

        // GET: MantenimientoRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MantenimientoRoles/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Activo")] SIBOACRoles sIBOACRoles)
        {
            if (ModelState.IsValid)
            {
                dbs.SIBOACRoles.Add(sIBOACRoles);
                string mensaje = Verificar(sIBOACRoles.Id);

                if (mensaje == "")
                {
                    dbs.SaveChanges();
                    Bitacora(sIBOACRoles, "I", "SIBOACRoles");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    return View(sIBOACRoles);
                }                
            }

            return View(sIBOACRoles);
        }

        // GET: PRUEBARoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACRoles sIBOACRoles = dbs.SIBOACRoles.Find(id);
            if (sIBOACRoles == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACRoles);
        }

        // POST: PRUEBARoles/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Activo")] SIBOACRoles sIBOACRoles)
        {            
            if (ModelState.IsValid)
            {
                var sIBOACRolesAntes = dbs.SIBOACRoles.AsNoTracking().Where(d => d.Id == sIBOACRoles.Id).FirstOrDefault();
                dbs.Entry(sIBOACRoles).State = EntityState.Modified;                
                dbs.SaveChanges();
                Bitacora(sIBOACRoles, "U", "SIBOACRoles", sIBOACRolesAntes);
                TempData["Type"] = "info";
                TempData["Message"] = "La edición se realizó correctamente";
                return RedirectToAction("Index");
            }
            return View(sIBOACRoles);
        }

        // GET: MantenimientoRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACRoles sIBOACRoles = dbs.SIBOACRoles.Find(id);
            if (sIBOACRoles == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACRoles);
        }

        // POST: MantenimientoRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACRoles sIBOACRoles = dbs.SIBOACRoles.Find(id);
            SIBOACRoles sIBOACRolesAntes = ObtenerCopia(sIBOACRoles);

            if (sIBOACRoles.Activo == false)

                sIBOACRoles.Activo = true;
            else
                sIBOACRoles.Activo = false;
            dbs.SaveChanges();
            Bitacora(sIBOACRoles,"U", "SIBOACRoles", sIBOACRolesAntes);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbs.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
