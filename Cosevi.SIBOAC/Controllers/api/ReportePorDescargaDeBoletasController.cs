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
    public class ReportePorDescargaDeBoletasController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/ReportePorDescargaDeBoletas
        public IQueryable<DTOReportePorDescargaDeBoletas> GetBOLETA([FromUri] int idRadio, [FromUri] DateTime desde, [FromUri] DateTime hasta)
        {
            var reportes =
            (from bo in db.BOLETA
             join po in db.PARTEOFICIAL on new { NumeroParte = bo.numeroparte } equals new { NumeroParte = po.NumeroParte }
             join del in db.DELEGACION on new { Id = bo.codigo_delegacion } equals new { Id = (string)del.Id }
             join au in db.AUTORIDAD on new { Id = bo.codigo_autoridad_registra } equals new { Id = (string)au.Id }
             where
             po.StatusPlano == idRadio &&
             po.Fecha >= desde && po.Fecha <= hasta


             select new DTOReportePorDescargaDeBoletas
             {

                 Serie = bo.serie,
                 Boletas = bo.numero_boleta,
                 FechaAccidente = po.Fecha,
                 Autoridad = bo.codigo_autoridad_registra,
                 Delegacion = del.Descripcion,
                 ClasePlaca = bo.clase_placa,
                 CodigoPlaca = bo.codigo_placa,
                 NumeroPlaca = bo.numero_placa,
                 FechaDescarga = bo.fecha_descarga
             });

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

        public class DTOReportePorDescargaDeBoletas
        {

            public int Serie { get; set; }
            public decimal Boletas { get; set; }
            public DateTime FechaAccidente { get; set; }
            public string Autoridad { get; set; }
            public string Delegacion { get; set; }
            public string ClasePlaca { get; set; }
            public string CodigoPlaca { get; set; }
            public string NumeroPlaca { get; set; }
            public string Usuario { get; set; }
            public string Nombre { get; set; }
            public Nullable<DateTime> FechaDescarga { get; set; }

        }
    }
}