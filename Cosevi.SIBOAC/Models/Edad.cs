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
    using System.Web.Mvc;

    public partial class Edad
    {    
        [DisplayName("Fecha de nacimiento m�nima")]
        [Required(ErrorMessage = "La fecha de nacimiento m�nima es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]        
        public System.DateTime FechaMinNacimiento { get; set; }
       
        [DisplayName("Fecha de nacimiento m�xima")]
        [Required(ErrorMessage = "La fecha de nacimiento m�xima es obligatoria")]
        [DataType(DataType.Date)]        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]        
        public System.DateTime FechaMaxNacimiento { get; set; }


        [DisplayName("Fecha por defecto")]
        [Required(ErrorMessage = "La fecha por defecto es obligatoria")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaPorDefecto { get; set; }


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
