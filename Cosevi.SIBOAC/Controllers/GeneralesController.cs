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
    public class GeneralesController : BaseController<GENERALES>
    {
        

        // GET: Generales
        public ActionResult Index(int? page)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var list =
           (
              from t in db.GENERALES
              join ti in db.INSPECTOR on new { Id = t.Inspector } equals new { Id = ti.Id }             
              select new
              {
                  t.Inspector,
                  ti.Nombre,
                  t.ConsecutivoBC,
                  t.ConsecutivoPO,
                  t.HandHeld,
                  t.Copias,
                  t.Piepagina,
                  t.ClaveIns,
                  t.ClaveAdmin,
                  t.Serie,
                  t.ConsecutivoSinDoc,
                  t.estado,
                  t.fecha_inicio,
                  t.fecha_fin,
                  t.ConsecutivoNumeroMarco,
                  t.PasswordActualizado
              }).ToList()

            .Select(x => new GENERALES
            {
                Inspector=x.Inspector,
                Nombre =  x.Nombre,
                ConsecutivoBC =x.ConsecutivoBC,
                ConsecutivoPO =  x.ConsecutivoPO,
                HandHeld= x.HandHeld,
                Copias= x.Copias,
                Piepagina=  x.Piepagina,
                ClaveIns= x.ClaveIns,
                ClaveAdmin=  x.ClaveAdmin,
                Serie=  x.Serie,
                ConsecutivoSinDoc= x.ConsecutivoSinDoc,
                estado= x.estado,
                fecha_inicio=  x.fecha_inicio,
                fecha_fin= x.fecha_fin,
                ConsecutivoNumeroMarco=  x.ConsecutivoNumeroMarco,
                PasswordActualizado=  x.PasswordActualizado

            });                    
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
           
        }

        // GET: Generales/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            var list =
            (
            from t in db.GENERALES
            join ti in db.INSPECTOR on new { Id = t.Inspector } equals new { Id = ti.Id }
            where t.Inspector == id
            select new
            {
                t.Inspector,
                ti.Nombre,
                t.ConsecutivoBC,
                t.ConsecutivoPO,
                t.HandHeld,
                t.Copias,
                t.Piepagina,
                t.ClaveIns,
                t.ClaveAdmin,
                t.Serie,
                t.ConsecutivoSinDoc,
                t.estado,
                t.fecha_inicio,
                t.fecha_fin,
                t.ConsecutivoNumeroMarco,
                t.PasswordActualizado
            }).ToList()

          .Select(x => new GENERALES
          {
              Inspector = x.Inspector,
              Nombre = x.Nombre,
              ConsecutivoBC = x.ConsecutivoBC,
              ConsecutivoPO = x.ConsecutivoPO,
              HandHeld = x.HandHeld,
              Copias = x.Copias,
              Piepagina = x.Piepagina,
              ClaveIns = x.ClaveIns,
              ClaveAdmin = x.ClaveAdmin,
              Serie = x.Serie,
              ConsecutivoSinDoc = x.ConsecutivoSinDoc,
              estado = x.estado,
              fecha_inicio = x.fecha_inicio,
              fecha_fin = x.fecha_fin,
              ConsecutivoNumeroMarco = x.ConsecutivoNumeroMarco,
              PasswordActualizado = x.PasswordActualizado

          }).SingleOrDefault();
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // GET: Generales/Create
        public ActionResult Create()
        {
            //se llenan los combos
            IEnumerable<SelectListItem> itemsInspectores = db.INSPECTOR
              .Select(o => new SelectListItem
              {
                  Value = o.Id.Trim(),
                  Text = o.Nombre
              });
            ViewBag.ComboInspectores = itemsInspectores;
            return View();
        }

        // POST: Generales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Inspector,ConsecutivoBC,ConsecutivoPO,HandHeld,Copias,Piepagina,ClaveIns,ClaveAdmin,Serie,ConsecutivoSinDoc,estado,fecha_inicio,fecha_fin,ConsecutivoNumeroMarco,PasswordActualizado")] GENERALES gENERALES)
        {
            if (ModelState.IsValid)
            {
                db.GENERALES.Add(gENERALES);
                string mensaje = Verificar(gENERALES.Inspector);
                if (mensaje == "")
                {
                    db.SaveChanges();
                    Bitacora(gENERALES, "I", "GENERALES");

                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");

                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    IEnumerable<SelectListItem> itemsInspectores = db.INSPECTOR
                    .Select(o => new SelectListItem
                    {
                        Value = o.Id.Trim(),
                        Text = o.Nombre
                    });
                    ViewBag.ComboInspectores = itemsInspectores;
                    return View(gENERALES);
                }
            }

            return View(gENERALES);
        }

        // GET: Generales/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
            (
            from t in db.GENERALES
            join ti in db.INSPECTOR on new { Id = t.Inspector } equals new { Id = ti.Id }
            where t.Inspector == id
            select new
            {
                t.Inspector,
                ti.Nombre,
                t.ConsecutivoBC,
                t.ConsecutivoPO,
                t.HandHeld,
                t.Copias,
                t.Piepagina,
                t.ClaveIns,
                t.ClaveAdmin,
                t.Serie,
                t.ConsecutivoSinDoc,
                t.estado,
                t.fecha_inicio,
                t.fecha_fin,
                t.ConsecutivoNumeroMarco,
                t.PasswordActualizado
            }).ToList()

          .Select(x => new GENERALES
          {
              Inspector = x.Inspector,
              Nombre = x.Nombre,
              ConsecutivoBC = x.ConsecutivoBC,
              ConsecutivoPO = x.ConsecutivoPO,
              HandHeld = x.HandHeld,
              Copias = x.Copias,
              Piepagina = x.Piepagina,
              ClaveIns = x.ClaveIns,
              ClaveAdmin = x.ClaveAdmin,
              Serie = x.Serie,
              ConsecutivoSinDoc = x.ConsecutivoSinDoc,
              estado = x.estado,
              fecha_inicio = x.fecha_inicio,
              fecha_fin = x.fecha_fin,
              ConsecutivoNumeroMarco = x.ConsecutivoNumeroMarco,
              PasswordActualizado = x.PasswordActualizado

          }).SingleOrDefault();
            if (list == null)
            {
                return HttpNotFound();
            }

            ViewBag.ComboInspectores = new SelectList(db.INSPECTOR.OrderBy(x => x.Nombre), "Id", "Nombre", id);

            return View(list);
        }

        // POST: Generales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Inspector,ConsecutivoBC,ConsecutivoPO,HandHeld,Copias,Piepagina,ClaveIns,ClaveAdmin,Serie,ConsecutivoSinDoc,estado,fecha_inicio,fecha_fin,ConsecutivoNumeroMarco,PasswordActualizado")] GENERALES gENERALES)
        {
            if (ModelState.IsValid)
            {
                var gENERALESAntes = db.GENERALES.AsNoTracking().Where(d => d.Inspector == gENERALES.Inspector).FirstOrDefault();
                db.Entry(gENERALES).State = EntityState.Modified;
                db.SaveChanges();
                Bitacora(gENERALES, "U", "GENERALES", gENERALESAntes);
                return RedirectToAction("Index");
            }
            return View(gENERALES);
        }

        // GET: Generales/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
           (
           from t in db.GENERALES
           join ti in db.INSPECTOR on new { Id = t.Inspector } equals new { Id = ti.Id }
           where t.Inspector == id
           select new
           {
               t.Inspector,
               ti.Nombre,
               t.ConsecutivoBC,
               t.ConsecutivoPO,
               t.HandHeld,
               t.Copias,
               t.Piepagina,
               t.ClaveIns,
               t.ClaveAdmin,
               t.Serie,
               t.ConsecutivoSinDoc,
               t.estado,
               t.fecha_inicio,
               t.fecha_fin,
               t.ConsecutivoNumeroMarco,
               t.PasswordActualizado
           }).ToList()

         .Select(x => new GENERALES
         {
             Inspector = x.Inspector,
             Nombre = x.Nombre,
             ConsecutivoBC = x.ConsecutivoBC,
             ConsecutivoPO = x.ConsecutivoPO,
             HandHeld = x.HandHeld,
             Copias = x.Copias,
             Piepagina = x.Piepagina,
             ClaveIns = x.ClaveIns,
             ClaveAdmin = x.ClaveAdmin,
             Serie = x.Serie,
             ConsecutivoSinDoc = x.ConsecutivoSinDoc,
             estado = x.estado,
             fecha_inicio = x.fecha_inicio,
             fecha_fin = x.fecha_fin,
             ConsecutivoNumeroMarco = x.ConsecutivoNumeroMarco,
             PasswordActualizado = x.PasswordActualizado

         }).SingleOrDefault();
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: Generales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GENERALES gENERALES = db.GENERALES.Find(id);
            
            db.GENERALES.Remove(gENERALES);
            db.SaveChanges();
            Bitacora(gENERALES, "D", "GENERALES");
            return RedirectToAction("Index");
        }
        // GET: Generales/Inactivar/5
        public ActionResult Inactivar(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list =
          (
          from t in db.GENERALES
          join ti in db.INSPECTOR on new { Id = t.Inspector } equals new { Id = ti.Id }
          where t.Inspector == id
          select new
          {
              t.Inspector,
              ti.Nombre,
              t.ConsecutivoBC,
              t.ConsecutivoPO,
              t.HandHeld,
              t.Copias,
              t.Piepagina,
              t.ClaveIns,
              t.ClaveAdmin,
              t.Serie,
              t.ConsecutivoSinDoc,
              t.estado,
              t.fecha_inicio,
              t.fecha_fin,
              t.ConsecutivoNumeroMarco,
              t.PasswordActualizado
          }).ToList()

        .Select(x => new GENERALES
        {
            Inspector = x.Inspector,
            Nombre = x.Nombre,
            ConsecutivoBC = x.ConsecutivoBC,
            ConsecutivoPO = x.ConsecutivoPO,
            HandHeld = x.HandHeld,
            Copias = x.Copias,
            Piepagina = x.Piepagina,
            ClaveIns = x.ClaveIns,
            ClaveAdmin = x.ClaveAdmin,
            Serie = x.Serie,
            ConsecutivoSinDoc = x.ConsecutivoSinDoc,
            estado = x.estado,
            fecha_inicio = x.fecha_inicio,
            fecha_fin = x.fecha_fin,
            ConsecutivoNumeroMarco = x.ConsecutivoNumeroMarco,
            PasswordActualizado = x.PasswordActualizado

        }).SingleOrDefault();
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: Generales/Delete/5
        [HttpPost, ActionName("Inactivar")]
        [ValidateAntiForgeryToken]
        public ActionResult InactivarConfirmed(string id)
        {
            GENERALES gENERALES = db.GENERALES.Find(id);
            GENERALES gENERALESAntes = ObtenerCopia(gENERALES);
            if (gENERALES.estado == "I")
            {
                gENERALES.estado = "A";
            }
            else
            {
                gENERALES.estado = "I";
            }           
            db.SaveChanges();
            Bitacora(gENERALES, "U", "GENERALES", gENERALESAntes);

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

        public string Verificar(string Id)
        {
            string mensaje = "";
            bool exist = db.INSPECTOR.Any(x => x.Id == Id);
            if (exist)
            {
                mensaje = "El registro con los siguientes datos ya se encuentra registrado:" +
                           " código inspector" + Id;

            }
            return mensaje;
        }
    }
}
