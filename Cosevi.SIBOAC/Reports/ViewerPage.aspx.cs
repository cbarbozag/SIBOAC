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
using System.Data.SqlClient;
using System.Text;
using System.Xml;

namespace Cosevi.SIBOAC.Reports
{
    public partial class ViewerPage : System.Web.UI.Page
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        private PC_HH_AndroidEntities dbPivot = new PC_HH_AndroidEntities();

        string connectionString = ConfigurationManager.AppSettings["AbrirConexionBD"];

        private SIBOACSecurityEntities dbSecurity = new SIBOACSecurityEntities();

        /// <summary>
        /// Método de logica para adjuntar imágenes, firmas, planos, función de botones especiales 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string reporteID = Request.QueryString["reporteID"];
                string nombreReporte = Request.QueryString["nombreReporte"];
                string parametros = Request.QueryString["parametros"];
                DataTable listaArchivos = new DataTable();
                listaArchivos.Columns.Add("NombreArchivo");
                DataTable listaArchivosB = new DataTable();
                listaArchivosB.Columns.Add("NombreArchivo");
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

                        string[] extensionRestringidaB = ConfigurationManager.AppSettings["ExtenException"].Split(',');

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
                        //string ruta = ConfigurationManager.AppSettings["UploadFilePath"];
                        string rutaServer = ConfigurationManager.AppSettings["UploadFilePath"];
                        string rutaVi = ConfigurationManager.AppSettings["RutaVirtual"];                        

                        #region Firmas Testigos

                        var tipoITestigo = db.TESTIGOXBOLETA.Where(a => a.serie == serie && a.numero == numero_boleta).Select(a => a.tipo_ide).ToList();
                        var IdTestigo = db.TESTIGOXBOLETA.Where(a => a.serie == serie && a.numero == numero_boleta).Select(a => a.identificacion).ToList();
                        var TestigoB = (db.TESTIGO.Where(a => tipoITestigo.Contains(a.tipo_ide) && IdTestigo.Contains(a.identificacion)).ToList());

                        listaFirmas.Columns.Add("ParteOficial");
                        listaFirmas.Columns.Add("Identificacion");

