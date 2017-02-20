using Cosevi.SIBOAC.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cosevi.SIBOAC.Reports
{
    public partial class ViewerPage : System.Web.UI.Page
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        private SIBOACSecurityEntities dbSecurity = new SIBOACSecurityEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string reporteID = Request.QueryString["reporteID"];
                string nombreReporte = Request.QueryString["nombreReporte"];
                string parametros = Request.QueryString["parametros"];

                if (String.IsNullOrEmpty(reporteID) || String.IsNullOrEmpty(nombreReporte) || String.IsNullOrEmpty(parametros))
                {
                    return;
                }

                ReportViewer1.Reset();
                ReportViewer1.LocalReport.ReportPath = Server.MapPath(String.Format("~/Reports/{0}.rdlc", nombreReporte));
            
                switch (reporteID)
                {
                    case "_ReimpresionDeBoletasDeCampo":

                        ReportViewer1.LocalReport.EnableExternalImages = true;

                        string[] param = parametros.Split(',');
                        int serie = Convert.ToInt32(param[0]);
                        decimal numero_boleta = Convert.ToDecimal(param[1]);

                        var fuente = (db.BOLETA.Where(a => a.serie == serie && a.numero_boleta == numero_boleta).Select(a => a.fuente).ToList());
                        string CodigoFuente = fuente.ToArray().FirstOrDefault() == null ? "0" : fuente.ToArray().FirstOrDefault().ToString();
                        string ruta = ConfigurationManager.AppSettings["DownloadFilePath"];
                        //var path = Server.MapPath(ruta);
                        var fileUsuario = string.Format("{0}{1}{2}-i.png", CodigoFuente, serie, numero_boleta);
                        var fileInspector = string.Format("{0}{1}{2}-u.png", CodigoFuente, serie, numero_boleta);
                        var fullPathUsuario = Path.Combine(ruta, fileUsuario);
                        var fullPathInspector = Path.Combine(ruta, fileInspector);

                        //Server.MapPath(fullPath)
                        string imgFirmaUsuarioPath = new Uri(fullPathUsuario).AbsoluteUri;
                        string imgFirmaInspectorPath = new Uri(fullPathInspector).AbsoluteUri;

                        //Array que contendrá los parámetros
                        ReportParameter[] parameters = new ReportParameter[2];
                       
                        //Establecemos el valor de los parámetros
                        parameters[0] = new ReportParameter("ImagenFirmaUsuarioPath", imgFirmaUsuarioPath);
                        parameters[1] = new ReportParameter("ImagenFirmaInspectorPath", imgFirmaInspectorPath);
                        ReportViewer1.LocalReport.SetParameters(parameters);
                     
                        break;
 
                }
                ReportDataSource RDS = new ReportDataSource("DataSet1", GetData(reporteID, parametros));
                ReportViewer1.LocalReport.DataSources.Add(RDS);
                ReportViewer1.LocalReport.Refresh();
                btnPrint.Visible = true;
                ReportViewer1.LocalReport.EnableHyperlinks = true;

            }
        }

        private object GetData(string reporteID, string parametros)
        {
            object result = null;

            switch (reporteID)
            {
                case "_Bitacora":
                    result = GetBitacoraData(parametros);
                    break;
                case "_DescargaInspector":
                    result = GetDescargaInspectorData(parametros);
                    break;
                case "_DescargaBoleta":
                    result = GetDescargaBoletaData(parametros);
                    break;
                case "_DescargaParteOficial":
                    result = GetReportePorDelegacionAutoridadData(parametros);
                    break;
                case "_ReporteListadoMultaFija":
                    result = GetReporteListadoMultafijaData(parametros);
                    break;
                case "_ReporteListadoParteOficial":
                    result = GetReporteListadoParteOficialData(parametros);
                    break;
                case "_ConsultaeImpresionDeParteOficial":
                    result = GetConsultaeImpresionDeParteOficialData(parametros);
                    break;
                case "_ReportePorUsuario":
                    result = GetReportePorUsuarioData(parametros);
                    break;

                case "_ConsultaeImpresionDeBoletas":
                    result = GetConsultaeImpresionDeBoletasData(parametros);
                    break;

                case "_ReporteStatusActualPlano":
                    result = GetReporteStatusActualPlanoData(parametros);
                    break;
                case "_ActividadOficial":
                    result = GetActividadOficialData(parametros);
                    break;
                case "_BitacoraAplicacion":
                    result = GetBitacoraAplicacionData(parametros);
                    break;
                case "_ReimpresionDeBoletasDeCampo":
                    result = GetReimpresionDeBoletasDeCampoData(parametros);
                    break;
                default:
                    break;
            }

            return result;
        }

        #region Datos
        private List<BitacoraSIBOAC> GetBitacoraData(string parametros)
        {
            string[] param = parametros.Split(',');
            DateTime fechaInicio = Convert.ToDateTime(param[0]);
            DateTime fechaFin = Convert.ToDateTime(param[1]);
            string nombreTabla = param[2];
            string operacion = param[3];
            string usuario = param[4];

            var bitacora = db.GetBitacoraData(fechaInicio, fechaFin, nombreTabla, usuario, operacion).ToList();
            return bitacora;
        }
        private List<GetActividadOficialData_Result> GetActividadOficialData(string parametros)
        {
            //SE agrega el codigo del usuario
         
            var usuarioSistema = User.Identity.Name;
      
            string[] param = parametros.Split(',');
            string CodigoInspector = param[0];
            DateTime fechaInicio = Convert.ToDateTime(param[1]);
            DateTime fechaFin = Convert.ToDateTime(param[2]);
        
            var actividadOificial = db.GetActividadOficialData(CodigoInspector,fechaInicio, fechaFin, usuarioSistema).ToList();
            return actividadOificial;
        }

        private List<GetDescargaInspectorData_Result> GetDescargaInspectorData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            DateTime fechaInicio = Convert.ToDateTime(param[0]);
            DateTime fechaFin = Convert.ToDateTime(param[1]);            
            string codigoOficial = param[2].Replace("|", ",").Replace("-", "").Trim(); ;
          
            var lista = db.GetDescargaInspectorData(fechaInicio, fechaFin, codigoOficial,usuarioSistema).ToList();
            return lista;
        }

        private List<GetDescargaBoletaData_Result> GetDescargaBoletaData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int TipoFecha = Convert.ToInt32(param[0]);
            DateTime fechaInicio = Convert.ToDateTime(param[1]);
            DateTime fechaFin = Convert.ToDateTime(param[2]);
  

            var lista = db.GetDescargaBoletaData(TipoFecha,fechaInicio, fechaFin,usuarioSistema).ToList();
            return lista;
        }


        private List<GetReportePorDelegacionAutoridadData_Result> GetReportePorDelegacionAutoridadData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int Valor = Convert.ToInt32(param[0]);
            DateTime FechaDesde = Convert.ToDateTime(param[1]);
            DateTime FechaHasta = Convert.ToDateTime(param[2]);    
            string idAutoridades =param[3].Replace("|", ",").Replace("-", "").Trim();
            string idDelegaciones = param[4].Replace("|",",").Replace("-","").Trim();

            var lista = db.GetReportePorDelegacionAutoridadData(Valor,FechaDesde, FechaHasta, idAutoridades, idDelegaciones,usuarioSistema).ToList();
            return lista;
        }

        private List<GetReporteListadoMultaFijaData_Result> GetReporteListadoMultafijaData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int Valor = Convert.ToInt32(param[0]);
            DateTime FechaDesde = Convert.ToDateTime(param[1]);
            DateTime FechaHasta = Convert.ToDateTime(param[2]);
            string idInspectores = param[3].Replace("|", ",").Replace("-", "").Trim();
            string idDelegaciones = param[4].Replace("|", ",").Replace("-", "").Trim();

            var lista = db.GetReporteListadoMultaFijaData(FechaDesde, FechaHasta, Valor, idInspectores, idDelegaciones,usuarioSistema).ToList();
            return lista;
        }
        private List<GetReporteListadoParteOficialData_Result> GetReporteListadoParteOficialData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int Valor = Convert.ToInt32(param[0]);
            DateTime FechaDesde = Convert.ToDateTime(param[1]);
            DateTime FechaHasta = Convert.ToDateTime(param[2]);
            string idInspectores = param[3].Replace("|", ",").Replace("-", "").Trim();
            string idDelegaciones = param[4].Replace("|", ",").Replace("-", "").Trim();

            var lista = db.GetReporteListadoParteOficialData(FechaDesde, FechaHasta, Valor, idInspectores, idDelegaciones,usuarioSistema).ToList();
            return lista;
        }

        private List<GetConsultaeImpresionDeParteOficialData_Result> GetConsultaeImpresionDeParteOficialData(string parametros)
        {
            string[] param = parametros.Split(',');
            int TipoConsulta = Convert.ToInt32(param[0]);
            string  Parametro1 =param[1];
            string Parametro2 = param[2];
            string Parametro3 = param[3];
            string Parametro4 = param[4];

            if (Parametro3 == "null")
            {
                db.Database.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                var lista1 = db.GetConsultaeImpresionDeParteOficialData(TipoConsulta, Parametro1, Parametro2, "-0","-0").ToList();
                return lista1;
            }
            db.Database.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
            var lista = db.GetConsultaeImpresionDeParteOficialData(TipoConsulta, Parametro1, Parametro2, Parametro3,Parametro4).ToList();
            return lista;
        }

        private List<GetReportePorUsuarioData_Result> GetReportePorUsuarioData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            string IdUsuario = param[0].Replace("|",",").Replace("-","").Trim();
            DateTime fechaInicio = Convert.ToDateTime(param[1]);
            DateTime fechaFin = Convert.ToDateTime(param[2]);


            var lista = db.GetReportePorUsuarioData(IdUsuario, fechaInicio, fechaFin,usuarioSistema).ToList();
            return lista;
        }

        private List<GetBitacoraDeAplicacion_Result> GetBitacoraAplicacionData(string parametros)
        {
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            string[] param = parametros.Split(',');
            string tipoConsulta1 = param[0];
            string tipoConsulta2 = param[1];
            
            if (param[2] != "null")
            {
                 fechaInicio = Convert.ToDateTime(param[2]);
            }            
            if (param[3] != "null")
            {
                fechaFin = Convert.ToDateTime(param[3]);
            }

            fechaInicio.ToString("dd-MM-yyyy");
            fechaFin.ToString("dd-MM-yyyy");
            string IdUsuario = param[4].Replace("|", ",").Replace("-", "").Trim();


            var lista = db.GetBitacoraDeAplicacion(tipoConsulta1, tipoConsulta2, fechaInicio, fechaFin, IdUsuario).ToList();
            return lista;
        }



        private List<GetConsultaeImpresionDeBoletasData_Result> GetConsultaeImpresionDeBoletasData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            DateTime FechaDesde = Convert.ToDateTime(param[0]);
            DateTime FechaHasta = Convert.ToDateTime(param[1]);
            string idDelegacion = param[2].Replace("|", ",").Replace("-", "").Trim();
            string idInspector = param[3].Replace("|", ",").Replace("-", "").Trim(); 

            var lista = db.GetConsultaeImpresionDeBoletasData(FechaDesde, FechaHasta, idDelegacion, idInspector,usuarioSistema).ToList();
            return lista;
        }

        private List<GetReporteStatusActualPlanoData_Result> GetReporteStatusActualPlanoData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int statusPlano = Convert.ToInt32(param[0]);
            DateTime FechaDesde = Convert.ToDateTime(param[1]);
            DateTime FechaHasta = Convert.ToDateTime(param[2]);
            string idDelegaciones = param[3].Replace("|", ",").Replace("-", "").Trim();
            string idAutoridades = param[4].Replace("|", ",").Replace("-", "").Trim();
            

            var lista = db.GetReporteStatusActualPlanoData(statusPlano, FechaDesde, FechaHasta, idAutoridades, idDelegaciones,usuarioSistema).ToList();
            return lista;
        }

        private List<GetReimpresionDeBoletasDeCampoData_Result> GetReimpresionDeBoletasDeCampoData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            string Serie = param[0];
            string NumeroBoleta = param[1];         

            var lista = db.GetReimpresionDeBoletasDeCampoData(Serie, NumeroBoleta,usuarioSistema).ToList();
            return lista;
        }

        #endregion


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            // Elimina archivos creados con mas de 10 minutos respecto a la hora actual
            var files = new DirectoryInfo(HttpContext.Current.Server.MapPath("")).GetFiles("*.pdf");
            foreach (var file in files)
            {
                if (DateTime.UtcNow - file.CreationTimeUtc > TimeSpan.FromMinutes(10))
                {
                    File.Delete(file.FullName);
                }
            }

            string fileName = Path.ChangeExtension(Path.GetTempFileName(), ".pdf");
            string fileReportName = "Reporte" + DateTime.Now.ToString("HHmmss") + ".pdf";

            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType,
               out encoding, out extension, out streamids, out warnings);

            FileStream fs = new FileStream(String.Format("{0}", fileName),
            FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            //Open existing PDF
            Document document = new Document(PageSize.LETTER);
            PdfReader reader = new PdfReader(fileName);

            //Getting a instance of new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath(fileReportName), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);

            float width = psize.Width;
            float height = psize.Height;

            //Add Page to new document
            while (i < n)
            {
                document.NewPage();
                p++;
                i++;

                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }

            //Attach javascript to the document
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();

            //Attach pdf to the iframe
            frmPrint.Attributes["src"] = fileReportName;

        }
    }
}