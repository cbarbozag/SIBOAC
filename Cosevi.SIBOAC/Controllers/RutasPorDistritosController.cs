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
    public class RutasPorDistritosController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: RutasPorDistritos
        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View(db.RUTASXDISTRITO.ToList());
        }

        // GET: RutasPorDistritos/Details/5
        public ActionResult Details(int? codigo_distrito, int ? codigo_ruta, int? km)
        {
            if (codigo_distrito == null|| codigo_ruta ==null || km==null)
            {
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RutasPorDistritos rutasPorDistritos = db.RUTASXDISTRITO.Find(codigo_distrito, codigo_ruta, km);
            if (rutasPorDistritos == null)
            {
                return HttpNotFound();
            }
            return View(rutasPorDistritos);
        }

        // GET: RutasPorDistritos/Create
        public ActionResult Create()
        {

            IEnumerable<SelectListItem> itemsDistritos = db.DISTRITO
            .Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Descripcion
            });
            ViewBag.ComboDistrito = itemsDistritos;

            IEnumerable<SelectListItem> itemsRuta = db.Ruta
           .Select(o => new SelectListItem
           {
               Value = o.Id.ToString(),
               Text = o.Inicia +"|"+ o.Termina
           });
            ViewBag.ComboRuta = itemsRuta;
            return View();
        }

        public string Verificar(int? codigo_distrito, int? codigo_ruta, int? km)
        {
            string mensaje = "";
            bool exist = db.RUTASXDISTRITO.Any(x => x.CodigoDistrito == codigo_distrito
                                                &&x.CodigoRuta == codigo_ruta
                                                &&x.Km==km);
            if (exist)
            {
                mensaje = "El codigo de distrito" +codigo_distrito + 
                     ", Código ruta "+ codigo_ruta+
                     ", kilometro "+ km+        
                    " ya esta registrado";
            }
            return mensaje;
        }

        // POST: RutasPorDistritos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoDistrito,CodigoRuta,Km,Estado,FechaDeInicio,FechaDeFin")] RutasPorDistritos rutasPorDistritos)
        {
            if (ModelState.IsValid)
            {
                db.RUTASXDISTRITO.Add(rutasPorDistritos);
                string mensaje = Verificar(rutasPorDistritos.CodigoDistrito, rutasPorDistritos.CodigoRuta, rutasPorDistritos.Km);
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
                    return View(rutasPorDistritos);
                }
            }

            return View(rutasPorDistritos);
        }

        // GET: RutasPorDistritos/Edit/5
        public ActionResult Edit(int? codigo_distrito, int? codigo_ruta, int? km)
        {
            if (codigo_distrito == null || codigo_ruta == null || km == null)
            {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
            RutasPorDistritos rutasPorDistritos = db.RUTASXDISTRITO.Find(codigo_distrito, codigo_ruta, km);
            if (rutasPorDistritos == null)
            {
                return HttpNotFound();
            }
            ViewBag.ComboDistrito = new SelectList(db.DISTRITO.OrderBy(x => x.Descripcion), "Id", "Descripcion", codigo_distrito);
            ViewBag.ComboRuta = new SelectList(db.Ruta.OrderBy(x => x.Id), "Id", "Inicio" + " " + "Termina", codigo_ruta);
            return View(rutasPorDistritos);
        }

        // POST: RutasPorDistritos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoDistrito,CodigoRuta,Km,Estado,FechaDeInicio,FechaDeFin")] RutasPorDistritos rutasPorDistritos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rutasPorDistritos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rutasPorDistritos);
        }

        // GET: RutasPorDistritos/Delete/5
        public ActionResult Delete(int? codigo_distrito, int? codigo_ruta, int? km)
        {
            if (codigo_distrito == null || codigo_ruta == null || km == null)
             {
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
                RutasPorDistritos rutasPorDistritos = db.RUTASXDISTRITO.Find(codigo_distrito, codigo_ruta, km);
            if (rutasPorDistritos == null)
            {
                return HttpNotFound();
            }
            return View(rutasPorDistritos);
        }

        // POST: RutasPorDistritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? codigo_distrito, int? codigo_ruta, int? km)
        {
            RutasPorDistritos rutasPorDistritos = db.RUTASXDISTRITO.Find(codigo_distrito, codigo_ruta, km);
            if (rutasPorDistritos.Estado == "A")
                rutasPorDistritos.Estado = "I";
            else
                rutasPorDistritos.Estado = "A";
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
