using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;
using System.Security.Claims;

namespace Cosevi.SIBOAC.Controllers
{
    public class SIBOACMenuOpciones1Controller : Controller
    {
        private SIBOACSecurityEntities db = new SIBOACSecurityEntities();

        // GET: SIBOACMenuOpciones1
        public ActionResult Index()
        {
            return View(db.SIBOACMenuOpciones.ToList());
        }

        // GET: SIBOACMenuOpciones1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACMenuOpciones sIBOACMenuOpciones = db.SIBOACMenuOpciones.Find(id);
            if (sIBOACMenuOpciones == null)
            {
                return HttpNotFound();
            }
            var listaRoles = (from r in db.SIBOACRoles select new { r.Id, r.Nombre });
            var ListaRolesActuales = sIBOACMenuOpciones.SIBOACRoles.Select(a => new { a.Id, a.Nombre });
            List<SelectListItem> ListaCheckbox = new List<SelectListItem>();
            foreach (var item in listaRoles)
            {
                SelectListItem dato = new SelectListItem();
                foreach (var i in ListaRolesActuales)
                {
                    if (item.Id == i.Id)
                    {
                        dato.Selected = true;
                        dato.Value = i.Id.ToString();
                        dato.Text = i.Nombre;
                        ListaCheckbox.Add(dato);
                    }
                       
                }

                if(!ListaCheckbox.Contains(dato))
                    ListaCheckbox.Add(new SelectListItem { Selected =false, Value = item.Id.ToString(), Text= item.Nombre});
            }
            ViewBag.ListaMostrar = ListaCheckbox;
            return View(sIBOACMenuOpciones);
        }

        // GET: SIBOACMenuOpciones1/Create
        public ActionResult Create()
        {
            var listaRoles = (from r in db.SIBOACRoles select new { r.Id, r.Nombre });
            List<SelectListItem> ListaCheckbox = new List<SelectListItem>();
            foreach (var item in listaRoles)
            {
               ListaCheckbox.Add(new SelectListItem { Selected = false, Value = item.Id.ToString(), Text = item.Nombre });
            }
            ViewBag.ListaMostrar = ListaCheckbox;
            return View();
        }

        // POST: SIBOACMenuOpciones1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuOpcionesID,Descripcion,URL,Estado,ParentID,Orden")] SIBOACMenuOpciones sIBOACMenuOpciones)
        {
            if (ModelState.IsValid)
            {
                var query_where2 = from a in db.SIBOACRoles
                                   select a;
                foreach (var i in query_where2)
                {
                    sIBOACMenuOpciones.SIBOACRoles.Add(i);
                }

                db.SIBOACMenuOpciones.Add(sIBOACMenuOpciones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sIBOACMenuOpciones);
        }

        // GET: SIBOACMenuOpciones1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACMenuOpciones sIBOACMenuOpciones = db.SIBOACMenuOpciones.Find(id);
            if (sIBOACMenuOpciones == null)
            {
                return HttpNotFound();
            }
            var listaRoles = (from r in db.SIBOACRoles select new { r.Id, r.Nombre });
            var ListaRolesActuales = sIBOACMenuOpciones.SIBOACRoles.Select(a => new { a.Id, a.Nombre });
            List<SelectListItem> ListaCheckbox = new List<SelectListItem>();
            foreach (var item in listaRoles)
            {
                SelectListItem dato = new SelectListItem();
                foreach (var i in ListaRolesActuales)
                {
                    if (item.Id == i.Id)
                    {
                        dato.Selected = true;
                        dato.Value = i.Id.ToString();
                        dato.Text = i.Nombre;
                        ListaCheckbox.Add(dato);
                    }

                }

                if (!ListaCheckbox.Contains(dato))
                    ListaCheckbox.Add(new SelectListItem { Selected = false, Value = item.Id.ToString(), Text = item.Nombre });
            }


            ViewBag.ListaMostrar = ListaCheckbox;

            return View(sIBOACMenuOpciones);
        }

        // POST: SIBOACMenuOpciones1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, int MenuOpcionesID, string Descripcion, string URL, bool Estado, int ParentID, int Orden, [System.Web.Http.FromUri] string[] SIBOACRoles) //[Bind(Include = "MenuOpcionesID,Descripcion,URL,Estado,ParentID,Orden")] SIBOACMenuOpciones sIBOACMenuOpciones
        {
            SIBOACMenuOpciones sIBOACMenuOpciones = new SIBOACMenuOpciones();
            if (ModelState.IsValid)
            {
                sIBOACMenuOpciones = db.SIBOACMenuOpciones.Find(id);
                if (sIBOACMenuOpciones == null)
                {
                    return HttpNotFound();
                }
                sIBOACMenuOpciones.Estado = Estado;
                sIBOACMenuOpciones.Descripcion =Descripcion;
                sIBOACMenuOpciones.URL  = URL;
                sIBOACMenuOpciones.ParentID  = ParentID;
                sIBOACMenuOpciones.Orden  = Orden;
                var rolesTem = sIBOACMenuOpciones.SIBOACRoles;

                 var query_where2 = from a in db.SIBOACRoles.Where (t =>SIBOACRoles.Contains(t.Id.ToString()))
                                   select a;


                //if (query_where2.Count() == 0)
                //{
                //    sIBOACMenuOpciones.SIBOACRoles.Clear();
                //}
                //if (rolesTem.Count == 0 && query_where2.Count() > 0)
                //{
                //    foreach (var j in query_where2)
                //    {
                //        sIBOACMenuOpciones.SIBOACRoles.Add(j);
                //    }
                //}
       
                           
                    for(int i=0; i<rolesTem.Count; i++)
                    {
                    if (query_where2.ToArray().Count() == 0)
                    {
                        sIBOACMenuOpciones.SIBOACRoles.Remove(rolesTem.ElementAt(i));
                        i--;
                    }
                    else
                    {
                        if (query_where2.ToArray().Where(a => a.Id == rolesTem.ElementAt(i).Id).Count() == 0)
                        {
                            sIBOACMenuOpciones.SIBOACRoles.Remove(rolesTem.ElementAt(i));
                            i--;
                        }
                    }
                                          

                    }
                for (int i = 0; i < query_where2.ToArray().Count(); i++)
                {
                    if(rolesTem.Count()==0)
                        sIBOACMenuOpciones.SIBOACRoles.Add(query_where2.ToArray().ElementAt(i));
                    else
                    {
                        if (rolesTem.Where(a => a.Id == query_where2.ToArray().ElementAt(i).Id).Count()==0)                        
                        {
                            sIBOACMenuOpciones.SIBOACRoles.Add(query_where2.ToArray().ElementAt(i));
                        }

                    }
                    
                }



                db.Entry(sIBOACMenuOpciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(sIBOACMenuOpciones);
        }

        // GET: SIBOACMenuOpciones1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACMenuOpciones sIBOACMenuOpciones = db.SIBOACMenuOpciones.Find(id);
            if (sIBOACMenuOpciones == null)
            {
                return HttpNotFound();
            }
            return View(sIBOACMenuOpciones);
        }

        // POST: SIBOACMenuOpciones1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACMenuOpciones sIBOACMenuOpciones = db.SIBOACMenuOpciones.Find(id);
            db.SIBOACMenuOpciones.Remove(sIBOACMenuOpciones);
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
