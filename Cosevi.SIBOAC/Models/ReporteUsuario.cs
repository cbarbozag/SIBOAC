using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cosevi.SIBOAC.Models
{
    public class ReporteUsuario
    {
        public string Usuario { get; set; }
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