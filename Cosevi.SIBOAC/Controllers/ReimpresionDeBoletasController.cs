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
        public ActionResult Index()
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
            infoBoleta.PiePagina = "LA CARGA NO DEBE TRANSPORTARSE DE FORMA QUE NO PROVOQUE POLVO U OTROS INCONVENIENTES";
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