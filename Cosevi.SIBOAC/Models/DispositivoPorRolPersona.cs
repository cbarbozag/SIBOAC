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

    public partial class DispositivoPorRolPersona
    {
        [DisplayName("C�digo Rol Persona")]
        [StringLength(2, ErrorMessage = "El codigo no debe ser mayor a 2 caracter.")]
        [Required(ErrorMessage = "El codigo es obligatorio")]
        public string CodigoRolPersona { get; set; }

        [DisplayName("C�digo Dispositivo")]
        [Required(ErrorMessage = "El c�digo es obligatorio")]
        public int CodigoDispositivo { get; set; }

        [DisplayName("Descripci�n")]
        public string DescripcionRolPersona { set; get;  }
        [DisplayName("Descripci�n")]
        public string DescripcionDispositivo { get; set; }

    }
}
