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
    public class AdjuntarArchivosBoleController : BaseController<BoletaAdjunto>
    {
        //  private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();
        private SIBOACSecurityEntities dbs = new SIBOACSecurityEntities();
        // GET: StatusPlano

        [SessionExpire]
        public ViewResult Index(string serie, string numero_boleta, string mensaje)
        {
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
            var NumeroBoletaT = numero_boleta;
            ViewBag.Valor = null;

            if ((seriet != "" && seriet != null) && (NumeroBoletaT != "" && NumeroBoletaT != null))
            {
                int numSerie = Convert.ToInt32(seriet);
                decimal numBoleta = Convert.ToDecimal(NumeroBoletaT);

                var adjuntos = db.OtrosAdjuntos.Where(oa => oa.fuente == "2" && oa.serie == numSerie && oa.numero == numBoleta).ToList();

                ViewBag.Adjuntos = adjuntos;

                bool exist = this.db.BOLETA.Any(x => x.fuente == "2" && x.serie == numSerie && x.numero_boleta == numBoleta);

                if (exist)
                {

                    var list =
                      (
                         from b in db.BOLETA
                         where b.serie == numSerie && b.numero_boleta == numBoleta
                         join a in db.AUTORIDAD on new { codigo = b.codigo_autoridad_registra } equals new { codigo = a.Id } into ba_join
                         from a in ba_join.DefaultIfEmpty()
                         join r in db.ROLPERSONA on new { codigo = b.codrol } equals new { codigo = r.Id } into br_join
                         from r in br_join.DefaultIfEmpty()
                         select new
                         {
                             CodigoAutoridad = b.codigo_autoridad_registra,
                             DescripcionAutoridad = a.Descripcion,
                             FechaBoleta = b.fecha_hora_boleta,
                             FechaRegistro = b.fecha_registro,
                             SerieBoleta = b.serie,
                             NumeroBoleta = b.numero_boleta,
                             CodigoRol = b.codrol,
                             ClasePlaca = b.clase_placa,
                             CodigoPlaca = b.codigo_placa,
                             NumeroPlaca = b.numero_placa,
                             DescripcionRol = r.Descripcion
                         }).ToList().Distinct()
                      .Select(x => new BoletaAdjunto
                      {
                          CodigoAutoridad = x.CodigoAutoridad,
                          DescripcionAutoridad = x.DescripcionAutoridad,
                          FechaBoleta = x.FechaBoleta,
                          FechaRegistro = x.FechaRegistro,
                          SerieBoleta = x.SerieBoleta,
                          NumeroBoleta = x.NumeroBoleta,
                          CodigoRol = x.CodigoRol,
                          ClasePlaca = x.ClasePlaca,
                          CodigoPlaca = x.CodigoPlaca,
                          NumeroPlaca = x.NumeroPlaca,
                          DescripcionRol = x.DescripcionRol

                      });


                    //Sí no trae datos es porque no existe 
                    if (list.Count() == 0 || list.FirstOrDefault() == null)
                    {
                        _tipoMensaje = "error";
                        _mensaje = "No se encontró información para la Boleta " + NumeroBoletaT + " " + seriet;
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
                else
                {
                    _tipoMensaje = "error";
                    _mensaje = "No se encontró boletas asociadas al Numero de Boleta " + NumeroBoletaT + " " + seriet;

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
                    string NumeroBoleta = Request.Form["NumeroBoleta"];
                    string fuente = String.Empty;

                    int serie2 = Convert.ToInt32(Serie);
                    decimal numBole2 = Convert.ToDecimal(NumeroBoleta);

                    var boleta = db.BOLETA.Where(po => po.fuente == "2" && po.serie == serie2 && po.numero_boleta == numBole2).FirstOrDefault();

                    if (boleta != null)
                    {
                        fuente = boleta.fuente;
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

                        string nombre = String.Format("{0}-{1}-{2}-{3}-{4}", fuente, Serie, NumeroBoleta, maxValue.Value + 1, nombreArchivo);
                        nombreArchivo = Path.Combine(directoryPath, nombre);
                        file.SaveAs(nombreArchivo);

                        db.OtrosAdjuntos.Add(new OtrosAdjuntos
                        {
                            consecutivo_extension = maxValue.Value + 1,
                            extension = ext,
                            fechaRegistro = DateTime.Now,
                            fuente = fuente,
                            nombre = nombre,
                            numero = Convert.ToDecimal(NumeroBoleta),
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
                decimal numBoleta = Convert.ToDecimal(fileParams[2]);

                var adjunto = db.OtrosAdjuntos.Where(oa => oa.fuente == fuente && oa.serie == numSerie && oa.numero == numBoleta).FirstOrDefault();

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