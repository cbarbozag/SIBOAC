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

    public partial class Ruta
    {
        [DisplayName("C�digo")]
        [Required(ErrorMessage = "El c�digo es obligatorio")]
        public int Id { get; set; }

        [DisplayName("Inicia")]
        [StringLength(50, ErrorMessage = "La descripci�n no debe ser mayor a 50 caracteres")]
        [Required(ErrorMessage = "La ubicaci�n es obligatoria")]
        public string Inicia { get; set; }

        [DisplayName("Termina")]
        [StringLength(50, ErrorMessage = "La descripci�n no debe ser mayor a 50 caracteres")]
        [Required(ErrorMessage = "La ubicaci�n es obligatoria")]
        public string Termina { get; set; }

        [DisplayName("Estado")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(1, ErrorMessage = "El estado no debe ser mayor a 1 caracter")]
        public string Estado { get; set; }

        [DisplayName("Fecha de Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaDeInicio { get; set; }

        [DisplayName("Fecha de Fin")]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaDeFin { get; set; }

        public string DescripcionRuta { 
            get { return Inicia + " | " + Termina; }
           
        }
    }
}
