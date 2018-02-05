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

    public partial class Leyenda_Poder_Judicial
    {
        [DisplayName("C�digo de Autoridad")]
        [StringLength(5, ErrorMessage = "El codigo no debe ser mayor a 5 caracteres.")]
        [Required(ErrorMessage = "El c�digo es obligatorio")]
        public string codigo_autoridad { get; set; }

        [DisplayName("Leyenda")]
        [StringLength(400, ErrorMessage = "La leyenda no debe ser mayor a 400 caracteres")]
        [Required(ErrorMessage = "La leyenda es obligatoria")]
        public string leyenda { get; set; }
        
        [DisplayName("Estado")]
        [StringLength(1, ErrorMessage = "El estado no debe ser mayor a 1 caracter.")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        public string estado { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public System.DateTime fecha_inicio { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de fin")]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public System.DateTime fecha_fin { get; set; }
    }
}