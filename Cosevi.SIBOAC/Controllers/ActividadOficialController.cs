using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cosevi.SIBOAC.Controllers
{
    [Authorize]
    public class ActividadOficialController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        // GET: ActividadOficial
        public ViewResult Index(string CodigoInspector, DateTime? FechaInicio, DateTime? FechaFin)
        {
            //faltan validaciones de mensajes.

            if (CodigoInspector == null || FechaInicio == null || FechaFin == null)
                return View();
            var list =
                (
                from b in db.BOLETA
                where b.codigo_inspector == CodigoInspector && (b.fecha_hora_boleta >= FechaInicio && b.fecha_hora_boleta <=FechaFin)
                join a in db.ARTICULOXBOLETA on new { fuente = b.fuente, serie = b.serie, numeroBoleta = b.numero_boleta }
                equals new { fuente = a.fuente, serie = a.serie, numeroBoleta = a.numero_boleta }          
                where a.multa>0  
                join i in db.INSPECTOR on new { codInspector = b.codigo_inspector } equals new { codInspector = i.Id }                
                select new
                {
                   Fuente= a.fuente,
                   Serie=  a.serie,
                   NumeroBoleta = a.numero_boleta,
                   FechaHoraBoleta = b.fecha_hora_boleta,
                   ClasePlaca = b.clase_placa,
                   CodigoPlaca = b.codigo_placa,
                   NumeroPlaca = b.numero_placa,
                   CodigoArticulo= a.codigo_articulo,
                   CodigoInspector= b.codigo_inspector,
                   NombreInspector=  i.Nombre
                }).ToList().Distinct()
                .Select(x => new ActividadOficial
                {
                    Fuente = x.Fuente,
                    Serie = x.Serie,
                    NumeroBoleta = x.NumeroBoleta,
                    FechaHoraBoleta = x.FechaHoraBoleta,                 
                    ClasePlaca = x.ClasePlaca,
                    CodigoPlaca = x.CodigoPlaca,
                    NumeroPlaca = x.NumeroPlaca,
                    CodigoArticulo = x.CodigoArticulo,
                    CodigoInspector = x.CodigoInspector,
                    NombreInspector = x.NombreInspector

                }).OrderBy(x=>x.NumeroBoleta);

            ViewBag.Datos = list;
            Session["Datos"] = list;
            return View();
        }

        public ActionResult Exportar(string Excel)
        {
            if (Excel == "Excel")
            {
                return ExportToExcel();
            }
            else
                return DownloadPartialViewPDF();

        }
        public ActionResult ExportToExcel()
        {
            var grid = new GridView();
            grid.DataSource = Session["Datos"];
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ReporteDiarioOficial.xls");
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


        public ActionResult DownloadPartialViewPDF()
        {
            var model = Session["Datos"];
            ViewBag.Datos = Session["Datos"];
            //Code to get content
            return new Rotativa.PartialViewAsPdf("_ListaActividad", model) { FileName = "ReporteDiarioOficial.pdf" };
        }

    }
}