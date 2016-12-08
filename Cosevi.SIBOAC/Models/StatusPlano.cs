using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cosevi.SIBOAC.Models
{
    public class StatusPlano
    {
        public string CodigoAutoridad { get; set; }

        public string DescripcionAutoridad { get; set; }

        public DateTime FechaAccidente { get; set; }

        public Nullable< DateTime> FechaDescarga  { get; set; }

        public decimal NumeroBoleta { get; set; }

        public string CodigoRol { get; set; }

        public string ClasePlaca { get; set; }

        public string CodigoPlaca { get; set; }

        public string NumeroPlaca { get; set; }
        public Nullable<int> EstadoPlano { get; set; }
        public string DescripcionRol { get; set; }
    }
}