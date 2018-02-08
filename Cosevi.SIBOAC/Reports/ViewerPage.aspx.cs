using Cosevi.SIBOAC.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using Svg;

namespace Cosevi.SIBOAC.Reports
{
    public partial class ViewerPage : System.Web.UI.Page
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        private PC_HH_AndroidEntities dbPivot = new PC_HH_AndroidEntities();

        private SIBOACSecurityEntities dbSecurity = new SIBOACSecurityEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string reporteID = Request.QueryString["reporteID"];
                string nombreReporte = Request.QueryString["nombreReporte"];
                string parametros = Request.QueryString["parametros"];
                DataTable listaArchivos = new DataTable();
                listaArchivos.Columns.Add("NombreArchivo");
                DataTable listaFirmas = new DataTable();
                listaFirmas.Columns.Add("NombreArchivo");
                DataTable listaPlanos = new DataTable();
                listaPlanos.Columns.Add("NombreArchivo");

                if (String.IsNullOrEmpty(reporteID) || String.IsNullOrEmpty(nombreReporte) || String.IsNullOrEmpty(parametros))
                {
                    return;
                }

                ReportViewer1.Reset();
                ReportViewer1.LocalReport.ReportPath = Server.MapPath(String.Format("~/Reports/{0}.rdlc", nombreReporte));

                switch (reporteID)
                {
                    case "_ReimpresionDeBoletasDeCampo":
                        #region ReimpresionDeBoletasDeCampo
                        ReportViewer1.LocalReport.EnableExternalImages = true;

                        string[] param = parametros.Split(',');
                        int serie = Convert.ToInt32(param[0]);
                        decimal numero_boleta = Convert.ToDecimal(param[1]);

                        var fuente = (db.BOLETA.Where(a => a.serie == serie && a.numero_boleta == numero_boleta).Select(a => a.fuente).ToList());
                        string CodigoFuente = fuente.ToArray().FirstOrDefault() == null ? "0" : fuente.ToArray().FirstOrDefault().ToString();

                        var NumeroParte = (db.BOLETA.Where(a => a.serie == serie && a.numero_boleta == numero_boleta).Select(a => a.numeroparte).ToList());
                        string NumParte = NumeroParte.ToArray().FirstOrDefault() == null ? "0" : NumeroParte.ToArray().FirstOrDefault().ToString();

                        var ideUsuario = (db.PERSONA.Where(a => a.Serie == serie && a.NumeroBoleta == numero_boleta).Select(a => a.identificacion).ToList());
                        string CodigoIde = ideUsuario.ToArray().FirstOrDefault() == null ? "0" : ideUsuario.ToArray().FirstOrDefault().ToString();

                        var CodigoInsp = db.BOLETA.Where(a => a.serie == serie && a.numero_boleta == numero_boleta && a.numeroparte == NumParte).Select(a => a.codigo_inspector).ToList();
                        string CodInsp = CodigoInsp.ToArray().FirstOrDefault() == null ? "0" : CodigoInsp.ToArray().FirstOrDefault().ToString();

                        string ruta = ConfigurationManager.AppSettings["DownloadFilePath"];
                        //var path = Server.MapPath(ruta);
                        var fileUsuario = string.Format("{0}{1}{2}-u-{3}.png", CodigoFuente, serie, numero_boleta, CodigoIde);
                        var fileInspector = string.Format("{0}{1}{2}-i-{3}.png", CodigoFuente, serie, numero_boleta, CodInsp);
                        var fullPathUsuario = Path.Combine(ruta, fileUsuario);
                        var fullPathInspector = Path.Combine(ruta, fileInspector);

                        //Server.MapPath(fullPath)
                        string imgFirmaUsuarioPath = new Uri(fullPathUsuario).AbsoluteUri;
                        string imgFirmaInspectorPath = new Uri(fullPathInspector).AbsoluteUri;

                        //Array que contendrá los parámetros
                        ReportParameter[] parameters = new ReportParameter[2];

                        //Establecemos el valor de los parámetros
                        parameters[0] = new ReportParameter("ImagenFirmaUsuarioPath", imgFirmaUsuarioPath);
                        parameters[1] = new ReportParameter("ImagenFirmaInspectorPath", imgFirmaInspectorPath);
                        ReportViewer1.LocalReport.SetParameters(parameters);
                        #endregion
                        break;

                    case "_ConsultaeImpresionDeParteOficial":
                        #region ConsultaeImpresionDeParteOficial
                        ReportViewer1.LocalReport.EnableExternalImages = true;

                        string[] param2 = parametros.Split(',');
                        int TipoConsulta = Convert.ToInt32(param2[0]);
                        string Parametro1 = param2[1];
                        string Parametro2 = param2[2];
                        string Parametro3 = param2[3];
                        //string Parametro4 = param2[4];

                        string[] extensionRestringida = ConfigurationManager.AppSettings["ExtenException"].Split(',');                                                

                        if (TipoConsulta == 1)
                        {
                            #region Consulta 1
                            string serieParte1 = Parametro1;                            
                            string numeroParte1 = Parametro2;                            

                            var fuente1 = (db.PARTEOFICIAL.Where(a => a.Serie == Parametro1 && a.NumeroParte == Parametro2).Select(a => a.Fuente).ToList());
                            string CodigoFuente1 = fuente1.ToArray().FirstOrDefault() == null ? "0" : fuente1.ToArray().FirstOrDefault().ToString();

                            var Boleta1 = (db.BOLETA.Where(a => a.serie_parteoficial == serieParte1 && a.numeroparte == numeroParte1).Select(a => a.numero_boleta).ToList());
                            var SerieBoleta1 = (db.BOLETA.Where(a => a.serie_parteoficial == serieParte1 && a.numeroparte == numeroParte1).Select(a => a.serie).ToList());

                            int serParte1 = Convert.ToInt32(Parametro1);
                            decimal numeParte1 = Convert.ToDecimal(Parametro2);

                            string ruta1 = ConfigurationManager.AppSettings["DownloadFilePath"];
                            string rutaPlano1 = ConfigurationManager.AppSettings["UploadFilePath"];

                            #region Convertir SVG a PNG
                            var extSvg = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serParte1 && oa.numero_boleta == numeParte1 && oa.extension == "SVG").Select(oa => oa.nombre);
                            

                            foreach (string item in extSvg)
                            {
                                string filePath = Path.Combine(rutaPlano1, item);
                                var sampleDoc = SvgDocument.Open(filePath);
                                string nombrePng = item.Replace(".svg", ".png");                                

                                string ext = Path.GetExtension(nombrePng).Replace(".", "");
                                int? maxValue = dbPivot.OtrosAdjuntos.Where(oa => String.Compare(oa.extension, ext, false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                string nombre = String.Format("{0}-{1}-{2}-{3}.{4}", CodigoFuente1, serParte1, numeParte1, maxValue.Value + 1, ext);
                                sampleDoc.Draw().Save(Path.Combine(rutaPlano1, nombre));

                                var svgConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente1, serParte1, numeParte1, item);
                                svgConvertido.extension = "svgc";

                                dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                {
                                    fuente = CodigoFuente1,
                                    serie = serParte1,
                                    numero_boleta = numeParte1,
                                    extension = ext,
                                    fechaRegistro = DateTime.Now,
                                    nombre = nombre,
                                    consecutivo_extension = maxValue.Value + 1
                                });

                                dbPivot.SaveChanges();
                                
                            }
                            #endregion

                            var ext1 = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serParte1 && oa.numero_boleta == numeParte1 && !extensionRestringida.Contains(oa.extension)).Select(oa => oa.nombre);

