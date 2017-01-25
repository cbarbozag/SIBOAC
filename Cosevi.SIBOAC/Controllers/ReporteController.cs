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

        private void GetData()
        {
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
            string reporteID = "_Bitacora";
            string nombreReporte = "rptBitacora";
            string parametros = String.Format("{0},{1},{2},{3},{4}", fechaInicio.ToString("yyyy-MM-dd"), fechaFin.ToString("yyyy-MM-dd"), nombreTabla, operacion, usuario);

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData();

            return View("_Bitacora");
        }

        private List<BitacoraSIBOAC> GetBitacoraData(DateTime fechaInicio, DateTime fechaFin, string nombreTabla, string operacion, string usuario)
        {
            var bitacora = db.GetBitacoraData(fechaInicio, fechaFin, nombreTabla, usuario, operacion).ToList();
            return bitacora;
        }

        public ActionResult GetDescargaBoleta(int TipoFecha, DateTime fechaInicio, DateTime fechaFin)
        {
            string reporteID = "_DescargaBoleta";
            string nombreReporte = "rptDescargaBolea";
            string parametros = String.Format("{0},{1},{2}",TipoFecha, fechaInicio.ToString("yyyy-MM-dd"), fechaFin.ToString("yyyy-MM-dd"));

            ViewBag.ReporteID = reporteID;
            ViewBag.NombreReporte = nombreReporte;
            ViewBag.Parametros = parametros;
            GetData();

            return View("_DescargaBoleta");
        }
    }
}