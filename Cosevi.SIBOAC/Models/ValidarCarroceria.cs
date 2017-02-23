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

    public partial class ValidarCarroceria
    {
        [DisplayName("C�digo ID Veh�culo")]
        [StringLength(2, ErrorMessage = "El codigo no debe ser mayor a 2 caracter.")]
        [Required(ErrorMessage = "El c�digo es obligatorio")]
        public string CodigoTipoIdVehiculo { get; set; }

        [DisplayName("C�digo Tipo Veh�culo")]
        [Required(ErrorMessage = "El c�digo es obligatorio")]
        public int CodigoTiposVehiculos { get; set; }

        [DisplayName("C�digo Carrocer�a")]
        [Required(ErrorMessage = "El c�digo es obligatorio")]
        public int CodigoCarroceria { get; set; }

        [DisplayName("Estado")]
        [StringLength(1, ErrorMessage = "El estado no debe ser mayor a 1 caracter.")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; }

        [DisplayName("Fecha Inicio")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public System.DateTime FechaDeInicio { get; set; }

        [DisplayName("Fecha Fin")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public System.DateTime FechaDeFin { get; set; }

        public string DescripcionCodigoTipoIdVehiculo
        { get; set; }

        public string DescripcionCodigoTiposVehiculos
        { get; set; }

        public string DescripcionCodigoCarroceria
        { get; set; }
    }
}
