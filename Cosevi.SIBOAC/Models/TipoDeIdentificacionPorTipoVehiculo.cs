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

    public partial class TipoDeIdentificacionPorTipoVehiculo
    {
        [DisplayName("C�digo Ident. Veh.")]
        [StringLength(2, ErrorMessage = "El c�digo no debe ser mayor a 2 caracteres")]
        [Required(ErrorMessage = "El codigo es obligatorio")]
        public string CodigoTipoIDEVehiculo { get; set; }

        [DisplayName("C�digo Tipo Veh.")]
        [Required(ErrorMessage = "El codigo es obligatorio")]
        public int CodigoTipoVeh { get; set; }

        [DisplayName("Estado")]
        [StringLength(1, ErrorMessage = "El estado no debe ser mayor a 1 caracter")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; }

        [DisplayName("Fecha de Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaDeInicio { get; set; }

        [DisplayName("Fecha de Fin")]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaDeFin { get; set; }

        [DisplayName("Descripci�n")]
        public string DescripcionCodigoTipoIDEVehiculo { get; set; }
        [DisplayName("Descripci�n")]
        public string DescripcionCodigoTipoVeh { get; set; }


    }
}