                            listaArchivos.Columns.Add("ParteOficial");                            

                            foreach (string item in ext1)
                            {
                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta1, item)).AbsoluteUri, numeroParte1);
                            }

                            var listFirma = (db.BOLETA.Where(a => a.serie_parteoficial == Parametro1 && a.numeroparte == Parametro2).ToList());
                            var listaTestigoP = (db.TESTIGOXPARTE.Where(a => a.serie == Parametro1 && a.numeroparte == Parametro2).ToList());
                            var listaTestigoB = (db.TESTIGO.Where(a => SerieBoleta1.Contains(a.serie) && Boleta1.Contains(a.numero)).ToList());

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");


                            string v_nombre = null;
                            foreach (var item in listFirma)
                            {
                                if (v_nombre == null)
                                {
                                    var FirmaInspector = string.Format("{0}{1}{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                    v_nombre = "1";
                                }
                                var FirmaUsuario = string.Format("{0}{1}{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);                                
                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                            }

                            foreach (var item in listaTestigoP)
                            {
                                var FirmaTestigoP = string.Format("{0}{1}{2}-t-{3}.png", item.fuente, item.serie, item.numeroparte, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaTestigoP)).AbsoluteUri, item.numeroparte, item.identificacion);
                            }

                            foreach (var item in listaTestigoB)
                            {
                                var FirmaTestigoB = string.Format("{0}{1}{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                            }

                            this.ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
                            Session["_ConsultaeImpresionDeParteOficialData"] = listaArchivos;
                            Session["_ConsultaeImpresionDeParteOficialDataFirma"] = listaFirmas;
                            #endregion
                        }

                        if (TipoConsulta == 2)
                        {
                            #region Consulta 2
                            int serieBoleta2 = Convert.ToInt32(Parametro1);
                            decimal numeroBoleta2 = Convert.ToDecimal(Parametro2);                            

                            var numeroPart2 = (db.BOLETA.Where(a => a.serie == serieBoleta2 && a.numero_boleta == numeroBoleta2).Select(a => a.numeroparte).ToList());
                            string CodigoNumParte2 = numeroPart2.ToArray().FirstOrDefault() == null ? "0" : numeroPart2.ToArray().FirstOrDefault().ToString();

                            var seriePart2 = (db.PARTEOFICIAL.Where(a => a.NumeroParte == CodigoNumParte2).Select(a => a.Serie).ToList());
                            string CodigoSerie2 = seriePart2.ToArray().FirstOrDefault() == null ? "0" : seriePart2.ToArray().FirstOrDefault().ToString();

                            var fuente2 = (db.PARTEOFICIAL.Where(a => a.Serie == CodigoSerie2 && a.NumeroParte == CodigoNumParte2).Select(a => a.Fuente).ToList());
                            string CodigoFuente2 = fuente2.ToArray().FirstOrDefault() == null ? "0" : fuente2.ToArray().FirstOrDefault().ToString();

                            int serieParte2 = Convert.ToInt32(CodigoSerie2);
                            decimal numeroParte2 = Convert.ToDecimal(CodigoNumParte2);


                            string ruta2 = ConfigurationManager.AppSettings["DownloadFilePath"];
                            string rutaPlano2 = ConfigurationManager.AppSettings["UploadFilePath"];

                            #region Convertir SVG a PNG
                            var extSvg = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero_boleta == numeroParte2 && oa.extension == "SVG").Select(oa => oa.nombre);


                            foreach (string item in extSvg)
                            {
                                string filePath = Path.Combine(rutaPlano2, item);
                                var sampleDoc = SvgDocument.Open(filePath);
                                string nombrePng = item.Replace(".svg", ".png");

                                string ext = Path.GetExtension(nombrePng).Replace(".", "");
                                int? maxValue = dbPivot.OtrosAdjuntos.Where(oa => String.Compare(oa.extension, ext, false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                string nombre = String.Format("{0}-{1}-{2}-{3}.{4}", CodigoFuente2, serieParte2, numeroParte2, maxValue.Value + 1, ext);
                                sampleDoc.Draw().Save(Path.Combine(rutaPlano2, nombre));

                                var svgConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente2, serieParte2, numeroParte2, item);
                                svgConvertido.extension = "svgc";

                                dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                {
                                    fuente = CodigoFuente2,
                                    serie = serieParte2,
                                    numero_boleta = numeroParte2,
                                    extension = ext,
                                    fechaRegistro = DateTime.Now,
                                    nombre = nombre,
                                    consecutivo_extension = maxValue.Value + 1
                                });

                                dbPivot.SaveChanges();

                            }
                            #endregion

                            var ext2 = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero_boleta == numeroParte2 && !extensionRestringida.Contains(oa.extension)).Select(oa => oa.nombre);

                            listaArchivos.Columns.Add("ParteOficial");

                            //var listaAdjuntos = ext2.Zip(CodigoNumParte2, (n, w) => new { NombreAr = n, NumPar = w });

                            foreach (var item in ext2)
                            {
                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta2, item)).AbsoluteUri, CodigoNumParte2);
                            }                            

                            var listFirma = (db.BOLETA.Where(a => CodigoSerie2.Contains(a.serie_parteoficial) && CodigoNumParte2.Contains(a.numeroparte) && a.serie == serieBoleta2 && a.numero_boleta == numeroBoleta2).ToList());
                            var listaTestigoP = (db.TESTIGOXPARTE.Where(a => a.serie == CodigoSerie2 && a.numeroparte == CodigoNumParte2).ToList());
                            var listaTestigoB = (db.TESTIGO.Where(a => a.serie == serieBoleta2 && a.numero == numeroBoleta2).ToList());

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");

                            string v_nombre = null;
                            foreach (var item in listFirma)
                            {
                                if (v_nombre == null)
                                {
                                    var FirmaInspector = string.Format("{0}{1}{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                    v_nombre = "1";
                                }
                                var FirmaUsuario = string.Format("{0}{1}{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);
                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                            }

                            foreach (var item in listaTestigoP)
                            {
                                var FirmaTestigoP = string.Format("{0}{1}{2}-t-{3}.png", item.fuente, item.serie, item.numeroparte, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaTestigoP)).AbsoluteUri, item.numeroparte, item.identificacion);
                            }

                            foreach (var item in listaTestigoB)
                            {
                                var FirmaTestigoB = string.Format("{0}{1}{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                            }

                            this.ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
                            Session["_ConsultaeImpresionDeParteOficialData"] = listaArchivos;
                            Session["_ConsultaeImpresionDeParteOficialDataFirma"] = listaFirmas;
                            #endregion
                        }

                        if (TipoConsulta == 3)
                        {
                            #region Consulta 3
                            var numeroBoleta3 = (db.PERSONA.Where(a => a.tipo_ide == Parametro1 && a.identificacion == Parametro2).Select(a => a.NumeroBoleta).ToList());                            
                            var seriBolet3 = (db.PERSONA.Where(a => a.tipo_ide == Parametro1 && a.identificacion == Parametro2).Select(a => a.Serie).ToList());

                            var numPartBo3 = (db.BOLETA.Where(a => seriBolet3.Contains(a.serie) && numeroBoleta3.Contains(a.numero_boleta) && a.numeroparte != "0").Select(a => a.numeroparte).ToList());
                            var serieP = (db.BOLETA.Where(a => seriBolet3.Contains(a.serie) && numeroBoleta3.Contains(a.numero_boleta) && a.numeroparte != "0").Select(a => a.serie_parteoficial).ToList());

                            var numPartPar3 = (db.PARTEOFICIAL.Where(a => numPartBo3.Contains(a.NumeroParte) && serieP.Contains(a.Serie)).Select(a => a.NumeroParte).ToList());
                            var numPartConv3 = numPartPar3.Select(s => Convert.ToDecimal(s)).ToList();

                            var seriePart3 = (db.PARTEOFICIAL.Where(a => numPartPar3.Contains(a.NumeroParte) && serieP.Contains(a.Serie)).Select(a => a.Serie).ToList());
                            var serParte3 = seriePart3.Select(s => Convert.ToInt32(s)).ToList();

                            var fuente3 = (db.PARTEOFICIAL.Where(a => seriePart3.Contains(a.Serie) && numPartPar3.Contains(a.NumeroParte)).Select(a => a.Fuente).ToList());

                            string rutaPlano3 = ConfigurationManager.AppSettings["UploadFilePath"];

                            #region Convertir SVG a PNG
                            var extSvg3 = db.OtrosAdjuntos.Where(oa => fuente3.Contains(oa.fuente) && serParte3.Contains(oa.serie) && numPartConv3.Contains(oa.numero_boleta) && oa.extension == "SVG").Select(oa => oa.nombre).ToList();
                            var extSvgFuente3 = db.OtrosAdjuntos.Where(oa => extSvg3.Contains(oa.nombre)).Select(oa => oa.fuente).ToList();
                            var extSvgSerie3 = db.OtrosAdjuntos.Where(oa => extSvg3.Contains(oa.nombre)).Select(oa => oa.serie).ToList();
                            var extSvgParte3 = db.OtrosAdjuntos.Where(oa => extSvg3.Contains(oa.nombre)).Select(oa => oa.numero_boleta).ToList();

                            var listPlanos3 = extSvg3.Zip(extSvgParte3, (n, w) => new { NombreAr = n, NumPar = w }).Zip(extSvgSerie3, (x, z) => Tuple.Create(x.NombreAr,x.NumPar,z)).Zip(extSvgFuente3, (y,r) => Tuple.Create(y.Item1,y.Item2,y.Item3,r));

                            foreach (var item in listPlanos3)
                            {
                                string filePath = Path.Combine(rutaPlano3, item.Item1);
                                var sampleDoc = SvgDocument.Open(filePath);
                                string nombrePng = item.Item1.Replace(".svg", ".png");

                                string ext = Path.GetExtension(nombrePng).Replace(".", "");
                                int? maxValue = dbPivot.OtrosAdjuntos.Where(oa => String.Compare(oa.extension, ext, false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                string nombre = String.Format("{0}-{1}-{2}-{3}.{4}", item.Item4, item.Item3, item.Item2, maxValue.Value + 1, ext);
                                sampleDoc.Draw().Save(Path.Combine(rutaPlano3, nombre));

                                var svgConvertido = dbPivot.OtrosAdjuntos.Find(item.Item4, item.Item3, item.Item2, item.Item1);
                                svgConvertido.extension = "svgc";

                                dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                {
                                    fuente = item.Item4,
                                    serie = item.Item3,
                                    numero_boleta = item.Item2,
                                    extension = ext,
                                    fechaRegistro = DateTime.Now,
                                    nombre = nombre,
                                    consecutivo_extension = maxValue.Value + 1
                                });

                                dbPivot.SaveChanges();

                            }
                            #endregion

                            var nombreAdjuntos3 = db.OtrosAdjuntos.Where(oa => fuente3.Contains(oa.fuente) && serParte3.Contains(oa.serie) && numPartConv3.Contains(oa.numero_boleta) && !extensionRestringida.Contains(oa.extension)).Select(oa => oa.nombre).ToList();
                            var numPartLista3 = db.OtrosAdjuntos.Where(oa => nombreAdjuntos3.Contains(oa.nombre)).Select(oa => oa.numero_boleta).ToList();

                            listaArchivos.Columns.Add("ParteOficial");

                            string ruta3 = ConfigurationManager.AppSettings["DownloadFilePath"];

                            var listaAdjuntos3 = nombreAdjuntos3.Zip(numPartLista3, (n, w) => new { NombreAr = n, NumPar = w });

                            foreach (var item in listaAdjuntos3)
                            {
                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta3, item.NombreAr)).AbsoluteUri, item.NumPar);
                            }

                            var listFirma = (db.BOLETA.Where(a => seriePart3.Contains(a.serie_parteoficial) && numPartPar3.Contains(a.numeroparte) && seriBolet3.Contains(a.serie) && numeroBoleta3.Contains(a.numero_boleta)).OrderBy(a => new {a.fuente_parteoficial,a.serie_parteoficial,a.numeroparte}).ToList());
                            var listaTestigoP = (db.TESTIGOXPARTE.Where(a => seriePart3.Contains(a.serie) && numPartPar3.Contains(a.numeroparte)).ToList());
                            var listaTestigoB = (db.TESTIGO.Where(a => seriBolet3.Contains(a.serie) && numeroBoleta3.Contains(a.numero)).ToList());

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");

                            string v_fuente = null;
                            string v_serie = null;
                            string v_numparte = null;

                            foreach (var item in listFirma)
                            {
                                if(v_fuente != item.fuente_parteoficial || v_serie != item.serie_parteoficial || v_numparte != item.numeroparte)
                                {
                                    var FirmaInspector = string.Format("{0}{1}{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                    v_fuente = item.fuente_parteoficial;
                                    v_serie = item.serie_parteoficial;
                                    v_numparte = item.numeroparte;
                                }
                                
                                var FirmaUsuario = string.Format("{0}{1}{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);                                
                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                            }

                            foreach (var item in listaTestigoP)
                            {
                                var FirmaTestigoP = string.Format("{0}{1}{2}-t-{3}.png", item.fuente, item.serie, item.numeroparte, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaTestigoP)).AbsoluteUri, item.numeroparte, item.identificacion);
                            }

                            foreach (var item in listaTestigoB)
                            {
                                var FirmaTestigoB = string.Format("{0}{1}{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                            }

                            this.ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
                            Session["_ConsultaeImpresionDeParteOficialData"] = listaArchivos;
                            Session["_ConsultaeImpresionDeParteOficialDataFirma"] = listaFirmas;
                            #endregion
                        }

                        if (TipoConsulta == 4)
                        {
                            #region Consulta 4
                            var numeroBoleta4 = (db.VEHICULO.Where(a => a.placa == Parametro1 && a.codigo == Parametro2 && a.clase == Parametro3).Select(a => a.NumeroBoleta).ToList());

                            var seriBolet4 = (db.VEHICULO.Where(a => a.placa == Parametro1 && a.codigo == Parametro2 && a.clase == Parametro3).Select(a => a.Serie).ToList());
                            
                            var numeroPart4 = (db.BOLETA.Where(a => seriBolet4.Contains(a.serie) && numeroBoleta4.Contains(a.numero_boleta)).Select(a => a.numeroparte).ToList());
                            var numPartPar4 = (db.PARTEOFICIAL.Where(a => numeroPart4.Contains(a.NumeroParte) && a.NumeroParte != "0").Select(a => a.NumeroParte).ToList());
                            var numPartConv4 = numPartPar4.Select(s => Convert.ToDecimal(s)).ToList();                            

                            var seriePart4 = (db.PARTEOFICIAL.Where(a => numPartPar4.Contains(a.NumeroParte)).Select(a => a.Serie).ToList());
                            var serParte4 = seriePart4.Select(s => Convert.ToInt32(s)).ToList();                            

                            var fuente4 = (db.PARTEOFICIAL.Where(a => seriePart4.Contains(a.Serie) && numPartPar4.Contains(a.NumeroParte)).Select(a => a.Fuente).ToList());

                            string rutaPlano4 = ConfigurationManager.AppSettings["UploadFilePath"];

                            #region Convertir SVG a PNG
                            var extSvg4 = db.OtrosAdjuntos.Where(oa => fuente4.Contains(oa.fuente) && serParte4.Contains(oa.serie) && numPartConv4.Contains(oa.numero_boleta) && oa.extension == "SVG").Select(oa => oa.nombre).ToList();
                            var extSvgFuente4 = db.OtrosAdjuntos.Where(oa => extSvg4.Contains(oa.nombre)).Select(oa => oa.fuente).ToList();
                            var extSvgSerie4 = db.OtrosAdjuntos.Where(oa => extSvg4.Contains(oa.nombre)).Select(oa => oa.serie).ToList();
                            var extSvgParte4 = db.OtrosAdjuntos.Where(oa => extSvg4.Contains(oa.nombre)).Select(oa => oa.numero_boleta).ToList();

                            var listPlanos4 = extSvg4.Zip(extSvgParte4, (n, w) => new { NombreAr = n, NumPar = w }).Zip(extSvgSerie4, (x, z) => Tuple.Create(x.NombreAr, x.NumPar, z)).Zip(extSvgFuente4, (y, r) => Tuple.Create(y.Item1, y.Item2, y.Item3, r));

                            foreach (var item in listPlanos4)
                            {
                                string filePath = Path.Combine(rutaPlano4, item.Item1);
                                var sampleDoc = SvgDocument.Open(filePath);
                                string nombrePng = item.Item1.Replace(".svg", ".png");

                                string ext = Path.GetExtension(nombrePng).Replace(".", "");
                                int? maxValue = dbPivot.OtrosAdjuntos.Where(oa => String.Compare(oa.extension, ext, false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                string nombre = String.Format("{0}-{1}-{2}-{3}.{4}", item.Item4, item.Item3, item.Item2, maxValue.Value + 1, ext);
                                sampleDoc.Draw().Save(Path.Combine(rutaPlano4, nombre));

                                var svgConvertido = dbPivot.OtrosAdjuntos.Find(item.Item4, item.Item3, item.Item2, item.Item1);
                                svgConvertido.extension = "svgc";

                                dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                {
                                    fuente = item.Item4,
                                    serie = item.Item3,
                                    numero_boleta = item.Item2,
                                    extension = ext,
                                    fechaRegistro = DateTime.Now,
                                    nombre = nombre,
                                    consecutivo_extension = maxValue.Value + 1
                                });

                                dbPivot.SaveChanges();

                            }
                            #endregion


                            var nombreAdjuntos4 = db.OtrosAdjuntos.Where(oa => fuente4.Contains(oa.fuente) && serParte4.Contains(oa.serie) && numPartConv4.Contains(oa.numero_boleta) && !extensionRestringida.Contains(oa.extension)).Select(oa => oa.nombre).ToList();
                            var numPartLista4 = db.OtrosAdjuntos.Where(oa => nombreAdjuntos4.Contains(oa.nombre)).Select(oa => oa.numero_boleta).ToList();

                            listaArchivos.Columns.Add("ParteOficial");
                            
                            string ruta4 = ConfigurationManager.AppSettings["DownloadFilePath"];

                            var listaAdjuntos4 = nombreAdjuntos4.Zip(numPartLista4, (n, w) => new { NombreAr = n, NumPar = w });

                            foreach (var item in listaAdjuntos4)
                            {
                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta4, item.NombreAr)).AbsoluteUri, item.NumPar);
                            }

                            var listFirma = (db.BOLETA.Where(a => seriePart4.Contains(a.serie_parteoficial) && numPartPar4.Contains(a.numeroparte) && seriBolet4.Contains(a.serie) && numeroBoleta4.Contains(a.numero_boleta)).OrderBy(a => new { a.fuente_parteoficial, a.serie_parteoficial, a.numeroparte }).ToList());
                            var listaTestigoP = (db.TESTIGOXPARTE.Where(a => seriePart4.Contains(a.serie) && numPartPar4.Contains(a.numeroparte)).ToList());
                            var listaTestigoB = (db.TESTIGO.Where(a => seriBolet4.Contains(a.serie) && numeroBoleta4.Contains(a.numero)).ToList());

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");

                            string p_fuente = null;
                            string p_serie = null;
                            string p_numparte = null;

                            foreach (var item in listFirma)
                            {
                                if (p_fuente != item.fuente_parteoficial || p_serie != item.serie_parteoficial || p_numparte != item.numeroparte)
                                {
                                    var FirmaInspector = string.Format("{0}{1}{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                    p_fuente = item.fuente_parteoficial;
                                    p_serie = item.serie_parteoficial;
                                    p_numparte = item.numeroparte;
                                }

                                var FirmaUsuario = string.Format("{0}{1}{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);
                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                               
                            }

                            foreach (var item in listaTestigoP)
                            {
                                var FirmaTestigoP = string.Format("{0}{1}{2}-t-{3}.png", item.fuente, item.serie, item.numeroparte, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaTestigoP)).AbsoluteUri, item.numeroparte, item.identificacion);
                            }

                            foreach (var item in listaTestigoB)
                            {
                                var FirmaTestigoB = string.Format("{0}{1}{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                            }

                            this.ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
                            Session["_ConsultaeImpresionDeParteOficialData"] = listaArchivos;
                            Session["_ConsultaeImpresionDeParteOficialDataFirma"] = listaFirmas;
                            #endregion
                        }

                        #endregion
                        break;


                }
                ReportDataSource RDS = new ReportDataSource("DataSet1", GetData(reporteID, parametros));
                ReportViewer1.LocalReport.DataSources.Add(RDS);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.ZoomMode = ZoomMode.Percent;
                ReportViewer1.ZoomPercent = 100;
                btnPrint.Visible = true;
                ReportViewer1.LocalReport.EnableHyperlinks = true;

            }
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {            
            e.DataSources.Add(new ReportDataSource("ArchivoDataSet", Session["_ConsultaeImpresionDeParteOficialData"])); 
            e.DataSources.Add(new ReportDataSource("FirmasDataSet", Session["_ConsultaeImpresionDeParteOficialDataFirma"]));
        }

        private object GetData(string reporteID, string parametros)
        {
            object result = null;

            switch (reporteID)
            {
                case "_Bitacora":
                    result = GetBitacoraData(parametros);
                    break;
                case "_DescargaInspector":
                    result = GetDescargaInspectorData(parametros);
                    break;
                case "_DescargaBoleta":
                    result = GetDescargaBoletaData(parametros);
                    break;
                case "_DescargaParteOficial":
                    result = GetReportePorDelegacionAutoridadData(parametros);
                    break;
                case "_ReporteListadoMultaFija":
                    result = GetReporteListadoMultafijaData(parametros);
                    break;
                case "_ReporteListadoParteOficial":
                    result = GetReporteListadoParteOficialData(parametros);
                    break;
                case "_ConsultaeImpresionDeParteOficial":
                    result = GetConsultaeImpresionDeParteOficialData(parametros);
                    break;
                case "_ReportePorUsuario":
                    result = GetReportePorUsuarioData(parametros);
                    break;

                case "_ConsultaeImpresionDeBoletas":
                    result = GetConsultaeImpresionDeBoletasData(parametros);
                    break;

                case "_ReporteStatusActualPlano":
                    result = GetReporteStatusActualPlanoData(parametros);
                    break;
                case "_ActividadOficial":
                    result = GetActividadOficialData(parametros);
                    break;
                case "_BitacoraAplicacion":
                    result = GetBitacoraAplicacionData(parametros);
                    break;
                case "_ReimpresionDeBoletasDeCampo":
                    result = GetReimpresionDeBoletasDeCampoData(parametros);
                    break;
                default:
                    break;
            }

            return result;
        }

        #region Datos
        private List<BitacoraSIBOAC> GetBitacoraData(string parametros)
        {
            string[] param = parametros.Split(',');
            DateTime fechaInicio = Convert.ToDateTime(param[0]);
            DateTime fechaFin = Convert.ToDateTime(param[1]);
            string nombreTabla = param[2];
            string operacion = param[3];
            string usuario = param[4];

            var bitacora = db.GetBitacoraData(fechaInicio, fechaFin, nombreTabla, usuario, operacion).ToList();
            return bitacora;
        }
        private List<GetActividadOficialData_Result> GetActividadOficialData(string parametros)
        {
            //SE agrega el codigo del usuario

            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            string CodigoInspector = param[0];
            DateTime fechaInicio = Convert.ToDateTime(param[1]);
            DateTime fechaFin = Convert.ToDateTime(param[2]);

            var actividadOificial = db.GetActividadOficialData(CodigoInspector, fechaInicio, fechaFin, usuarioSistema).ToList();
            return actividadOificial;
        }

        private List<GetDescargaInspectorData_Result> GetDescargaInspectorData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            DateTime fechaInicio = Convert.ToDateTime(param[0]);
            DateTime fechaFin = Convert.ToDateTime(param[1]);
            string codigoOficial = param[2].Replace("|", ",").Replace("-", "").Trim(); ;

            var lista = db.GetDescargaInspectorData(fechaInicio, fechaFin, codigoOficial, usuarioSistema).ToList();
            return lista;
        }

        private List<GetDescargaBoletaData_Result> GetDescargaBoletaData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int TipoFecha = Convert.ToInt32(param[0]);
            DateTime fechaInicio = Convert.ToDateTime(param[1]);
            DateTime fechaFin = Convert.ToDateTime(param[2]);


            var lista = db.GetDescargaBoletaData(TipoFecha, fechaInicio, fechaFin, usuarioSistema).ToList();
            return lista;
        }


        private List<GetReportePorDelegacionAutoridadData_Result> GetReportePorDelegacionAutoridadData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int Valor = Convert.ToInt32(param[0]);
            DateTime FechaDesde = Convert.ToDateTime(param[1]);
            DateTime FechaHasta = Convert.ToDateTime(param[2]);
            string idAutoridades = param[3].Replace("|", ",").Replace("-", "").Trim();
            string idDelegaciones = param[4].Replace("|", ",").Replace("-", "").Trim();

            var lista = db.GetReportePorDelegacionAutoridadData(Valor, FechaDesde, FechaHasta, idAutoridades, idDelegaciones, usuarioSistema).ToList();
            return lista;
        }

        private List<GetReporteListadoMultaFijaData_Result> GetReporteListadoMultafijaData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int Valor = Convert.ToInt32(param[0]);
            DateTime FechaDesde = Convert.ToDateTime(param[1]);
            DateTime FechaHasta = Convert.ToDateTime(param[2]);
            string idInspectores = param[3].Replace("|", ",").Replace("-", "").Trim();
            string idDelegaciones = param[4].Replace("|", ",").Replace("-", "").Trim();

            var lista = db.GetReporteListadoMultaFijaData(FechaDesde, FechaHasta, Valor, idInspectores, idDelegaciones, usuarioSistema).ToList();
            return lista;
        }
        private List<GetReporteListadoParteOficialData_Result> GetReporteListadoParteOficialData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int Valor = Convert.ToInt32(param[0]);
            DateTime FechaDesde = Convert.ToDateTime(param[1]);
            DateTime FechaHasta = Convert.ToDateTime(param[2]);
            string idInspectores = param[3].Replace("|", ",").Replace("-", "").Trim();
            string idDelegaciones = param[4].Replace("|", ",").Replace("-", "").Trim();

            var lista = db.GetReporteListadoParteOficialData(FechaDesde, FechaHasta, Valor, idInspectores, idDelegaciones, usuarioSistema).ToList();
            return lista;
        }

        private List<GetConsultaeImpresionDeParteOficialData_Result> GetConsultaeImpresionDeParteOficialData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int TipoConsulta = Convert.ToInt32(param[0]);
            string Parametro1 = param[1];
            string Parametro2 = param[2];
            string Parametro3 = param[3];
            //string Parametro4 = param[4];

            if (Parametro3 == "null")
            {
                db.Database.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
                var lista1 = db.GetConsultaeImpresionDeParteOficialData(TipoConsulta, Parametro1, Parametro2, "-0", usuarioSistema).ToList();
                return lista1;
            }
            db.Database.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
            var lista = db.GetConsultaeImpresionDeParteOficialData(TipoConsulta, Parametro1, Parametro2, Parametro3, usuarioSistema).ToList();
            return lista;
        }

        private List<GetReportePorUsuarioData_Result> GetReportePorUsuarioData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            string IdUsuario = param[0].Replace("|", ",").Replace("-", "").Trim();
            DateTime fechaInicio = Convert.ToDateTime(param[1]);
            DateTime fechaFin = Convert.ToDateTime(param[2]);


            var lista = db.GetReportePorUsuarioData(IdUsuario, fechaInicio, fechaFin, usuarioSistema).ToList();
            return lista;
        }

        private List<GetBitacoraDeAplicacion_Result> GetBitacoraAplicacionData(string parametros)
        {
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            string[] param = parametros.Split(',');
            string tipoConsulta1 = param[0];
            string tipoConsulta2 = param[1];

            if (param[2] != "null")
            {
                fechaInicio = Convert.ToDateTime(param[2]);
            }
            if (param[3] != "null")
            {
                fechaFin = Convert.ToDateTime(param[3]);
            }

            fechaInicio.ToString("dd-MM-yyyy");
            fechaFin.ToString("dd-MM-yyyy");
            string IdUsuario = param[4].Replace("|", ",").Replace("-", "").Trim();


            var lista = db.GetBitacoraDeAplicacion(tipoConsulta1, tipoConsulta2, fechaInicio, fechaFin, IdUsuario).ToList();
            return lista;
        }



        private List<GetConsultaeImpresionDeBoletasData_Result> GetConsultaeImpresionDeBoletasData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            DateTime FechaDesde = Convert.ToDateTime(param[0]);
            DateTime FechaHasta = Convert.ToDateTime(param[1]);
            string idDelegacion = param[2].Replace("|", ",").Replace("-", "").Trim();
            string idInspector = param[3].Replace("|", ",").Replace("-", "").Trim();

            var lista = db.GetConsultaeImpresionDeBoletasData(FechaDesde, FechaHasta, idDelegacion, idInspector, usuarioSistema).ToList();
            return lista;
        }

        private List<GetReporteStatusActualPlanoData_Result> GetReporteStatusActualPlanoData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            int statusPlano = Convert.ToInt32(param[0]);
            DateTime FechaDesde = Convert.ToDateTime(param[1]);
            DateTime FechaHasta = Convert.ToDateTime(param[2]);
            string idDelegaciones = param[3].Replace("|", ",").Replace("-", "").Trim();
            string idAutoridades = param[4].Replace("|", ",").Replace("-", "").Trim();


            var lista = db.GetReporteStatusActualPlanoData(statusPlano, FechaDesde, FechaHasta, idAutoridades, idDelegaciones, usuarioSistema).ToList();
            return lista;
        }

        private List<GetReimpresionDeBoletasDeCampoData_Result> GetReimpresionDeBoletasDeCampoData(string parametros)
        {
            var usuarioSistema = User.Identity.Name;

            string[] param = parametros.Split(',');
            string Serie = param[0];
            string NumeroBoleta = param[1];

            var lista = db.GetReimpresionDeBoletasDeCampoData(Serie, NumeroBoleta, usuarioSistema).ToList();
            return lista;
        }

        #endregion


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            string reporteID = Request.QueryString["reporteID"];
            string parametros = Request.QueryString["parametros"];
            List<string> lstPDF = new List<string>();

            switch (reporteID)
            {

                case "_ConsultaeImpresionDeParteOficial":

                    string[] parame = parametros.Split(',');
                    int TipoConsulta = Convert.ToInt32(parame[0]);
                    string Parametro1 = parame[1];
                    string Parametro2 = parame[2];
                    string Parametro3 = parame[3];

                    if (TipoConsulta == 1)
                    {
                        int serieParte1 = Convert.ToInt32(Parametro1);
                        decimal numeroParte1 = Convert.ToDecimal(Parametro2);

                        var fuente1 = (db.PARTEOFICIAL.Where(a => a.Serie == Parametro1 && a.NumeroParte == Parametro2).Select(a => a.Fuente).ToList());
                        string CodigoFuente1 = fuente1.ToArray().FirstOrDefault() == null ? "0" : fuente1.ToArray().FirstOrDefault().ToString();

                        lstPDF = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serieParte1 && oa.numero_boleta == numeroParte1 && oa.extension == "PDF").Select(oa => oa.nombre).ToList();
                    }
                    if (TipoConsulta == 2)
                    {
                        int serieBoleta2 = Convert.ToInt32(Parametro1);
                        decimal numeroBoleta2 = Convert.ToDecimal(Parametro2);

                        var numeroPart2 = (db.BOLETA.Where(a => a.serie == serieBoleta2 && a.numero_boleta == numeroBoleta2).Select(a => a.numeroparte).ToList());
                        string CodigoNumParte2 = numeroPart2.ToArray().FirstOrDefault() == null ? "0" : numeroPart2.ToArray().FirstOrDefault().ToString();

                        var seriePart2 = (db.PARTEOFICIAL.Where(a => a.NumeroParte == CodigoNumParte2).Select(a => a.Serie).ToList());
                        string CodigoSerie2 = seriePart2.ToArray().FirstOrDefault() == null ? "0" : seriePart2.ToArray().FirstOrDefault().ToString();

                        var fuente2 = (db.PARTEOFICIAL.Where(a => a.Serie == CodigoSerie2 && a.NumeroParte == CodigoNumParte2).Select(a => a.Fuente).ToList());
                        string CodigoFuente2 = fuente2.ToArray().FirstOrDefault() == null ? "0" : fuente2.ToArray().FirstOrDefault().ToString();

                        int serieParte2 = Convert.ToInt32(CodigoSerie2);
                        decimal numeroParte2 = Convert.ToDecimal(CodigoNumParte2);                        

                        lstPDF = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero_boleta == numeroParte2 && oa.extension == "PDF").Select(oa => oa.nombre).ToList();                        
                    }
                    break;
            }

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;

            // Elimina archivos creados con mas de 10 minutos respecto a la hora actual
            var files = new DirectoryInfo(HttpContext.Current.Server.MapPath("")).GetFiles("*.pdf");
            foreach (var file in files)
            {
                if (DateTime.UtcNow - file.CreationTimeUtc > TimeSpan.FromMinutes(10))
                {
                    File.Delete(file.FullName);
                }
            }

            string fileName = Path.ChangeExtension(Path.GetTempFileName(), ".pdf");
            string fileReportName = "Reporte" + DateTime.Now.ToString("HHmmss") + ".pdf";

            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType,
               out encoding, out extension, out streamids, out warnings);

            FileStream fs = new FileStream(String.Format("{0}", fileName),
            FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();

            //Open existing PDF
            Document document = new Document(PageSize.LETTER);
            List<PdfReader> lstReader = new List<PdfReader>(); //Lista de PDFs
            Dictionary<int, int> numberOfPages = new Dictionary<int, int>(); //Numero de paginas por cada PDF

            lstReader.Add(new PdfReader(fileName));
            numberOfPages.Add(0, lstReader[0].NumberOfPages);

            int newIndex = 1;
            string ruta = ConfigurationManager.AppSettings["UploadFilePath"];
            
            foreach (var item in lstPDF)
            {
                lstReader.Add(new PdfReader(Path.Combine(ruta, item)));
                numberOfPages.Add(newIndex, lstReader[newIndex].NumberOfPages);
                newIndex++;
            }

            //Getting a instance of new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(
               HttpContext.Current.Server.MapPath(fileReportName), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            int i = 0;
            int p = 0;
            Rectangle psize = lstReader[0].GetPageSize(1);

            float width = psize.Width;
            float height = psize.Height;

            //Add Page to new document
            for (int indexPDF = 0; indexPDF < lstReader.Count; indexPDF++)
            {
                while (i < numberOfPages[indexPDF])
                {
                    document.NewPage();
                    p++;
                    i++;

                    PdfImportedPage page1 = writer.GetImportedPage(lstReader[indexPDF], i);
                    cb.AddTemplate(page1, 0, 0);
                }
                i = 0;
            }


            //Attach javascript to the document
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();

            //Attach pdf to the iframe
            frmPrint.Attributes["src"] = fileReportName;

        }
    }
}