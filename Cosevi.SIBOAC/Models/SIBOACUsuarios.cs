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
    using System.ComponentModel.DataAnnotations;

    public partial class SIBOACUsuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SIBOACUsuarios()
        {
            this.SIBOACRoles = new HashSet<SIBOACRoles>();
        }
    
        public int Id { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Usuarios es obligatorio")]
        public string Usuario { get; set; }

        [StringLength(4, ErrorMessage = "El código no debe ser mayor a 4 caracteres")]
        public string codigo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaDeActualizacionClave { get; set; }
        public Nullable<bool> Activo { get; set; }

        [StringLength(9, ErrorMessage = "La identificación no debe ser mayor a 9 caracteres")]
        [Required(ErrorMessage = "La Identificación es obligatoria")]
        public string Identificacion { get; set; }
        public string LugarTrabajo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime UltimoIngreso { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SIBOACRoles> SIBOACRoles { get; set; }
    }
}
