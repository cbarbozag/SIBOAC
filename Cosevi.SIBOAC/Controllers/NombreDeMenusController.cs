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
    public class NombreDeMenusController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: NombreDeMenus
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.Nombre_Menu.ToList());
        }

        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.Nombre_Menu.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El codigo " + id + " ya esta registrado";
            }
            return mensaje;
        }

        // GET: NombreDeMenus/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NombreDeMenu nombreDeMenu = db.Nombre_Menu.Find(id);
            if (nombreDeMenu == null)
            {
                return HttpNotFound();
            }
            return View(nombreDeMenu);
        }

        // GET: NombreDeMenus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NombreDeMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NombreDeMenuMovil,Estado")] NombreDeMenu nombreDeMenu)
        {
            if (ModelState.IsValid)
            {
                db.Nombre_Menu.Add(nombreDeMenu);
                string mensaje = Verificar(nombreDeMenu.Id);
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
                    return View(nombreDeMenu);
                }
            }

            return View(nombreDeMenu);
        }

        // GET: NombreDeMenus/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NombreDeMenu nombreDeMenu = db.Nombre_Menu.Find(id);
            if (nombreDeMenu == null)
            {
                return HttpNotFound();
            }
            return View(nombreDeMenu);
        }

        // POST: NombreDeMenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NombreDeMenuMovil,Estado")] NombreDeMenu nombreDeMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nombreDeMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nombreDeMenu);
        }

        // GET: NombreDeMenus/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NombreDeMenu nombreDeMenu = db.Nombre_Menu.Find(id);
            if (nombreDeMenu == null)
            {
                return HttpNotFound();
            }
            return View(nombreDeMenu);
        }

        // POST: NombreDeMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NombreDeMenu nombreDeMenu = db.Nombre_Menu.Find(id);
            if (nombreDeMenu.Estado == "I")
                nombreDeMenu.Estado = "A";
            else
                nombreDeMenu.Estado = "I";
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
