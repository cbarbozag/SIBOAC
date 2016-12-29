using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Controllers
{
   
    public class ReimpresionDeBoletasController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        // GET: ReimpresionDeBoletas
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="numero_boleta"></param>
        /// <returns></returns>
        public ViewResult Index(int? serie, decimal? numero_boleta)
        {
            if (serie == null && numero_boleta == null)
                return View();
            ViewBag.Datos = null;
          
            ViewBag.type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.message = TempData["Message"] != null ? TempData["Message"].ToString() : "";

            var boleta =
                (
                from BOLETA in db.BOLETA
                where BOLETA.serie ==serie && BOLETA.numero_boleta==numero_boleta
                join PERSONA in db.PERSONA
                     on new { BOLETA.tipo_ide, BOLETA.identificacion, NumeroBoleta = BOLETA.numero_boleta, Serie = BOLETA.serie }
                 equals new { PERSONA.tipo_ide, PERSONA.identificacion, PERSONA.NumeroBoleta, PERSONA.Serie } into PERSONA_join
                from PERSONA in PERSONA_join.DefaultIfEmpty()
                join TIPO_IDENTIFICACION in db.TIPO_IDENTIFICACION on new { Id = BOLETA.tipo_ide } equals new { Id = TIPO_IDENTIFICACION.Id } into TIPO_IDENTIFICACION_join
                from TIPO_IDENTIFICACION in TIPO_IDENTIFICACION_join.DefaultIfEmpty()
                join ROLPERSONA in db.ROLPERSONA on new { Id = BOLETA.codrol } equals new { Id = ROLPERSONA.Id } into ROLPERSONA_join
                from ROLPERSONA in ROLPERSONA_join.DefaultIfEmpty()
                join DELEGACION in db.DELEGACION on new { Id = BOLETA.codigo_delegacion } equals new { Id = DELEGACION.Id }
                join VEHICULO in db.VEHICULO
                     on new { clase = BOLETA.clase_placa, codigo = BOLETA.codigo_placa, placa = BOLETA.numero_placa, Serie = BOLETA.serie, NumeroBoleta = BOLETA.numero_boleta }
                 equals new { VEHICULO.clase, VEHICULO.codigo, VEHICULO.placa, VEHICULO.Serie, VEHICULO.NumeroBoleta } into VEHICULO_join
                from VEHICULO in VEHICULO_join.DefaultIfEmpty()
                join TIPOVEH in db.TIPOVEH on new { Id = (int)VEHICULO.codveh } equals new { Id = TIPOVEH.Id } into TIPOVEH_join
                from TIPOVEH in TIPOVEH_join.DefaultIfEmpty()
                join CARROCERIA in db.CARROCERIA on new { Id = (int)VEHICULO.tipo_carroceria } equals new { Id = CARROCERIA.Id } into CARROCERIA_join
                from CARROCERIA in CARROCERIA_join.DefaultIfEmpty()
                join MARCA in db.MARCA on new { Id = VEHICULO.marca } equals new { Id = MARCA.Id } into MARCA_join
                from MARCA in MARCA_join.DefaultIfEmpty()
                join OficinaParaImpugnars in db.OficinaParaImpugnars on new { Id = BOLETA.codOficinaImpugnacion } equals new { Id = OficinaParaImpugnars.Id } into OficinaParaImpugnars_join
                from OficinaParaImpugnars in OficinaParaImpugnars_join.DefaultIfEmpty()
                join INSPECTOR in db.INSPECTOR on new { Id = BOLETA.codigo_inspector } equals new { Id = INSPECTOR.Id }
                join GENERALES in db.GENERALES on new { Inspector = BOLETA.codigo_inspector } equals new { Inspector = GENERALES.Inspector } into GENERALES_join
                from GENERALES in GENERALES_join.DefaultIfEmpty()
                select new
                {
                    BOLETA.fuente,
                    Serie = BOLETA.serie,
                    NumeroBoleta = BOLETA.numero_boleta,
                    FechaHoraBoleta = BOLETA.fecha_hora_boleta,
                    CodigoDelegacion = BOLETA.codigo_delegacion,
                    DescripcionDelegacion = DELEGACION.Descripcion,
                    CodigoAutoridad = BOLETA.codigo_autoridad_registra,
                    DescripcionAutoridad = (((from AUTORIDAD in db.AUTORIDAD
                                               where
                                                 AUTORIDAD.Id == BOLETA.codigo_autoridad_registra
                                               select new
                                               {
                                                   AUTORIDAD.Descripcion
                                               }).Distinct()).FirstOrDefault().Descripcion),
                    CodigoRol = BOLETA.codrol,
                    DescripcionRol = ROLPERSONA.Descripcion,
                    Usuario = PERSONA.apellido1 + " " + PERSONA.apellido2 + " " + PERSONA.nombre,
                    BOLETA.tipo_ide,
                    TipoDocumento = TIPO_IDENTIFICACION.Descripcion,
                    Sexo = PERSONA.sexo,
                    FechaNacimiento = PERSONA.FechaNacimiento,
                    DireccionUsuario = PERSONA.senasDireccion,
                    TipoLicencia = BOLETA.tipo_lic,
                    Identificacion = BOLETA.identificacion,
                    LugarHechos = BOLETA.lugar_hechos,
                    Km = BOLETA.kilometro,
                    Placa = BOLETA.numero_placa,
                    codveh = (int?)VEHICULO.codveh,
                    DescripcionTipoAutomovil = TIPOVEH.Descripcion,
                    tipo_carroceria = (int?)VEHICULO.tipo_carroceria,
                    DescripcionCarroceria = CARROCERIA.Descripcion,
                    CodigoMarca = BOLETA.marca,
                    DescripcionMarca = MARCA.Descripcion,
                    RevisionTecnica = VEHICULO.rev_tecnica == "1" ? "SI" : "NO",
                    CodigoOficinaImpugna = BOLETA.codOficinaImpugnacion,
                    DescripcionOficinaImpugna = OficinaParaImpugnars.Descripcion,
                    NivelGases = BOLETA.humo,
                    Velocidad = BOLETA.velocidad,
                    CodigoInspector = BOLETA.codigo_inspector,
                    NombreInspector = INSPECTOR.Nombre,
                    ParteOficial = (BOLETA.fuente_parteoficial + "-" + BOLETA.serie_parteoficial + "-" + BOLETA.numeroparte),
                    ClasePlaca = BOLETA.clase_placa,
                    CodigoPlaca = BOLETA.codigo_placa,
                    PiePagina = GENERALES.Piepagina
                }).ToList().Take(1);

            if (boleta.Count() == 0 || boleta.FirstOrDefault() == null)
            {

                ViewBag.type = "";
                ViewBag.message = "";
                ViewBag.type = TempData["Type"] = "error";
                ViewBag.message = TempData["Message"] = "No se encontró información la boleta  " + serie + " " + numero_boleta;
                return View();
            }
            InformacionBoleta infoBoleta = new InformacionBoleta();

            foreach (var item in boleta)
            {

                infoBoleta.NumeroBoleta = item.NumeroBoleta.ToString();
                infoBoleta.FechaHoraBoleta = item.FechaHoraBoleta == null ? DateTime.Now : (DateTime)item.FechaHoraBoleta;
                infoBoleta.DescripcionDelegacion = item.DescripcionDelegacion;
                infoBoleta.DescripcionAutoridad = item.DescripcionAutoridad;
                infoBoleta.DescripcionRol = item.DescripcionRol;
                infoBoleta.Usuario = item.Usuario;
                infoBoleta.TipoDocumento = item.TipoDocumento;
                infoBoleta.Sexo = item.Sexo;
                infoBoleta.FechaNacimiento = item.FechaNacimiento == null ? DateTime.Now : (DateTime)item.FechaNacimiento;
                infoBoleta.DireccionUsuario = item.DireccionUsuario;
                infoBoleta.TipoLicencia = item.TipoLicencia;
                infoBoleta.Identificacion = item.Identificacion;
                infoBoleta.LugarHechos = item.LugarHechos;
                infoBoleta.Km = item.Km == null ? 0 : (int)item.Km;
                infoBoleta.Placa = item.Placa;
                infoBoleta.DescripcionTipoAutomovil = item.DescripcionTipoAutomovil;
                infoBoleta.DescripcionCarroceria = item.DescripcionCarroceria;
                infoBoleta.DescripcionMarca = item.DescripcionMarca;
                infoBoleta.RevisionTecnica = item.RevisionTecnica;
                infoBoleta.DescripcionOficinaImpugna = item.DescripcionOficinaImpugna;
                infoBoleta.NivelGases = item.NivelGases;
                infoBoleta.Velocidad = item.Velocidad == null ? 0 : (int)item.Velocidad;
                infoBoleta.NombreInspector = item.NombreInspector;
                infoBoleta.CodigoInspector = item.CodigoInspector;
                infoBoleta.ParteOficial = item.ParteOficial;
                infoBoleta.PiePagina = item.PiePagina.Replace("@", "<br/>");
                infoBoleta.ClasePlaca = item.ClasePlaca;
                infoBoleta.CodigoPlaca = item.CodigoPlaca;   
            }
          
            //articulos por boleta 
            var articulos =
                (
                from ARTICULOXBOLETA in db.ARTICULOXBOLETA
                where ARTICULOXBOLETA.serie == serie && ARTICULOXBOLETA.numero_boleta == numero_boleta
                join CATARTICULO in db.CATARTICULO
                     on new { Id = ARTICULOXBOLETA.codigo_articulo, Conducta = ARTICULOXBOLETA.conducta, FechaDeInicio = ARTICULOXBOLETA.Fecha_Inicio, FechaDeFin = ARTICULOXBOLETA.Fecha_Final }
                 equals new { Id = CATARTICULO.Id, CATARTICULO.Conducta, FechaDeInicio = CATARTICULO.FechaDeInicio, FechaDeFin = CATARTICULO.FechaDeFin }
                select new
                {
                    ARTICULOXBOLETA.codigo_articulo,
                    ARTICULOXBOLETA.conducta,
                    ARTICULOXBOLETA.Fecha_Inicio,
                    ARTICULOXBOLETA.Fecha_Final,
                    ARTICULOXBOLETA.fuente,
                    ARTICULOXBOLETA.serie,
                    ARTICULOXBOLETA.numero_boleta,
                    multa = ARTICULOXBOLETA.multa == null ? 0 : ARTICULOXBOLETA.multa,
                    CATARTICULO.Descripcion,
                    CATARTICULO.Puntos
                }).ToList().Distinct();
            
            //Recorre la lista de articulos para agregarlos a la clase articulos y luego a la lista
            foreach (var item in articulos)
            {
                Articulos _articulo = new Articulos(item.codigo_articulo, item.Descripcion, item.multa, item.Puntos);
                infoBoleta.ListaArticulos.Add(_articulo);               
            }
            ViewBag.type = "";
            ViewBag.message = "";
            ViewBag.Datos = infoBoleta;
            Session["Datos"] = infoBoleta ;
            return View();
        }

        public ActionResult DownloadPartialViewPDF(string Imprimir,int? serie,decimal? numero_boleta)
        {
            if (Imprimir != null)
                return RedirectToAction("Index",new { serie,numero_boleta});
            var model = Session["Datos"];
            ViewBag.Datos = Session["Datos"];       
           
            return new Rotativa.PartialViewAsPdf("_MostrarBoletaView",model) { FileName = "Boleta.pdf" };
        }
    }
}