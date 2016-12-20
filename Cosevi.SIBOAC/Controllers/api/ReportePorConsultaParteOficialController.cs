using Cosevi.SIBOAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cosevi.SIBOAC.Controllers.api
{
    public class ReportePorConsultaParteOficialController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/ReportePorConsultaParteOficial
        public IQueryable<DTOReportePorConsultaParteOficial> GetReportePorConsultaParteOficial([FromUri] string  serieParte , 
            [FromUri] string numeroParte, [FromUri] int ? serieBoleta, [FromUri] decimal ? numeroBoleta, [FromUri] string tipoId, 
            [FromUri] string numeroID, [FromUri] string numeroPlaca, [FromUri] string codigoPlaca, [FromUri] string clasePlaca)
        {
            var reportes = (from pto in db.PARTEOFICIAL
                            join bo in db.BOLETA on new { serie_parteoficial = pto.Serie, numeroparte = pto.NumeroParte }
                            equals new { bo.serie_parteoficial, bo.numeroparte }
                            join pe in db.PERSONA on new { bo.tipo_ide, bo.identificacion, bo.numero_boleta, bo.serie }
                            equals new { pe.tipo_ide, pe.identificacion, numero_boleta = pe.NumeroBoleta, serie = pe.Serie }
                            join de in db.DELEGACION on new { codigo_delegacion = (string)bo.codigo_delegacion } equals new { codigo_delegacion = de.Id }
                            join ro in db.ROLPERSONA on new { codrol = (string)bo.codrol } equals new { codrol = ro.Id }
                            where
                            (serieParte == null ? 1 == 1: pto.Serie == serieParte && numeroParte==null? 1==1: pto.NumeroParte == numeroParte) &&
                            (serieBoleta == null ? 1 == 1 : bo.serie == serieBoleta && numeroBoleta == null ? 1 == 1 : bo.numero_boleta == numeroBoleta) &&
                            (tipoId == null ? 1 == 1 : pe.tipo_ide == tipoId && numeroID == null ? 1 == 1 : pe.identificacion == numeroID) &&
                            (numeroPlaca == null ? 1 == 1 : bo.numero_placa == numeroPlaca && codigoPlaca == null ? 1 == 1 : bo.codigo_placa == codigoPlaca && clasePlaca == null ? 1 == 1:bo.clase_placa == clasePlaca)
                            select new DTOReportePorConsultaParteOficial
                            {
                                SerieBoleta = bo.serie,
                                NumeroBoleta = bo.numero_boleta,
                                FechaAccidente = pto.Fecha,
                                Autoridad = (((from a in db.AUTORIDAD where bo.codigo_autoridad_registra == a.Id
                                select new {a.Descripcion}).Distinct()).FirstOrDefault().Descripcion),
                                Delegacion = de.Descripcion,
                                ClasePlaca = bo.clase_placa,
                                CodigoNumeroPlaca = bo.codigo_placa +" " + bo.numero_boleta,
                                Identificacion = pe.tipo_ide+" "+pe.identificacion,
                                Nombre = pe.nombre +" " + pe.apellido1 + " "+ pe.apellido2,
                                Rol = ro.Descripcion,
                                SerieParte = bo.serie_parteoficial,
                                NumeroParte = bo.numeroparte
                            }).OrderBy(x => (x.SerieBoleta + x.NumeroBoleta));

            return reportes;
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class DTOReportePorConsultaParteOficial
    {
        public int SerieBoleta { get; set; }
        public decimal NumeroBoleta  { get; set; }
        public DateTime FechaAccidente { get; set; }
        public string Autoridad { get; set; }
        public string Delegacion { get; set; }
        public string ClasePlaca { get; set; }
        public string CodigoNumeroPlaca { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string SerieParte { get; set; }
        public string NumeroParte { get; set; }
    }
}
