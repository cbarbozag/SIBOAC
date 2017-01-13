using Cosevi.SIBOAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Cosevi.SIBOAC.Controllers
{
    public class ReporteController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Reporte
        public ActionResult Index(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                return View(id);
            }
        }

        public ActionResult GetBitacora(DateTime fechaInicio, DateTime fechaFin, string nombreTabla, string operacion, string usuario, string esReporte)
        {
            var bitacora = GetBitacoraData(fechaInicio, fechaFin, nombreTabla, operacion, usuario);

            if (esReporte == "true")
            {
                return ExportToExcel(bitacora);
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
            string xml = String.Empty;
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(dataToExport.GetType());

            using (System.IO.MemoryStream xmlStream = new System.IO.MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, dataToExport);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                xml = xmlDoc.InnerXml;
            }

            var fName = string.Format("Reporte{0}.xls", DateTime.Now.ToString("s"));

            byte[] fileContents = System.Text.Encoding.UTF8.GetBytes(xml);

            return File(fileContents, "application/vnd.ms-excel", fName);
        }

    }
}