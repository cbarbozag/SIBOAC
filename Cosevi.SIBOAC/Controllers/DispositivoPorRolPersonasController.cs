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
    public class DispositivoPorRolPersonasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: DispositivoPorRolPersonas
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.DISPXROLPERSONA.ToList());
        }

        // GET: DispositivoPorRolPersonas/Details/5
        public ActionResult Details(string CodRol, int? CodDisp)
        {
            if (CodRol == null|| CodDisp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivoPorRolPersona dispositivoPorRolPersona = db.DISPXROLPERSONA.Find(CodRol, CodDisp);
            if (dispositivoPorRolPersona == null)
            {
                return HttpNotFound();
            }
            return View(dispositivoPorRolPersona);
        }

        // GET: DispositivoPorRolPersonas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DispositivoPorRolPersonas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoRolPersona,CodigoDispositivo")] DispositivoPorRolPersona dispositivoPorRolPersona)
        {
            if (ModelState.IsValid)
            {
                db.DISPXROLPERSONA.Add(dispositivoPorRolPersona);
                string mensaje = Verificar(dispositivoPorRolPersona.DescripcionRolPersona,
                                            dispositivoPorRolPersona.CodigoDispositivo);
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
                    return View(dispositivoPorRolPersona);
                }
            }

            return View(dispositivoPorRolPersona);
        }

        // GET: DispositivoPorRolPersonas/Edit/5
        public ActionResult Edit(string CodRol, int? CodDisp)
        {
            if (CodRol == null|| CodDisp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DispositivoPorRolPersona dispositivoPorRolPersona = db.DISPXROLPERSONA.Find(CodRol, CodDisp);
            if (dispositivoPorRolPersona == null)
            {
                return HttpNotFound();
            }


            return View(dispositivoPorRolPersona);
        }

        // POST: DispositivoPorRolPersonas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoRolPersona,CodigoDispositivo")] DispositivoPorRolPersona dispositivoPorRolPersona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dispositivoPorRolPersona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dispositivoPorRolPersona);
        }

     


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string Verificar(string CodRol, int CodDisp)
        {
            string mensaje = "";
            bool exist = db.DISPXROLPERSONA.Any(x => x.CodigoRolPersona == CodRol
                                                    && x.CodigoDispositivo == CodDisp);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrados:" +
                           " código de Rol Persona" + CodRol +
                           ", código de Dispositivo" + CodDisp;

            }
            return mensaje;
        }
    }
}
