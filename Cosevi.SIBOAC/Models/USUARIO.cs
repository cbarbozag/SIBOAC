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
    
    public partial class USUARIO
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Codigo_Oficina { get; set; }
        public string Estado { get; set; }
        public Nullable<System.DateTime> Fecha_Registro { get; set; }
        public Nullable<System.DateTime> Fecha_Actualizacion { get; set; }
        public string Usuario_Registra { get; set; }
        public string Maquina { get; set; }
        public Nullable<bool> Reportes { get; set; }
        public Nullable<bool> Modifica { get; set; }
        public Nullable<bool> Imprimeparte { get; set; }
    }
}
