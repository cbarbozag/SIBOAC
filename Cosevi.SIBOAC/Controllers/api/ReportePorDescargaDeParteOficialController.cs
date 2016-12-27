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

        // GET: api/ReportePorDescargaDeParteOficial
        public IQueryable<DTOReportePorDescargaDeParteOficial> GetReportePorDescargaDeParteOficial([FromUri] int idRadio, [FromUri] string idDelegaciones, [FromUri] string idAutoridades, [FromUri] DateTime desde, [FromUri] DateTime hasta)
        {
            if (idRadio == 1)
            {
                var reportes = (from pto in db.PARTEOFICIAL
                                join bo in db.BOLETA on new { SerieParte = pto.Serie, NumeroParte = pto.NumeroParte } equals new { SerieParte = bo.numeroSerie, NumeroParte = bo.numeroparte }
                                join de in db.DELEGACION on new { Codigo_delegacion = bo.codigo_delegacion } equals new { Codigo_delegacion = de.Id }
                                where
                                  pto.fecha_descarga >= desde && pto.fecha_descarga <= hasta &&
                                  bo.codigo_autoridad_registra == idAutoridades &&
                                  bo.codigo_delegacion == idDelegaciones
                                select new DTOReportePorDescargaDeParteOficial
                                {
                                    SerieParte = pto.Serie,
                                    NumeroParte = pto.NumeroParte,
                                    FechaDescarga = pto.fecha_descarga,
                                    FechaEntrega = pto.fecha_entrega,
                                    FechaAccidente = pto.Fecha,
                                    SerieNumeroBoleta = bo.serie + "-" + bo.numero_boleta,
                                    Autoridad = (((from a in db.AUTORIDAD
                                                   where bo.codigo_autoridad_registra == a.Id
                                                   select new { a.Descripcion }).Distinct()).FirstOrDefault().Descripcion),
                                    Delegacion = de.Descripcion,
                                    InfoPlaca = bo.clase_placa + "-" + bo.codigo_placa + "-" + bo.numero_placa,
                                    StatusPlano = pto.StatusPlano,
                                    PlacaConfiscada = bo.placa_confiscada,
                                    VehDetenido = bo.auto_detenido
                                }).Distinct();

                return reportes;
            }

            if(idRadio == 2)
            {
                var reportes = (from pto in db.PARTEOFICIAL
                                join bo in db.BOLETA on new { SerieParte = pto.Serie, NumeroParte = pto.NumeroParte } equals new { SerieParte = bo.numeroSerie, NumeroParte = bo.numeroparte }
                                join de in db.DELEGACION on new { Codigo_delegacion = bo.codigo_delegacion } equals new { Codigo_delegacion = de.Id }
                                where
                                  pto.fecha_entrega >= desde && pto.fecha_entrega <= hasta &&
                                  bo.codigo_autoridad_registra == idAutoridades &&
                                  bo.codigo_delegacion == idDelegaciones
                                select new DTOReportePorDescargaDeParteOficial
                                {
                                    SerieParte = pto.Serie,
                                    NumeroParte = pto.NumeroParte,
                                    FechaDescarga = pto.fecha_descarga,
                                    FechaEntrega = pto.fecha_entrega,
                                    FechaAccidente = pto.Fecha,
                                    SerieNumeroBoleta = bo.serie + "-" + bo.numero_boleta,
                                    Autoridad = (((from a in db.AUTORIDAD
                                                   where bo.codigo_autoridad_registra == a.Id
                                                   select new { a.Descripcion }).Distinct()).FirstOrDefault().Descripcion),
                                    Delegacion = de.Descripcion,
                                    InfoPlaca = bo.clase_placa + "-" + bo.codigo_placa + "-" + bo.numero_placa,
                                    StatusPlano = pto.StatusPlano,
                                    PlacaConfiscada = bo.placa_confiscada,
                                    VehDetenido = bo.auto_detenido
                                }).Distinct();

                return reportes;
            }

            if (idRadio == 3)
            {
                var reportes = (from pto in db.PARTEOFICIAL
                                join bo in db.BOLETA on new { SerieParte = pto.Serie, NumeroParte = pto.NumeroParte } equals new { SerieParte = bo.numeroSerie, NumeroParte = bo.numeroparte }
                                join de in db.DELEGACION on new { Codigo_delegacion = bo.codigo_delegacion } equals new { Codigo_delegacion = de.Id }
                                where
                                  pto.Fecha >= desde && pto.Fecha <= hasta &&
                                  bo.codigo_autoridad_registra == idAutoridades &&
                                  bo.codigo_delegacion == idDelegaciones
                                select new DTOReportePorDescargaDeParteOficial
                                {
                                    SerieParte = pto.Serie,
                                    NumeroParte = pto.NumeroParte,
                                    FechaDescarga = pto.fecha_descarga,
                                    FechaEntrega = pto.fecha_entrega,
                                    FechaAccidente = pto.Fecha,
                                    SerieNumeroBoleta = bo.serie + "-" + bo.numero_boleta,
                                    Autoridad = (((from a in db.AUTORIDAD
                                                   where bo.codigo_autoridad_registra == a.Id
                                                   select new { a.Descripcion }).Distinct()).FirstOrDefault().Descripcion),
                                    Delegacion = de.Descripcion,
                                    InfoPlaca = bo.clase_placa + "-" + bo.codigo_placa + "-" + bo.numero_placa,
                                    StatusPlano = pto.StatusPlano,
                                    PlacaConfiscada = bo.placa_confiscada,
                                    VehDetenido = bo.auto_detenido
                                }).Distinct();

                return reportes;
            }

            return null;
            
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

    public class DTOReportePorDescargaDeParteOficial
    {
        public string SerieParte { get; set; }
        public string NumeroParte { get; set; }
        public Nullable<DateTime> FechaDescarga { get; set; }
        public Nullable<DateTime> FechaEntrega{ get; set; }
        public DateTime FechaAccidente { get; set; }
        public string SerieNumeroBoleta { get; set; }
        public string Autoridad { get; set; }
        public string Delegacion { get; set; }
        public string InfoPlaca { get; set; }
        public int ? StatusPlano { get; set; }
        public string PlacaConfiscada { get; set; }
        public string VehDetenido { get; set; }



    }

}