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
    public class AlineacionHorizontalsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: AlineacionHorizontals
        public ActionResult Index()
        {
            return View(db.ALINHORI.ToList());
        }
        public string  Verificar(int id)
        {
            string mensaje = "";
           List<AlineacionHorizontal> alineacionTem = db.ALINHORI.ToList();
            for (int i = 0; i < alineacionTem.Count; i++)
            {
                if (alineacionTem[i].Id == id)
                {
                    mensaje = "El codigo " + id+" ya esta registrado";
                    return mensaje;
                }
            }
            return "";
        }

        // GET: AlineacionHorizontals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            if (alineacionHorizontal == null)
            {
                return HttpNotFound();
            }
            return View(alineacionHorizontal);
        }

        // GET: AlineacionHorizontals/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AlineacionHorizontals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] AlineacionHorizontal alineacionHorizontal)
        {
            if (ModelState.IsValid)
            {
                db.ALINHORI.Add(alineacionHorizontal);
                string mensaje = Verificar(alineacionHorizontal.Id);
                if(mensaje =="")
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = mensaje;
                    return View(alineacionHorizontal);
                }
                
            }

            return View(alineacionHorizontal);
        }

        // GET: AlineacionHorizontals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            if (alineacionHorizontal == null)
            {
                return HttpNotFound();
            }
            return View(alineacionHorizontal);
        }

        // POST: AlineacionHorizontals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] AlineacionHorizontal alineacionHorizontal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alineacionHorizontal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alineacionHorizontal);
        }

        // GET: AlineacionHorizontals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            if (alineacionHorizontal == null)
            {
                return HttpNotFound();
            }
            return View(alineacionHorizontal);
        }

        // POST: AlineacionHorizontals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AlineacionHorizontal alineacionHorizontal = db.ALINHORI.Find(id);
            if(alineacionHorizontal.Estado=="A")
                alineacionHorizontal.Estado = "I";
            else
                alineacionHorizontal.Estado = "A";
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
