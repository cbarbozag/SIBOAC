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
using System.Data.Entity.Validation;
using System.Web.Security;
using System.Configuration;
using System.IO;
using ExcelDataReader;
using System.Data.SqlClient;
using System.Collections;

namespace Cosevi.SIBOAC.Controllers
{
    public class MantenimientoUsuariosController : BaseController<SIBOACUsuarios>
    {
        private SIBOACSecurityEntities dbs = new SIBOACSecurityEntities();

        string connectionString = ConfigurationManager.AppSettings["AbrirConexionBD"];

        // GET: MantenimientoUsuarios
        [SessionExpire]
        public ActionResult Index(int ? page, string searchString)
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            var list = from s in dbs.SIBOACUsuarios.ToList().OrderBy(s => s.Activo) select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.Usuario.Contains(searchString)
                                        || s.Identificacion.Contains(searchString)
                                        || s.Nombre.Contains(searchString)
                                        || s.Email.Contains(searchString)
                                        || s.codigo.Contains(searchString));
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));            
          
        }


        public string Verificar(String usuario)
        {
            string mensaje = "";
            bool exist = dbs.SIBOACUsuarios.Any(x => x.Usuario == usuario);
            if (exist)
            {
                mensaje = "El usuario " + usuario + " ya existe";
            }
            return mensaje;
        }


        // GET: MantenimientoUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = dbs.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }

            var listaRoles = (from r in dbs.SIBOACRoles select new { r.Id, r.Nombre });
            var ListaRolesActuales = sIBOACUsuarios.SIBOACRoles.Select(a => new { a.Id, a.Nombre });
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
            return View(sIBOACUsuarios);
        }

        // GET: MantenimientoUsuarios/Create
        public ActionResult Create()
        {
            var listaRoles = (from r in dbs.SIBOACRoles select new { r.Id, r.Nombre });
            List<SelectListItem> ListaCheckbox = new List<SelectListItem>();
            foreach (var item in listaRoles)
            {
                ListaCheckbox.Add(new SelectListItem { Selected = false, Value = item.Id.ToString(), Text = item.Nombre });
            }
            ViewBag.ListaMostrar = ListaCheckbox;
            return View();
        }

        // POST: MantenimientoUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = " Id, Nombre, Usuario, Identificacion, LugarTrabajo, Email, codigo, Activo ")] SIBOACUsuarios sIBOACUsuarios, [System.Web.Http.FromUri] string[] SIBOACRoles)
        {
            if (ModelState.IsValid)
            {
                                
                string mensaje = Verificar(sIBOACUsuarios.Usuario.ToString());
                if (mensaje == "")
                {
                    if (SIBOACRoles != null)
                    {
                        var query_where2 = from a in dbs.SIBOACRoles.Where(t => SIBOACRoles.Contains(t.Id.ToString()))
                                           select a;

                        foreach (var i in query_where2)
                        {
                            sIBOACUsuarios.SIBOACRoles.Add(i);
                        }
                    }

                    sIBOACUsuarios.Contrasena = sIBOACUsuarios.Usuario;
                    dbs.SIBOACUsuarios.Add(sIBOACUsuarios);

                    if (sIBOACUsuarios.codigo == null)
                    {
                        sIBOACUsuarios.codigo = "000";
                    }

                    sIBOACUsuarios.FechaDeActualizacionClave = DateTime.Now;
                    sIBOACUsuarios.UltimoIngreso = DateTime.Now;
                    dbs.SaveChanges();
                    Bitacora(sIBOACUsuarios, "I", "SIBOACUsuarios");
                    TempData["Type"] = "success";
                    TempData["Message"] = "El registro se realizó correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Type = "warning";
                    ViewBag.Message = mensaje;
                    var listaRoles = (from r in dbs.SIBOACRoles select new { r.Id, r.Nombre });
                    List<SelectListItem> ListaCheckbox = new List<SelectListItem>();
                    foreach (var item in listaRoles)
                    {
                        ListaCheckbox.Add(new SelectListItem { Selected = false, Value = item.Id.ToString(), Text = item.Nombre });
                    }
                    ViewBag.ListaMostrar = ListaCheckbox;
                    return View(sIBOACUsuarios);
                }
            }

            return View(sIBOACUsuarios);
        }

        // GET: MantenimientoUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            //var usuario = User.Identity.Name;
            //string mensaje = "No puede editar el usuario " + usuario;

                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = dbs.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }

            var listaRoles = (from r in dbs.SIBOACRoles select new { r.Id, r.Nombre });
            var ListaRolesActuales = sIBOACUsuarios.SIBOACRoles.Select(a => new { a.Id, a.Nombre });
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

            return View(sIBOACUsuarios);
        }

        // POST: MantenimientoUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( int Id, string Usuario, string Identificacion, string LugarTrabajo, string Email,string Contrasena,string Nombre,string codigo, string FechaDeActualizacionClave, string Activo, [System.Web.Http.FromUri] string[] SIBOACRoles)
        {
            SIBOACUsuarios sIBOACUsuarios = new SIBOACUsuarios();
            if (ModelState.IsValid)
                {
                    var sIBOACUsuariosAntes = dbs.SIBOACUsuarios.AsNoTracking().Where(d => d.Id == Id).FirstOrDefault();

                    sIBOACUsuarios = dbs.SIBOACUsuarios.Find(Id);               
                    sIBOACUsuarios.Usuario = Usuario;
                    sIBOACUsuarios.Identificacion = Identificacion;
                    sIBOACUsuarios.LugarTrabajo = LugarTrabajo;
                    sIBOACUsuarios.Email = Email;
                    sIBOACUsuarios.Nombre = Nombre;
                    sIBOACUsuarios.codigo = codigo ==null?null:codigo;
                    sIBOACUsuarios.FechaDeActualizacionClave = DateTime.Now;
                    sIBOACUsuarios.Contrasena = sIBOACUsuariosAntes.Contrasena;
                    sIBOACUsuarios.Activo = sIBOACUsuariosAntes.Activo;
                    var rolesTem = sIBOACUsuarios.SIBOACRoles;

                   if (SIBOACRoles == null)
                    {
                        for (int i = 0; i < rolesTem.Count; i++)
                        {                           
                                sIBOACUsuarios.SIBOACRoles.Remove(rolesTem.ElementAt(i));
                                i--;                            
                        }
                    }
                else
                {
                    var query_where2 = from a in dbs.SIBOACRoles.Where(t => SIBOACRoles.Contains(t.Id.ToString()))
                                       select a;

                    for (int i = 0; i < rolesTem.Count; i++)
                    {
                        if (query_where2 == null || query_where2.ToArray().Count() == 0)
                        {
                            sIBOACUsuarios.SIBOACRoles.Remove(rolesTem.ElementAt(i));
                            i--;

                        }
                        else
                        {
                            if (query_where2.ToArray().Where(a => a.Id == rolesTem.ElementAt(i).Id).Count() == 0)
                            {
                                sIBOACUsuarios.SIBOACRoles.Remove(rolesTem.ElementAt(i));
                                i--;

                            }
                        }


                    }
                    for (int i = 0; i < query_where2.ToArray().Count(); i++)
                    {
                        if (rolesTem.Count() == 0)
                            sIBOACUsuarios.SIBOACRoles.Add(query_where2.ToArray().ElementAt(i));
                        else
                        {
                            if (rolesTem.Where(a => a.Id == query_where2.ToArray().ElementAt(i).Id).Count() == 0)
                            {
                                sIBOACUsuarios.SIBOACRoles.Add(query_where2.ToArray().ElementAt(i));
                            }

                        }

                    }


                }


                dbs.Entry(sIBOACUsuarios).State = EntityState.Modified;

                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges

                        dbs.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }

                    Bitacora(sIBOACUsuarios, "U", "SIBOACUsuarios", sIBOACUsuariosAntes);
                    TempData["Type"] = "info";
                    TempData["Message"] = "La edición se realizó correctamente";
                    return RedirectToAction("Index");
                }

            ViewBag.IdUsuario = new SelectList(dbs.SIBOACUsuarios, "Id", "Nombre", sIBOACUsuarios.Id);
            return View(sIBOACUsuarios);
        }

        // GET: MantenimientoUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SIBOACUsuarios sIBOACUsuarios = dbs.SIBOACUsuarios.Find(id);
            if (sIBOACUsuarios == null)
            {
                return HttpNotFound();
            }

            var listaRoles = (from r in dbs.SIBOACRoles select new { r.Id, r.Nombre });
            var ListaRolesActuales = sIBOACUsuarios.SIBOACRoles.Select(a => new { a.Id, a.Nombre });
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

            return View(sIBOACUsuarios);
        }

        // POST: MantenimientoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SIBOACUsuarios sIBOACUsuarios = dbs.SIBOACUsuarios.Find(id);
            SIBOACUsuarios sIBOACUsuariosAntes = ObtenerCopia(sIBOACUsuarios);
            if (sIBOACUsuarios.Activo == false)

                sIBOACUsuarios.Activo = true;
            else
                sIBOACUsuarios.Activo = false;
            dbs.SaveChanges();
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

        [HttpPost]
        public ActionResult UploadFiles(SIBOACUsuarios sIBOACUsuarios)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        //    MemoryStream excelStream = new MemoryStream();
                        //   file.InputStream.CopyTo(excelStream);

                        Stream stream = file.InputStream;

                        IExcelDataReader reader = null;

                        if (file.FileName.EndsWith(".xls"))
                        {
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);

                        }
                        else if(file.FileName.EndsWith(".xlsx"))
                        {
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View();
                        }

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };

                        SqlConnection connection = new SqlConnection(connectionString);
                        connection.Open();

                        var dataSet = reader.AsDataSet(conf);
                        var dataTable = dataSet.Tables[0];

                        int row = 0;
                        int col = 0;

                        for (var k = row; k < dataTable.Rows.Count; k++)
                        {
                            ArrayList usuarioP = new ArrayList();
                            for (var j = col; j < dataTable.Columns.Count; j++)
                            {
                                usuarioP.Add(dataTable.Rows[k][j]);
                            }

                            string mensajePlantilla = "";
                            string usuarioPla = Convert.ToString(usuarioP[0]);
                            bool exist = dbs.SIBOACUsuarios.Any(x => x.Usuario == usuarioPla);

                            if (exist)
                            {
                                mensajePlantilla = "El usuario con la Cédula: " + Convert.ToString(usuarioP[0]) + " ya existe, verifique y vuelva a cargar la plantilla";

                                TempData["Type"] = "error";
                                TempData["Message"] = mensajePlantilla;

                                return Json(new { result = true, msg = mensajePlantilla });
                            }
                            else
                            {
                                dbs.SIBOACUsuarios.Add(new SIBOACUsuarios
                                {
                                    Usuario = Convert.ToString(usuarioP[0]),
                                    Email = Convert.ToString(usuarioP[1]),
                                    Contrasena = Convert.ToString(usuarioP[0]),
                                    Nombre = Convert.ToString(usuarioP[2]),
                                    codigo = "000",
                                    FechaDeActualizacionClave = DateTime.Now,
                                    Activo = Convert.ToBoolean(1),
                                    Identificacion = Convert.ToString(usuarioP[0]),
                                    LugarTrabajo = Convert.ToString(usuarioP[3]),
                                    UltimoIngreso = DateTime.Now
                                });

                                dbs.SaveChanges();
                            }

                            
                        }

                    }

                    TempData["Type"] = "success";
                    TempData["Message"] = "Plantilla Cargada Exitosamente";

                    return Json(new { result = true, msg = "Plantilla Cargada Exitosamente" });
                }
                catch (Exception ex)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Ocurrió un error. Detalles: " + ex.Message;
                    return Json(new { result = false, msg = "Ocurrió un error. Detalles: " + ex.Message });
                }
            }
            else
            {
                TempData["Type"] = "warning";
                TempData["Message"] = "No hay archivo seleccionado.";
                return Json(new { result = false, msg = "No hay archivo seleccionado." });
            }
        }

    }
}
