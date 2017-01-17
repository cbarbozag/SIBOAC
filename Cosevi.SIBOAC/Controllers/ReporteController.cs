using Cosevi.SIBOAC.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cosevi.SIBOAC.Controllers
{
    public class ReporteController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        private SIBOACSecurityEntities dbSecurity = new SIBOACSecurityEntities();

        private void GetData() {
            List<SIBOACTablas> tablas = dbSecurity.SIBOACTablas.ToList();
            List<SIBOACUsuarios> usuarios = dbSecurity.SIBOACUsuarios.Where(u => u.Activo.Value).ToList();

            tablas.Insert(0, new SIBOACTablas() { Id = 0, Descripcion = "Todas" });
            ViewBag.Tablas = tablas;

            usuarios.Insert(0, new SIBOACUsuarios() { Id = 0, Usuario = "Todos" });
            ViewBag.Usuarios = usuarios;
        }

        // GET: Reporte
        [SessionExpire]
        public ActionResult Index(string id)
        {
            GetData();
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
            var bitacora = GetBitacoraData(fechaInicio, fechaFin, nombreTabla, operacion, usuario);

            GetData();

            if (tipoReporte == "xls")
            {
                return ExportToExcel(bitacora);
            }
            else
                if (tipoReporte == "pdf")
            {
                //return ExportToPDF(bitacora);
                return DownloadPDF(bitacora);
            }
            else
            {
                return View("_Bitacora", bitacora);
            }
        }

        private List<BitacoraSIBOAC> GetBitacoraData(DateTime fechaInicio, DateTime fechaFin, string nombreTabla, string operacion, string usuario)
        {
            var bitacora = db.GetBitacoraData(fechaInicio, fechaFin, nombreTabla, usuario, operacion).ToList();
            return bitacora;
        }

        public ActionResult ExportToExcel(object dataToExport)
        {
            var grid = new GridView();
            var fName = string.Format("Reporte{0}.xls", DateTime.Now.ToString("s"));

            grid.DataSource = dataToExport;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", String.Format( "attachment; filename={0}", fName));
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();
        }

        public ActionResult DownloadPDF(object dataToExport)
        {
            var grid = new GridView();

            grid.DataSource = dataToExport;
            grid.DataBind();
            
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            //because iTextSharp takes XHTML and css to pdf.so we need to pass data in XHTML format 
            string export = String.Format("< html >< head >{0}</ head >< body >{1}</ body ></ html > ", " < style > table{ border - spacing: 10px; border - collapse: separate; }</ style > ", sw.ToString());
            //converting all data into bytes in UTF-8 format
            var bytes = System.Text.Encoding.UTF8.GetBytes(export);
            //Now prepare docment using iTextsharp module
            //And print using PDF writer
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.LETTER.Rotate(), 10, 10, 10, 10);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();

                var XmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                XmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return new FileStreamResult(output, "application/pdf");
            }
        }

        public ActionResult ExportToPDF(object dataToExport)
        {
            var fName = string.Format("Reporte{0}.pdf", DateTime.Now.ToString("s"));

            return new Rotativa.PartialViewAsPdf("_Bitacora", dataToExport) { FileName = fName };
        }
    }
}