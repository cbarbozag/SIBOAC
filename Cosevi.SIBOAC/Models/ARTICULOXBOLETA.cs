//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cosevi.SIBOAC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ARTICULOXBOLETA
    {
        public string codigo_articulo { get; set; }
        public string conducta { get; set; }
        public System.DateTime Fecha_Inicio { get; set; }
        public System.DateTime Fecha_Final { get; set; }
        public string fuente { get; set; }
        public int serie { get; set; }
        public decimal numero_boleta { get; set; }
        public Nullable<decimal> multa { get; set; }
    }
}
