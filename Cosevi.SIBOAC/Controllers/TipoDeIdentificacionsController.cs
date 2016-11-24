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
    public class TipoDeIdentificacionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: TipoDeIdentificacions
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.TIPO_IDENTIFICACION.ToList());
        }


        public string Verificar(string id)
        {
            string mensaje = "";
            bool exist = db.TIPO_IDENTIFICACION.Any(x => x.Id == id);
            if (exist)
            {
                mensaje = "El código " + id + " ya esta registrado";
            }
            return mensaje;
        }


        // GET: TipoDeIdentificacions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeIdentificacion tipoDeIdentificacion = db.TIPO_IDENTIFICACION.Find(id);
            if (tipoDeIdentificacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeIdentificacion);
        }

        // GET: TipoDeIdentificacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDeIdentificacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Indice")] TipoDeIdentificacion tipoDeIdentificacion)
        {
            if (ModelState.IsValid)
            {
                db.TIPO_IDENTIFICACION.Add(tipoDeIdentificacion);
                string mensaje = Verificar(tipoDeIdentificacion.Id);
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
                    return View(tipoDeIdentificacion);
                }
            }

            return View(tipoDeIdentificacion);
        }

        // GET: TipoDeIdentificacions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDeIdentificacion tipoDeIdentificacion = db.TIPO_IDENTIFICACION.Find(id);
            if (tipoDeIdentificacion == null)
            {
                return HttpNotFound();
            }
            return View(tipoDeIdentificacion);
        }

        // POST: TipoDeIdentificacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Indice")] TipoDeIdentificacion tipoDeIdentificacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDeIdentificacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDeIdentificacion);
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
