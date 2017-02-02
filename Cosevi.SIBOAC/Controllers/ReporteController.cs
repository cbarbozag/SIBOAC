using Cosevi.SIBOAC.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
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
                    List<SIBOACTablas> tablas = dbSecurity.SIBOACTablas.ToList();
                    List<SIBOACUsuarios> usuarios = dbSecurity.SIBOACUsuarios.Where(u => u.Activo.Value).ToList();

                    tablas.Insert(0, new SIBOACTablas() { Id = 0, Descripcion = "Todas" });
                    ViewBag.Tablas = tablas;

                    usuarios.Insert(0, new SIBOACUsuarios() { Id = 0, Usuario = "Todos" });
                    ViewBag.Usuarios = usuarios;

                    break;
                case "_DescargaInspector":
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
                                     Descripcion = x.Identificacion + " - "+ x.Nombre,
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
                case "_DescargaBoleta":

                    break;
                case "_DescargaParteOficial":

                    var listaSeleccionadosDelegacion = ViewBag.DelegacionesSeleccionadas;
                    var listaSeleccionadosAutoridad = ViewBag.AutoridadesSeleccionadas;

                    var listaDelegaciones = (from r in db.DELEGACION
                                 select new {
                                     r.Id,
                                     r.Descripcion }).ToList().Distinct()
                                 .Select(x => new item
                                 { Id= x.Id,
                                   Descripcion = x.Descripcion,
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
                                            }).ToList().Distinct()
                             .Select(x => new item
                             {
                                 Id = x.Id,
                                 Descripcion = x.Descripcion ,                                 
                                 Seleccionado = false
                             }
                             );
                    if (listaSeleccionadosAutoridad != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosAutoridad;
                        List<item> _listAutoridad = listaAutoridades.ToList();
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
                case "_ConsultaeImpresionDeParteOficial":
                  
                    break;
                case "_ReportePorUsuario":                                        

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
                                     Id = x.Id.ToString(),
                                     Usuario = x.Usuario + " - " + x.Nombre,
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
                            if (_list.ToArray().Any(a => a.Id == _listUsuarios.ElementAt(i).Id))
                            {
                                _listUsuarios.ElementAt(i).Seleccionado = true;
                            }

                        }
                        listaUsuarios = _listUsuarios;
                    }

                    ViewBag.Usuarios = listaUsuarios.OrderBy(a => a.Descripcion);
                    break;

                case "_ConsultaeImpresionDeBoletas":
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
                        listaDelegacion3 = _listInspector;
                    }                    
                    ViewBag.Inspector = listaInspector3.OrderBy(a => a.Descripcion);
                    break;

                case "_ReporteStatusActualPlano":
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
                                                r.Descripcion
                                            }).ToList().Distinct()
                             .Select(x => new item
                             {
                                 Id = x.Id,
                                 Descripcion =x.Id+" - "+ x.Descripcion,
                                 Seleccionado = false
                             }
                             );
                    if (listaSeleccionadosAutoridad2 != null)
                    {
                        List<item> _list = new List<item>();
                        _list = (List<item>)listaSeleccionadosAutoridad2;
                        List<item> _listAutoridad = listaAutoridades2.ToList();
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
                case "_ActividadOficial":
                    var Inspector = (dbSecurity.SIBOACUsuarios.Where(a => a.Usuario == User.Identity.Name).Select(a => a.codigo).ToList());
                    string  CodigoInspector = Inspector.ToArray().FirstOrDefault() == null ? ViewBag.Inspector : Inspector.ToArray().FirstOrDefault().ToString();
                    ViewBag.CodigoInspector = CodigoInspector;
                    break;
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
            var lista = db.GetActividadOficialData(CodigoInspector, fechaInicio, fechaFin).ToList();
            return lista;
        }

        private List<BitacoraSIBOAC> GetBitacoraData(DateTime fechaInicio, DateTime fechaFin, string nombreTabla, string operacion, string usuario)
        {
            var bitacora = db.GetBitacoraData(fechaInicio, fechaFin, nombreTabla, usuario, operacion).ToList();
            return bitacora;
        }

        public ActionResult GetReporteDescargaInspector(DateTime desde, DateTime hasta, string numeroHH, [FromUri] string [] listaInspectores)
        {
            string reporteID = "_DescargaInspector";
            string nombreReporte = "DescargaInspector";
            string idInspectores = "";
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
                                                  Descripcion = x.Identificacion + " - "+ x.Nombre,
                                                  Seleccionado = true
                                              }
                                            );
            ViewBag.InspectoresSeleccionados = lstInspectoresSeleccionados.ToList();
            string parametros = String.Format("{0},{1},{2},{3}", desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), numeroHH, idInspectores);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_DescargaInspector");
        }

        private List<GetDescargaInspectorData_Result> GetDescargaInspectorData(DateTime hasta, DateTime desde, string numeroHH, [FromUri] string [] listaInspectores)
        {
            string idInspectores = "";
            foreach (var i in listaInspectores)
            {
                idInspectores += "-" + i + "-|";

            }
            if (idInspectores.Length > 0)
            {
                idInspectores = idInspectores.Substring(0, idInspectores.Length - 1);
            }
            var lista = db.GetDescargaInspectorData(hasta, desde, numeroHH, idInspectores).ToList();

            return lista;
        }

        private List<GetConsultaeImpresionDeParteOficialData_Result> GetConsultaeImpresionParteOficialData( int idRadio,  string serieParte,
             string numeroParte,  int? serieBoleta,  decimal? numeroBoleta,  string tipoId,
             string numeroID, string numeroPlaca, string codigoPlaca, string clasePlaca)
        {
            if(idRadio ==1) // Consulta por parte oficial
            {
                var lista1 = db.GetConsultaeImpresionDeParteOficialData(1, serieBoleta.ToString(), numeroBoleta.ToString(),null).ToList();
                return lista1;
            }
            if (idRadio == 2)//consulta por Boleta de citación
            {
                var lista2 = db.GetConsultaeImpresionDeParteOficialData(2, serieParte, numeroParte,null).ToList();
                return lista2;
            }
            if (idRadio == 3)//Indentificación del implicado
            {
                var lista3 = db.GetConsultaeImpresionDeParteOficialData(3, tipoId, numeroID, null).ToList();
                return lista3;
            }
            if (idRadio == 4)//Placa
            {
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
            var lista = db.GetDescargaBoletaData(opcionRadio, fechaDesde, fechaHasta).ToList();
            return lista;
        }

        [System.Web.Http.HttpPost]
        public ActionResult GetReporteDescargaParteOficial(DateTime desde, DateTime hasta, int radio, [FromUri] string[]  listaAutoridades,[FromUri] string [] listaDelegaciones)
        {

            string reporteID = "_DescargaParteOficial";
            string nombreReporte = "DescargaParteOficial";
            string idDelegaciones = "";
            string idAutoridades = "";
            Session["ListaDelegaciones"] = listaDelegaciones;
            Session["ListaAutoridades"] = listaAutoridades;
            foreach (var i in listaDelegaciones)
            {
                idDelegaciones += "-"+i+"-|";

            }
            if (idDelegaciones.Length > 0)
            {
                idDelegaciones = idDelegaciones.Substring(0, idDelegaciones.Length - 1);
            }
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
                                                     Descripcion = x.Descripcion,
                                                     Seleccionado = true
                                                 }
                                               );
            ViewBag.DelegacionesSeleccionadas = lstDelegacionesSeleccionadas.ToList();

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
                                            Descripcion = x.Descripcion,
                                            Seleccionado = true
                                        }
                                      );
            ViewBag.AutoridadesSeleccionadas = lstAutoridadesSeleccionadas.ToList();
            string parametros = String.Format("{0}, {1}, {2}, {3}, {4}", desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), radio, idAutoridades, idDelegaciones);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_DescargaParteOficial");
        }

        private List<GetDescargaParteOficialData_Result> GetDescargaParteOficialData(DateTime desde, DateTime hasta, int radio, string idAutoridades, string listaDelegaciones)
        {
            string idDelegaciones = "";
            foreach (var i in listaDelegaciones)
            {
                idDelegaciones += "-" + i + "-|";

            }
            if (idDelegaciones.Length > 0)
            {
                idDelegaciones = idDelegaciones.Substring(0, idDelegaciones.Length - 1);
            }
            var lista = db.GetDescargaParteOficialData(desde, hasta, radio, idAutoridades, idDelegaciones).ToList();
            // return lista;
            return lista;
        }

        public ActionResult GetReportePorUsuario([FromUri] string [] listaUsuarios, DateTime desde, DateTime hasta)
        {
            string reporteID = "_ReportePorUsuario";
            string nombreReporte = "ReportePorUsuario";
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
                                where listaUsuarios.Contains(r.Id.ToString())
                                select new
                                 {
                                     r.Id,
                                     r.Usuario,
                                     r.Nombre
                                 }).ToList().Distinct()
                                .Select(x => new item
                                {
                                    Id = x.Id.ToString(),
                                    Usuario = x.Usuario + " - " + x.Nombre,
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
            string idUsuarios = "";
            foreach (var i in listaUsuarios)
            {
                idUsuarios += "-" + i + "-|";

            }
            if (idUsuarios.Length > 0)
            {
                idUsuarios = idUsuarios.Substring(0, idUsuarios.Length - 1);
            }
            var lista = db.GetReportePorUsuarioData(idUsuarios, desde, hasta).ToList();
            return lista;
        }




        public ActionResult GetConsultaeImpresionDeBoletas([FromUri] DateTime desde, DateTime hasta, [FromUri] string [] listaDelegacion, [FromUri] string [] listaInspector)
        {
            string reporteID = "_ConsultaeImpresionDeBoletas";
            string nombreReporte = "ConsultaeImpresionDeBoletas";
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
            ViewBag.InspectoresSeleccionados = lstInspectoresSeleccionados.ToList();

            string parametros = String.Format(" {0}, {1}, {2}, {3} ", desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), idDelegacion, idInspector);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ConsultaeImpresionDeBoletas");
        }

        private List<GetConsultaeImpresionDeBoletasData_Result> GetConsultaeImpresionDeBoletasData(DateTime desde, DateTime hasta, string listaDelegacion, string listaInspector)
        {
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
                 lista = db.GetConsultaeImpresionDeBoletasData(desde, hasta, idDelegacion, idInspector).ToList();
            return lista;
        }

        public ActionResult GetReporteStatusActualPlano(int radio, DateTime desde, DateTime hasta, [FromUri] string [] listaDelegacion, [FromUri] string [] listaAutoridad)
        {

            string reporteID = "_ReporteStatusActualPlano";
            string nombreReporte = "StatusActualPlano";
            string idDelegaciones = "";
            string idAutoridades = "";
            Session["ListaDelegaciones"] = listaDelegacion;
            Session["ListaAutoridades"] = listaAutoridad;
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
                                            Descripcion = x.Descripcion,
                                            Seleccionado = true
                                        }
                                      );
            ViewBag.AutoridadesSeleccionadas = lstAutoridadesSeleccionadas.ToList();
            string parametros = String.Format("{0}, {1}, {2}, {3}, {4}", radio, desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), idAutoridades, idDelegaciones);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ReporteStatusActualPlano");
        }

        private List<GetReporteStatusActualPlanoData_Result> GetReporteStatusActualPlanoData(int radio, DateTime desde, DateTime hasta, string listaDelegacion, string listaAutoridad)
        {
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
            var lista = db.GetReporteStatusActualPlanoData(radio, desde, hasta, idDelegaciones, idAutoridades).ToList();
            // return lista;
            return lista;
        }
    }
}