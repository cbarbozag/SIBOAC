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
    
        public virtual DbSet<EstadoCivil> EstadoCivil { get; set; }
        public virtual DbSet<Iluminacion> Iluminacion { get; set; }
        public virtual DbSet<Maniobra> Maniobra { get; set; }
        public virtual DbSet<MotivoPorNoFirmar> MotivoPorNoFirmars { get; set; }
        public virtual DbSet<Obstaculo> Obstaculo { get; set; }
        public virtual DbSet<OficinaParaImpugnar> OficinaParaImpugnars { get; set; }
        public virtual DbSet<RolDePersonaPorVehiculo> RolDePersonaPorVehiculoes { get; set; }
        public virtual DbSet<Dispositivo> Dispositivoes1 { get; set; }
        public virtual DbSet<Hospital> HOSPITAL { get; set; }
        public virtual DbSet<Inconsistencia> INCONSISTENCIA { get; set; }
        public virtual DbSet<MarcaDeAutomovil> MARCA { get; set; }
        public virtual DbSet<OtroTipoTransporte> OTROTIPOTRANSPORTE { get; set; }
        public virtual DbSet<EstadoDeLaCalzada> ESTCALZADA { get; set; }
        public virtual DbSet<TipoDeEstructura> ESTRUCTURA { get; set; }
        public virtual DbSet<OpcionDeFormulario> OPCIONFORMULARIO { get; set; }
        public virtual DbSet<OpcionesDelPlano> OPCIONPLANO { get; set; }
        public virtual DbSet<Plantillas> PLANTILLAS { get; set; }
        public virtual DbSet<Provincia> PROVINCIA { get; set; }
        public virtual DbSet<Sentido> SENTIDO { get; set; }
        public virtual DbSet<Senalamiento> SEÑALAMIENTO { get; set; }
        public virtual DbSet<Sexo> SEXO { get; set; }
        public virtual DbSet<TipoDeIdentificacion> TIPO_IDENTIFICACION { get; set; }
        public virtual DbSet<TipoDeLicencia> TIPO_LICENCIA { get; set; }
        public virtual DbSet<TipoDeVehiculo> TIPOVEH { get; set; }
    }
}