                        foreach (var item in TestigoB)
                        {
                            var FirmaTestigoB = string.Format("{0}-{1}-{2}-t-{3}.png", CodigoFuente, serie, numero_boleta, item.identificacion);

                            string existeT = @"" + rutaServer + "\\" + FirmaTestigoB;

                            if (System.IO.File.Exists(existeT))
                            {
                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta, FirmaTestigoB)).AbsoluteUri, numero_boleta, item.identificacion);
                            }
                            else
                            {
                                SqlConnection connection = new SqlConnection(connectionString);
                                connection.Open();

                                //Especificamos la consulta que nos devuelve la imagen
                                SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo and Identificacion=@ident",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = CodigoFuente;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = serie;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = numero_boleta;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "t";
                                    cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = item.identificacion;

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                    if (barrImg != null)
                                    {
                                        //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                        string strfn = Server.MapPath(rutaVi + CodigoFuente.ToString() + "-" + serie.ToString() + "-" + numero_boleta.ToString() + "-t-" + item.identificacion + ".png");

                                        FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                        fs.Write(barrImg, 0, barrImg.Length);
                                        fs.Flush();
                                        fs.Close();

                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta, FirmaTestigoB)).AbsoluteUri, numero_boleta, item.identificacion);
                                }
                                    
                            }
                        }
                        #endregion                                              

                        string numero_B = Convert.ToString(numero_boleta);

                        #region Adjuntar archivos

                        var adjBoleta = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente && oa.serie == serie && oa.numero == numero_boleta && !extensionRestringidaB.Contains(oa.extension)).Select(oa => oa.nombre);

                        listaArchivosB.Columns.Add("NumBoleta");
                       
                        foreach (var item in adjBoleta)
                        {
                            listaArchivosB.Rows.Add(new Uri(Path.Combine(ruta, item)).AbsoluteUri, numero_boleta);
                        }

                        if (adjBoleta.Count() == 0)
                        {
                            SqlConnection connection = new SqlConnection(connectionString);
                            connection.Open();

                            var adj = db.IMAGENES.Where(a => a.Fuente == CodigoFuente && a.Serie == serie && a.Numero == numero_B).ToList();
                            int contador = 1;
                            foreach (var adjFinal in adj)
                            {
                                //Especificamos la consulta que nos devuelve la imagen
                                SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                        "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                        "and Tipo=@tipo and Identificacion=@ident",
                                                        connection);
                                //Especificamos el parámetro ID de la consulta
                                cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = CodigoFuente;
                                cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = serie;
                                cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = param[1];
                                cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "a";
                                cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = adjFinal.Identificacion;

                                //Ejecutamos un Scalar para recuperar sólo la imagen
                                byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                var existeA = string.Format("{0}-{1}-{2}-a-{3}.png", CodigoFuente, serie, param[1], adjFinal.Identificacion.Trim());
                                string existeAdj = @"" + rutaServer + "\\" + existeA;

                                if (System.IO.File.Exists(existeAdj))
                                {

                                    listaArchivos.Rows.Add(new Uri(Path.Combine(ruta, existeA)).AbsoluteUri, numero_boleta);

                                }
                                else
                                {
                                    if (barrImg != null)
                                    {
                                        //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                        string strfn = Server.MapPath(rutaVi + CodigoFuente.ToString() + "-" + serie.ToString() + "-" + param[1].ToString() + "-a-" + adjFinal.Identificacion.Trim() + ".png");

                                        FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                        fs.Write(barrImg, 0, barrImg.Length);
                                        fs.Flush();
                                        fs.Close();

                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta, existeA)).AbsoluteUri, numero_boleta);

                                        contador++;
                                    }                                    
                                }

                            }
                        }
                        #endregion

                        ReportParameter[] parameters = new ReportParameter[2];

                        #region Firma Usuario

                        var fileUsuario = string.Format("{0}-{1}-{2}-u-{3}.png", CodigoFuente, serie, numero_boleta, CodigoIde);

                        string existeU = @"" + rutaServer + "\\" + fileUsuario;                        

                        if (System.IO.File.Exists(existeU))
                        {
                            var fullPathUsuario = Path.Combine(ruta, fileUsuario);

                            string imgFirmaUsuarioPath = new Uri(fullPathUsuario).AbsoluteUri;
                            parameters[0] = new ReportParameter("ImagenFirmaUsuarioPath", imgFirmaUsuarioPath);
                        }
                        else
                        {
                            SqlConnection connection = new SqlConnection(connectionString);
                            connection.Open();

                            //Especificamos la consulta que nos devuelve la imagen
                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                    "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                    "and Tipo=@tipo and Identificacion=@ident",
                                                    connection);
                            //Especificamos el parámetro ID de la consulta
                            cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = CodigoFuente;
                            cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = serie;
                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = numero_boleta;
                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "u";
                            cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = CodigoIde;

                            //Ejecutamos un Scalar para recuperar sólo la imagen
                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                            if (barrImg != null)
                            {
                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                string strfn = Server.MapPath(rutaVi + CodigoFuente.ToString() + "-" + serie.ToString() + "-" + numero_boleta.ToString() + "-u-" + CodigoIde + ".png");

                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                fs.Write(barrImg, 0, barrImg.Length);
                                fs.Flush();
                                fs.Close();                               
                            }

                            var fullPathUsuario = Path.Combine(ruta, fileUsuario);
                            string imgFirmaUsuarioPath = new Uri(fullPathUsuario).AbsoluteUri;
                            parameters[0] = new ReportParameter("ImagenFirmaUsuarioPath", imgFirmaUsuarioPath);

                        }
                        #endregion

                        #region Fimra Inspector

                        var fileInspector = string.Format("{0}-{1}-{2}-i-{3}.png", CodigoFuente, serie, numero_boleta, CodInsp);

                        string existeI = @"" + rutaServer + "\\" + fileInspector;

                        if (System.IO.File.Exists(existeI))
                        {
                            var fullPathInspector = Path.Combine(ruta, fileInspector);
                            string imgFirmaInspectorPath = new Uri(fullPathInspector).AbsoluteUri;
                            parameters[1] = new ReportParameter("ImagenFirmaInspectorPath", imgFirmaInspectorPath);
                        }
                        else
                        {                           

                            SqlConnection connection = new SqlConnection(connectionString);
                            connection.Open();

                            //Especificamos la consulta que nos devuelve la imagen
                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                    "where Numero=@numero " +
                                                    "and Tipo=@tipo",
                                                    connection);
                            //Especificamos el parámetro ID de la consulta
                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = CodInsp;
                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "I";

                            //Ejecutamos un Scalar para recuperar sólo la imagen
                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                            if (barrImg != null)
                            {
                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                string strfn = Server.MapPath(rutaVi + CodigoFuente.ToString() + "-" + serie.ToString() + "-" + numero_boleta.ToString() + "-i-" + CodInsp + ".png");

                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                fs.Write(barrImg, 0, barrImg.Length);
                                fs.Flush();
                                fs.Close();                                
                            }

                            var fullPathInspector = Path.Combine(ruta, fileInspector);
                            string imgFirmaInspectorPath = new Uri(fullPathInspector).AbsoluteUri;
                            parameters[1] = new ReportParameter("ImagenFirmaInspectorPath", imgFirmaInspectorPath);

                        }

                        #endregion

                        ReportViewer1.LocalReport.SetParameters(parameters);

                        this.ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;                        
                        Session["_ConsultaeImpresionDeParteOficialDataBoleta"] = listaArchivosB;
                        Session["_ConsultaeImpresionDeParteOficialDataFirma"] = listaFirmas;
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
                            //string ruta1 = ConfigurationManager.AppSettings["UploadFilePath"];
                            string rutaPlano1 = ConfigurationManager.AppSettings["UploadFilePath"];

                            #region Convertir SVG a PNG
                            var extSvg = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serParte1 && oa.numero == numeParte1 && oa.extension == "SVG").Select(oa => oa.nombre);
                            

                            foreach (string item in extSvg)
                            {
                                string filePath = Path.Combine(rutaPlano1, item);
                                var sampleDoc = SvgDocument.Open(filePath);
                                string nombrePng = item.Replace(".svg", ".png");                                

                                string ext = Path.GetExtension(nombrePng).Replace(".", "");
                                int? maxValue = dbPivot.OtrosAdjuntos.Where(oa => String.Compare(oa.extension, ext, false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                //string nombre = item;
                                sampleDoc.Draw().Save(Path.Combine(rutaPlano1, nombrePng));

                                var svgConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente1, serParte1, numeParte1, item);
                                svgConvertido.extension = "svgc";

                                dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                {
                                    fuente = CodigoFuente1,
                                    serie = serParte1,
                                    numero = numeParte1,
                                    extension = ext,
                                    fechaRegistro = DateTime.Now,
                                    nombre = nombrePng,
                                    consecutivo_extension = maxValue.Value + 1
                                });

                                dbPivot.SaveChanges();
                                
                            }
                            #endregion

                            var ext1 = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serParte1 && oa.numero == numeParte1 && !extensionRestringida.Contains(oa.extension)).Select(oa => oa.nombre);

                            listaArchivos.Columns.Add("ParteOficial");                            

                            foreach (string item in ext1)
                            {
                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta1, item)).AbsoluteUri, numeroParte1);
                            }

                            var listFirma = (db.BOLETA.Where(a => a.serie_parteoficial == Parametro1 && a.numeroparte == Parametro2).ToList());
                            var listaTestigoP = (db.TESTIGOXPARTE.Where(a => a.serie == Parametro1 && a.numeroparte == Parametro2).ToList());
                            var listaTestigoB = (db.TESTIGO.Where(a => SerieBoleta1.Contains(a.serie.GetValueOrDefault()) && Boleta1.Contains(a.numero.GetValueOrDefault())).ToList());

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");


                            string v_nombre = null;
                            foreach (var item in listFirma)
                            {
                                if (v_nombre == null)
                                {
                                    var FirmaInspector = string.Format("{0}-{1}-{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                    v_nombre = "1";
                                }
                                var FirmaUsuario = string.Format("{0}-{1}-{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);                                
                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                            }

                            foreach (var item in listaTestigoP)
                            {
                                var FirmaTestigoP = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numeroparte, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaTestigoP)).AbsoluteUri, item.numeroparte, item.identificacion);
                            }

                            foreach (var item in listaTestigoB)
                            {
                                var FirmaTestigoB = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

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
                            var extSvg = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero == numeroParte2 && oa.extension == "SVG").Select(oa => oa.nombre);


                            foreach (string item in extSvg)
                            {
                                string filePath = Path.Combine(rutaPlano2, item);
                                var sampleDoc = SvgDocument.Open(filePath);
                                string nombrePng = item.Replace(".svg", ".png");

                                string ext = Path.GetExtension(nombrePng).Replace(".", "");
                                int? maxValue = dbPivot.OtrosAdjuntos.Where(oa => String.Compare(oa.extension, ext, false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                //string nombre = String.Format("{0}-{1}-{2}-{3}.{4}", CodigoFuente2, serieParte2, numeroParte2, maxValue.Value + 1, ext);
                                sampleDoc.Draw().Save(Path.Combine(rutaPlano2, nombrePng));

                                var svgConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente2, serieParte2, numeroParte2, item);
                                svgConvertido.extension = "svgc";

                                dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                {
                                    fuente = CodigoFuente2,
                                    serie = serieParte2,
                                    numero = numeroParte2,
                                    extension = ext,
                                    fechaRegistro = DateTime.Now,
                                    nombre = nombrePng,
                                    consecutivo_extension = maxValue.Value + 1
                                });

                                dbPivot.SaveChanges();

                            }
                            #endregion

                            var ext2 = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero == numeroParte2 && !extensionRestringida.Contains(oa.extension)).Select(oa => oa.nombre);

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
                                    var FirmaInspector = string.Format("{0}-{1}-{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                    v_nombre = "1";
                                }
                                var FirmaUsuario = string.Format("{0}-{1}-{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);
                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                            }

                            foreach (var item in listaTestigoP)
                            {
                                var FirmaTestigoP = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numeroparte, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaTestigoP)).AbsoluteUri, item.numeroparte, item.identificacion);
                            }

                            foreach (var item in listaTestigoB)
                            {
                                var FirmaTestigoB = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

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
                            var extSvg3 = db.OtrosAdjuntos.Where(oa => fuente3.Contains(oa.fuente) && serParte3.Contains(oa.serie) && numPartConv3.Contains(oa.numero) && oa.extension == "SVG").Select(oa => oa.nombre).ToList();
                            var extSvgFuente3 = db.OtrosAdjuntos.Where(oa => extSvg3.Contains(oa.nombre)).Select(oa => oa.fuente).ToList();
                            var extSvgSerie3 = db.OtrosAdjuntos.Where(oa => extSvg3.Contains(oa.nombre)).Select(oa => oa.serie).ToList();
                            var extSvgParte3 = db.OtrosAdjuntos.Where(oa => extSvg3.Contains(oa.nombre)).Select(oa => oa.numero).ToList();

                            var listPlanos3 = extSvg3.Zip(extSvgParte3, (n, w) => new { NombreAr = n, NumPar = w }).Zip(extSvgSerie3, (x, z) => Tuple.Create(x.NombreAr,x.NumPar,z)).Zip(extSvgFuente3, (y,r) => Tuple.Create(y.Item1,y.Item2,y.Item3,r));

                            foreach (var item in listPlanos3)
                            {
                                string filePath = Path.Combine(rutaPlano3, item.Item1);
                                var sampleDoc = SvgDocument.Open(filePath);
                                string nombrePng = item.Item1.Replace(".svg", ".png");

                                string ext = Path.GetExtension(nombrePng).Replace(".", "");
                                int? maxValue = dbPivot.OtrosAdjuntos.Where(oa => String.Compare(oa.extension, ext, false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                //string nombre = String.Format("{0}-{1}-{2}-{3}.{4}", item.Item4, item.Item3, item.Item2, maxValue.Value + 1, ext);
                                sampleDoc.Draw().Save(Path.Combine(rutaPlano3, nombrePng));

                                var svgConvertido = dbPivot.OtrosAdjuntos.Find(item.Item4, item.Item3, item.Item2, item.Item1);
                                svgConvertido.extension = "svgc";

                                dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                {
                                    fuente = item.Item4,
                                    serie = item.Item3,
                                    numero = item.Item2,
                                    extension = ext,
                                    fechaRegistro = DateTime.Now,
                                    nombre = nombrePng,
                                    consecutivo_extension = maxValue.Value + 1
                                });

                                dbPivot.SaveChanges();

                            }
                            #endregion

                            var nombreAdjuntos3 = db.OtrosAdjuntos.Where(oa => fuente3.Contains(oa.fuente) && serParte3.Contains(oa.serie) && numPartConv3.Contains(oa.numero) && !extensionRestringida.Contains(oa.extension)).Select(oa => oa.nombre).ToList();
                            var numPartLista3 = db.OtrosAdjuntos.Where(oa => nombreAdjuntos3.Contains(oa.nombre)).Select(oa => oa.numero).ToList();

                            listaArchivos.Columns.Add("ParteOficial");

                            string ruta3 = ConfigurationManager.AppSettings["DownloadFilePath"];

                            var listaAdjuntos3 = nombreAdjuntos3.Zip(numPartLista3, (n, w) => new { NombreAr = n, NumPar = w });

                            foreach (var item in listaAdjuntos3)
                            {
                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta3, item.NombreAr)).AbsoluteUri, item.NumPar);
                            }

                            var listFirma = (db.BOLETA.Where(a => seriePart3.Contains(a.serie_parteoficial) && numPartPar3.Contains(a.numeroparte) && seriBolet3.Contains(a.serie) && numeroBoleta3.Contains(a.numero_boleta)).OrderBy(a => new {a.fuente_parteoficial,a.serie_parteoficial,a.numeroparte}).ToList());
                            var listaTestigoP = (db.TESTIGOXPARTE.Where(a => seriePart3.Contains(a.serie) && numPartPar3.Contains(a.numeroparte)).ToList());
                            var listaTestigoB = (db.TESTIGO.Where(a => seriBolet3.Contains(a.serie.GetValueOrDefault()) && numeroBoleta3.Contains(a.numero.GetValueOrDefault())).ToList());

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");

                            string v_fuente = null;
                            string v_serie = null;
                            string v_numparte = null;

                            foreach (var item in listFirma)
                            {
                                if(v_fuente != item.fuente_parteoficial || v_serie != item.serie_parteoficial || v_numparte != item.numeroparte)
                                {
                                    var FirmaInspector = string.Format("{0}-{1}-{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                    v_fuente = item.fuente_parteoficial;
                                    v_serie = item.serie_parteoficial;
                                    v_numparte = item.numeroparte;
                                }
                                
                                var FirmaUsuario = string.Format("{0}-{1}-{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);                                
                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                            }

                            foreach (var item in listaTestigoP)
                            {
                                var FirmaTestigoP = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numeroparte, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaTestigoP)).AbsoluteUri, item.numeroparte, item.identificacion);
                            }

                            foreach (var item in listaTestigoB)
                            {
                                var FirmaTestigoB = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

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
                            var extSvg4 = db.OtrosAdjuntos.Where(oa => fuente4.Contains(oa.fuente) && serParte4.Contains(oa.serie) && numPartConv4.Contains(oa.numero) && oa.extension == "SVG").Select(oa => oa.nombre).ToList();
                            var extSvgFuente4 = db.OtrosAdjuntos.Where(oa => extSvg4.Contains(oa.nombre)).Select(oa => oa.fuente).ToList();
                            var extSvgSerie4 = db.OtrosAdjuntos.Where(oa => extSvg4.Contains(oa.nombre)).Select(oa => oa.serie).ToList();
                            var extSvgParte4 = db.OtrosAdjuntos.Where(oa => extSvg4.Contains(oa.nombre)).Select(oa => oa.numero).ToList();

                            var listPlanos4 = extSvg4.Zip(extSvgParte4, (n, w) => new { NombreAr = n, NumPar = w }).Zip(extSvgSerie4, (x, z) => Tuple.Create(x.NombreAr, x.NumPar, z)).Zip(extSvgFuente4, (y, r) => Tuple.Create(y.Item1, y.Item2, y.Item3, r));

                            foreach (var item in listPlanos4)
                            {
                                string filePath = Path.Combine(rutaPlano4, item.Item1);
                                var sampleDoc = SvgDocument.Open(filePath);
                                string nombrePng = item.Item1.Replace(".svg", ".png");

                                string ext = Path.GetExtension(nombrePng).Replace(".", "");
                                int? maxValue = dbPivot.OtrosAdjuntos.Where(oa => String.Compare(oa.extension, ext, false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                //string nombre = String.Format("{0}-{1}-{2}-{3}.{4}", item.Item4, item.Item3, item.Item2, maxValue.Value + 1, ext);
                                sampleDoc.Draw().Save(Path.Combine(rutaPlano4, nombrePng));

                                var svgConvertido = dbPivot.OtrosAdjuntos.Find(item.Item4, item.Item3, item.Item2, item.Item1);
                                svgConvertido.extension = "svgc";

                                dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                {
                                    fuente = item.Item4,
                                    serie = item.Item3,
                                    numero = item.Item2,
                                    extension = ext,
                                    fechaRegistro = DateTime.Now,
                                    nombre = nombrePng,
                                    consecutivo_extension = maxValue.Value + 1
                                });

                                dbPivot.SaveChanges();

                            }
                            #endregion


                            var nombreAdjuntos4 = db.OtrosAdjuntos.Where(oa => fuente4.Contains(oa.fuente) && serParte4.Contains(oa.serie) && numPartConv4.Contains(oa.numero) && !extensionRestringida.Contains(oa.extension)).Select(oa => oa.nombre).ToList();
                            var numPartLista4 = db.OtrosAdjuntos.Where(oa => nombreAdjuntos4.Contains(oa.nombre)).Select(oa => oa.numero).ToList();

                            listaArchivos.Columns.Add("ParteOficial");
                            
                            string ruta4 = ConfigurationManager.AppSettings["DownloadFilePath"];

                            var listaAdjuntos4 = nombreAdjuntos4.Zip(numPartLista4, (n, w) => new { NombreAr = n, NumPar = w });

                            foreach (var item in listaAdjuntos4)
                            {
                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta4, item.NombreAr)).AbsoluteUri, item.NumPar);
                            }

                            var listFirma = (db.BOLETA.Where(a => seriePart4.Contains(a.serie_parteoficial) && numPartPar4.Contains(a.numeroparte) && seriBolet4.Contains(a.serie) && numeroBoleta4.Contains(a.numero_boleta)).OrderBy(a => new { a.fuente_parteoficial, a.serie_parteoficial, a.numeroparte }).ToList());
                            var listaTestigoP = (db.TESTIGOXPARTE.Where(a => seriePart4.Contains(a.serie) && numPartPar4.Contains(a.numeroparte)).ToList());
                            var listaTestigoB = (db.TESTIGO.Where(a => seriBolet4.Contains(a.serie.GetValueOrDefault()) && numeroBoleta4.Contains(a.numero.GetValueOrDefault())).ToList());

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");

                            string p_fuente = null;
                            string p_serie = null;
                            string p_numparte = null;

                            foreach (var item in listFirma)
                            {
                                if (p_fuente != item.fuente_parteoficial || p_serie != item.serie_parteoficial || p_numparte != item.numeroparte)
                                {
                                    var FirmaInspector = string.Format("{0}-{1}-{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                    p_fuente = item.fuente_parteoficial;
                                    p_serie = item.serie_parteoficial;
                                    p_numparte = item.numeroparte;
                                }

                                var FirmaUsuario = string.Format("{0}-{1}-{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);
                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                               
                            }

                            foreach (var item in listaTestigoP)
                            {
                                var FirmaTestigoP = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numeroparte, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaTestigoP)).AbsoluteUri, item.numeroparte, item.identificacion);
                            }

                            foreach (var item in listaTestigoB)
                            {
                                var FirmaTestigoB = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

                                listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                            }

                            this.ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
                            Session["_ConsultaeImpresionDeParteOficialData"] = listaArchivos;
                            Session["_ConsultaeImpresionDeParteOficialDataFirma"] = listaFirmas;
                            #endregion
                        }

                        #endregion
                        break;

                    case "_ImpresionDeParteOficial":
                        #region ImpresionDeParteOficial
                        ReportViewer1.LocalReport.EnableExternalImages = true;

                        string[] param23 = parametros.Split(',');
                        int TipoConsulta2 = Convert.ToInt32(param23[0]);
                        string Parametro4 = param23[1];
                        string Parametro5 = param23[2];
                        string Parametro6 = param23[3];                        

                        string[] extensionRestringidaIPO = ConfigurationManager.AppSettings["ExtenException"].Split(',');

                        if (TipoConsulta2 == 1)
                        {
                            #region Consulta 1
                            string serieParte1 = Parametro4;
                            string numeroParte1 = Parametro5;

                            var fuente1 = (db.PARTEOFICIAL.Where(a => a.Serie == Parametro4 && a.NumeroParte == Parametro5).Select(a => a.Fuente).ToList());
                            string CodigoFuente1 = fuente1.ToArray().FirstOrDefault() == null ? "0" : fuente1.ToArray().FirstOrDefault().ToString();

                            var Boleta1 = (db.BOLETA.Where(a => a.serie_parteoficial == serieParte1 && a.numeroparte == numeroParte1).Select(a => a.numero_boleta).ToList());
                            var SerieBoleta1 = (db.BOLETA.Where(a => a.serie_parteoficial == serieParte1 && a.numeroparte == numeroParte1).Select(a => a.serie).ToList());

                            int serParte1 = Convert.ToInt32(Parametro4);
                            decimal numeParte1 = Convert.ToDecimal(Parametro5);

                            string ruta1 = ConfigurationManager.AppSettings["DownloadFilePath"];
                            //string ruta1 = ConfigurationManager.AppSettings["UploadFilePath"];
                            string rutaPlano1 = ConfigurationManager.AppSettings["UploadFilePath"];
                            string rutaV = ConfigurationManager.AppSettings["RutaVirtual"];

                            #region Convertir SVG a PNG

                            var extSvg = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serParte1 && oa.numero == numeParte1 && oa.extension == "SVG").ToList();

                            foreach (var item in extSvg)
                            {
                                var existeSVG = item.nombre;
                                string existeAdjS = @"" + rutaPlano1 + "\\" + existeSVG;
                                string nombrePng = item.nombre.Replace(".svg", ".png");

                                string strfn = Path.Combine(@"" + rutaPlano1 + "\\" + nombrePng);

                                if (System.IO.File.Exists(existeAdjS))
                                {

                                    var sampleDoc = SvgDocument.Open(existeAdjS);                                        
                                    sampleDoc.Draw().Save(strfn);

                                    int? maxValue = db.OtrosAdjuntos.Where(oa => oa.serie == item.serie && oa.numero == item.numero && String.Compare(oa.extension, "png", false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                    var svgConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente1, serParte1, numeParte1, item.nombre);
                                    svgConvertido.extension = "svgc";

                                    dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                    {
                                        fuente = CodigoFuente1,
                                        serie = serParte1,
                                        numero = numeParte1,
                                        extension = "png",
                                        fechaRegistro = DateTime.Now,
                                        nombre = nombrePng,
                                        consecutivo_extension = maxValue.Value + 1
                                    });

                                    dbPivot.SaveChanges();
                                }

                            }
                            #endregion

                            #region Planos

                            var listPlanos = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serParte1 && oa.numero == numeParte1 && oa.nombre.Contains("-p-") && !extensionRestringidaIPO.Contains(oa.extension)).ToList();

                            listaPlanos.Columns.Add("ParteOficial");

                            foreach (var itmeP in listPlanos)
                            {
                                
                                if (itmeP.extension.Contains("c"))
                                {
                                    listaPlanos.Rows.Add(new Uri(Path.Combine(ruta1, itmeP.nombre)).AbsoluteUri, numeroParte1);
                                }
                                else
                                {
                                    string strfn = Path.Combine(@"" + rutaPlano1 + "\\" + itmeP.nombre);

                                    System.Drawing.Bitmap bitmap1;
                                    bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(strfn);
                                    bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                    bitmap1.Save(strfn);                                    

                                    var PlanoConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente1, serParte1, numeParte1, itmeP.nombre);
                                    PlanoConvertido.extension = itmeP.extension + "c";

                                    dbPivot.SaveChanges();

                                    listaPlanos.Rows.Add(new Uri(Path.Combine(ruta1, itmeP.nombre)).AbsoluteUri, numeroParte1);
                                }                                                                
                            }

                            if (listPlanos.Count() == 0)
                            {
                                SqlConnection connection = new SqlConnection(connectionString);                                
                                connection.Open();

                                var adj = db.IMAGENES.Where(a => a.Fuente == CodigoFuente1 && a.Serie == serParte1 && a.Numero == numeroParte1 && a.Tipo == "C").ToList();
                                foreach (var adjFinal in adj)
                                {
                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = CodigoFuente1;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = Parametro4;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = Parametro5;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "C";

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                    var existeA = string.Format("{0}-{1}-{2}-p-{3}.png", CodigoFuente1, Parametro4, Parametro5, adjFinal.Identificacion.Trim());
                                    string existeAdj = @"" + rutaPlano1 + "\\" + existeA;

                                    if (System.IO.File.Exists(existeAdj))
                                    {
                                        listaPlanos.Rows.Add(new Uri(Path.Combine(ruta1, existeA)).AbsoluteUri, numeroParte1);
                                    }
                                    else
                                    {
                                        if (barrImg != null)
                                        {
                                            //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                            string strfn = Server.MapPath(rutaV + CodigoFuente1.ToString() + "-" + Parametro4.ToString() + "-" + Parametro5.ToString() + "-p-" + adjFinal.Identificacion.Trim() + ".png");

                                            FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);                                            
                                            fs.Write(barrImg, 0, barrImg.Length);                                            
                                            fs.Flush();
                                            fs.Close();
                                            
                                            string strfn2 = Path.Combine(@"" + rutaPlano1 + "\\" + existeA);

                                            System.Drawing.Bitmap bitmap1;
                                            bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(strfn2);
                                            bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                            bitmap1.Save(strfn2);

                                        }                   
                                        
                                        listaPlanos.Rows.Add(new Uri(Path.Combine(ruta1, existeA)).AbsoluteUri, numeroParte1);
                                    }

                                }
                            }
                            #endregion
                            
                            #region Ajuntos 1
                            var ext1 = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serParte1 && oa.numero == numeParte1 && !oa.nombre.Contains("-p-") && !oa.nombre.Contains("-u-") && !oa.nombre.Contains("-i-") && !oa.nombre.Contains("-t-") && !extensionRestringidaIPO.Contains(oa.extension)).ToList();

                            listaArchivos.Columns.Add("ParteOficial");

                            foreach (var item in ext1)
                            {
                                if (item.extension.Contains("c"))
                                {
                                    listaArchivos.Rows.Add(new Uri(Path.Combine(ruta1, item.nombre)).AbsoluteUri, numeroParte1);
                                }
                                else
                                {
                                    string existeAdj = @"" + rutaPlano1 + "\\" + item.nombre;

                                    Stream stream = File.OpenRead(existeAdj);
                                    System.Drawing.Image sourceImage = System.Drawing.Image.FromStream(stream, false, false);

                                    int width = sourceImage.Width;
                                    int height = sourceImage.Height;
                                    stream.Close();

                                    if (width > height)
                                    {

                                        System.Drawing.Bitmap bitmap1;
                                        bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(existeAdj);
                                        bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                        bitmap1.Save(existeAdj);

                                        var PlanoConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente1, serParte1, numeParte1, item.nombre);
                                        PlanoConvertido.extension = item.extension + "c";

                                        dbPivot.SaveChanges();

                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta1, item.nombre)).AbsoluteUri, numeroParte1);
                                    }
                                    else
                                    {
                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta1, item.nombre)).AbsoluteUri, numeroParte1);
                                    }
                                }
                                                        
                            }

                            if (ext1.Count()==0)
                            {                                
                                
                                SqlConnection connection = new SqlConnection(connectionString);                                
                                connection.Open();

                                var adj = db.IMAGENES.Where(a => a.Fuente == CodigoFuente1 && a.Serie == serParte1 && a.Numero == numeroParte1 && a.Tipo == "A").ToList();
                                foreach (var adjFinal in adj)
                                {
                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo and Identificacion=@ident",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = CodigoFuente1;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = Parametro4;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = Parametro5;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "a";
                                    cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = adjFinal.Identificacion;

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                    var existeA = string.Format("{0}-{1}-{2}-a-{3}.png", CodigoFuente1, Parametro4, Parametro5, adjFinal.Identificacion.Trim());
                                    string existeAdj = @"" + rutaPlano1 + "\\" + existeA;

                                    if (System.IO.File.Exists(existeAdj))
                                    {
                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta1, existeA)).AbsoluteUri, numeroParte1);
                                    }
                                    else
                                    {                                       
                                        if (barrImg != null)
                                        {
                                            //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                            string strfn = Server.MapPath(rutaV + CodigoFuente1.ToString() + "-" + Parametro4.ToString() + "-" + Parametro5.ToString() + "-a-" + adjFinal.Identificacion.Trim() + ".png");

                                            FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                            fs.Write(barrImg, 0, barrImg.Length);
                                            fs.Flush();                                            
                                            fs.Close();

                                            System.Drawing.Bitmap bitmap1;
                                            bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(existeAdj);
                                            bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                            bitmap1.Save(existeAdj);

                                        }
                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta1, existeA)).AbsoluteUri, numeroParte1);
                                    }                                    
                                }                                
                            }
                            #endregion

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");

                            #region Firmas Testigos P                            

                            //var TipoidTP = db.TESTIGOXPARTE.Where(a => a.serie == Parametro4 && a.numeroparte == Parametro5).Select(a => a.tipo_ide).ToList();
                            //var IdTP = db.TESTIGOXPARTE.Where(a => a.serie == Parametro4 && a.numeroparte == Parametro5).Select(a => a.identificacion).ToList();

                            var firmaTP = (db.TESTIGOXPARTE.Where(a => a.serie == Parametro4 && a.numeroparte == Parametro5).ToList());

                            foreach (var item in firmaTP)
                            {
                                var FirmaTestigoP = string.Format("{0}-{1}-{2}-t-{3}.png", CodigoFuente1, Parametro4, Parametro5, item.identificacion);

                                string existeT = @"" + rutaPlano1 + "\\" + FirmaTestigoP;

                                if (System.IO.File.Exists(existeT))
                                {
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaTestigoP)).AbsoluteUri, Parametro5, item.identificacion);
                                }
                                else
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();

                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo and Identificacion=@ident",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = CodigoFuente1;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = Parametro4;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = Parametro5;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "t";
                                    cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = item.identificacion;

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();                                    

                                    if (barrImg != null)
                                    {
                                        //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                        string strfn = Server.MapPath(rutaV + CodigoFuente1.ToString() + "-" + Parametro4.ToString() + "-" + Parametro5.ToString() + "-t-" + item.identificacion + ".png");

                                        FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                        fs.Write(barrImg, 0, barrImg.Length);
                                        fs.Flush();
                                        fs.Close();

                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaTestigoP)).AbsoluteUri, Parametro5, item.identificacion);
                                    }
                                    
                                }                                 
                                
                            }
                            #endregion

                            #region Firmas Testigos B                            
                            
                            var firmaTB = db.TESTIGOXBOLETA.Where(a => SerieBoleta1.Contains(a.serie) && Boleta1.Contains(a.numero)).ToList();

                            foreach (var item in firmaTB)
                            {
                                var FirmaTestigoB = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

                                string existeT = @"" + rutaPlano1 + "\\" + FirmaTestigoB;

                                if (System.IO.File.Exists(existeT))
                                {
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                                }
                                else
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();

                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo and Identificacion=@ident",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item.fuente;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item.serie;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.numero;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "t";
                                    cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = item.identificacion;

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();                                    

                                    if (barrImg != null)
                                    {
                                        //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                        string strfn = Server.MapPath(rutaV + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero.ToString() + "-t-" + item.identificacion + ".png");

                                        FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                        fs.Write(barrImg, 0, barrImg.Length);
                                        fs.Flush();
                                        fs.Close();
                                    }

                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);                                

                                }                                    
                            }
                            #endregion

                            #region Firma Inspector

                            var listFirma = (db.BOLETA.Where(a => a.serie_parteoficial == Parametro4 && a.numeroparte == Parametro5).ToList());
                            string v_nombre = null;
                            foreach (var item in listFirma)
                            {
                                if (v_nombre == null)
                                {
                                    var FirmaInspector = string.Format("{0}-{1}-{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);

                                    string existeT = @"" + rutaPlano1 + "\\" + FirmaInspector;

                                    if (System.IO.File.Exists(existeT))
                                    {
                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                        v_nombre = "1";
                                    }
                                    else
                                    {
                                        SqlConnection connection = new SqlConnection(connectionString);
                                        connection.Open();

                                        //Especificamos la consulta que nos devuelve la imagen
                                        SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                "where Numero=@numero " +
                                                                "and Tipo=@tipo",
                                                                connection);
                                        //Especificamos el parámetro ID de la consulta
                                        cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.codigo_inspector;
                                        cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "i";


                                        //Ejecutamos un Scalar para recuperar sólo la imagen
                                        byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();                                                 

                                        if (barrImg != null)
                                        {
                                            //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                            string strfn = Server.MapPath(rutaV + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero_boleta.ToString() + "-i-" + item.codigo_inspector + ".png");

                                            FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                            fs.Write(barrImg, 0, barrImg.Length);
                                            fs.Flush();
                                            fs.Close();
                                        }

                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                        v_nombre = "1";                                        
                                    }                                       
                                }
                            }
                            #endregion

                            #region Fimra Usuario

                            foreach (var item in listFirma)
                            {                               
                                var FirmaUsuario = string.Format("{0}-{1}-{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);

                                string existeT = @"" + rutaPlano1 + "\\" + FirmaUsuario;

                                if (System.IO.File.Exists(existeT))
                                {
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                                }
                                else
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();

                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item.fuente;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item.serie;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.numero_boleta;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "F";

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();                                    

                                    if (barrImg!=null)
                                    {
                                        //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                        string strfn = Server.MapPath(rutaV + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero_boleta.ToString() + "-u-" + item.identificacion + ".png");

                                        FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                        fs.Write(barrImg, 0, barrImg.Length);
                                        fs.Flush();
                                        fs.Close();
                                    }
                                   
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta1, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);

                                }
                            }
                            #endregion

                            this.ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
                            Session["_ConsultaeImpresionDeParteOficialData"] = listaArchivos;
                            Session["_ConsultaeImpresionDeParteOficialDataFirma"] = listaFirmas;
                            Session["_ConsultaeImpresionDeParteOficialDataPnano"] = listaPlanos;
                            #endregion
                        }

                        if (TipoConsulta2 == 2)
                        {
                            #region Consulta 2
                            int serieBoleta2 = Convert.ToInt32(Parametro4);
                            decimal numeroBoleta2 = Convert.ToDecimal(Parametro5);

                            var numeroPart2 = (db.BOLETA.Where(a => a.serie == serieBoleta2 && a.numero_boleta == numeroBoleta2).Select(a => a.numeroparte).ToList());
                            string CodigoNumParte2 = numeroPart2.ToArray().FirstOrDefault() == null ? "0" : numeroPart2.ToArray().FirstOrDefault().ToString();

                            var seriePart2 = (db.PARTEOFICIAL.Where(a => a.NumeroParte == CodigoNumParte2).Select(a => a.Serie).ToList());
                            string CodigoSerie2 = seriePart2.ToArray().FirstOrDefault() == null ? "0" : seriePart2.ToArray().FirstOrDefault().ToString();

                            var fuente2 = (db.PARTEOFICIAL.Where(a => a.Serie == CodigoSerie2 && a.NumeroParte == CodigoNumParte2).Select(a => a.Fuente).ToList());
                            string CodigoFuente2 = fuente2.ToArray().FirstOrDefault() == null ? "0" : fuente2.ToArray().FirstOrDefault().ToString();

                            var serBole2 = (db.BOLETA.Where(a => a.fuente_parteoficial == CodigoFuente2 && a.serie_parteoficial == CodigoSerie2 && a.numeroparte == CodigoNumParte2).Select(a => a.serie).ToList());
                            var numBole2 = (db.BOLETA.Where(a => a.fuente_parteoficial == CodigoFuente2 && a.serie_parteoficial == CodigoSerie2 && a.numeroparte == CodigoNumParte2).Select(a => a.numero_boleta).ToList());

                            int serieParte2 = Convert.ToInt32(CodigoSerie2);
                            decimal numeroParte2 = Convert.ToDecimal(CodigoNumParte2);


                            string ruta2 = ConfigurationManager.AppSettings["DownloadFilePath"];
                            string rutaPlano2 = ConfigurationManager.AppSettings["UploadFilePath"];
                            string rutaV2 = ConfigurationManager.AppSettings["RutaVirtual"];

                            #region Convertir SVG a PNG

                            var extSvg = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero == numeroParte2 && oa.extension == "SVG").ToList();

                            foreach (var item in extSvg)
                            {
                                var existeSVG = item.nombre;
                                string existeAdjS = @"" + rutaPlano2 + "\\" + existeSVG;
                                string nombrePng = item.nombre.Replace(".svg", ".png");

                                string strfn = Path.Combine(@"" + rutaPlano2 + "\\" + nombrePng);

                                if (System.IO.File.Exists(existeAdjS))
                                {
                                    var sampleDoc = SvgDocument.Open(existeAdjS);
                                    sampleDoc.Draw().Save(strfn);

                                    int? maxValue = db.OtrosAdjuntos.Where(oa => oa.serie == item.serie && oa.numero == item.numero && String.Compare(oa.extension, "png", false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                    var svgConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente2, serieParte2, numeroParte2, item);
                                    svgConvertido.extension = "svgc";

                                    dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                    {
                                        fuente = CodigoFuente2,
                                        serie = serieParte2,
                                        numero = numeroParte2,
                                        extension = "png",
                                        fechaRegistro = DateTime.Now,
                                        nombre = nombrePng,
                                        consecutivo_extension = maxValue.Value + 1
                                    });

                                    dbPivot.SaveChanges();
                                }

                            }
                            #endregion

                            #region Planos

                            var listPlanos = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero == numeroParte2 && oa.nombre.Contains("-p-") && !extensionRestringidaIPO.Contains(oa.extension)).ToList();

                            listaPlanos.Columns.Add("ParteOficial");

                            foreach (var itmeP in listPlanos)
                            {
                                if (itmeP.extension.Contains("c"))
                                {
                                    listaPlanos.Rows.Add(new Uri(Path.Combine(ruta2, itmeP.nombre)).AbsoluteUri, CodigoNumParte2);
                                }
                                else
                                {
                                    string strfn = Path.Combine(@"" + rutaPlano2 + "\\" + itmeP.nombre);

                                    System.Drawing.Bitmap bitmap1;
                                    bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(strfn);
                                    bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                    bitmap1.Save(strfn);

                                    var PlanoConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente2, serieParte2, numeroParte2, itmeP.nombre);
                                    PlanoConvertido.extension = itmeP.extension + "c";

                                    dbPivot.SaveChanges();

                                    listaPlanos.Rows.Add(new Uri(Path.Combine(ruta2, itmeP.nombre)).AbsoluteUri, CodigoNumParte2);
                                }                                
                            }

                            if (listPlanos.Count() == 0)
                            {
                                SqlConnection connection = new SqlConnection(connectionString);
                                connection.Open();

                                var adj = db.IMAGENES.Where(a => a.Fuente == CodigoFuente2 && a.Serie == serieParte2 && a.Numero == CodigoNumParte2 && a.Tipo == "C").ToList();
                                foreach (var adjFinal in adj)
                                {
                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = CodigoFuente2;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = serieParte2;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = CodigoNumParte2;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "C";

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                    var existeA = string.Format("{0}-{1}-{2}-p-{3}.png", CodigoFuente2, serieParte2, CodigoNumParte2, adjFinal.Identificacion.Trim());
                                    string existeAdj = @"" + rutaPlano2 + "\\" + existeA;

                                    if (System.IO.File.Exists(existeAdj))
                                    {
                                        listaPlanos.Rows.Add(new Uri(Path.Combine(ruta2, existeA)).AbsoluteUri, CodigoNumParte2);
                                    }
                                    else
                                    {
                                        if (barrImg != null)
                                        {
                                            //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                            string strfn = Server.MapPath(rutaV2 + CodigoFuente2.ToString() + "-" + serieParte2.ToString() + "-" + CodigoNumParte2.ToString() + "-p-" + adjFinal.Identificacion.Trim() + ".png");

                                            FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                            fs.Write(barrImg, 0, barrImg.Length);
                                            fs.Flush();
                                            fs.Close();

                                            string strfn2 = Path.Combine(@"" + rutaPlano2 + "\\" + existeA);

                                            System.Drawing.Bitmap bitmap1;
                                            bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(strfn2);
                                            bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                            bitmap1.Save(strfn2);
                                        }
                                        listaPlanos.Rows.Add(new Uri(Path.Combine(ruta2, existeA)).AbsoluteUri, CodigoNumParte2);
                                    }
                                }
                            }
                            #endregion

                            #region Adjuntos

                            var ext2 = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero == numeroParte2 && !oa.nombre.Contains("-p-") && !oa.nombre.Contains("-u-") && !oa.nombre.Contains("-i-") && !oa.nombre.Contains("-t-") && !extensionRestringidaIPO.Contains(oa.extension)).ToList();

                            listaArchivos.Columns.Add("ParteOficial");

                            foreach (var item in ext2)
                            {
                                if (item.extension.Contains("c"))
                                {
                                    listaArchivos.Rows.Add(new Uri(Path.Combine(ruta2, item.nombre)).AbsoluteUri, numeroParte2);
                                }
                                else
                                {
                                    string existeAdj = @"" + rutaPlano2 + "\\" + item.nombre;

                                    Stream stream = File.OpenRead(existeAdj);
                                    System.Drawing.Image sourceImage = System.Drawing.Image.FromStream(stream, false, false);

                                    int width = sourceImage.Width;
                                    int height = sourceImage.Height;
                                    stream.Close();

                                    if (width > height)
                                    {

                                        System.Drawing.Bitmap bitmap1;
                                        bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(existeAdj);
                                        bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                        bitmap1.Save(existeAdj);

                                        var PlanoConvertido = dbPivot.OtrosAdjuntos.Find(CodigoFuente2, serieParte2, numeroParte2, item.nombre);
                                        PlanoConvertido.extension = item.extension + "c";

                                        dbPivot.SaveChanges();

                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta2, item.nombre)).AbsoluteUri, numeroParte2);
                                    }
                                    else
                                    {
                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta2, item.nombre)).AbsoluteUri, numeroParte2);
                                    }
                                }
                            }

                            if (ext2.Count() == 0)
                            {

                                SqlConnection connection = new SqlConnection(connectionString);
                                connection.Open();

                                var adj = db.IMAGENES.Where(a => a.Fuente == CodigoFuente2 && a.Serie == serieParte2 && a.Numero == CodigoNumParte2).ToList();

                                foreach (var adjFinal in adj)
                                {
                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo and Identificacion=@ident",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = CodigoFuente2;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = serieParte2;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = CodigoNumParte2;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "a";
                                    cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = adjFinal.Identificacion;

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                    var existeA = string.Format("{0}-{1}-{2}-a-{3}.png", CodigoFuente2, serieParte2, CodigoNumParte2, adjFinal.Identificacion.Trim());
                                    string existeAdj = @"" + rutaPlano2 + "\\" + existeA;

                                    if (System.IO.File.Exists(existeAdj))
                                    {
                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta2, existeA)).AbsoluteUri, CodigoNumParte2);
                                    }
                                    else
                                    {
                                        if (barrImg != null)
                                        {
                                            //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                            string strfn = Server.MapPath(rutaV2 + CodigoFuente2.ToString() + "-" + serieParte2.ToString() + "-" + CodigoNumParte2.ToString() + "-a-" + adjFinal.Identificacion.Trim() + ".png");

                                            FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                            fs.Write(barrImg, 0, barrImg.Length);
                                            fs.Flush();
                                            fs.Close();

                                            System.Drawing.Bitmap bitmap1;
                                            bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(existeAdj);
                                            bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                            bitmap1.Save(existeAdj);
                                        }

                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta2, existeA)).AbsoluteUri, CodigoNumParte2);
                                    }

                                }
                            }
                            #endregion

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");

                            #region Firma testigos Parte

                            //var TipoidTP = db.TESTIGOXPARTE.Where(a => a.serie == CodigoSerie2 && a.numeroparte == CodigoNumParte2).Select(a => a.tipo_ide).ToList();
                            //var IdTP = db.TESTIGOXPARTE.Where(a => a.serie == CodigoSerie2 && a.numeroparte == CodigoNumParte2).Select(a => a.identificacion).ToList();
                            var firmaTP = (db.TESTIGOXPARTE.Where(a => a.serie == CodigoSerie2 && a.numeroparte == CodigoNumParte2).ToList());

                            foreach (var item in firmaTP)
                            {
                                var FirmaTestigoP = string.Format("{0}-{1}-{2}-t-{3}.png", CodigoFuente2, CodigoSerie2, CodigoNumParte2, item.identificacion);

                                string existeT = @"" + rutaPlano2 + "\\" + FirmaTestigoP;

                                if (System.IO.File.Exists(existeT))
                                {
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaTestigoP)).AbsoluteUri, CodigoNumParte2, item.identificacion);
                                }
                                else
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();

                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo and Identificacion=@ident",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = CodigoFuente2;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = CodigoSerie2;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = CodigoNumParte2;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "t";
                                    cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = item.identificacion;

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                    if (barrImg != null)
                                    {
                                        //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                        string strfn = Server.MapPath(rutaV2 + CodigoFuente2.ToString() + "-" + CodigoSerie2.ToString() + "-" + CodigoNumParte2.ToString() + "-t-" + item.identificacion + ".png");

                                        FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                        fs.Write(barrImg, 0, barrImg.Length);
                                        fs.Flush();
                                        fs.Close();
                                    }

                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaTestigoP)).AbsoluteUri, CodigoNumParte2, item.identificacion);
                                }

                            }

                            #endregion

                            #region Firma testigos Boleta

                            var firmaTB = db.TESTIGOXBOLETA.Where(a => serBole2.Contains(a.serie) && numBole2.Contains(a.numero)).ToList();

                            foreach (var item in firmaTB)
                            {
                                var FirmaTestigoB = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

                                string existeT = @"" + rutaPlano2 + "\\" + FirmaTestigoB;

                                if (System.IO.File.Exists(existeT))
                                {
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                                }
                                else
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();

                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo and Identificacion=@ident",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item.fuente;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item.serie;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.numero;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "t";
                                    cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = item.identificacion;

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                    if (barrImg != null)
                                    {
                                        //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                        string strfn = Server.MapPath(rutaV2 + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero.ToString() + "-t-" + item.identificacion + ".png");

                                        FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                        fs.Write(barrImg, 0, barrImg.Length);
                                        fs.Flush();
                                        fs.Close();
                                    }

                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                                }
                            }
                            #endregion

                            #region Firma Inspector

                            var listFirma = (db.BOLETA.Where(a => a.serie_parteoficial == CodigoSerie2 && a.numeroparte == CodigoNumParte2).ToList());
                            string v_nombre = null;
                            foreach (var item in listFirma)
                            {
                                if (v_nombre == null)
                                {
                                    var FirmaInspector = string.Format("{0}-{1}-{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);

                                    string existeT = @"" + rutaPlano2 + "\\" + FirmaInspector;

                                    if (System.IO.File.Exists(existeT))
                                    {
                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                        v_nombre = "1";
                                    }
                                    else
                                    {
                                        SqlConnection connection = new SqlConnection(connectionString);
                                        connection.Open();

                                        //Especificamos la consulta que nos devuelve la imagen
                                        SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                "where Numero=@numero " +
                                                                "and Tipo=@tipo",
                                                                connection);
                                        //Especificamos el parámetro ID de la consulta
                                        cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.codigo_inspector;
                                        cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "i";


                                        //Ejecutamos un Scalar para recuperar sólo la imagen
                                        byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                        if (barrImg != null)
                                        {
                                            //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                            string strfn = Server.MapPath(rutaV2 + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero_boleta.ToString() + "-i-" + item.codigo_inspector + ".png");

                                            FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                            fs.Write(barrImg, 0, barrImg.Length);
                                            fs.Flush();
                                            fs.Close();
                                        }

                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                        v_nombre = "1";
                                    }
                                }
                            }
                            #endregion

                            #region Fimra Usuario

                            foreach (var item in listFirma)
                            {
                                var FirmaUsuario = string.Format("{0}-{1}-{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);

                                string existeT = @"" + rutaPlano2 + "\\" + FirmaUsuario;

                                if (System.IO.File.Exists(existeT))
                                {
                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                                }
                                else
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();

                                    //Especificamos la consulta que nos devuelve la imagen
                                    SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                            "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                            "and Tipo=@tipo",
                                                            connection);
                                    //Especificamos el parámetro ID de la consulta
                                    cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item.fuente;
                                    cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item.serie;
                                    cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.numero_boleta;
                                    cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "F";

                                    //Ejecutamos un Scalar para recuperar sólo la imagen
                                    byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                    if (barrImg != null)
                                    {
                                        //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                        string strfn = Server.MapPath(rutaV2 + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero_boleta.ToString() + "-u-" + item.identificacion + ".png");

                                        FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                        fs.Write(barrImg, 0, barrImg.Length);
                                        fs.Flush();
                                        fs.Close();
                                    }

                                    listaFirmas.Rows.Add(new Uri(Path.Combine(ruta2, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);

                                }
                            }

                            #endregion                        

                            this.ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
                            Session["_ConsultaeImpresionDeParteOficialData"] = listaArchivos;
                            Session["_ConsultaeImpresionDeParteOficialDataFirma"] = listaFirmas;
                            #endregion
                        }

                        if (TipoConsulta2 == 3)
                        {
                            #region Consulta 3
                            var numeroBoleta3 = (db.BOLETA.Where(a => a.tipo_ide == Parametro4 && a.identificacion == Parametro5 && a.fuente_parteoficial == "4").Select(a => a.numero_boleta).ToList());
                            var seriBolet3 = (db.BOLETA.Where(a => a.tipo_ide == Parametro4 && a.identificacion == Parametro5 && a.fuente_parteoficial == "4").Select(a => a.serie).ToList());

                            var listaPartes3 = (db.BOLETA.Where(a => a.tipo_ide == Parametro4 && a.identificacion == Parametro5 && a.fuente_parteoficial == "4").ToList());

                            listaArchivos.Columns.Add("ParteOficial");

                            listaFirmas.Columns.Add("ParteOficial");
                            listaFirmas.Columns.Add("Identificacion");

                            foreach (var lisPart3 in listaPartes3)
                            {
                                
                                var numPartPar3 = (db.PARTEOFICIAL.Where(a => a.NumeroParte == lisPart3.numeroparte && a.Serie == lisPart3.serie_parteoficial).Select(a => a.NumeroParte).ToList());
                                var numPartConv3 = numPartPar3.Select(s => Convert.ToDecimal(s)).ToList();

                                var seriePart3 = (db.PARTEOFICIAL.Where(a => a.NumeroParte == lisPart3.numeroparte && a.Serie == lisPart3.serie_parteoficial).Select(a => a.Serie).ToList());
                                var serParte3 = seriePart3.Select(s => Convert.ToInt32(s)).ToList();

                                var fuente3 = (db.PARTEOFICIAL.Where(a => a.Serie == lisPart3.serie_parteoficial && a.NumeroParte == lisPart3.numeroparte).Select(a => a.Fuente).ToList());

                                string ruta3 = ConfigurationManager.AppSettings["DownloadFilePath"];
                                string rutaPlano3 = ConfigurationManager.AppSettings["UploadFilePath"];
                                string rutaV3 = ConfigurationManager.AppSettings["RutaVirtual"];

                                var resultAdjParte = fuente3.Zip(serParte3, (e1, e2) => new { e1, e2 }).Zip(numPartPar3, (z1, e3) => Tuple.Create(z1.e1, z1.e2, e3));


                                #region Convertir SVG a PNG

                                var extSvg3 = db.OtrosAdjuntos.Where(oa => fuente3.Contains(oa.fuente) && serParte3.Contains(oa.serie) && numPartConv3.Contains(oa.numero) && oa.extension == "SVG").Select(oa => oa.nombre).ToList();
                                var extSvgFuente3 = db.OtrosAdjuntos.Where(oa => extSvg3.Contains(oa.nombre)).Select(oa => oa.fuente).ToList();
                                var extSvgSerie3 = db.OtrosAdjuntos.Where(oa => extSvg3.Contains(oa.nombre)).Select(oa => oa.serie).ToList();
                                var extSvgParte3 = db.OtrosAdjuntos.Where(oa => extSvg3.Contains(oa.nombre)).Select(oa => oa.numero).ToList();

                                var listPlanos3 = extSvg3.Zip(extSvgParte3, (n, w) => new { NombreAr = n, NumPar = w }).Zip(extSvgSerie3, (x, z) => Tuple.Create(x.NombreAr, x.NumPar, z)).Zip(extSvgFuente3, (y, r) => Tuple.Create(y.Item1, y.Item2, y.Item3, r));

                                foreach (var item in listPlanos3)
                                {
                                    var existeSVG = item.Item1;
                                    string existeAdjS = @"" + rutaPlano3 + "\\" + existeSVG;
                                    string nombrePng = item.Item1.Replace(".svg", ".png");

                                    string strfn = Path.Combine(@"" + rutaPlano3 + "\\" + nombrePng);

                                    if (System.IO.File.Exists(existeAdjS))
                                    {
                                        var sampleDoc = SvgDocument.Open(existeAdjS);
                                        sampleDoc.Draw().Save(strfn);

                                        int? maxValue = db.OtrosAdjuntos.Where(oa => oa.serie == item.Item3 && oa.numero == item.Item2 && String.Compare(oa.extension, "png", false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                        var svgConvertido = dbPivot.OtrosAdjuntos.Find(item.Item4, item.Item3, item.Item2, item.Item1);
                                        svgConvertido.extension = "svgc";

                                        dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                        {
                                            fuente = item.Item4,
                                            serie = item.Item3,
                                            numero = item.Item2,
                                            extension = "png",
                                            fechaRegistro = DateTime.Now,
                                            nombre = nombrePng,
                                            consecutivo_extension = maxValue.Value + 1
                                        });

                                        dbPivot.SaveChanges();
                                    }

                                }
                                #endregion

                                #region Planos

                                int serieP = Convert.ToInt32(lisPart3.serie_parteoficial);
                                decimal numP = Convert.ToDecimal(lisPart3.numeroparte);

                                var listPlanos = db.OtrosAdjuntos.Where(oa => oa.fuente == lisPart3.fuente_parteoficial && oa.serie == serieP && oa.numero == numP && oa.nombre.Contains("-p-") && !extensionRestringidaIPO.Contains(oa.extension)).ToList();

                                listaPlanos.Columns.Add("ParteOficial");

                                foreach (var itmeP in listPlanos)
                                {
                                    if (itmeP.extension.Contains("c"))
                                    {
                                        listaPlanos.Rows.Add(new Uri(Path.Combine(ruta3, itmeP.nombre)).AbsoluteUri, numP);
                                    }
                                    else
                                    {
                                        string strfn = Path.Combine(@"" + rutaPlano3 + "\\" + itmeP.nombre);

                                        System.Drawing.Bitmap bitmap1;
                                        bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(strfn);
                                        bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                        bitmap1.Save(strfn);

                                        var PlanoConvertido = dbPivot.OtrosAdjuntos.Find(lisPart3.fuente_parteoficial, serieP, numP, itmeP.nombre);
                                        PlanoConvertido.extension = itmeP.extension + "c";

                                        dbPivot.SaveChanges();

                                        listaPlanos.Rows.Add(new Uri(Path.Combine(ruta3, itmeP.nombre)).AbsoluteUri, numP);
                                    }                                    
                                }

                                if (listPlanos.Count() == 0)
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();

                                    var adj = db.IMAGENES.Where(a => a.Fuente == lisPart3.fuente_parteoficial && a.Serie == serieP && a.Numero == lisPart3.numeroparte && a.Tipo == "C").ToList();
                                    foreach (var adjFinal in adj)
                                    {
                                        //Especificamos la consulta que nos devuelve la imagen
                                        SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                "and Tipo=@tipo",
                                                                connection);
                                        //Especificamos el parámetro ID de la consulta
                                        cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = lisPart3.fuente_parteoficial;
                                        cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = serieP;
                                        cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = lisPart3.numeroparte;
                                        cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "C";

                                        //Ejecutamos un Scalar para recuperar sólo la imagen
                                        byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                        var existeA = string.Format("{0}-{1}-{2}-p-{3}.png", lisPart3.fuente_parteoficial, serieP, lisPart3.numeroparte, adjFinal.Identificacion.Trim());
                                        string existeAdj = @"" + rutaPlano3 + "\\" + existeA;

                                        if (System.IO.File.Exists(existeAdj))
                                        {
                                            listaPlanos.Rows.Add(new Uri(Path.Combine(ruta3, existeA)).AbsoluteUri, numP);
                                        }
                                        else
                                        {
                                            if (barrImg != null)
                                            {
                                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                string strfn = Server.MapPath(rutaV3 + lisPart3.fuente_parteoficial.ToString() + "-" + serieP.ToString() + "-" + lisPart3.numeroparte.ToString() + "-p-" + adjFinal.Identificacion.Trim() + ".png");

                                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                fs.Write(barrImg, 0, barrImg.Length);
                                                fs.Flush();
                                                fs.Close();

                                                string strfn2 = Path.Combine(@"" + rutaPlano3 + "\\" + existeA);

                                                System.Drawing.Bitmap bitmap1;
                                                bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(strfn2);
                                                bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                                bitmap1.Save(strfn2);
                                            }
                                            listaPlanos.Rows.Add(new Uri(Path.Combine(ruta3, existeA)).AbsoluteUri, numP);
                                        }

                                    }
                                }
                                #endregion

                                #region Adjuntos

                                var nombreAdjuntos3 = db.OtrosAdjuntos.Where(oa => fuente3.Contains(oa.fuente) && serParte3.Contains(oa.serie) && numPartConv3.Contains(oa.numero) && !oa.nombre.Contains("-p-") && !oa.nombre.Contains("-u-") && !oa.nombre.Contains("-i-") && !oa.nombre.Contains("-t-") && !extensionRestringidaIPO.Contains(oa.extension)).Select(oa => oa.nombre).ToList();
                                var numPartLista3 = db.OtrosAdjuntos.Where(oa => nombreAdjuntos3.Contains(oa.nombre)).Select(oa => oa.numero).ToList();
                                var extPartLista3 = db.OtrosAdjuntos.Where(oa => nombreAdjuntos3.Contains(oa.nombre)).Select(oa => oa.extension).ToList();

                                var listaAdjuntos3 = nombreAdjuntos3.Zip(numPartLista3, (n, w) => new { NombreAr = n, NumPar = w }).Zip(extPartLista3, (x, z) => Tuple.Create(x.NombreAr, x.NumPar, z));

                                foreach (var item in listaAdjuntos3)
                                {                                    

                                    if (item.Item3.Contains("c"))
                                    {
                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta3, item.Item1)).AbsoluteUri, item.Item2);
                                    }
                                    else
                                    {
                                        foreach (var item3 in resultAdjParte)
                                        {
                                            string existeAdj = @"" + rutaPlano3 + "\\" + item.Item1;

                                            Stream stream = File.OpenRead(existeAdj);
                                            System.Drawing.Image sourceImage = System.Drawing.Image.FromStream(stream, false, false);

                                            int width = sourceImage.Width;
                                            int height = sourceImage.Height;
                                            stream.Close();

                                            if (width > height)
                                            {

                                                System.Drawing.Bitmap bitmap1;
                                                bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(existeAdj);
                                                bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                                bitmap1.Save(existeAdj);

                                                var PlanoConvertido = dbPivot.OtrosAdjuntos.Find(item3.Item1, item3.Item2, item3, item.Item1);
                                                PlanoConvertido.extension = item.Item3 + "c";

                                                dbPivot.SaveChanges();

                                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta3, item.Item1)).AbsoluteUri, item.Item2);
                                            }
                                            else
                                            {
                                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta3, item.Item1)).AbsoluteUri, item.Item2);
                                            }
                                        }
                                    }
                                }

                                if (listaAdjuntos3.Count() == 0)
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();
                                    foreach (var item2 in resultAdjParte)
                                    {
                                        var adj = db.IMAGENES.Where(a => a.Fuente == item2.Item1 && a.Serie == item2.Item2 && a.Numero == item2.Item3).ToList();
                                        foreach (var adjFinal in adj)
                                        {
                                            //Especificamos la consulta que nos devuelve la imagen
                                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                    "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                    "and Tipo=@tipo and Identificacion=@ident",
                                                                    connection);
                                            //Especificamos el parámetro ID de la consulta
                                            cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item2.Item1;
                                            cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item2.Item2;
                                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item2.Item3;
                                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "a";
                                            cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = adjFinal.Identificacion;

                                            //Ejecutamos un Scalar para recuperar sólo la imagen
                                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                            var existeA = string.Format("{0}-{1}-{2}-a-{3}.png", item2.Item1, item2.Item2, item2.Item3, adjFinal.Identificacion.Trim());
                                            string existeAdj = @"" + rutaPlano3 + "\\" + existeA;

                                            if (System.IO.File.Exists(existeAdj))
                                            {
                                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta3, existeA)).AbsoluteUri, item2.Item3);
                                            }
                                            else
                                            {
                                                if (barrImg != null)
                                                {
                                                    //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                    string strfn = Server.MapPath(rutaV3 + item2.Item1.ToString() + "-" + item2.Item2.ToString() + "-" + item2.Item3.ToString() + "-a-" + adjFinal.Identificacion.Trim() + ".png");

                                                    FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                    fs.Write(barrImg, 0, barrImg.Length);
                                                    fs.Flush();
                                                    fs.Close();

                                                    System.Drawing.Bitmap bitmap1;
                                                    bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(existeAdj);
                                                    bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                                    bitmap1.Save(existeAdj);
                                                }

                                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta3, existeA)).AbsoluteUri, item2.Item3);
                                            }

                                        }
                                    }
                                }
                                #endregion                                

                                #region Firmas Testigos P

                                foreach (var item3 in resultAdjParte)
                                {
                                    string ser3 = Convert.ToString(item3.Item2);
                                    //var TipoidTP = (db.TESTIGOXPARTE.Where(a => a.serie == ser3 && a.numeroparte == item3.Item3).Select(a => a.tipo_ide).ToList());
                                    //var IdTP = (db.TESTIGOXPARTE.Where(a => a.serie == ser3 && a.numeroparte == item3.Item3).Select(a => a.identificacion).ToList());
                                    var firmaTP = (db.TESTIGOXPARTE.Where(a => ser3.Contains(a.serie) && item3.Item3.Contains(a.numeroparte)).ToList());

                                    foreach (var item in firmaTP)
                                    {
                                        var FirmaTestigoP = string.Format("{0}-{1}-{2}-t-{3}.png", item3.Item1, item3.Item2, item3.Item3, item.identificacion);

                                        string existeT = @"" + rutaPlano3 + "\\" + FirmaTestigoP;

                                        if (System.IO.File.Exists(existeT))
                                        {
                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaTestigoP)).AbsoluteUri, item3.Item3, item.identificacion);
                                        }
                                        else
                                        {
                                            SqlConnection connection = new SqlConnection(connectionString);
                                            connection.Open();

                                            //Especificamos la consulta que nos devuelve la imagen
                                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                    "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                    "and Tipo=@tipo and Identificacion=@ident",
                                                                    connection);
                                            //Especificamos el parámetro ID de la consulta
                                            cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item3.Item1;
                                            cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item3.Item2;
                                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item3.Item3;
                                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "t";
                                            cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = item.identificacion;

                                            //Ejecutamos un Scalar para recuperar sólo la imagen
                                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                            if (barrImg != null)
                                            {
                                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                string strfn = Server.MapPath(rutaV3 + item3.Item1.ToString() + "-" + item3.Item2.ToString() + "-" + item3.Item3.ToString() + "-t-" + item.identificacion + ".png");

                                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                fs.Write(barrImg, 0, barrImg.Length);
                                                fs.Flush();
                                                fs.Close();
                                            }

                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaTestigoP)).AbsoluteUri, item3.Item3, item.identificacion);
                                        }
                                    }
                                }
                                #endregion

                                #region Firmas Testigos B

                                foreach (var item3 in resultAdjParte)
                                {
                                    var firmaTB = db.TESTIGOXBOLETA.Where(a => seriBolet3.Contains(a.serie) && numeroBoleta3.Contains(a.numero)).ToList();

                                    foreach (var item in firmaTB)
                                    {
                                        var FirmaTestigoB = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

                                        string existeT = @"" + rutaPlano3 + "\\" + FirmaTestigoB;

                                        if (System.IO.File.Exists(existeT))
                                        {
                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                                        }
                                        else
                                        {
                                            SqlConnection connection = new SqlConnection(connectionString);
                                            connection.Open();

                                            //Especificamos la consulta que nos devuelve la imagen
                                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                    "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                    "and Tipo=@tipo and Identificacion=@ident",
                                                                    connection);
                                            //Especificamos el parámetro ID de la consulta
                                            cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item.fuente;
                                            cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item.serie;
                                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.numero;
                                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "t";
                                            cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = item.identificacion;

                                            //Ejecutamos un Scalar para recuperar sólo la imagen
                                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                            if (barrImg != null)
                                            {
                                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                string strfn = Server.MapPath(rutaV3 + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero.ToString() + "-t-" + item.identificacion + ".png");

                                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                fs.Write(barrImg, 0, barrImg.Length);
                                                fs.Flush();
                                                fs.Close();
                                            }

                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                                        }
                                    }
                                }
                                #endregion

                                #region Firma Inspector

                                var listFirma = (db.BOLETA.Where(a => seriePart3.Contains(a.serie_parteoficial) && numPartPar3.Contains(a.numeroparte)).OrderBy(a => new { a.fuente_parteoficial, a.serie_parteoficial, a.numeroparte }).ToList());
                                string v_fuente = null;
                                string v_serie = null;
                                string v_numparte = null;

                                foreach (var item in listFirma)
                                {
                                    if (v_fuente != item.fuente_parteoficial || v_serie != item.serie_parteoficial || v_numparte != item.numeroparte)
                                    {
                                        var FirmaInspector = string.Format("{0}-{1}-{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);

                                        string existeT = @"" + rutaPlano3 + "\\" + FirmaInspector;

                                        if (System.IO.File.Exists(existeT))
                                        {
                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                            v_fuente = item.fuente_parteoficial;
                                            v_serie = item.serie_parteoficial;
                                            v_numparte = item.numeroparte;
                                        }
                                        else
                                        {
                                            SqlConnection connection = new SqlConnection(connectionString);
                                            connection.Open();

                                            //Especificamos la consulta que nos devuelve la imagen
                                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                    "where Numero=@numero " +
                                                                    "and Tipo=@tipo",
                                                                    connection);
                                            //Especificamos el parámetro ID de la consulta
                                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.codigo_inspector;
                                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "i";


                                            //Ejecutamos un Scalar para recuperar sólo la imagen
                                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                            if (barrImg != null)
                                            {
                                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                string strfn = Server.MapPath(rutaV3 + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero_boleta.ToString() + "-i-" + item.codigo_inspector + ".png");

                                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                fs.Write(barrImg, 0, barrImg.Length);
                                                fs.Flush();
                                                fs.Close();
                                            }

                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                            v_fuente = item.fuente_parteoficial;
                                            v_serie = item.serie_parteoficial;
                                            v_numparte = item.numeroparte;
                                        }
                                    }

                                }
                                #endregion

                                #region Firma Usuario

                                foreach (var item in listFirma)
                                {
                                    var FirmaUsuario = string.Format("{0}-{1}-{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);

                                    string existeT = @"" + rutaPlano3 + "\\" + FirmaUsuario;

                                    if (System.IO.File.Exists(existeT))
                                    {
                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                                    }
                                    else
                                    {
                                        SqlConnection connection = new SqlConnection(connectionString);
                                        connection.Open();

                                        //Especificamos la consulta que nos devuelve la imagen
                                        SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                "and Tipo=@tipo",
                                                                connection);
                                        //Especificamos el parámetro ID de la consulta
                                        cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item.fuente;
                                        cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item.serie;
                                        cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.numero_boleta;
                                        cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "F";

                                        //Ejecutamos un Scalar para recuperar sólo la imagen
                                        byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                        if (barrImg != null)
                                        {
                                            //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                            string strfn = Server.MapPath(rutaV3 + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero_boleta.ToString() + "-u-" + item.identificacion + ".png");

                                            FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                            fs.Write(barrImg, 0, barrImg.Length);
                                            fs.Flush();
                                            fs.Close();
                                        }

                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta3, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                                    }
                                }
                                #endregion
                            }

                            this.ReportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;
                            Session["_ConsultaeImpresionDeParteOficialData"] = listaArchivos;
                            Session["_ConsultaeImpresionDeParteOficialDataFirma"] = listaFirmas;
                            #endregion
                        }

                        if (TipoConsulta2 == 4)
                        {
                            #region Consulta 4
                            var numeroBoleta4 = (db.BOLETA.Where(a => a.numero_placa == Parametro4 && a.codigo_placa == Parametro5 && a.clase_placa == Parametro6 && a.fuente_parteoficial == "4").Select(a => a.numero_boleta).ToList());
                            var seriBolet4 = (db.BOLETA.Where(a => a.numero_placa == Parametro4 && a.codigo_placa == Parametro5 && a.clase_placa == Parametro6 && a.fuente_parteoficial == "4").Select(a => a.serie).ToList());

                            var listaPartes4 = (db.BOLETA.Where(a => a.numero_placa == Parametro4 && a.codigo_placa == Parametro5 && a.clase_placa == Parametro6 && a.fuente_parteoficial == "4").ToList());

                            foreach (var lisPart4 in listaPartes4)
                            {
                                var numPartPar4 = (db.PARTEOFICIAL.Where(a => a.NumeroParte == lisPart4.numeroparte && a.Serie == lisPart4.serie_parteoficial).Select(a => a.NumeroParte).ToList());
                                var numPartConv4 = numPartPar4.Select(s => Convert.ToDecimal(s)).ToList();

                                var seriePart4 = (db.PARTEOFICIAL.Where(a => a.NumeroParte == lisPart4.numeroparte && a.Serie == lisPart4.serie_parteoficial).Select(a => a.Serie).ToList());
                                var serParte4 = seriePart4.Select(s => Convert.ToInt32(s)).ToList();

                                var fuente4 = (db.PARTEOFICIAL.Where(a => seriePart4.Contains(a.Serie) && numPartPar4.Contains(a.NumeroParte)).Select(a => a.Fuente).ToList());

                                string ruta4 = ConfigurationManager.AppSettings["DownloadFilePath"];
                                string rutaPlano4 = ConfigurationManager.AppSettings["UploadFilePath"];
                                string rutaV4 = ConfigurationManager.AppSettings["RutaVirtual"];

                                var resultAdjParte = fuente4.Zip(serParte4, (e1, e2) => new { e1, e2 }).Zip(numPartPar4, (z1, e3) => Tuple.Create(z1.e1, z1.e2, e3));

                                #region Planos

                                int serieP = Convert.ToInt32(lisPart4.serie_parteoficial);
                                decimal numP = Convert.ToDecimal(lisPart4.numeroparte);

                                var listPlanos = db.OtrosAdjuntos.Where(oa => oa.fuente == lisPart4.fuente_parteoficial && oa.serie == serieP && oa.numero == numP && oa.nombre.Contains("-p-") && !extensionRestringidaIPO.Contains(oa.extension)).ToList();

                                listaPlanos.Columns.Add("ParteOficial");

                                foreach (var itmeP in listPlanos)
                                {
                                    if (itmeP.extension.Contains("c"))
                                    {
                                        listaPlanos.Rows.Add(new Uri(Path.Combine(ruta4, itmeP.nombre)).AbsoluteUri, numP);
                                    }
                                    else
                                    {
                                        string strfn = Path.Combine(@"" + rutaPlano4 + "\\" + itmeP.nombre);

                                        System.Drawing.Bitmap bitmap1;
                                        bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(strfn);
                                        bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                        bitmap1.Save(strfn);

                                        var PlanoConvertido = dbPivot.OtrosAdjuntos.Find(lisPart4.fuente_parteoficial, serieP, numP, itmeP.nombre);
                                        PlanoConvertido.extension = itmeP.extension + "c";

                                        dbPivot.SaveChanges();

                                        listaPlanos.Rows.Add(new Uri(Path.Combine(ruta4, itmeP.nombre)).AbsoluteUri, numP);
                                    }                                    
                                }

                                if (listPlanos.Count() == 0)
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();

                                    var adj = db.IMAGENES.Where(a => a.Fuente == lisPart4.fuente_parteoficial && a.Serie == serieP && a.Numero == lisPart4.numeroparte && a.Tipo == "C").ToList();
                                    foreach (var adjFinal in adj)
                                    {
                                        //Especificamos la consulta que nos devuelve la imagen
                                        SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                "and Tipo=@tipo",
                                                                connection);
                                        //Especificamos el parámetro ID de la consulta
                                        cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = lisPart4.fuente_parteoficial;
                                        cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = serieP;
                                        cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = lisPart4.numeroparte;
                                        cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "C";

                                        //Ejecutamos un Scalar para recuperar sólo la imagen
                                        byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                        var existeA = string.Format("{0}-{1}-{2}-p-{3}.png", lisPart4.fuente_parteoficial, serieP, lisPart4.numeroparte, adjFinal.Identificacion.Trim());
                                        string existeAdj = @"" + rutaPlano4 + "\\" + existeA;

                                        if (System.IO.File.Exists(existeAdj))
                                        {
                                            listaPlanos.Rows.Add(new Uri(Path.Combine(ruta4, existeA)).AbsoluteUri, numP);
                                        }
                                        else
                                        {
                                            if (barrImg != null)
                                            {
                                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                string strfn = Server.MapPath(rutaV4 + lisPart4.fuente_parteoficial.ToString() + "-" + serieP.ToString() + "-" + lisPart4.numeroparte.ToString() + "-p-" + adjFinal.Identificacion.Trim() + ".png");

                                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                fs.Write(barrImg, 0, barrImg.Length);
                                                fs.Flush();
                                                fs.Close();

                                                string strfn2 = Path.Combine(@"" + rutaPlano4 + "\\" + existeA);

                                                System.Drawing.Bitmap bitmap1;
                                                bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(strfn2);
                                                bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                                bitmap1.Save(strfn2);
                                            }
                                            listaPlanos.Rows.Add(new Uri(Path.Combine(ruta4, existeA)).AbsoluteUri, numP);
                                        }

                                    }
                                }
                                #endregion

                                #region Convertir SVG a PNG
                                var extSvg4 = db.OtrosAdjuntos.Where(oa => fuente4.Contains(oa.fuente) && serParte4.Contains(oa.serie) && numPartConv4.Contains(oa.numero) && oa.extension == "SVG").Select(oa => oa.nombre).ToList();
                                var extSvgFuente4 = db.OtrosAdjuntos.Where(oa => extSvg4.Contains(oa.nombre)).Select(oa => oa.fuente).ToList();
                                var extSvgSerie4 = db.OtrosAdjuntos.Where(oa => extSvg4.Contains(oa.nombre)).Select(oa => oa.serie).ToList();
                                var extSvgParte4 = db.OtrosAdjuntos.Where(oa => extSvg4.Contains(oa.nombre)).Select(oa => oa.numero).ToList();

                                var listPlanos4 = extSvg4.Zip(extSvgParte4, (n, w) => new { NombreAr = n, NumPar = w }).Zip(extSvgSerie4, (x, z) => Tuple.Create(x.NombreAr, x.NumPar, z)).Zip(extSvgFuente4, (y, r) => Tuple.Create(y.Item1, y.Item2, y.Item3, r));

                                foreach (var item in listPlanos4)
                                {
                                    var existeSVG = item.Item1;
                                    string existeAdjS = @"" + rutaPlano4 + "\\" + existeSVG;
                                    string nombrePng = item.Item1.Replace(".svg", ".png");

                                    string strfn = Path.Combine(@"" + rutaPlano4 + "\\" + nombrePng);

                                    if (System.IO.File.Exists(existeAdjS))
                                    {

                                        var sampleDoc = SvgDocument.Open(existeAdjS);
                                        sampleDoc.Draw().Save(strfn);

                                        int? maxValue = db.OtrosAdjuntos.Where(oa => oa.serie == item.Item3 && oa.numero == item.Item2 && String.Compare(oa.extension, "png", false) == 0).Max(a => a.consecutivo_extension) ?? 0;

                                        var svgConvertido = dbPivot.OtrosAdjuntos.Find(item.Item4, item.Item3, item.Item2, item.Item1);
                                        svgConvertido.extension = "svgc";

                                        dbPivot.OtrosAdjuntos.Add(new OtrosAdjuntos
                                        {
                                            fuente = item.Item4,
                                            serie = item.Item3,
                                            numero = item.Item2,
                                            extension = "png",
                                            fechaRegistro = DateTime.Now,
                                            nombre = nombrePng,
                                            consecutivo_extension = maxValue.Value + 1
                                        });

                                        dbPivot.SaveChanges();
                                    }
                                }
                                #endregion

                                #region Adjuntos

                                var nombreAdjuntos4 = db.OtrosAdjuntos.Where(oa => fuente4.Contains(oa.fuente) && serParte4.Contains(oa.serie) && numPartConv4.Contains(oa.numero) && !oa.nombre.Contains("-p-") && !oa.nombre.Contains("-u-") && !oa.nombre.Contains("-i-") && !oa.nombre.Contains("-t-") && !extensionRestringidaIPO.Contains(oa.extension)).Select(oa => oa.nombre).ToList();
                                var numPartLista4 = db.OtrosAdjuntos.Where(oa => nombreAdjuntos4.Contains(oa.nombre)).Select(oa => oa.numero).ToList();
                                var extPartLista4 = db.OtrosAdjuntos.Where(oa => nombreAdjuntos4.Contains(oa.nombre)).Select(oa => oa.extension).ToList();

                                listaArchivos.Columns.Add("ParteOficial");
                                
                                var listaAdjuntos4 = nombreAdjuntos4.Zip(numPartLista4, (n, w) => new { NombreAr = n, NumPar = w }).Zip(extPartLista4, (x, z) => Tuple.Create(x.NombreAr, x.NumPar, z));

                                foreach (var item in listaAdjuntos4)
                                {                                    
                                    if (item.Item3.Contains("c"))
                                    {
                                        listaArchivos.Rows.Add(new Uri(Path.Combine(ruta4, item.Item1)).AbsoluteUri, item.Item2);                                        
                                    }
                                    else
                                    {
                                        foreach (var item3 in resultAdjParte)
                                        {
                                            string existeAdj = @"" + rutaPlano4 + "\\" + item.Item1;

                                            Stream stream = File.OpenRead(existeAdj);
                                            System.Drawing.Image sourceImage = System.Drawing.Image.FromStream(stream, false, false);

                                            int width = sourceImage.Width;
                                            int height = sourceImage.Height;
                                            stream.Close();

                                            if (width > height)
                                            {

                                                System.Drawing.Bitmap bitmap1;
                                                bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(existeAdj);
                                                bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                                bitmap1.Save(existeAdj);

                                                var PlanoConvertido = dbPivot.OtrosAdjuntos.Find(item3.Item1, item3.Item2, item3, item.Item1);
                                                PlanoConvertido.extension = item.Item3 + "c";

                                                dbPivot.SaveChanges();

                                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta4, item.Item1)).AbsoluteUri, item.Item2);
                                            }
                                            else
                                            {
                                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta4, item.Item1)).AbsoluteUri, item.Item2);
                                            }
                                        }
                                    }
                                }

                                if (listaAdjuntos4.Count() == 0)
                                {
                                    SqlConnection connection = new SqlConnection(connectionString);
                                    connection.Open();
                                    foreach (var item2 in resultAdjParte)
                                    {
                                        var adj = db.IMAGENES.Where(a => a.Fuente == item2.Item1 && a.Serie == item2.Item2 && a.Numero == item2.Item3).ToList();
                                        foreach (var adjFinal in adj)
                                        {
                                            //Especificamos la consulta que nos devuelve la imagen
                                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                    "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                    "and Tipo=@tipo and Identificacion=@ident",
                                                                    connection);
                                            //Especificamos el parámetro ID de la consulta
                                            cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item2.Item1;
                                            cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item2.Item2;
                                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item2.Item3;
                                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "a";
                                            cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = adjFinal.Identificacion;

                                            //Ejecutamos un Scalar para recuperar sólo la imagen
                                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                            var existeA = string.Format("{0}-{1}-{2}-a-{3}.png", item2.Item1, item2.Item2, item2.Item3, adjFinal.Identificacion.Trim());
                                            string existeAdj = @"" + rutaPlano4 + "\\" + existeA;

                                            if (System.IO.File.Exists(existeAdj))
                                            {
                                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta4, existeA)).AbsoluteUri, item2.Item3);
                                            }
                                            else
                                            {
                                                if (barrImg != null)
                                                {
                                                    //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                    string strfn = Server.MapPath(rutaV4 + item2.Item1.ToString() + "-" + item2.Item2.ToString() + "-" + item2.Item3.ToString() + "-a-" + adjFinal.Identificacion.Trim() + ".png");

                                                    FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                    fs.Write(barrImg, 0, barrImg.Length);
                                                    fs.Flush();
                                                    fs.Close();

                                                    System.Drawing.Bitmap bitmap1;
                                                    bitmap1 = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(existeAdj);
                                                    bitmap1.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                                                    bitmap1.Save(existeAdj);
                                                }

                                                listaArchivos.Rows.Add(new Uri(Path.Combine(ruta4, existeA)).AbsoluteUri, item2.Item3);
                                            }

                                        }
                                    }
                                }
                                #endregion

                                listaFirmas.Columns.Add("ParteOficial");
                                listaFirmas.Columns.Add("Identificacion");

                                #region Firmas Testigos P

                                foreach (var item3 in resultAdjParte)
                                {
                                    string ser3 = Convert.ToString(item3.Item2);
                                    //var TipoidTP = (db.TESTIGOXPARTE.Where(a => a.serie == ser3 && a.numeroparte == item3.Item3).Select(a => a.tipo_ide).ToList());
                                    //var IdTP = (db.TESTIGOXPARTE.Where(a => a.serie == ser3 && a.numeroparte == item3.Item3).Select(a => a.identificacion).ToList());
                                    var firmaTP = (db.TESTIGOXPARTE.Where(a => ser3.Contains(a.serie) && item3.Item3.Contains(a.numeroparte)).ToList());

                                    foreach (var item in firmaTP)
                                    {
                                        var FirmaTestigoP = string.Format("{0}-{1}-{2}-t-{3}.png", item3.Item1, item3.Item2, item3.Item3, item.identificacion);

                                        string existeT = @"" + rutaPlano4 + "\\" + FirmaTestigoP;

                                        if (System.IO.File.Exists(existeT))
                                        {
                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaTestigoP)).AbsoluteUri, item3.Item3, item.identificacion);
                                        }
                                        else
                                        {
                                            SqlConnection connection = new SqlConnection(connectionString);
                                            connection.Open();

                                            //Especificamos la consulta que nos devuelve la imagen
                                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                    "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                    "and Tipo=@tipo and Identificacion=@ident",
                                                                    connection);
                                            //Especificamos el parámetro ID de la consulta
                                            cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item3.Item1;
                                            cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item3.Item2;
                                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item3.Item3;
                                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "t";
                                            cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = item.identificacion;

                                            //Ejecutamos un Scalar para recuperar sólo la imagen
                                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                            if (barrImg != null)
                                            {
                                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                string strfn = Server.MapPath(rutaV4 + item3.Item1.ToString() + "-" + item3.Item2.ToString() + "-" + item3.Item3.ToString() + "-t-" + item.identificacion + ".png");

                                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                fs.Write(barrImg, 0, barrImg.Length);
                                                fs.Flush();
                                                fs.Close();
                                            }

                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaTestigoP)).AbsoluteUri, item3.Item3, item.identificacion);
                                        }
                                    }
                                }
                                #endregion

                                #region Firmas Testigos B

                                foreach (var item3 in resultAdjParte)
                                {
                                    var firmaTB = db.TESTIGOXBOLETA.Where(a => seriBolet4.Contains(a.serie) && numeroBoleta4.Contains(a.numero)).ToList();

                                    foreach (var item in firmaTB)
                                    {
                                        var FirmaTestigoB = string.Format("{0}-{1}-{2}-t-{3}.png", item.fuente, item.serie, item.numero, item.identificacion);

                                        string existeT = @"" + rutaPlano4 + "\\" + FirmaTestigoB;

                                        if (System.IO.File.Exists(existeT))
                                        {
                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                                        }
                                        else
                                        {
                                            SqlConnection connection = new SqlConnection(connectionString);
                                            connection.Open();

                                            //Especificamos la consulta que nos devuelve la imagen
                                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                    "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                    "and Tipo=@tipo and Identificacion=@ident",
                                                                    connection);
                                            //Especificamos el parámetro ID de la consulta
                                            cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item.fuente;
                                            cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item.serie;
                                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.numero;
                                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "t";
                                            cmdSelect.Parameters.Add("@ident", SqlDbType.Char, 15).Value = item.identificacion;

                                            //Ejecutamos un Scalar para recuperar sólo la imagen
                                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                            if (barrImg != null)
                                            {
                                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                string strfn = Server.MapPath(rutaV4 + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero.ToString() + "-t-" + item.identificacion + ".png");

                                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                fs.Write(barrImg, 0, barrImg.Length);
                                                fs.Flush();
                                                fs.Close();
                                            }

                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaTestigoB)).AbsoluteUri, item.numero, item.identificacion);
                                        }
                                    }
                                }
                                #endregion

                                #region Firma Inspector

                                var listFirma = (db.BOLETA.Where(a => seriePart4.Contains(a.serie_parteoficial) && numPartPar4.Contains(a.numeroparte)).OrderBy(a => new { a.fuente_parteoficial, a.serie_parteoficial, a.numeroparte }).ToList());
                                string v_fuente = null;
                                string v_serie = null;
                                string v_numparte = null;

                                foreach (var item in listFirma)
                                {
                                    if (v_fuente != item.fuente_parteoficial || v_serie != item.serie_parteoficial || v_numparte != item.numeroparte)
                                    {
                                        var FirmaInspector = string.Format("{0}-{1}-{2}-i-{3}.png", item.fuente, item.serie, item.numero_boleta, item.codigo_inspector);

                                        string existeT = @"" + rutaPlano4 + "\\" + FirmaInspector;

                                        if (System.IO.File.Exists(existeT))
                                        {
                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                            v_fuente = item.fuente_parteoficial;
                                            v_serie = item.serie_parteoficial;
                                            v_numparte = item.numeroparte;
                                        }
                                        else
                                        {
                                            SqlConnection connection = new SqlConnection(connectionString);
                                            connection.Open();

                                            //Especificamos la consulta que nos devuelve la imagen
                                            SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                    "where Numero=@numero " +
                                                                    "and Tipo=@tipo",
                                                                    connection);
                                            //Especificamos el parámetro ID de la consulta
                                            cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.codigo_inspector;
                                            cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "i";


                                            //Ejecutamos un Scalar para recuperar sólo la imagen
                                            byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                            if (barrImg != null)
                                            {
                                                //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                                string strfn = Server.MapPath(rutaV4 + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero_boleta.ToString() + "-i-" + item.codigo_inspector + ".png");

                                                FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                                fs.Write(barrImg, 0, barrImg.Length);
                                                fs.Flush();
                                                fs.Close();
                                            }

                                            listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaInspector)).AbsoluteUri, item.numeroparte, item.codigo_inspector);
                                            v_fuente = item.fuente_parteoficial;
                                            v_serie = item.serie_parteoficial;
                                            v_numparte = item.numeroparte;
                                        }
                                    }
                                }
                                #endregion

                                #region Firma Usuario

                                foreach (var item in listFirma)
                                {
                                    var FirmaUsuario = string.Format("{0}-{1}-{2}-u-{3}.png", item.fuente, item.serie, item.numero_boleta, item.identificacion);

                                    string existeT = @"" + rutaPlano4 + "\\" + FirmaUsuario;

                                    if (System.IO.File.Exists(existeT))
                                    {
                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                                    }
                                    else
                                    {
                                        SqlConnection connection = new SqlConnection(connectionString);
                                        connection.Open();

                                        //Especificamos la consulta que nos devuelve la imagen
                                        SqlCommand cmdSelect = new SqlCommand("select Imagen from IMAGENES " +
                                                                "where Fuente=@fuente and Serie=@serie and Numero=@numero " +
                                                                "and Tipo=@tipo",
                                                                connection);
                                        //Especificamos el parámetro ID de la consulta
                                        cmdSelect.Parameters.Add("@fuente", SqlDbType.Char, 1).Value = item.fuente;
                                        cmdSelect.Parameters.Add("@serie", SqlDbType.Int).Value = item.serie;
                                        cmdSelect.Parameters.Add("@numero", SqlDbType.Char, 10).Value = item.numero_boleta;
                                        cmdSelect.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = "F";

                                        //Ejecutamos un Scalar para recuperar sólo la imagen
                                        byte[] barrImg = (byte[])cmdSelect.ExecuteScalar();

                                        if (barrImg != null)
                                        {
                                            //Grabamos la imagen al disco (en un directorio accesible desde IIS) para poder servirla                            
                                            string strfn = Server.MapPath(rutaV4 + item.fuente.ToString() + "-" + item.serie.ToString() + "-" + item.numero_boleta.ToString() + "-u-" + item.identificacion + ".png");

                                            FileStream fs = new FileStream(strfn, FileMode.CreateNew, FileAccess.Write);
                                            fs.Write(barrImg, 0, barrImg.Length);
                                            fs.Flush();
                                            fs.Close();
                                        }

                                        listaFirmas.Rows.Add(new Uri(Path.Combine(ruta4, FirmaUsuario)).AbsoluteUri, item.numero_boleta, item.identificacion);
                                    }
                                }
                                #endregion
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

        /// <summary>
        /// Método para enviar los parametros a los subReportes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {            
            e.DataSources.Add(new ReportDataSource("ArchivosBoletaDataSet", Session["_ConsultaeImpresionDeParteOficialDataBoleta"]));
            e.DataSources.Add(new ReportDataSource("ArchivosDataSet", Session["_ConsultaeImpresionDeParteOficialData"])); 
            e.DataSources.Add(new ReportDataSource("FirmasDataSet", Session["_ConsultaeImpresionDeParteOficialDataFirma"]));
            e.DataSources.Add(new ReportDataSource("PlanosDataSet", Session["_ConsultaeImpresionDeParteOficialDataPnano"]));
            
        }

        /// <summary>
        /// Método de obtener el ID del Reporte que desea consultar
        /// </summary>
        /// <param name="reporteID"> Id del reporte a consultar</param>
        /// <param name="parametros">Parametros por los cuales quiere consultar</param>
        /// <returns></returns>
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
                case "_ImpresionDeParteOficial":
                    result = ConsultaeImpresionDeParteOficialData(parametros);
                    break;
                default:
                    break;
            }

            return result;
        }

        #region Datos
        /// <summary>
        /// Métodos donde se envian al Procedimiento almacenado los parametros o datos por lo cual el usuario consulta cada uno de los repotes.
        /// </summary>
        /// <param name="parametros"></param>
        /// <returns></returns>
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

        private List<ConsultaeImpresionDeParteOficialData_Result> ConsultaeImpresionDeParteOficialData(string parametros)
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
                var lista1 = db.ConsultaeImpresionDeParteOficialData(TipoConsulta, Parametro1, Parametro2, "-0", usuarioSistema).ToList();
                return lista1;
            }
            db.Database.CommandTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["CommandTimeout"]);
            var lista = db.ConsultaeImpresionDeParteOficialData(TipoConsulta, Parametro1, Parametro2, Parametro3, usuarioSistema).ToList();
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

        /// <summary>
        /// Este método es para el evento del boton de imprimir el repote en el cual se le adjunta a la impresion los PDF asosicados al reporte.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                        lstPDF = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serieParte1 && oa.numero == numeroParte1 && oa.extension == "PDF").Select(oa => oa.nombre).ToList();
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

                        lstPDF = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero == numeroParte2 && oa.extension == "PDF").Select(oa => oa.nombre).ToList();                        
                    }
                    break;

                case "_ImpresionDeParteOficial":

                    string[] parame2 = parametros.Split(',');
                    int TipoConsulta2 = Convert.ToInt32(parame2[0]);
                    string Parametro4 = parame2[1];
                    string Parametro5 = parame2[2];
                    string Parametro6 = parame2[3];

                    if (TipoConsulta2 == 1)
                    {
                        int serieParte1 = Convert.ToInt32(Parametro4);
                        decimal numeroParte1 = Convert.ToDecimal(Parametro5);

                        var fuente1 = (db.PARTEOFICIAL.Where(a => a.Serie == Parametro4 && a.NumeroParte == Parametro5).Select(a => a.Fuente).ToList());
                        string CodigoFuente1 = fuente1.ToArray().FirstOrDefault() == null ? "0" : fuente1.ToArray().FirstOrDefault().ToString();

                        lstPDF = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente1 && oa.serie == serieParte1 && oa.numero == numeroParte1 && oa.extension == "PDF").Select(oa => oa.nombre).ToList();
                    }
                    if (TipoConsulta2 == 2)
                    {
                        int serieBoleta2 = Convert.ToInt32(Parametro4);
                        decimal numeroBoleta2 = Convert.ToDecimal(Parametro5);

                        var numeroPart2 = (db.BOLETA.Where(a => a.serie == serieBoleta2 && a.numero_boleta == numeroBoleta2).Select(a => a.numeroparte).ToList());
                        string CodigoNumParte2 = numeroPart2.ToArray().FirstOrDefault() == null ? "0" : numeroPart2.ToArray().FirstOrDefault().ToString();

                        var seriePart2 = (db.PARTEOFICIAL.Where(a => a.NumeroParte == CodigoNumParte2).Select(a => a.Serie).ToList());
                        string CodigoSerie2 = seriePart2.ToArray().FirstOrDefault() == null ? "0" : seriePart2.ToArray().FirstOrDefault().ToString();

                        var fuente2 = (db.PARTEOFICIAL.Where(a => a.Serie == CodigoSerie2 && a.NumeroParte == CodigoNumParte2).Select(a => a.Fuente).ToList());
                        string CodigoFuente2 = fuente2.ToArray().FirstOrDefault() == null ? "0" : fuente2.ToArray().FirstOrDefault().ToString();

                        int serieParte2 = Convert.ToInt32(CodigoSerie2);
                        decimal numeroParte2 = Convert.ToDecimal(CodigoNumParte2);

                        lstPDF = db.OtrosAdjuntos.Where(oa => oa.fuente == CodigoFuente2 && oa.serie == serieParte2 && oa.numero == numeroParte2 && oa.extension == "PDF").Select(oa => oa.nombre).ToList();
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