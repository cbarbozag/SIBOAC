using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cosevi.SIBOAC.Models
{
    public class BoletaAdjunto
    {
        public string CodigoAutoridad { get; set; }

        public string DescripcionAutoridad { get; set; }

        public Nullable<DateTime> FechaBoleta { get; set; }

        public Nullable<DateTime> FechaRegistro { get; set; }

        public int SerieBoleta { get; set; }

        public decimal NumeroBoleta { get; set; }

        public string CodigoRol { get; set; }

        public string ClasePlaca { get; set; }

        public string CodigoPlaca { get; set; }

        public string NumeroPlaca { get; set; }

        public string DescripcionRol { get; set; }

        public Nullable<System.DateTime> FechaModificado { get; set; }
        public string UsuarioModificaPlano { get; set; }
    }
}