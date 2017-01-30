﻿using Cosevi.SIBOAC.Models;
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

                   
                    break;
                case "_DescargaBoleta":

                    break;
                case "_DescargaParteOficial":

                  
                    var listaDelegaciones = (from r in db.DELEGACION
                                 select new {
                                     r.Id,
                                     r.Descripcion }).ToList().Distinct()
                                 .Select(x => new item
                                 { Id= x.Id,
                                   Descripcion = x.Descripcion}
                                 );
                    ViewBag.Delegaciones = listaDelegaciones;
                    var listaAutoridades = (from r in db.AUTORIDAD
                                 select new
                                 {
                                     r.Id,
                                     r.Descripcion
                                 }).ToList().Distinct()
                             .Select(x => new item
                             {
                                 Id = x.Id,
                                 Descripcion = x.Descripcion
                             }
                             );
                    ViewBag.Autoridades = listaAutoridades;
                    break;
                case "_ConsultaeImpresionDeParteOficial":
                  
                    break;
                case "_ReportePorUsuario":
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
                                     Usuario = x.Usuario+" - "+x.Nombre,                                     
                                 }
                                 );
                    ViewBag.Usuarios = listaUsuarios;
                    break;
                case "_ConsultaeImpresionDeBoletas":

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

        private List<BitacoraSIBOAC> GetBitacoraData(DateTime fechaInicio, DateTime fechaFin, string nombreTabla, string operacion, string usuario)
        {
            var bitacora = db.GetBitacoraData(fechaInicio, fechaFin, nombreTabla, usuario, operacion).ToList();
            return bitacora;
        }

        public ActionResult GetReporteDescargaInspector(DateTime hasta, DateTime desde, string numeroHH, string codigoInspector)
        {
            string reporteID = "_DescargaInspector";
            string nombreReporte = "DescargaInspector";
            string parametros = String.Format("{0},{1},{2},{3}", hasta.ToString("yyyy-MM-dd"), desde.ToString("yyyy-MM-dd"), numeroHH, codigoInspector);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_DescargaInspector");
        }

        private List<GetDescargaInspectorData_Result> GetDescargaInspectorData(DateTime hasta, DateTime desde, string numeroHH, string codigoInspector)
        {
            var lista = db.GetDescargaInspectorData(hasta, desde, numeroHH, codigoInspector).ToList();

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
        public ActionResult GetReporteDescargaParteOficial(DateTime desde, DateTime hasta, int radio, string  listaAutoridades,[FromUri] string [] listaDelegaciones)
        {

            string reporteID = "_DescargaParteOficial";
            string nombreReporte = "DescargaParteOficial";
            string idDelegaciones = "";
            foreach (var i in listaDelegaciones)
            {
                idDelegaciones += "-"+i+"-|";

            }
            if (idDelegaciones.Length > 0)
            {
                idDelegaciones = idDelegaciones.Substring(0, idDelegaciones.Length - 1);
            }
            string parametros = String.Format("{0}, {1}, {2}, {3}, {4}", desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), radio,"321", idDelegaciones);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_DescargaParteOficial");
        }
        private List<GetDescargaParteOficialData_Result1> GetDescargaParteOficialData(DateTime desde, DateTime hasta, int radio, string idAutoridades, string listaDelegaciones)
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
            foreach (var i in listaUsuarios)
            {
                idUsuarios += "-" + i + "-|";

            }
            if (idUsuarios.Length > 0)
            {
                idUsuarios = idUsuarios.Substring(0, idUsuarios.Length - 1);
            }
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




        public ActionResult GetConsultaeImpresionDeBoletas([FromUri] DateTime desde, DateTime hasta, [FromUri] string idDelegaciones, [FromUri] string idInspectores)
        {
            string reporteID = "_ConsultaeImpresionDeBoletas";
            string nombreReporte = "ConsultaeImpresionDeBoletas";
            string parametros = String.Format(" {0}, {1}, {2}, {3} ", desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), idDelegaciones, idInspectores);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ConsultaeImpresionDeBoletas");
        }

        private List<GetConsultaeImpresionDeBoletasData_Result> GetConsultaeImpresionDeBoletasData(DateTime desde, DateTime hasta, [FromUri] string idDelegaciones, [FromUri] string idInspectores)
        {
            var lista = db.GetConsultaeImpresionDeBoletasData(desde, hasta, idDelegaciones, idInspectores).ToList();
            return lista;
        }
    }
}