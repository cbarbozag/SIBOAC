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
using System.Web.Mvc;

namespace Cosevi.SIBOAC.Controllers.api
{
    public class ReportePorConsultaImpresionDeBoletasController : ApiController
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: api/ReportePorConsultaImpresionDeBoletas
        public IQueryable<DTOReportePorConsultaImpresionDeBoletas> GetReportePorConsultaImpresionDeBoletas([FromUri] string[] idDelegaciones, [FromUri] string[] idInspectores, [FromUri] DateTime desde, [FromUri] DateTime hasta)
        {
            var reportes =
                (from bo in db.BOLETA
                 join pro in db.PROVINCIA on new { Provincia = bo.cod_provincia } equals new { Provincia = pro.Id }
                 join apb in db.ARTICULOXBOLETA on new { Fuente = bo.fuente, Serie = bo.serie, Boleta = bo.numero_boleta } equals new { Fuente = apb.fuente, Serie = apb.serie, Boleta = apb.numero_boleta }
                 join del in db.DELEGACION on new { Codigo_delegacion = bo.codigo_delegacion } equals new { Codigo_delegacion = del.Id }
                 join can in db.CANTON on new { Cod_canton = bo.cod_canton } equals new { Cod_canton = can.Id }
                 join dis in db.DISTRITO on new { Cod_distrito = bo.cod_distrito } equals new { Cod_distrito = dis.Id }
                 where
                 (bo.fecha_hora_boleta >= desde) && 
                 (bo.fecha_hora_boleta <= hasta) &&
                 idDelegaciones.Contains(bo.codigo_delegacion) &&
                 idInspectores.Contains(bo.codigo_inspector)
                 select new DTOReportePorConsultaImpresionDeBoletas
                 {
                     DescripcionDelegacion = del.Descripcion,
                     CodigoInspector = bo.codigo_inspector,
                     Serie = bo.serie,
                     Boletas = bo.numero_boleta,
                     FechaInfraccion = bo.fecha_hora_boleta,
                     FechaDescarga = bo.fecha_descarga,
                     CodigoArticulo = apb.codigo_articulo,
                     Provincia = pro.Descripcion + "-" + can.Descripcion + "-" + dis.Descripcion,
                     CoordenadaX = bo.x,
                     CoordenadaY = bo.y
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

     

        public class DTOReportePorConsultaImpresionDeBoletas
        {
            public string DescripcionDelegacion { get; set; }
            public string CodigoInspector { get; set; }
            public int Serie { get; set; }
            public decimal Boletas { get; set; }
            public Nullable<DateTime> FechaInfraccion { get; set; }
            public Nullable<DateTime> FechaDescarga { get; set; }
            public string CodigoArticulo { get; set; }
            public string Provincia { get; set; }
            public string CoordenadaX { get; set; }
            public string CoordenadaY { get; set; }

        }
    }
}