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
    public class ReportePorEstadoActualDelPlanoController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/ReportePorEstadoActualDelPlano
        public IQueryable<DTOReportePorEstadoActualDelPlano> GetReportePorEstadoActualDelPlano([FromUri] int statusPlano, [FromUri] string[] idDelegaciones, [FromUri] string[] idAutoridades, [FromUri] DateTime desde, [FromUri] DateTime hasta)
        {

            var reportes = (from bo in db.BOLETA
                            join po in db.PARTEOFICIAL on new { NumeroParte = bo.numeroparte } equals new { NumeroParte = po.NumeroParte }
                            join per in db.PERSONA on bo.identificacion equals per.identificacion
                            join del in db.DELEGACION on new { Id = bo.codigo_delegacion } equals new { Id = (string)del.Id }
                            join au in db.AUTORIDAD on new { Id = bo.codigo_autoridad_registra } equals new { Id = (string)au.Id }
                            where
                              po.StatusPlano == statusPlano &&
                              po.Fecha >= desde && po.Fecha <= hasta
                              &&
                              idDelegaciones.Contains(bo.codigo_delegacion) &&
                              idAutoridades.Contains(bo.codigo_autoridad_registra)
                            select new DTOReportePorEstadoActualDelPlano
                            {
                                Autoridad = bo.codigo_autoridad_registra,
                                Serie = bo.serie,
                                NumeroParte = bo.numeroparte,
                                Boletas = bo.numero_boleta,
                                FechaAccidente = po.Fecha,
                                FechaDescarga = bo.fecha_descarga,
                                identificacion = bo.identificacion,
                                nombre = per.nombre + " " + per.apellido1 + " " + per.apellido2

                            }).Distinct();
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

    public class DTOReportePorEstadoActualDelPlano
    {
        public string Autoridad { get; set; }
        public int Serie { get; set; }
        public string NumeroParte { get; set; }
        public decimal Boletas { get; set; }
        public DateTime FechaAccidente { get; set; }
        public Nullable<DateTime> FechaDescarga { get; set; }
        public string identificacion { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
    }

}