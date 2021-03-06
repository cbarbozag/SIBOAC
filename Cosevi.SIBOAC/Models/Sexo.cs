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

    public partial class Sexo
    {
        [DisplayName("C�digo")]
        [StringLength(1, ErrorMessage = "El c�digo no debe ser mayor a 1 caracter")]
        [Required(ErrorMessage = "El c�digo es obligatorio")]
        public string Id { get; set; }

        [DisplayName("Descripci�n")]
        [StringLength(10, ErrorMessage = "La descripci�n no debe ser mayor a 10 caracteres")]
        [Required(ErrorMessage = "La descripci�n es obligatoria")]
        public string Descripcion { get; set; }

        [StringLength(1, ErrorMessage = "El estado no debe ser mayor a 1 caracter.")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        [DisplayName("Estado")]
        public string Estado { get; set; }


        //[DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de inicio")] //etiqueta Fecha de inicio
        public System.DateTime FechaDeInicio { get; set; }

        //[DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de fin")] //etiqueta Fecha de fin
        public System.DateTime FechaDeFin { get; set; }
    }
}
