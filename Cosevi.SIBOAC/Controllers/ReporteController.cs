using Cosevi.SIBOAC.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cosevi.SIBOAC.Controllers
{
    public class ReporteController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        private SIBOACSecurityEntities dbSecurity = new SIBOACSecurityEntities();

        private void GetData(string id)
        {
            switch (id)
            {
                case "_Bitacora":
                   
                    #region "_Bitacora"
                    List<SIBOACTablas> tablas = dbSecurity.SIBOACTablas.ToList();
                    List<SIBOACUsuarios> usuarios = dbSecurity.SIBOACUsuarios.Where(u => u.Activo.Value).ToList();

                    tablas.Insert(0, new SIBOACTablas() { Id = 0, Descripcion = "Todas" });
                    ViewBag.Tablas = tablas;

                    usuarios.Insert(0, new SIBOACUsuarios() { Id = 0, Usuario = "Todos" });
                    ViewBag.Usuarios = usuarios;

                    break;
                #endregion
                case "_DescargaInspector":
                  
                    #region "_DescargaInspector"
                    var listaSeleccionadosInspector = ViewBag.InspectoresSeleccionados;

                    var listaInspectores = (from r in db.INSPECTOR
                                            where r.Id.Trim() !="" 
                                             select new
                                             {
                                                 r.Id,
                                                 r.Nombre,
                                                 r.Identificacion
                                             }).ToList().Distinct()
                                 .Select(x => new item
                                 {
                                     Id = x.Id,
                                     Descripcion = x.Id + " - "+ x.Nombre,
                                     Seleccionado = false
                                 }
                                 );



                    if (listaSeleccionadosInspector != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosInspector;
                        List<item> _listInspectores = listaInspectores.ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listInspectores.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listInspectores.ElementAt(i).Id))
                            {
                                _listInspectores.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaInspectores = _listInspectores;
                    }

                    ViewBag.Inspectores = listaInspectores.OrderBy(a => a.Descripcion);


                    break;
                #endregion
                case "_DescargaBoleta":

                    break;
                case "_DescargaParteOficial":
               
                    #region "DescargaParteOficial"
                    var listaSeleccionadosDelegacion = ViewBag.DelegacionesSeleccionadas;
                    var listaSeleccionadosAutoridad = ViewBag.AutoridadesSeleccionadas;

                    var listaDelegaciones = (from r in db.DELEGACION
                                 select new {
                                     r.Id,
                                     r.Descripcion }).ToList().Distinct()
                                 .Select(x => new item
                                 { Id= x.Id,
                                   Descripcion = x.Id + " - "+ x.Descripcion,
                                  Seleccionado =false
                                 }
                                 );



                    if (listaSeleccionadosDelegacion != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosDelegacion;
                        List<item> _listDelegaciones = listaDelegaciones.ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listDelegaciones.Count(); i++)
                        {
                          if(_list.ToArray().Any(a=>a.Id==_listDelegaciones.ElementAt(i).Id))
                            {
                                _listDelegaciones.ElementAt(i).Seleccionado = true;
                            }
                       
                        }
                        listaDelegaciones = _listDelegaciones;
                    }
                   
                    ViewBag.Delegaciones = listaDelegaciones.OrderBy(a => a.Descripcion);
                    
              
                    var listaAutoridades = (from r in db.AUTORIDAD
                                            select new
                                            {
                                                r.Id,
                                                r.Descripcion
                                            }).Distinct().ToList()
                             .Select(x => new item
                             {
                                 Id = x.Id,
                                 Descripcion = x.Id + " - " + x.Descripcion.Trim(),                                 
                                 Seleccionado = false
                             }
                             );
                    if (listaSeleccionadosAutoridad != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosAutoridad;
                        List<item> _listAutoridad = listaAutoridades.Distinct().ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listAutoridad.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listAutoridad.ElementAt(i).Id))
                            {
                                _listAutoridad.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaAutoridades = _listAutoridad;
                    }

                    ViewBag.Autoridades = listaAutoridades.OrderBy(a => a.Descripcion);
                    break;
                #endregion
                case "_ConsultaeImpresionDeParteOficial":
                  
                    break;
                case "_ReportePorUsuario":
               
                    #region "Reporte Por Usuario"
                    var listaSeleccionadosUsuarios = ViewBag.UsuariosSeleccionados;

                    var listaUsuarios = (from r in dbSecurity.SIBOACUsuarios
                                         select new
                                         {
                                             r.Id,
                                             r.Usuario,
                                             r.Nombre
                                         }).ToList().Distinct()
                                 .Select(x => new item
                                 {
                                     Id = x.Usuario.ToString(),
                                     Usuario = x.Usuario + " - " + x.Nombre,
                                     Seleccionado = false
                                 }
                                 );

                    if (listaSeleccionadosUsuarios != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosUsuarios;
                        List<item> _listUsuarios = listaUsuarios.ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listUsuarios.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Usuario == _listUsuarios.ElementAt(i).Usuario))
                            {
                                _listUsuarios.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaUsuarios = _listUsuarios;
                    }

                    ViewBag.Usuarios = listaUsuarios.OrderBy(a => a.Descripcion);
                    break;
                #endregion
                case "_ConsultaeImpresionDeBoletas":
          
                    #region "Consulta E Impresion de Boletas"
                    var listaSeleccionadosDelegacion3 = ViewBag.DelegacionesSeleccionadas;
                    var listaSeleccionadosInspector3 = ViewBag.InspectorSeleccionadas;

                    var listaDelegacion3 = (from r in db.DELEGACION
                                             select new
                                             {
                                                 r.Id,
                                                 r.Descripcion
                                             }).ToList().Distinct()
                                 .Select(x => new item
                                 {
                                     Id = x.Id,
                                     Descripcion = x.Id +" - "+ x.Descripcion
                                 }
                                 );

                    if (listaSeleccionadosDelegacion3 != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosDelegacion3;
                        List<item> _listDelegaciones = listaDelegacion3.ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listDelegaciones.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listDelegaciones.ElementAt(i).Id))
                            {
                                _listDelegaciones.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaDelegacion3 = _listDelegaciones;
                    }
                    ViewBag.Delegacion = listaDelegacion3.OrderBy(a => a.Descripcion);

                    var listaInspector3 = (from r in db.INSPECTOR
                                            select new
                                            {
                                                r.Id,
                                                r.Nombre                                                
                                            }).ToList().Distinct()
                             .Select(x => new item
                             {
                                 Id = x.Id,
                                 Descripcion = x.Id+ " - "+x.Nombre
                             }
                             );
                    if (listaSeleccionadosInspector3 != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosInspector3;
                        List<item> _listInspector = listaInspector3.ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listInspector.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listInspector.ElementAt(i).Id))
                            {
                                _listInspector.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaInspector3 = _listInspector;
                    }                    
                    ViewBag.Inspector = listaInspector3.OrderBy(a => a.Descripcion);
                    break;
                #endregion

                case "_ReporteStatusActualPlano":
                   
                    #region "Reporte Status Actual del Plano"
                    var listaSeleccionadosDelegacion2 = ViewBag.DelegacionesSeleccionadas;
                    var listaSeleccionadosAutoridad2 = ViewBag.AutoridadesSeleccionadas;

                    var listaDelegaciones2 = (from r in db.DELEGACION
                                             select new
                                             {
                                                 r.Id,
                                                 r.Descripcion
                                             }).ToList().Distinct()
                                 .Select(x => new item
                                 {
                                     Id = x.Id,
                                     Descripcion =x.Id+" - "+  x.Descripcion,
                                     Seleccionado = false
                                 }
                                 );



                    if (listaSeleccionadosDelegacion2 != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosDelegacion2;
                        List<item> _listDelegaciones = listaDelegaciones2.ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listDelegaciones.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listDelegaciones.ElementAt(i).Id))
                            {
                                _listDelegaciones.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaDelegaciones2 = _listDelegaciones;
                    }

                    ViewBag.Delegacion = listaDelegaciones2.OrderBy(a => a.Descripcion);


                    var listaAutoridades2 = (from r in db.AUTORIDAD                                             
                                             select new
                                            {
                                                r.Id,
                                                r.Descripcion,
                                            }).Distinct().ToList()
                             .Select(x => new item
                             {
                                 Id = x.Id,
                                 Descripcion =x.Id+" - "+ x.Descripcion.Trim(),
                                 Seleccionado = false
                             }
                             );
                    if (listaSeleccionadosAutoridad2 != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosAutoridad2;
                        List<item> _listAutoridad = listaAutoridades2.Distinct().ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listAutoridad.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listAutoridad.ElementAt(i).Id))
                            {
                                _listAutoridad.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaAutoridades2 = _listAutoridad;
                    }

                    ViewBag.Autoridad = listaAutoridades2.OrderBy(a => a.Descripcion);
                    break;
                #endregion

                case "_ActividadOficial":
                  
                    #region "Actividad Oficial"
                    var Inspector = (dbSecurity.SIBOACUsuarios.Where(a => a.Usuario == User.Identity.Name).Select(a => a.codigo).ToList());
                    string  CodigoInspector = Inspector.ToArray().FirstOrDefault() == null ? ViewBag.Inspector : Inspector.ToArray().FirstOrDefault().ToString();
                    ViewBag.CodigoInspector = CodigoInspector;
                    break;
                   #endregion

                case "_ReporteListadoMultaFija":
          
                    #region "Reporte Listado Multa Fija"
                    var listaSelecDelegacion = ViewBag.DelegacionesSeleccionadas;
                    var listaSelecInspector= ViewBag.InspectoresSeleccionadas;

                    var listaDelegacion = (from r in db.DELEGACION
                                             select new
                                             {
                                                 r.Id,
                                                 r.Descripcion
                                             }).ToList().Distinct()
                                 .Select(x => new item
                                 {
                                     Id = x.Id,
                                     Descripcion = x.Id + " - " + x.Descripcion,
                                     Seleccionado = false
                                 }
                                 );



                    if (listaSelecDelegacion != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSelecDelegacion;
                        List<item> _listDelegaciones = listaDelegacion.ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listDelegaciones.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listDelegaciones.ElementAt(i).Id))
                            {
                                _listDelegaciones.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaDelegacion = _listDelegaciones;
                    }

                    ViewBag.Delegaciones = listaDelegacion.OrderBy(a => a.Descripcion);


                    var listaInspector= (from r in db.INSPECTOR
                                         where r.Id.Trim() !=""
                                            select new
                                            {
                                                r.Id,
                                                r.Nombre ,
                                                r.Apellido1,
                                                r.Apellido2
                                            }).Distinct().ToList()
                             .Select(x => new item
                             {
                                 Id = x.Id,
                                 Descripcion = x.Id + " - " + x.Nombre + " " + x.Apellido1 + " " + x.Apellido2,
                                 Seleccionado = false
                             }
                             );
                    if (listaSelecInspector != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSelecInspector;
                        List<item> _listInspect = listaInspector.Distinct().ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listInspect.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listInspect.ElementAt(i).Id))
                            {
                                _listInspect.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaInspector = _listInspect;
                    }

                    ViewBag.Inspectores = listaInspector.OrderBy(a => a.Descripcion);
                    break;
                #endregion

                case "_ReporteListadoParteOficial":
                    #region "Reporte Listado Parte Oficial"

                    var _listaSelecDelegacion = ViewBag.DelegacionesSeleccionadas;
                    var _listaSelecInspector = ViewBag.InspectoresSeleccionadas;

                    var _listaDelegacion = (from r in db.DELEGACION
                                           select new
                                           {
                                               r.Id,
                                               r.Descripcion
                                           }).ToList().Distinct()
                                 .Select(x => new item
                                 {
                                     Id = x.Id,
                                     Descripcion = x.Id + " - " + x.Descripcion,
                                     Seleccionado = false
                                 }
                                 );



                    if (_listaSelecDelegacion != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)_listaSelecDelegacion;
                        List<item> _listDelegaciones = _listaDelegacion.ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listDelegaciones.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listDelegaciones.ElementAt(i).Id))
                            {
                                _listDelegaciones.ElementAt(i).Seleccionado = true;
                            }

                        }
                        _listaDelegacion = _listDelegaciones;
                    }

                    ViewBag.Delegaciones = _listaDelegacion.OrderBy(a => a.Descripcion);


                    var _listaInspector = (from r in db.INSPECTOR
                                          where r.Id.Trim() != ""
                                          select new
                                          {
                                              r.Id,
                                              r.Nombre,
                                              r.Apellido1,
                                              r.Apellido2
                                          }).Distinct().ToList()
                             .Select(x => new item
                             {
                                 Id = x.Id,
                                 Descripcion = x.Id + " - " + x.Nombre + " " + x.Apellido1 + " " + x.Apellido2,
                                 Seleccionado = false
                             }
                             );
                    if (_listaSelecInspector != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)_listaSelecInspector;
                        List<item> _listInspect = _listaInspector.Distinct().ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listInspect.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Id == _listInspect.ElementAt(i).Id))
                            {
                                _listInspect.ElementAt(i).Seleccionado = true;
                            }

                        }
                        _listaInspector = _listInspect;
                    }

                    ViewBag.Inspectores = _listaInspector.OrderBy(a => a.Descripcion);
                    break;
                #endregion

                case "_BitacoraAplicacion":

                    #region "Bitacora de Aplicacion"
                    var listaSeleccionadosUsuarios2 = ViewBag.UsuariosSeleccionados;

                    var listaUsuarios2 = (from r in dbSecurity.SIBOACUsuarios
                                         select new
                                         {
                                             r.Id,
                                             r.Usuario,
                                             r.Nombre
                                         }).ToList().Distinct()
                                 .Select(x => new item
                                 {
                                     Id = x.Usuario.ToString(),
                                     Usuario = x.Usuario + " - " + x.Nombre,
                                     Seleccionado = false
                                 }
                                 );

                    if (listaSeleccionadosUsuarios2 != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosUsuarios2;
                        List<item> _listUsuarios = listaUsuarios2.ToList();
                        List<item> _temp = new List<item>();

                        for (int i = 0; i < _listUsuarios.Count(); i++)
                        {
                            if (_list.ToArray().Any(a => a.Usuario == _listUsuarios.ElementAt(i).Usuario))
                            {
                                _listUsuarios.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaUsuarios2 = _listUsuarios;
                    }

                    ViewBag.Usuarios = listaUsuarios2.OrderBy(a => a.Descripcion);
                    break;
                #endregion
                default:
                    break;
            }

       

        }
        public class item
        {
            public string Id { get; set; }
            public string Descripcion { get; set; }
            public string Nombre { get; set; }
            public string Usuario { get; set; }

            public bool Seleccionado { get; set; }
        }
 
        // GET: Reporte
        [SessionExpire]
        public ActionResult Index(string id)
        {
            GetData(id);
            if (String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                return View(id);
            }
        }

        #region Bitacora
        public ActionResult GetBitacora(DateTime fechaInicio, DateTime fechaFin, string nombreTabla, string operacion, string usuario, string tipoReporte)
        {
            string reporteID = "_Bitacora";
            string nombreReporte = "rptBitacora";
            string parametros = String.Format("{0},{1},{2},{3},{4}", fechaInicio.ToString("yyyy-MM-dd"), fechaFin.ToString("yyyy-MM-dd"), nombreTabla, operacion, usuario);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_Bitacora");
        }

        private List<BitacoraSIBOAC> GetBitacoraData(DateTime fechaInicio, DateTime fechaFin, string nombreTabla, string operacion, string usuario)
        {
            var bitacora = db.GetBitacoraData(fechaInicio, fechaFin, nombreTabla, usuario, operacion).ToList();
            return bitacora;
        }
        #endregion

        #region Actividad Oficial
        public ActionResult GetActividadOficial(string CodigoInspector, DateTime fechaInicio, DateTime fechaFin)
        {
            string reporteID = "_ActividadOficial";
            string nombreReporte = "ActividadOficial";
            string parametros = String.Format("{0},{1},{2}",CodigoInspector, fechaInicio.ToString("yyyy-MM-dd"), fechaFin.ToString("yyyy-MM-dd"));
            ViewBag.Inspector = CodigoInspector;
            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ActividadOficial");
        }
        private List<GetActividadOficialData_Result> GetActividadOficialData(string CodigoInspector,DateTime fechaInicio, DateTime fechaFin)
        {
            var usuarioSistema = User.Identity.Name;

            var lista = db.GetActividadOficialData(CodigoInspector, fechaInicio, fechaFin,usuarioSistema).ToList();
            return lista;
        }

        #endregion

        #region Reporte Descarga Inspector        
        public ActionResult GetReporteDescargaInspector(DateTime desde, DateTime hasta, [FromUri] string inspectorValues)
        {
            string reporteID = "_DescargaInspector";
            string nombreReporte = "DescargaInspector";
            string idInspectores = "";
            string[] listaInspectores = inspectorValues.Split(',');
            foreach (var i in listaInspectores)
            {
                idInspectores += "-" + i + "-|";

            }
            if (idInspectores.Length > 0)
            {
                idInspectores = idInspectores.Substring(0, idInspectores.Length - 1);
            }

            var lstInspectoresSeleccionados = (from r in db.INSPECTOR
                                                where listaInspectores.Contains(r.Id)
                                                select new
                                                {
                                                    r.Id,
                                                    r.Nombre,
                                                    r.Identificacion
                                                }).ToList().Distinct()
                                              .Select(x => new item
                                              {
                                                  Id = x.Id,
                                                  Descripcion = x.Id + " - "+ x.Nombre,
                                                  Seleccionado = true
                                              }
                                            );
            ViewBag.InspectoresSeleccionados = lstInspectoresSeleccionados.ToList();
            string parametros = String.Format("{0},{1},{2}", desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), idInspectores);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_DescargaInspector");
        }

        private List<GetDescargaInspectorData_Result> GetDescargaInspectorData(DateTime hasta, DateTime desde, string listaInspectores)
        {
            var usuarioSistema = User.Identity.Name;

            string idInspectores = "";
            foreach (var i in listaInspectores)
            {
                idInspectores += "-" + i + "-|";

            }
            if (idInspectores.Length > 0)
            {
                idInspectores = idInspectores.Substring(0, idInspectores.Length - 1);
            }
            var lista = db.GetDescargaInspectorData(hasta, desde, idInspectores,usuarioSistema).ToList();

            return lista;
        }

        #endregion

        #region ConsultaeImpresionParteOficialData
        private List<GetConsultaeImpresionDeParteOficialData_Result> GetConsultaeImpresionParteOficialData( int idRadio,  string serieParte,
             string numeroParte,  int? serieBoleta,  decimal? numeroBoleta,  string tipoId,
             string numeroID, string numeroPlaca, string codigoPlaca, string clasePlaca)
        {
            if(idRadio ==1) // Consulta por parte oficial
            {
                db.Database.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                var lista1 = db.GetConsultaeImpresionDeParteOficialData(1, serieBoleta.ToString(), numeroBoleta.ToString(),null).ToList();
                return lista1;
            }
            if (idRadio == 2)//consulta por Boleta de citación
            {
                db.Database.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                var lista2 = db.GetConsultaeImpresionDeParteOficialData(2, serieParte, numeroParte,null).ToList();
                
                return lista2;
            }
            if (idRadio == 3)//Indentificación del implicado
            {
                db.Database.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                var lista3 = db.GetConsultaeImpresionDeParteOficialData(3, tipoId, numeroID, null).ToList();
                return lista3;
            }
            if (idRadio == 4)//Placa
            {
                db.Database.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                var lista4 = db.GetConsultaeImpresionDeParteOficialData(4, numeroPlaca, codigoPlaca, clasePlaca).ToList();
                return lista4;
            }

            return null;
        }

        public ActionResult GetConsultaeImpresionParteOficial([FromUri] int opcionConsulta, [FromUri] string serieParte,
            [FromUri] string numeroParte, [FromUri] int? serieBoleta, [FromUri] decimal? numeroBoleta, [FromUri] string tipoId,
            [FromUri] string numeroID, [FromUri] string numeroPlaca, [FromUri] string codigoPlaca, [FromUri] string clasePlaca)
        {

            string reporteID = "_ConsultaeImpresionDeParteOficial";
            string nombreReporte = "ConsultaeImpresionDeParteOficial";
            string parametros = "";
            if (opcionConsulta == 1)
            {
                parametros = String.Format("{0},{1},{2},{3}", opcionConsulta.ToString(), serieParte, numeroParte, "null");
             }
            if(opcionConsulta == 2)
            {
               
                parametros = String.Format("{0},{1},{2},{3}", opcionConsulta.ToString(), serieBoleta.ToString(), numeroBoleta.ToString(), "null");
            }
            if (opcionConsulta == 3)
            {
                parametros = String.Format("{0},{1},{2},{3}", opcionConsulta.ToString(), tipoId, numeroID, "null");

            }
            if (opcionConsulta == 4)
            {
                parametros = String.Format("{0},{1},{2},{3}", opcionConsulta.ToString(), numeroPlaca, codigoPlaca, clasePlaca);


            }


            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ConsultaeImpresionDeParteOficial");
        }

        #endregion

        #region Descarga Boleta
        public ActionResult GetDescargaBoleta(int opcionRadio, DateTime fechaDesde, DateTime fechaHasta)
        {
            string reporteID = "_DescargaBoleta";
            string nombreReporte = "DescargaBoleta";
            string parametros = String.Format("{0},{1},{2}", opcionRadio, fechaDesde.ToString("yyyy-MM-dd"), fechaHasta.ToString("yyyy-MM-dd"));

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_DescargaBoleta");
        }

        private List<GetDescargaBoletaData_Result> GetDescargaBoletaData(int opcionRadio, DateTime fechaDesde, DateTime fechaHasta)
        {
            var usuarioSistema = User.Identity.Name;

            var lista = db.GetDescargaBoletaData(opcionRadio, fechaDesde, fechaHasta,usuarioSistema).ToList();
            return lista;
        }

        #endregion


        #region Reporte Por Delegacion Autoridad o Descarga Parte Oficial
        [System.Web.Http.HttpPost]
        public ActionResult GetReportePorDelegacionAutoridad(DateTime desde, DateTime hasta, int radio, [FromUri] string autoridadesValues, [FromUri] string delegacionesValues)
        {

            string reporteID = "_DescargaParteOficial";
            string nombreReporte = "DescargaParteOficial";
            string idDelegaciones = "";
            string idAutoridades = "";
            string[] listaDelegaciones = delegacionesValues.Split(',');
            foreach (var i in listaDelegaciones)
            {
                idDelegaciones += "-"+i+"-|";

            }
            if (idDelegaciones.Length > 0)
            {
                idDelegaciones = idDelegaciones.Substring(0, idDelegaciones.Length - 1);
            }

            string[] listaAutoridades = autoridadesValues.Split(',');
            foreach (var i in listaAutoridades)
            {
                idAutoridades += "-" + i + "-|";

            }
            if (idAutoridades.Length > 0)
            {
                idAutoridades = idAutoridades.Substring(0, idAutoridades.Length - 1);
            }
            var lstDelegacionesSeleccionadas = (from r in db.DELEGACION
                                                where listaDelegaciones.Contains(r.Id)
                                                select new
                                                {
                                                    r.Id,
                                                    r.Descripcion
                                                }).ToList().Distinct()
                                                 .Select(x => new item
                                                 {
                                                     Id = x.Id,
                                                     Descripcion = x.Id + " - "+ x.Descripcion,
                                                     Seleccionado = true
                                                 }
                                               );
            ViewBag.DelegacionesSeleccionadas = lstDelegacionesSeleccionadas.Distinct().ToList();

            var lstAutoridadesSeleccionadas = (from r in db.AUTORIDAD
                                               where listaAutoridades.Contains(r.Id)
                                               select new
                                               {
                                                   r.Id,
                                                   r.Descripcion
                                               }).ToList().Distinct()
                                        .Select(x => new item
                                        {
                                            Id = x.Id,
                                            Descripcion =x.Id +" - "+ x.Descripcion.Trim(),
                                            Seleccionado = true
                                        }
                                      );
            ViewBag.AutoridadesSeleccionadas = lstAutoridadesSeleccionadas.Distinct().ToList();
            string parametros = String.Format("{0}, {1}, {2}, {3}, {4}", radio, desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), idAutoridades, idDelegaciones);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_DescargaParteOficial");
        }

        private List<GetReportePorDelegacionAutoridadData_Result> GetReportePorDelegacionAutoridadData(DateTime desde, DateTime hasta, int radio, string idAutoridades, string listaDelegaciones)
        {
            var usuarioSistema = User.Identity.Name;

            var lista = db.GetReportePorDelegacionAutoridadData(radio, desde, hasta, idAutoridades, listaDelegaciones,usuarioSistema).ToList();
            // return lista;
            return lista;
        }

        #endregion

        #region Reporte Listado Multa Fija
        [System.Web.Http.HttpPost]
        public ActionResult GetReporteListadoMultaFija(DateTime desde, DateTime hasta, int radio, [FromUri] string inspectorValues, [FromUri] string delegacionesValues)
        {

            string reporteID = "_ReporteListadoMultaFija";
            string nombreReporte = "ReporteListadoMultaFija";
            string idDelegaciones = "";
            string idInspectores = "";
            string[] listaInspectores = inspectorValues.Split(',');
            string[] listaDelegaciones = delegacionesValues.Split(',');
            foreach (var i in listaDelegaciones)
            {
                idDelegaciones += "-" + i + "-|";

            }
            if (idDelegaciones.Length > 0)
            {
                idDelegaciones = idDelegaciones.Substring(0, idDelegaciones.Length - 1);
            }
            foreach (var i in listaInspectores)
            {
                idInspectores += "-" + i + "-|";

            }
            if (idInspectores.Length > 0)
            {
                idInspectores = idInspectores.Substring(0, idInspectores.Length - 1);
            }
            var lstDelegacionesSeleccionadas = (from r in db.DELEGACION
                                                where listaDelegaciones.Contains(r.Id)
                                                select new
                                                {
                                                    r.Id,
                                                    r.Descripcion
                                                }).ToList().Distinct()
                                                 .Select(x => new item
                                                 {
                                                     Id = x.Id,
                                                     Descripcion = x.Id + " - " + x.Descripcion,
                                                     Seleccionado = true
                                                 }
                                               );
            ViewBag.DelegacionesSeleccionadas = lstDelegacionesSeleccionadas.Distinct().ToList();

            var lstInspectoresSeleccionadas = (from r in db.INSPECTOR
                                               where listaInspectores.Contains(r.Id)
                                               select new
                                               {
                                                   r.Id,
                                                   r.Nombre,
                                                   r.Apellido1,
                                                   r.Apellido2
                                               }).ToList().Distinct()
                                        .Select(x => new item
                                        {
                                            Id = x.Id,
                                            Descripcion = x.Id + " - " + x.Nombre+" "+ x.Apellido1 + " "+ x.Apellido2 ,
                                            Seleccionado = true
                                        }
                                      );
            ViewBag.InspectoresSeleccionadas = lstInspectoresSeleccionadas.Distinct().ToList();
            string parametros = String.Format("{0}, {1}, {2}, {3}, {4}", radio, desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), idInspectores, idDelegaciones);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ReporteListadoMultaFija");
        }

        private List<GetReporteListadoMultaFijaData_Result> GetReporteListadoMultaFijaData(DateTime desde, DateTime hasta, int radio, string idInspectores, string listaDelegaciones)
        {
            var usuarioSistema = User.Identity.Name;

            var lista = db.GetReporteListadoMultaFijaData(desde, hasta, radio, idInspectores, listaDelegaciones,usuarioSistema).ToList();
            // return lista;
            return lista;
        }

        #endregion

        #region Reporte Listado Parte Oficial
        [System.Web.Http.HttpPost]
        public ActionResult GetReporteListadoParteOficial(DateTime desde, DateTime hasta, int radio, [FromUri] string inspectorValues, [FromUri] string delegacionesValues)
        {

            string reporteID = "_ReporteListadoParteOficial";
            string nombreReporte = "ReporteListadoParteOficial";
            string idDelegaciones = "";
            string idInspectores = "";
            string[] listaInspectores = inspectorValues.Split(',');
            string[] listaDelegaciones = delegacionesValues.Split(',');
            foreach (var i in listaDelegaciones)
            {
                idDelegaciones += "-" + i + "-|";

            }
            if (idDelegaciones.Length > 0)
            {
                idDelegaciones = idDelegaciones.Substring(0, idDelegaciones.Length - 1);
            }
            foreach (var i in listaInspectores)
            {
                idInspectores += "-" + i + "-|";

            }
            if (idInspectores.Length > 0)
            {
                idInspectores = idInspectores.Substring(0, idInspectores.Length - 1);
            }
            var lstDelegacionesSeleccionadas = (from r in db.DELEGACION
                                                where listaDelegaciones.Contains(r.Id)
                                                select new
                                                {
                                                    r.Id,
                                                    r.Descripcion
                                                }).ToList().Distinct()
                                                 .Select(x => new item
                                                 {
                                                     Id = x.Id,
                                                     Descripcion = x.Id + " - " + x.Descripcion,
                                                     Seleccionado = true
                                                 }
                                               );
            ViewBag.DelegacionesSeleccionadas = lstDelegacionesSeleccionadas.Distinct().ToList();

            var lstInspectoresSeleccionadas = (from r in db.INSPECTOR
                                               where listaInspectores.Contains(r.Id)
                                               select new
                                               {
                                                   r.Id,
                                                   r.Nombre,
                                                   r.Apellido1,
                                                   r.Apellido2
                                               }).ToList().Distinct()
                                        .Select(x => new item
                                        {
                                            Id = x.Id,
                                            Descripcion = x.Id + " - " + x.Nombre + " " + x.Apellido1 + " " + x.Apellido2,
                                            Seleccionado = true
                                        }
                                      );
            ViewBag.InspectoresSeleccionadas = lstInspectoresSeleccionadas.Distinct().ToList();
            string parametros = String.Format("{0}, {1}, {2}, {3}, {4}", radio, desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), idInspectores, idDelegaciones);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ReporteListadoParteOficial");
        }

        private List<GetReporteListadoParteOficialData_Result> GetReporteListadoParteOficialData(DateTime desde, DateTime hasta, int radio, string idInspectores, string listaDelegaciones)
        {
            var usuarioSistema = User.Identity.Name;


            var lista = db.GetReporteListadoParteOficialData(desde, hasta, radio, idInspectores, listaDelegaciones,usuarioSistema).ToList();
            // return lista;
            return lista;
        }

        #endregion

        #region Reporte Por Usuario
        public ActionResult GetReportePorUsuario([FromUri] string usuariosValues, DateTime desde, DateTime hasta)
        {
            string reporteID = "_ReportePorUsuario";
            string nombreReporte = "ReportePorUsuario";
            string idUsuarios = "";
            string[] listaUsuarios = usuariosValues.Split(',');
            //Session["ListaUsuarios"] = listaUsuarios;
            foreach (var i in listaUsuarios)
            {
                idUsuarios += "-" + i + "-|";

            }
            if (idUsuarios.Length > 0)
            {
                idUsuarios = idUsuarios.Substring(0, idUsuarios.Length - 1);
            }
            var lstaUsuariosSeleccionados = (from r in dbSecurity.SIBOACUsuarios
                                where listaUsuarios.Contains(r.Usuario.ToString())
                                select new
                                 {
                                     r.Id,
                                     r.Usuario,
                                     r.Nombre
                                 }).ToList().Distinct()
                                .Select(x => new item
                                {
                                    Id = x.Usuario.ToString(),
                                    Usuario = x.Usuario + " - " + x.Nombre,
                                    Seleccionado = true
                                }
                                );
            ViewBag.UsuariosSeleccionados = lstaUsuariosSeleccionados.ToList();
            string parametros = String.Format("{0},{1},{2}", idUsuarios, desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"));

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ReportePorUsuario");
        }

        private List<GetReportePorUsuarioData_Result> GetReportePorUsuarioData(string listaUsuarios, DateTime desde, DateTime hasta)
        {
            var usuarioSistema = User.Identity.Name;

            string idUsuarios = "";
            foreach (var i in listaUsuarios)
            {
                idUsuarios += "-" + i + "-|";

            }
            if (idUsuarios.Length > 0)
            {
                idUsuarios = idUsuarios.Substring(0, idUsuarios.Length - 1);
            }
            var lista = db.GetReportePorUsuarioData(idUsuarios, desde, hasta,usuarioSistema).ToList();
            return lista;
        }
        #endregion


        #region Consulta e Impresion de  Boletas
        public ActionResult GetConsultaeImpresionDeBoletas([FromUri] DateTime desde, DateTime hasta, [FromUri] string delegacionesValues, [FromUri] string inspectorValues)
        {
            string reporteID = "_ConsultaeImpresionDeBoletas";
            string nombreReporte = "ConsultaeImpresionDeBoletas";
            string idDelegacion = "";
            string idInspector = "";
            string[] listaInspector = inspectorValues.Split(',');
            string[] listaDelegacion = delegacionesValues.Split(',');
            foreach (var i in listaDelegacion)
            {
                idDelegacion += "-" + i + "-|";

            }
            if (idDelegacion.Length > 0)
            {
                idDelegacion = idDelegacion.Substring(0, idDelegacion.Length - 1);
            }
            foreach (var i in listaInspector)
            {
                idInspector += "-" + i + "-|";

            }
            if (idInspector.Length > 0)
            {
                idInspector = idInspector.Substring(0, idInspector.Length - 1);
            }
            var lstDelegacionesSeleccionadas = (from r in db.DELEGACION
                                                where listaDelegacion.Contains(r.Id)
                                                select new
                                                {
                                                    r.Id,
                                                    r.Descripcion
                                                }).ToList().Distinct()
                                                .Select(x => new item
                                                {
                                                    Id = x.Id,
                                                    Descripcion = x.Descripcion,
                                                    Seleccionado = true
                                                }
                                              );
            ViewBag.DelegacionesSeleccionadas = lstDelegacionesSeleccionadas.ToList();

            var lstInspectoresSeleccionados = (from r in db.INSPECTOR
                                               where listaInspector.Contains(r.Id)
                                               select new
                                               {
                                                   r.Id,
                                                   r.Nombre,
                                                   r.Identificacion
                                               }).ToList().Distinct()
                                              .Select(x => new item
                                              {
                                                  Id = x.Id,
                                                  Descripcion = x.Identificacion + " - " + x.Nombre,
                                                  Seleccionado = true
                                              }
                                            );
            ViewBag.InspectorSeleccionadas = lstInspectoresSeleccionados.ToList();

            string parametros = String.Format(" {0}, {1}, {2}, {3} ", desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), idDelegacion, idInspector);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ConsultaeImpresionDeBoletas");
        }

        private List<GetConsultaeImpresionDeBoletasData_Result> GetConsultaeImpresionDeBoletasData(DateTime desde, DateTime hasta, string listaDelegacion, string listaInspector)
        {
            var usuarioSistema = User.Identity.Name;

            string idDelegacion = "";
            string idInspector = "";
            foreach (var i in listaDelegacion)
            {
                idDelegacion += "-" + i + "-|";

            }
            if (idDelegacion.Length > 0)
            {
                idDelegacion = idDelegacion.Substring(0, idDelegacion.Length - 1);
            }
            foreach (var i in listaInspector)
            {
                idInspector += "-" + i + "-|";

            }
            if (idInspector.Length > 0)
            {
                idInspector = idInspector.Substring(0, idInspector.Length - 1);
            }
            var
                 lista = db.GetConsultaeImpresionDeBoletasData(desde, hasta, idDelegacion, idInspector,usuarioSistema).ToList();
            return lista;
        }

        #endregion


        #region Reporte Status Actual Plano
        public ActionResult GetReporteStatusActualPlano(int radio, DateTime desde, DateTime hasta, [FromUri] string delegacionesValues, [FromUri] string autoridadesValues)
        {

            string reporteID = "_ReporteStatusActualPlano";
            string nombreReporte = "ReporteStatusActualPlano";
            string idDelegaciones = "";
            string idAutoridades = "";
            //Session["ListaDelegaciones"] = listaDelegacion;
            //Session["ListaAutoridades"] = listaAutoridad;
            string[] listaDelegacion = delegacionesValues.Split(',');
            string[] listaAutoridad = autoridadesValues.Split(',');
            foreach (var i in listaDelegacion)
            {
                idDelegaciones += "-" + i + "-|";

            }
            if (idDelegaciones.Length > 0)
            {
                idDelegaciones = idDelegaciones.Substring(0, idDelegaciones.Length - 1);
            }
            foreach (var i in listaAutoridad)
            {
                idAutoridades += "-" + i + "-|";

            }
            if (idAutoridades.Length > 0)
            {
                idAutoridades = idAutoridades.Substring(0, idAutoridades.Length - 1);
            }
            var lstDelegacionesSeleccionadas = (from r in db.DELEGACION
                                                where listaDelegacion.Contains(r.Id)
                                                select new
                                                {
                                                    r.Id,
                                                    r.Descripcion
                                                }).ToList().Distinct()
                                                 .Select(x => new item
                                                 {
                                                     Id = x.Id,
                                                     Descripcion = x.Descripcion,
                                                     Seleccionado = true
                                                 }
                                               );
            ViewBag.DelegacionesSeleccionadas = lstDelegacionesSeleccionadas.ToList();

            var lstAutoridadesSeleccionadas = (from r in db.AUTORIDAD
                                               where listaAutoridad.Contains(r.Id)
                                               select new
                                               {
                                                   r.Id,
                                                   r.Descripcion
                                               }).ToList().Distinct()
                                        .Select(x => new item
                                        {
                                            Id = x.Id,
                                            Descripcion = x.Descripcion.Trim(),
                                            Seleccionado = true
                                        }
                                      );
            ViewBag.AutoridadesSeleccionadas = lstAutoridadesSeleccionadas.Distinct().ToList();
            string parametros = String.Format("{0}, {1}, {2}, {3}, {4}", radio, desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), idAutoridades, idDelegaciones);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ReporteStatusActualPlano");
        }

        private List<GetReporteStatusActualPlanoData_Result> GetReporteStatusActualPlanoData(int radio, DateTime desde, DateTime hasta, string listaDelegacion, string listaAutoridad)
        {
            var usuarioSistema = User.Identity.Name;

            string idDelegaciones = "";
            foreach (var i in listaDelegacion)
            {
                idDelegaciones += "-" + i + "-|";

            }
            if (idDelegaciones.Length > 0)
            {
                idDelegaciones = idDelegaciones.Substring(0, idDelegaciones.Length - 1);
            }
            string idAutoridades = "";
            foreach (var i in listaAutoridad)
            {
                idAutoridades += "-" + i + "-|";

            }
            if (idAutoridades.Length > 0)
            {
                idAutoridades = idAutoridades.Substring(0, idAutoridades.Length - 1);
            }
            var lista = db.GetReporteStatusActualPlanoData(radio, desde, hasta, idDelegaciones, idAutoridades,usuarioSistema).ToList();
            // return lista;
            return lista;
        }
        #endregion

        #region Bitacora de Aplicacion
        public ActionResult GetBitacoraAplicacion(string tipoConsulta1, string tipoConsulta2, DateTime ? desde, DateTime ? hasta, [FromUri] string [] listaUsuarios)
        {
            string reporteID = "_BitacoraAplicacion";
            string nombreReporte = "BitacoraAplicacion";
            string idUsuarios = "";                        
            Session["ListaUsuarios"] = listaUsuarios;
            foreach (var i in listaUsuarios)
            {
                idUsuarios += "-" + i + "-|";

            }
            if (idUsuarios.Length > 0)
            {
                idUsuarios = idUsuarios.Substring(0, idUsuarios.Length - 1);
            }
            var lstaUsuariosSeleccionados = (from r in dbSecurity.SIBOACUsuarios
                                             where listaUsuarios.Contains(r.Usuario.ToString())
                                             select new
                                             {
                                                 r.Id,
                                                 r.Usuario,
                                                 r.Nombre
                                             }).ToList().Distinct()
                                .Select(x => new item
                                {
                                    Id = x.Usuario.ToString(),
                                    Usuario = x.Usuario + " - " + x.Nombre,
                                    Seleccionado = true
                                }
                                );
            ViewBag.UsuariosSeleccionados = lstaUsuariosSeleccionados.ToList();
            string parametros = "";

            if (tipoConsulta1 == "1" && tipoConsulta2 == "1")
            {
                parametros = String.Format("{0},{1},{2},{3},{4}", tipoConsulta1, tipoConsulta2, desde?.ToString("dd-MM-yyyy"), hasta?.ToString("dd-MM-yyyy"), idUsuarios);
            }
            if (tipoConsulta1 == "1" && tipoConsulta2 == null)
            {
                parametros = String.Format("{0},{1},{2},{3},{4}", tipoConsulta1, "", desde?.ToString("dd-MM-yyyy"), hasta?.ToString("dd-MM-yyyy"), "");
            }
            if (tipoConsulta1 == null && tipoConsulta2 == "1")
            {
                parametros = String.Format("{0},{1},{2},{3},{4}", "", tipoConsulta2, "null", "null", idUsuarios);
            }

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_BitacoraAplicacion");
        }

        private List<GetBitacoraDeAplicacion_Result> GetBitacoraAplicacionData(string tipoconsulta1, string tipoconsulta2, DateTime desde, DateTime hasta, string listaUsuarios)
        {            

            string idUsuarios = "";
            foreach (var i in listaUsuarios)
            {
                idUsuarios += "-" + i + "-|";

            }
            if (idUsuarios.Length > 0)
            {
                idUsuarios = idUsuarios.Substring(0, idUsuarios.Length - 1);
            }

            if (tipoconsulta1 == "1" && tipoconsulta2 == "1")
            {
                var lista = db.GetBitacoraDeAplicacion(tipoconsulta1, tipoconsulta2, desde, hasta, idUsuarios).ToList();
                return lista;
            }

            if (tipoconsulta1 == "1" && tipoconsulta2 == "")
            {
                var lista2 = db.GetBitacoraDeAplicacion(tipoconsulta1, null, desde, hasta, null).ToList();
                return lista2;
            }

            if (tipoconsulta1 == "" && tipoconsulta2 == "1")
            {
                var lista = db.GetBitacoraDeAplicacion(null, tipoconsulta2, null, null, idUsuarios).ToList();
                return lista;
            }

            return null;
           
        }
        #endregion
    
        #region "Reimpresion de Boletas de Campo"
        public ActionResult GetReimpresionDeBoletasDeCampo(string Serie,string NumeroBoleta)
        {
            string reporteID = "_ReimpresionDeBoletasDeCampo";
            string nombreReporte = "ReimpresionDeBoletasDeCampo";
         
            string parametros = String.Format("{0},{1}",Serie,NumeroBoleta);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ReimpresionDeBoletasDeCampo");
        }

        private List<GetReimpresionDeBoletasDeCampoData_Result> GetReimpresionDeBoletasDeCampoData(string Serie, string NumeroBoleta)
        {
            var usuarioSistema = User.Identity.Name;          
            var lista = db.GetReimpresionDeBoletasDeCampoData(Serie,NumeroBoleta,usuarioSistema).ToList();

            return lista;
        }
        #endregion

    }
}