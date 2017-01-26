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

                   
                    break;
                case "_DescargaBoleta":

                    break;
                case "_DescargaParteOficial":

                    break;
                case "_ConsultaeImpresionDeParteOficial":

                    break;
                default:
                    break;
            }

       

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

        public ActionResult GetConsultaeImpresionParteOficial([FromUri] int idRadio, [FromUri] string serieParte,
            [FromUri] string numeroParte, [FromUri] int? serieBoleta, [FromUri] decimal? numeroBoleta, [FromUri] string tipoId,
            [FromUri] string numeroID, [FromUri] string numeroPlaca, [FromUri] string codigoPlaca, [FromUri] string clasePlaca)
        {

            string reporteID = "_ConsultaeImpresionDeParteOficial";
            string nombreReporte = "ConsultaeImpresionDeParteOficial";
            string parametros = "";
            if (idRadio == 1)
            {
                parametros = String.Format("{0},{1},{2},{3}", idRadio.ToString(), serieBoleta.ToString(), numeroBoleta.ToString(), "null");

            }
            if(idRadio ==2)
            {
                parametros = String.Format("{0},{1},{2},{3}", idRadio.ToString(), serieParte, numeroParte, "null");

            }
            if (idRadio == 3)
            {
                parametros = String.Format("{0},{1},{2},{3}", idRadio.ToString(), tipoId, numeroID, "null");

            }
            if (idRadio == 4)
            {
                parametros = String.Format("{0},{1},{2},{3}", idRadio.ToString(), numeroPlaca, codigoPlaca, clasePlaca);


            }


            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_ConsultaeImpresionDeParteOficial");
        }
        private List<GetDescargaInspectorData_Result> GetDescargaInspectorData(DateTime hasta, DateTime desde, string numeroHH, string codigoInspector)
        {
            var lista = db.GetDescargaInspectorData(hasta, desde, numeroHH, codigoInspector).ToList();

            return lista;
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


        public ActionResult GetReporteDescargaParteOficial(DateTime desde, DateTime hasta, int radio, [FromUri] string idAutoridades, [FromUri] string idDelegaciones)
        {

            string reporteID = "_DescargaParteOficial";
            string nombreReporte = "DescargaParteOficial";
            string parametros = String.Format("{0}, {1}, {2}, {3}, {4}", desde.ToString("yyyy-MM-dd"), hasta.ToString("yyyy-MM-dd"), radio, idAutoridades, idDelegaciones);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData(reporteID);

            return View("_DescargaInspector");
        }
        private List<GetDescargaParteOficialData_Result> GetDescargaParteOficialData(DateTime desde, DateTime hasta, int radio, [FromUri] string idAutoridades, [FromUri] string idDelegaciones)
        {
            var lista = db.GetDescargaParteOficialData(desde, hasta, radio, idAutoridades, idDelegaciones).ToList();
            return lista;
        }
    }
}