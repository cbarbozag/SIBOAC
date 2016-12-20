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
        public IQueryable<Boletas> GetBOLETA()
        {
            return db.BOLETA;
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

            public string Serie { get; set; }
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