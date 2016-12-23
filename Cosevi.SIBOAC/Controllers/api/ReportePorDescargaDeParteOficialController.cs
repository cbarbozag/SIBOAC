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
    public class ReportePorDescargaDeParteOficialController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/ReportePorDescargaDeBoletas
        public IQueryable<DTOReportePorDescargaDeParteOficial> GetReportePorDescargaDeParteOficial([FromUri] int idRadio, [FromUri] DateTime desde, [FromUri] DateTime hasta)
        {

            if (idRadio == 1)
            {
                var reportes =
                //(from bo in db.BOLETA
                // join po in db.PARTEOFICIAL on new { NumeroParte = bo.numeroparte } equals new { NumeroParte = po.NumeroParte }
                // join del in db.DELEGACION on new { Id = bo.codigo_delegacion } equals new { Id = (string)del.Id }
                // join au in db.AUTORIDAD on new { Id = bo.codigo_autoridad_registra } equals new { Id = (string)au.Id }
                // where
                // po.fecha_descarga >= desde && po.fecha_descarga <= hasta

                (from bo in db.BOLETA
                 join per in db.PERSONA on bo.identificacion equals per.identificacion
                 join del in db.DELEGACION on new { Id = bo.codigo_delegacion } equals new { Id = (string)del.Id }
                 join au in db.AUTORIDAD on new { Id = bo.codigo_autoridad_registra } equals new { Id = (string)au.Id }
                 where
                 (bo.fecha_descarga >= desde) &&
                 (bo.fecha_descarga <= hasta)
                 select new DTOReportePorDescargaDeParteOficial
                 {
                     Serie = bo.serie,
                     Boletas = bo.numero_boleta,
                     FechaDescarga = bo.fecha_descarga,
                     FechaAccidente = bo.fecha_hora_boleta,
                     Autoridad = bo.codigo_autoridad_registra,
                     CodigoDelegacion = bo.codigo_delegacion,
                     ClasePlaca = bo.clase_placa,
                     CodigoPlaca = bo.codigo_placa,
                     NumeroPlaca = bo.numero_placa,
                     Identificacion = bo.identificacion,
                     Nombre = per.nombre + " " + per.apellido1 + " " + per.apellido2,
                     CodRol = bo.codrol,
                     SerieParteOficial = bo.serie_parteoficial,
                     NumeroParte = bo.numeroparte,
                     DescripcionAutoridad = au.Descripcion,
                     DescripcionDelegacion = del.Descripcion

                 }).Distinct();

                return reportes;
            }

            else
            {
                 var reportes =
                (from bo in db.BOLETA
                 join per in db.PERSONA on bo.identificacion equals per.identificacion
                 join del in db.DELEGACION on new { Codigo_delegacion = bo.codigo_delegacion } equals new { Codigo_delegacion = del.Id }
                 join au in db.AUTORIDAD on new { Codigo_autoridad_registra = bo.codigo_autoridad_registra } equals new { Codigo_autoridad_registra = au.Id }
                 where
                 bo.fecha_hora_boleta >= desde && bo.fecha_hora_boleta <= hasta

                 select new DTOReportePorDescargaDeParteOficial
                 {
                     Serie = bo.serie,
                     Boletas = bo.numero_boleta,
                     FechaDescarga = bo.fecha_descarga,
                     FechaAccidente = bo.fecha_hora_boleta,
                     Autoridad = bo.codigo_autoridad_registra,
                     CodigoDelegacion = bo.codigo_delegacion,
                     ClasePlaca = bo.clase_placa,
                     CodigoPlaca = bo.codigo_placa,
                     NumeroPlaca = bo.numero_placa,
                     Identificacion = bo.identificacion,
                     Nombre = per.nombre,
                     Apellido1 = per.apellido1,
                     Apellido2 = per.apellido2,
                     CodRol = bo.codrol,
                     SerieParteOficial = bo.serie_parteoficial,
                     NumeroParte = bo.numeroparte,
                     DescripcionAutoridad = au.Descripcion,
                     DescripcionDelegacion = del.Descripcion
                 });

                return reportes;
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public class DTOReportePorDescargaDeParteOficial
        {

            public int Serie { get; set; }
            public decimal Boletas { get; set; }
            public Nullable<DateTime> FechaAccidente { get; set; }
            public string Autoridad { get; set; }
            public string Delegacion { get; set; }
            public string ClasePlaca { get; set; }
            public string CodigoPlaca { get; set; }
            public string NumeroPlaca { get; set; }
            public string Usuario { get; set; }
            public string Nombre { get; set; }
            public Nullable<DateTime> FechaDescarga { get; set; }
            public string CodigoDelegacion { get; set; }
            public string Identificacion { get; set; }
            public string Apellido1 { get; set; }
            public string Apellido2 { get; set; }
            public string CodRol { get; set; }
            public string SerieParteOficial { get; set; }
            public string NumeroParte { get; set; }
            public string DescripcionAutoridad { get; set; }
            public string DescripcionDelegacion { get; set; }

        }
    }
}