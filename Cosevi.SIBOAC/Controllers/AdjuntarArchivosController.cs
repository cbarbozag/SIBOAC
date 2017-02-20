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
        //  private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
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

                var adjuntos = db.OtrosAdjuntos.Where(oa => oa.serie == numSerie && oa.numero_boleta == numParte).ToList();

                ViewBag.Adjuntos = adjuntos;

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
                      DescripcionRol = x.DescripcionRol

                  });

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
                        return View();
                    }

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
                    string fuente = String.Empty;

                    var parte = db.PARTEOFICIAL.Where(po => po.Serie == Serie && po.NumeroParte == NumeroParte).FirstOrDefault();

                    if (parte != null)
                    {
                        fuente = parte.Fuente;
                    }

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

                        int? maxValue = db.OtrosAdjuntos.Where(oa => String.Compare(oa.extension, ext, false) == 0).Max(a => a.consecutivo_extension) ?? 0;


                        string directoryPath = ConfigurationManager.AppSettings["UploadFilePath"];

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        string nombre = String.Format("{0}-{1}-{2}-{3}.{4}", fuente, Serie, NumeroParte, maxValue.Value + 1, ext);
                        nombreArchivo = Path.Combine(directoryPath, nombre);
                        file.SaveAs(nombreArchivo);

                        db.OtrosAdjuntos.Add(new OtrosAdjuntos
                        {
                            consecutivo_extension = maxValue.Value + 1,
                            extension = ext,
                            fechaRegistro = DateTime.Now,
                            fuente = fuente,
                            nombre = nombre,
                            numero_boleta = Convert.ToDecimal(NumeroParte),
                            serie = Convert.ToInt32(Serie)
                        });

                        db.SaveChanges();
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

                var adjunto = db.OtrosAdjuntos.Where(oa => oa.fuente == fuente && oa.serie == numSerie && oa.numero_boleta == numParte).FirstOrDefault();

                if (adjunto != null)
                {
                    string directoryPath = ConfigurationManager.AppSettings["UploadFilePath"];
                    System.IO.File.Delete(Path.Combine(directoryPath, item));

                    db.OtrosAdjuntos.Remove(adjunto);
                    db.SaveChanges();
                }
            }

            return Json(new { result = true });
        }
    }
}