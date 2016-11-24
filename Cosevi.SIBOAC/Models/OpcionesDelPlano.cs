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

    public partial class OpcionesDelPlano
    {
        [DisplayName("C�digo")]
        [Required(ErrorMessage = "El c�digo es obligatorio")]
        public short Id { get; set; }


        [DisplayName("Descripci�n")]
        [StringLength(50, ErrorMessage = "La descripci�n no debe ser mayor a 50 caracteres")]
        [Required(ErrorMessage = "La descripci�n es obligatoria")]
        public string Descripcion { get; set; }


        [StringLength(1, ErrorMessage = "El estado no debe ser mayor a 1 caracter.")]
        [DisplayName("Estado")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public Nullable<System.DateTime> FechaDeInicio { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de fin")]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public Nullable<System.DateTime> FechaDeFin { get; set; }
    }
}
