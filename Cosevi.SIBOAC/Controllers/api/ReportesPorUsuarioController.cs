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
    public class ReportesPorUsuarioController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/ReportesPorUsuario
        public IQueryable<DTOReportesPorUsuario> GetReportesPorUsuario([FromUri] string[] idUsuarios, [FromUri] DateTime desde, [FromUri] DateTime hasta)
        {
            var reportes = (from bo in db.BOLETA
                            join pto in db.PARTEOFICIAL on new { numeroparte = bo.numeroparte } equals new { numeroparte = pto.NumeroParte }
                            join su in db.SIBOACUsuarios on new { usuario_entregaPlano = pto.usuario_entregaPlano } equals new { usuario_entregaPlano = su.Id.ToString() }
                            where
                             idUsuarios.Contains(pto.usuario_entregaPlano) &&
                             pto.Fecha >= desde && pto.Fecha <= hasta
                            select new DTOReportesPorUsuario
                            {
                                Usuario = su.Usuario,
                                Nombre = su.Nombre,
                                Autoridad = bo.codigo_autoridad_registra,
                                FechaAccidente = pto.Fecha,
                                Serie = bo.serie.ToString(),
                                NumeroParte = bo.numeroparte,
                                Boletas = bo.numero_boleta,
                                FechaDescarga = bo.fecha_descarga,
                                ClasePlaca = bo.clase_placa,
                                CodigoPlaca = bo.codigo_placa,
                                NumeroPlaca = bo.numero_placa,
                                EstadoPlano = pto.StatusPlano
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
    }

    public class DTOReportesPorUsuario
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Autoridad { get; set; }
        public DateTime FechaAccidente { get; set; }
        public string Serie { get; set; }
        public string NumeroParte { get; set; }
        public decimal Boletas { get; set; }
        public Nullable<DateTime> FechaDescarga { get; set; }
        public string NumeroPlaca { get; set; }
        public string ClasePlaca { get; set; }
        public string CodigoPlaca { get; set; }
        public Nullable<int> EstadoPlano { get; set; }
    }
}