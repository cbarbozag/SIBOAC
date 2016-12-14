using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Cosevi.SIBOAC.Models;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Cosevi.SIBOAC.Controllers
{
    public class ReportePorUsuarioController : Controller
    {

        public ActionResult Index()
        {
            ViewBag.Type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.Message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            return View();
        }
        //private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        //public ViewResult Index(IEnumerable<string> SelectedUsuario, DateTime? FechaInicio, DateTime? FechaFin)
        //{
        //    //faltan validaciones de mensajes.

        //    if (SelectedUsuario == null || FechaInicio == null || FechaFin == null)
        //        return View();
        //    var list =
        //        (
        //        from p in db.PARTEOFICIAL
        //        where p.usuario_entregaPlano == SelectedUsuario.ToString() && (p.Fecha >= FechaInicio && p.Fecha <= FechaFin)
        //        join b in db.BOLETA on new { fuente = p.Fuente, serie = p.Serie, numeroParte = p.NumeroParte }
        //        equals new { fuente = b.fuente_parteoficial, serie = b.serie_parteoficial, numeroParte = b.numeroparte }
        //        where b.numeroparte == p.NumeroParte
        //        join u in db.USUARIO on new { SelectedUsuario = p.usuario_entregaPlano } equals new { SelectedUsuario = u.Usuario }
        //        select new
        //        {
        //            Usuario = u.Usuario,
        //            Autoridad = b.codigo_autoridad_registra,
        //            FechaAccidente = p.Fecha,
        //            Serie = b.serie,
        //            NumeroParte = b.numeroparte,
        //            Boletas = b.numero_boleta,
        //            FechaDescarga = b.fecha_descarga,
        //            ClasePlaca = b.clase_placa,
        //            CodigoPlaca = b.codigo_placa,
        //            NumeroPlaca = b.numero_placa,
        //            EstadoPlano = p.StatusPlano

        //        }).ToList().Distinct()
        //        .Select(x => new ReporteUsuario
        //        {
        //            Usuario = x.Usuario,
        //            Autoridad = x.Autoridad,
        //            FechaAccidente = x.FechaAccidente,
        //            Serie = x.Serie.ToString(),
        //            NumeroParte = x.NumeroParte,
        //            Boletas = x.Boletas,
        //            FechaDescarga = x.FechaDescarga,
        //            ClasePlaca = x.ClasePlaca,
        //            CodigoPlaca = x.CodigoPlaca,
        //            NumeroPlaca = x.NumeroPlaca,
        //            EstadoPlano = x.EstadoPlano

        //        }).OrderBy(x => x.Usuario);

        //    ViewBag.Datos = list;
        //    Session["Datos"] = list;
        //    return View();
        //}

        //// GET: ReportePorUsuario
        //public ActionResult Index()
        //{

        //    List<SelectListItem> listSelectListItems = new List<SelectListItem>();

        //    foreach (USUARIO usuario in db.USUARIO)
        //    {
        //        SelectListItem selectList = new SelectListItem()
        //        {
        //            //Text = city.Name,
        //            //Value = city.ID.ToString(),
        //            //Selected = city.IsSelected
        //            //Value = parteOfi.NumeroParte,

        //            Text = usuario.Usuario
        //        };
        //        listSelectListItems.Add(selectList);
        //    }

        //    USUARIO usuarios = new USUARIO()
        //    {
        //        UsuariosLis = listSelectListItems
        //    };

        //    return View(usuarios);
        //}

        //[HttpPost]
        //public string Index(IEnumerable<string> SelectedUsuario)
        //{
        //    if (SelectedUsuario == null)
        //    {
        //        return "No seleccionó ningún usuario";
        //    }
        //    else
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        sb.Append("You selected – " +string.Join(",", SelectedUsuario));
        //        return sb.ToString();
        //    }
        //}

        //public ActionResult Exportar(string Excel)
        //{
        //    if (Excel == "Excel")
        //    {
        //        return ExportToExcel();
        //    }
        //    else
        //        return DownloadPartialViewPDF();

        //}
        //public ActionResult ExportToExcel()
        //{
        //    var grid = new GridView();
        //    grid.DataSource = Session["Datos"];
        //    grid.DataBind();
        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=ReportePorUsuario.xls");
        //    Response.ContentType = "application/ms-excel";

        //    Response.Charset = "";
        //    StringWriter sw = new StringWriter();

        //    HtmlTextWriter htw = new HtmlTextWriter(sw);

        //    grid.RenderControl(htw);

        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.End();

        //    return View();
        //}


        //public ActionResult DownloadPartialViewPDF()
        //{
        //    var model = Session["Datos"];
        //    ViewBag.Datos = Session["Datos"];
        //    //Code to get content
        //    return new Rotativa.PartialViewAsPdf("_ListaActividad", model) { FileName = "ReporteDiarioOficial.pdf" };
        //}
    }
}