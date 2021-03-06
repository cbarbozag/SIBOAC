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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Leyenda_Puntos
    {

        [DisplayName("C�digo")]        
        [Required(ErrorMessage = "El c�digo es obligatorio")]
        public int Codigo { get; set; }

        [DisplayName("Descripci�n")]
        [StringLength(50, ErrorMessage = "La descripci�n no debe ser mayor a 50 caracteres")]
        [Required(ErrorMessage = "La descripci�n es obligatoria")]
        public string Descripcion { get; set; }

        [DisplayName("Estado")]
        [StringLength(1, ErrorMessage = "El estado no debe ser mayor a 1 caracter.")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public System.DateTime Fecha_Inicio { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de fin")]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public System.DateTime Fecha_Final { get; set; }
    }
}
