using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Controllers.api
{
    public class ReportePorDescargaDeInspectorController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/ReportePorDescargaDeInspector
        public IQueryable<DTOReportePorDescargaDeInspector> GetReportePorDescargaDelInspector([FromUri] string numeroHH, [FromUri] string codigoInspector, [FromUri] DateTime? desde, [FromUri] DateTime? hasta)
        {
            var reportes = (from bo in db.BOLETA
                            join de in db.DELEGACION on new { codigo_delegacion = bo.codigo_delegacion } equals new { codigo_delegacion = de.Id }
                            join ins in db.INSPECTOR on new { Codigo_inspector = bo.codigo_inspector } equals new { Codigo_inspector = ins.Id }
                            where
                                bo.fecha_hora_boleta >= desde && bo.fecha_hora_boleta <= hasta &&
                                bo.numeroHH == numeroHH &&
                                bo.codigo_inspector == codigoInspector
                            select new DTOReportePorDescargaDeInspector
                            {
                                SerieBoleta = bo.serie,
                                NumeroBoleta = bo.numero_boleta,
                                CodigoInspector = bo.codigo_inspector,
                                NombreInspector = ins.Nombre,
                                FechaDescarga = bo.fecha_descarga,
                                FechaAccidente = bo.fecha_hora_boleta,
                                Autoridad = (((from a in db.AUTORIDAD where bo.codigo_autoridad_registra == a.Id
                                select new { a.Descripcion }).Distinct()).FirstOrDefault().Descripcion),
                                Delegacion = de.Descripcion,
                                ClasePlaca = bo.clase_placa,
                                CodigoNumeroPlaca = bo.codigo_placa +" "+bo.numero_placa,
                                NumeroHH = bo.numeroHH,
                                SerieNumParte  = bo.serie_parteoficial+"-"+ bo.numeroparte
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

    public class DTOReportePorDescargaDeInspector
    {
        public int SerieBoleta { get; set; }
        public decimal NumeroBoleta  { get; set; }
        public string CodigoInspector { get; set; }
        public string NombreInspector { get; set; }
        public DateTime ? FechaDescarga { get; set; }
        public DateTime ? FechaAccidente { get; set; }
        public string Autoridad { get; set; }
        public string Delegacion { get; set; }
        public string ClasePlaca { get; set; }
        public string CodigoNumeroPlaca { get; set; }
        public string NumeroHH { get; set; }
        public string SerieNumParte { get; set; }
    }
}