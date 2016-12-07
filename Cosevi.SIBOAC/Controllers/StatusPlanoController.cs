﻿using System;
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
    public class StatusPlanoController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        // GET: StatusPlano
        public ViewResult Index(string serie, string NumeroParte)
        {
            var seriet = serie;
            var NumeroParteT = NumeroParte;
            ViewBag.Valor = null;
            string _mensaje = "";
            string _tipoMensaje = "";
            if ((seriet != ""&&seriet!=null) && (NumeroParteT !=""&&NumeroParteT!=null))
            {
                ViewBag.type = TempData["Type"] != null ? TempData["Type"].ToString() : "";
                ViewBag.message = TempData["Message"] != null ? TempData["Message"].ToString() : "";
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
                                              
                        if (item.EstadoPlano == 1)
                        {

                            _tipoMensaje = "info";
                            _mensaje =  "El Parte Oficial " + seriet + " " + NumeroParteT +
                                " poseee un plano que ya se elaboró en campo, no podrá su cambiar su estado de entrega.";
                            
                        }
                        else if (item.EstadoPlano == 2)
                        {

                            _tipoMensaje = "info";
                            _mensaje = "El Parte Oficial " + seriet + " " + NumeroParteT +
                                " Se cerró sin intención de entrega posterior, no podrá cambiar su estado de entrega.";
                            
                        }
                        else
                        {
                            ViewBag.type = "";
                            ViewBag.message = "";
                            ViewBag.Valor = list;
                            Session["Datos"] = list;
                           Session["NumParte"] = NumeroParteT;
                            Session["SerieParte"] = seriet;

                        }
                        break;

                    }
                  
                }
            }

            ViewBag.type = "";
            ViewBag.message = "";
            ViewBag.type= TempData["Type"] =_tipoMensaje;
            ViewBag.message =TempData["Message"] = _mensaje;
            return View();            
        }

        public ActionResult ExportToExcel()
        {
            var grid = new GridView();
            grid.DataSource = Session["Datos"];
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=StatusPlano.xls");
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
            ViewBag.Valor = Session["Datos"];
            //Code to get content
            return new Rotativa.PartialViewAsPdf("_PlanoView", model) { FileName = "StatusPlano.pdf" };
        }

        public ActionResult GuardarEstado(string EntregoPlano)
        {
            string SerieParte = Session["SerieParte"].ToString();
            string NumParte = Session["NumParte"].ToString();

            if (EntregoPlano!=""&& EntregoPlano=="1")
            {
                var queryStatusPlano =
                     from parteOficial in db.PARTEOFICIAL
                     where
                       parteOficial.Serie == SerieParte && parteOficial.NumeroParte == NumParte
                     select parteOficial;
                foreach (var parteOficial in queryStatusPlano)
                {
                    parteOficial.fecha_entrega = DateTime.Now;
                    parteOficial.StatusPlano = 1;
                    parteOficial.usuario_entregaPlano = "UsuarioEntrega";
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
                
            return View();
        }
    }
}