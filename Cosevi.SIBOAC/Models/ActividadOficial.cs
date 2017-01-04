using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cosevi.SIBOAC.Models
{
    public class ActividadOficial
    {
        public string Fuente { get; set; }

        public int Serie { get; set; }

        public decimal NumeroBoleta { get; set; }

        public Nullable<DateTime> FechaHoraBoleta { get; set; }

        public string ClasePlaca { get; set; }

        public string CodigoPlaca { get; set; }
        public string NumeroPlaca { get; set; }
        public string CodigoArticulo { get; set; }
        public string CodigoInspector { get; set; }

        public string NombreInspector { get; set; }

       
    }
}