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

    public partial class AlineacionHorizontal
    {

        [DisplayName("Codigo")]
        [Required(ErrorMessage = "El código es obligatorio")]
        public int Id { get; set; }


        [DisplayName("Descripción")]
        [StringLength(30, ErrorMessage = "La descripción no debe ser mayor a 30 caracteres")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }


        [StringLength(1, ErrorMessage = "El estado no debe ser mayor a 1 caracter.")]
        [DisplayName("Estado")]
        public string Estado { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de inicio")] 
        public Nullable<System.DateTime> FechaDeInicio { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de fin")] 
        public Nullable<System.DateTime> FechaDeFin { get; set; }
    }
}
