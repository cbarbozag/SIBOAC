﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cosevi.SIBOAC.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PC_HH_AndroidEntities : DbContext
    {
        public PC_HH_AndroidEntities()
            : base("name=PC_HH_AndroidEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ESTCIVIL> ESTCIVILs { get; set; }
        public virtual DbSet<ILUMINACION> ILUMINACION { get; set; }
        public virtual DbSet<MANIOBRA> MANIOBRAs { get; set; }
        public virtual DbSet<MotivoPorNoFirmar> MotivoPorNoFirmars { get; set; }
        public virtual DbSet<Obstaculo> Obstaculo { get; set; }
        public virtual DbSet<OficinaParaImpugnar> OficinaParaImpugnars { get; set; }
        public virtual DbSet<RolDePersonaPorVehiculo> RolDePersonaPorVehiculoes { get; set; }
        public virtual DbSet<Dispositivo> Dispositivoes1 { get; set; }
    }
}
