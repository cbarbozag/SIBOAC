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
using System.Configuration;

namespace Cosevi.SIBOAC.Controllers
{
    public class AdjuntarArchivosController : BaseController<StatusPlano>
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        private PC_HH_AndroidEntities dbpivot = new PC_HH_AndroidEntities();
        private SIBOACSecurityEntities dbs = new SIBOACSecurityEntities();         
        // GET: StatusPlano       

        [SessionExpire]
        public ViewResult Index(string serie, string NumeroParte, string mensaje)
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

            if ((seriet != "" && seriet != null) && (NumeroParteT != "" && NumeroParteT != null))
            {
                int numSerie = Convert.ToInt32(seriet);
                decimal numParte = Convert.ToDecimal(NumeroParteT);

                var adjuntos = db.OtrosAdjuntos.Where(oa => oa.serie == numSerie && oa.numero == numParte && !oa.nombre.Contains("-u-") && !oa.nombre.Contains("-i-") && !oa.nombre.Contains("-t-")).ToList();

                ViewBag.Adjuntos = adjuntos;

                bool exist = this.db.BOLETA.Any(x => x.serie_parteoficial == seriet && x.numeroparte == NumeroParteT);

                if (exist) { 

                var list =
                  (
                     from p in db.PARTEOFICIAL
                     where p.Serie == seriet && p.NumeroParte == NumeroParteT
                     join b in db.BOLETA on new { fuente_parteoficial = p.Fuente, serie_parteoficial = p.Serie, numeroparte = p.NumeroParte } equals new { fuente_parteoficial = b.fuente_parteoficial, serie_parteoficial = b.serie_parteoficial, numeroparte = b.numeroparte } into o_join
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
                      DescripcionRol = x.DescripcionRol

                  }).Distinct();
              

                //Sí no trae datos es porque no existe 
                if (list.Count() == 0 || list.FirstOrDefault() == null)
                {
                    _tipoMensaje = "error";
                    _mensaje = "No se encontró información para el Parte Oficial " + NumeroParteT + " " + seriet;                        
                }
                if (list.Count() > 0)
                {
                    foreach (var item in list)
                    {

                        Session["Datos"] = list;
                        ViewBag.Valor = list;
                        ViewBag.EstadoPlano = item.EstadoPlano.ToString();
                        return View();
                    }

                }
                }
                else
                { 
                    _tipoMensaje = "error";
                    _mensaje = "No se encontró boletas asociadas al Parte Oficial " + NumeroParteT + " " + seriet;

                }
            }

            ViewBag.type = "";
            ViewBag.message = "";
            ViewBag.type = TempData["Type"] = _tipoMensaje;
            ViewBag.message = TempData["Message"] = _mensaje;
            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles()
        {
            if (Request.Files.Count > 0)
            {
                try
                {

                    string Serie = Request.Form["Serie"];
                    string NumeroParte = Request.Form["NumeroParte"];
                    string EntregoPlano = Request.Form["EntregoPlano"];
                    string fuente = String.Empty;

                    var parte = db.PARTEOFICIAL.Where(po => po.Serie == Serie && po.NumeroParte == NumeroParte).FirstOrDefault();

                    if (parte != null)
                    {
                        fuente = parte.Fuente;
                    }

                    if (EntregoPlano == "1")
                    {
                        var queryStatusPlano =
                             from parteOficial in db.PARTEOFICIAL
                             where
                               parteOficial.Serie == Serie && parteOficial.NumeroParte == NumeroParte
                             select parteOficial;
                        foreach (var parteOficial in queryStatusPlano)
                        {
                            if (EntregoPlano == "1")
                            {
                                parteOficial.StatusPlano = 6;
                            }
                            //parteOficial.fecha_entrega = DateTime.Now;
                            //var Inspector = (dbs.SIBOACUsuarios.Where(a => a.Usuario == User.Identity.Name).Select(a => a.Usuario).ToList());
                            //var codigo = User.Identity.Name;
                            //codigo = Inspector.ToArray().FirstOrDefault() == null ? null : Inspector.ToArray().FirstOrDefault().ToString();
                            //parteOficial.usuario_entregaPlano = codigo;


                        }
                        db.SaveChanges();

                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            string nombreArchivo;


                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                nombreArchivo = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                nombreArchivo = file.FileName;
                            }

                            string ext = Path.GetExtension(nombreArchivo).Replace(".", "");

                            //Valida si la extension es permitida
                            string[] allowFileTypes = ConfigurationManager.AppSettings["AllowFileTypes"].Split(',');
                            bool isAllowExt = false;

                            foreach (var item in allowFileTypes)
                            {
                                if (String.Compare(item, ext, true) == 0)
                                {
                                    isAllowExt = true;
                                    break;
                                }
                            }

                            if (!isAllowExt)
                            {
                                TempData["Type"] = "warning";
                                TempData["Message"] = "Extensión no válida.";

                                return Json(new { result = false, msg = "Extensión no válida." });
                            }

                            int serie2 = Convert.ToInt32(Serie);
                            decimal numero2 = Convert.ToDecimal(NumeroParte);

                            string extConv = ext + "c";

                            int? maxValue = db.OtrosAdjuntos.Where(oa => oa.serie == serie2 && oa.numero == numero2 && (String.Compare(oa.extension, ext, false) == 0 || String.Compare(oa.extension, extConv, false) == 0) && !oa.nombre.Contains("-u-") && !oa.nombre.Contains("-i-") && !oa.nombre.Contains("-t-")).Max(a => a.consecutivo_extension) ?? 0;


                            string directoryPath = ConfigurationManager.AppSettings["UploadFilePath"];

                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            string nombre = String.Format("{0}-{1}-{2}-p-{3}", fuente, Serie, NumeroParte, nombreArchivo);
                            nombreArchivo = Path.Combine(directoryPath, nombre);
                            file.SaveAs(nombreArchivo);

                            string link = @"" + directoryPath + "\\" + nombre;

                            db.OtrosAdjuntos.Add(new OtrosAdjuntos
                            {
                                consecutivo_extension = maxValue.Value + 1,
                                extension = ext,
                                fechaRegistro = DateTime.Now,
                                fuente = fuente,
                                nombre = nombre,
                                numero = Convert.ToDecimal(NumeroParte),
                                serie = Convert.ToInt32(Serie),
                                linkArchivo = link
                            });

                            db.SIBOACBITADJUNTOS.Add(new SIBOACBITADJUNTOS
                            {
                                serie = Convert.ToString(Serie),
                                numero = Convert.ToString(NumeroParte),
                                tipo = "Parte Oficial",
                                funcion = "Agregó Plano",
                                usuario = User.Identity.Name,
                                fechaHora = DateTime.Now,
                                nombreArchivo = nombre
                            });

                            db.SaveChanges();

                        }

                        TempData["Type"] = "success";
                        TempData["Message"] = "Plano adjuntado exitosamente!";

                        return Json(new { result = true, msg = "Plano adjuntado exitosamente!" });
                        // return RedirectToAction("Index");
                    }
                    else
                    {
                        HttpFileCollectionBase files = Request.Files;
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            string nombreArchivo;


                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                nombreArchivo = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                nombreArchivo = file.FileName;
                            }

                            string ext = Path.GetExtension(nombreArchivo).Replace(".", "");

                            //Valida si la extension es permitida
                            string[] allowFileTypes = ConfigurationManager.AppSettings["AllowFileTypes"].Split(',');
                            bool isAllowExt = false;

                            foreach (var item in allowFileTypes)
                            {
                                if (String.Compare(item, ext, true) == 0)
                                {
                                    isAllowExt = true;
                                    break;
                                }
                            }

                            if (!isAllowExt)
                            {
                                TempData["Type"] = "warning";
                                TempData["Message"] = "Extensión no válida.";

                                return Json(new { result = false, msg = "Extensión no válida." });
                            }

                            int serie2 = Convert.ToInt32(Serie);
                            decimal numero2 = Convert.ToDecimal(NumeroParte);

                            string extConv = ext + "c";

                            int? maxValue = db.OtrosAdjuntos.Where(oa => oa.serie == serie2 && oa.numero == numero2 && (String.Compare(oa.extension, ext, false) == 0 || String.Compare(oa.extension, extConv, false) == 0) && !oa.nombre.Contains("-p-") && !oa.nombre.Contains("-u-") && !oa.nombre.Contains("-i-") && !oa.nombre.Contains("-t-")).Max(a => a.consecutivo_extension) ?? 0;


                            string directoryPath = ConfigurationManager.AppSettings["UploadFilePath"];

                            if (!Directory.Exists(directoryPath))
                            {
                                Directory.CreateDirectory(directoryPath);
                            }

                            string nombre = String.Format("{0}-{1}-{2}-{3}-{4}", fuente, Serie, NumeroParte, maxValue.Value + 1, nombreArchivo);
                            nombreArchivo = Path.Combine(directoryPath, nombre);
                            file.SaveAs(nombreArchivo);

                            string link = @"" + directoryPath + "\\" + nombre;

                            db.OtrosAdjuntos.Add(new OtrosAdjuntos
                            {
                                consecutivo_extension = maxValue.Value + 1,
                                extension = ext,
                                fechaRegistro = DateTime.Now,
                                fuente = fuente,
                                nombre = nombre,
                                numero = Convert.ToDecimal(NumeroParte),
                                serie = Convert.ToInt32(Serie),
                                linkArchivo = link
                            });

                            db.SIBOACBITADJUNTOS.Add(new SIBOACBITADJUNTOS
                            {
                                serie = Convert.ToString(Serie),
                                numero = Convert.ToString(NumeroParte),
                                tipo = "Parte Oficial",
                                funcion = "Agregó Adjunto",
                                usuario = User.Identity.Name,
                                fechaHora = DateTime.Now,
                                nombreArchivo = nombre
                            });

                            db.SaveChanges();

                        }
                   
                    }

                    TempData["Type"] = "success";
                    TempData["Message"] = "Archivo adjuntado exitosamente!";

                    return Json(new { result = true, msg = "Archivo adjuntado exitosamente!" });
                }
                catch (Exception ex)
                {
                    TempData["Type"] = "error";
                    TempData["Message"] = "Ocurrió un error. Detalles: " + ex.Message;
                    return Json(new { result = false, msg = "Ocurrió un error. Detalles: " + ex.Message });
                }
            }
            else
            {
                TempData["Type"] = "warning";
                TempData["Message"] = "No hay archivo seleccionado.";
                return Json(new { result = false, msg = "No hay archivo seleccionado." });
            }
        }

        public ActionResult DeleteFiles(List<string> fileList)
        {
            foreach (string item in fileList)
            {
                string[] fileParams = item.Split('-');
                string fuente = fileParams[0];
                int numSerie = Convert.ToInt32(fileParams[1]);
                decimal numParte = Convert.ToDecimal(fileParams[2]);
                string esPlano = fileParams[3];                

                var adjunto = db.OtrosAdjuntos.Where(oa => oa.fuente == fuente && oa.serie == numSerie && oa.numero == numParte && oa.nombre == item && !oa.nombre.Contains("-u-") && !oa.nombre.Contains("-i-") && !oa.nombre.Contains("-t-")).FirstOrDefault();

                if (esPlano == "p")
                {
                    if (adjunto != null)
                    {
                        db.SIBOACBITADJUNTOS.Add(new SIBOACBITADJUNTOS
                        {
                            serie = Convert.ToString(adjunto.serie),
                            numero = Convert.ToString(adjunto.numero),
                            tipo = "Parte Oficial",
                            funcion = "Eliminó Plano",
                            usuario = User.Identity.Name,
                            fechaHora = DateTime.Now,
                            nombreArchivo = adjunto.nombre
                        });

                        string directoryPath = ConfigurationManager.AppSettings["UploadFilePath"];
                        System.IO.File.Delete(Path.Combine(directoryPath, item));
                        db.OtrosAdjuntos.Remove(adjunto);
                        db.SaveChanges();

                        string serie = fileParams[1];
                        string numero = fileParams[2];
                        var queryStatusPlano =
                             from parteOficial in db.PARTEOFICIAL
                             where
                               parteOficial.Serie == serie && parteOficial.NumeroParte == numero
                             select parteOficial;
                        foreach (var parteOficial in queryStatusPlano)
                        {                            
                                parteOficial.StatusPlano = 3;                                                    
                        }
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (adjunto != null)
                    {
                        db.SIBOACBITADJUNTOS.Add(new SIBOACBITADJUNTOS
                        {
                            serie = Convert.ToString(adjunto.serie),
                            numero = Convert.ToString(adjunto.numero),
                            tipo = "Parte Oficial",
                            funcion = "Eliminó Adjunto",
                            usuario = User.Identity.Name,
                            fechaHora = DateTime.Now,
                            nombreArchivo = adjunto.nombre
                        });

                        string directoryPath = ConfigurationManager.AppSettings["UploadFilePath"];
                        System.IO.File.Delete(Path.Combine(directoryPath, item));
                        db.OtrosAdjuntos.Remove(adjunto);
                        db.SaveChanges();

                    }
                }                                                             
            }
            TempData["Type"] = "warning";
            TempData["Message"] = "Se eliminó correctamente.";
            return Json(new { result = true, msg = "Se eliminó correctamente." });
        }
    }
}