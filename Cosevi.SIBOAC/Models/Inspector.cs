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

    public partial class Inspector
    {
        [DisplayName("Código")]
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(4, ErrorMessage = "El código no debe ser mayor a 4 caracteres")]
        public string Id { get; set; }

        [DisplayName("Tipo de identificación")]
        [Required(ErrorMessage = "El tipo de identificación es obligatorio")]
        public string TipoDeIdentificacion { get; set; }

        [DisplayName("Identificación")]
        [Required(ErrorMessage = "La identificación es obligatoria")]
        public string Identificacion { get; set; }

        [DisplayName("Nombre Completo")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "La descripción no debe ser mayor a 50 caracteres")]
        public string Nombre { get; set; }

        [DisplayName("Primer apellido")]
        public string Apellido1 { get; set; }

        [DisplayName("Segundo apellido")]
        public string Apellido2 { get; set; }

        [DisplayName("Ad honorem")]
        //[Required(ErrorMessage = "El campo es obligatorio")]
        //[StringLength(10, ErrorMessage = "El código no debe ser mayor a 10 caracteres")]
        public string Adonoren { get; set; }

        [DisplayName("Fecha de inclusión")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string FechaDeInclusion { get; set; }

        [DisplayName("Fecha de exclusión")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string FechaDeExclusion { get; set; }

        [DisplayName("Documento de inclusión")]
        public string DocumentoDeInclusion { get; set; }

        [DisplayName("Documento de exclusión")]
        public string DocumentoDeExclusion { get; set; }

        [DisplayName("Fecha de registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaReag { get; set; }

        [DisplayName("Documento de registro")]
        [StringLength(20, ErrorMessage = "El documento no debe ser mayor a 20 caracteres")]
        public string DocumentoReag { get; set; }

        [DisplayName("Codigo de delegación")]
        public string CodigoDeDelegacion { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Ingrese un correo valido")]
        public string Email { get; set; }

        [DisplayName("Estado")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        [StringLength(1, ErrorMessage = "El estado no debe ser mayor a 1 caracter")]
        public string Estado { get; set; }

        [DisplayName("Fecha de Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaDeInicio { get; set; }

        [DisplayName("Fecha de Fin")]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaDeFin { get; set; }
    }
}
