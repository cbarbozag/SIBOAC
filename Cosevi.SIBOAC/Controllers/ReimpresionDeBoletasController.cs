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
        // GET: ReimpresionDeBoletas
        public ActionResult Index(string serie, string numero_boleta)
        {
            InformacionBoleta infoBoleta = new InformacionBoleta();
            infoBoleta.NumeroBoleta = "2-2016-9990084";
            infoBoleta.DescripcionDelegacion = "SAN JOSE";
            infoBoleta.DescripcionAutoridad = "ADMINISTRATIVA (COSEVI)";
            infoBoleta.DescripcionRol = "Conductor";
            infoBoleta.Usuario = "Hernandez Jimenez Jeffry";
            infoBoleta.TipoDocumento = "Cedula de identidad o juvenil";
            infoBoleta.Sexo = "M";
            infoBoleta.FechaNacimiento = DateTime.Now;
            infoBoleta.DireccionUsuario = "San Jose Perez Zeledon San Isidro del General Abajo de un palo de mango";
            infoBoleta.TipoLicencia = "B2";
            infoBoleta.TipoDocumento = "CI 11562323";
            infoBoleta.LugarHechos = "Heredia Heredia Heredia";
            infoBoleta.Km = 27;
            infoBoleta.Placa = "YYY666";
            infoBoleta.DescripcionTipoAutomovil = "Automóvil";
            infoBoleta.DescripcionCarroceria = "Convertible";
            infoBoleta.DescripcionMarca = "Alfa Romeo";
            infoBoleta.RevisionTecnica = "Sí";
            infoBoleta.DescripcionOficinaImpugna = "Heredia";
            infoBoleta.NivelGases = "0%";
            infoBoleta.Velocidad = 200;
            Articulos articulos1 = new Articulos("143A",
                                                                         "Conduce bajo influencias" +
                                                                         " de bebidas alcholicas superiora 0,50 g" +
                                                                         " hasta 0,75 g por litro de sangre", 309574.5, 6);
            Articulos articulos2 = new Articulos("110H",
                                                                         "Retiro de Vehiculo por encontrarse estacionado " +
                                                                         " en espacion preferencial, sin justificación",
                                                                         0.0, 0);
            infoBoleta.ListaArticulos.Add(articulos1);
            infoBoleta.ListaArticulos.Add(articulos2);


            infoBoleta.CodigoInspector = "";
            infoBoleta.PiePagina = "ADVERTENCIAS DE LEY AL INFRACTOR@LEY DE TRÁNSITO POR VÍAS PÚBLICAS TERRESTRES Y SEGURIDAD VIAL  N° 9078@1.Multa Fija: De no presentarse inconformidad dentro de un plazo improrrogable de diez días, contados a partir del día siguiente a la fecha de la confección de la boleta, la misma tomará@firmeza(artículos 163, 164, 165).@2.En caso de disconformidad podrá recurrir ante la Unidad de Impugnaciones de Boletas de Citación del COSEVI, dentro del plazo improrrogable de diez días hábiles(artículo 163).@3.Ante casos de accidente se debe apersonar ante la Autoridad Judicial competente dentro de los diez días hábiles, para manifestarse si acepta o no los cargos o se abstiene de declarar.@4.Dentro del plazo de diez días hábiles, contados a partir de la firmeza de la infracción, se podrá cancelar la multa impuesta menos un quince por ciento (15 %), excluyendo de tal@excepción las infracciones contenidas en el artículo 143 de esta ley; que constituyen las más@severamente sancionadas.@5.Además, correrán intereses de mora del 3 % mensual sobre el monto original hasta un máximo de 36 %, después de transcurridos veinte (20) días hábiles a partir de su firmeza(artículo 194).@6.Si la multa no se cancela dentro del plazo de veinte(20) días hábiles, a partir de su firmeza, puede ser enviada a@Cobro Judicial(artículo 195).@";
            infoBoleta.PiePagina = infoBoleta.PiePagina.Replace("@", "<br/>");
            infoBoleta.NombreInspector = "Pruebas Hand Held";
            ViewBag.Datos = infoBoleta;
            Session["Datos"] = infoBoleta;
            return View();
        }

        public ActionResult DownloadPartialViewPDF()
        {
            var model = Session["Datos"];
            ViewBag.Datos = Session["Datos"];
            return new Rotativa.PartialViewAsPdf("_MostrarBoletaView",model) { FileName = "Boleta.pdf" };
        }
    }
}