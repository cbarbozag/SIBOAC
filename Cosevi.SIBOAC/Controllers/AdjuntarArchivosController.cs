using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Drawing;
using Rotativa;

namespace Cosevi.SIBOAC.Controllers
{
    public class AdjuntarArchivosController : BaseController<StatusPlano>
    {
      //  private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        private SIBOACSecurityEntities dbs = new SIBOACSecurityEntities();
        // GET: StatusPlano

        [SessionExpire]
        public ViewResult Index(string serie, string NumeroParte,string mensaje)
        {
            ViewBag.EstadoPlano = "";
            string _mensaje = "";
            string _tipoMensaje = "";
            ViewBag.type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
            ViewBag.message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
            if (mensaje != null)
            {
                _tipoMensaje = "error";
                _mensaje = "Actualizado correctamente";
            }

            var seriet = serie;
            var NumeroParteT = NumeroParte;
            ViewBag.Valor = null;
          
            if ((seriet != ""&&seriet!=null) && (NumeroParteT !=""&&NumeroParteT!=null))
            {
             
                var list =
                  (
                     from p in db.PARTEOFICIAL
                     join b in db.BOLETA on new { fuente_parteoficial = p.Fuente, serie_parteoficial = p.Serie, numeroparte = p.NumeroParte } equals new { fuente_parteoficial = b.fuente_parteoficial, serie_parteoficial = b.serie_parteoficial, numeroparte = b.numeroparte } into o_join
                     where p.Serie == seriet && p.NumeroParte == NumeroParteT  
                     from b in o_join.DefaultIfEmpty()
                     join a in db.AUTORIDAD on new { codigo = b.codigo_autoridad_registra } equals new { codigo = a.Id } into ba_join
                     from a in ba_join.DefaultIfEmpty()
                     join r in db.ROLPERSONA on new { codigo = b.codrol } equals new { codigo = r.Id } into br_join
                     from r in br_join.DefaultIfEmpty()
                     select new
                     {
                         CodigoAutoridad = b.codigo_autoridad_registra,
                         DescripcionAutoridad = a.Descripcion,
                         FechaAccidente = p.Fecha,
                         FechaDescarga = p.fecha_descarga,
                         NumeroBoleta = b.numero_boleta,
                         CodigoRol = b.codrol,
                         ClasePlaca = b.clase_placa,
                         CodigoPlaca = b.codigo_placa,
                         NumeroPlaca = b.numero_placa,
                         EstadoPlano = p.StatusPlano,
                         DescripcionRol = r.Descripcion
                     }).ToList().Distinct()
                  .Select(x => new StatusPlano
                   {
                       CodigoAutoridad = x.CodigoAutoridad,
                       DescripcionAutoridad = x.DescripcionAutoridad,
                       FechaAccidente = x.FechaAccidente,
                       FechaDescarga = x.FechaDescarga,
                       NumeroBoleta = x.NumeroBoleta,
                       CodigoRol = x.CodigoRol,
                       ClasePlaca = x.ClasePlaca,
                       CodigoPlaca = x.CodigoPlaca,
                       NumeroPlaca = x.NumeroPlaca,
                       EstadoPlano = x.EstadoPlano,
                       DescripcionRol= x.DescripcionRol

                   });
              
                //Sí no trae datos es porque no existe 
                if (list.Count() == 0 || list.FirstOrDefault() == null)
                {
                    _tipoMensaje = "error";  
                   _mensaje= "No se encontró información para el Parte Oficial " + NumeroParteT + " "+ seriet;
                   
                }
                if(list.Count()>0)
                {
                    foreach (var item in list)
                    {
                                              
                            Session["Datos"] = list;
                            ViewBag.Valor = list;
                            return View();
                    }
                  
                }
            }

            ViewBag.type = "";
            ViewBag.message = "";
            ViewBag.type= TempData["Type"] =_tipoMensaje;
            ViewBag.message =TempData["Message"] = _mensaje;
            return View();            
        }

    }
}