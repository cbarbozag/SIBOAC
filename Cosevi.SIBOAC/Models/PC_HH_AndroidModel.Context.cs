﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
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
        public virtual DbSet<Peaton> Peaton { get; set; }
        public virtual DbSet<Revision> Revision { get; set; }
        public virtual DbSet<RolPorPersona> ROLPERSONA { get; set; }
        public virtual DbSet<Ruta> Ruta { get; set; }
        public virtual DbSet<Tiempo> Tiempo { get; set; }
        public virtual DbSet<TipoDeAccidente> TIPOACCIDENTE { get; set; }
        public virtual DbSet<TipoDeCalzada> TIPOCALZADA { get; set; }
        public virtual DbSet<TipoDeDocumento> TIPODOCUMENTO { get; set; }
        public virtual DbSet<TipoIdDeVehiculo> TIPOIDEVEHICULO { get; set; }
        public virtual DbSet<TipoDeSenalExistente> TIPOSEÑALEXISTE { get; set; }
        public virtual DbSet<ClaseDePlaca> CLASE { get; set; }
        public virtual DbSet<CodigoDeLaPlaca> CODIGO { get; set; }
        public virtual DbSet<CondicionDeLaCalzada> CONDCALZADA { get; set; }
        public virtual DbSet<CondicionDeLaPersona> CONDPERSONA { get; set; }
        public virtual DbSet<Dano> DAÑO { get; set; }
        public virtual DbSet<Delegacion> DELEGACION { get; set; }
        public virtual DbSet<Delito> DELITO { get; set; }
        public virtual DbSet<DepositoDePlaca> DEPOSITOPLACA { get; set; }
        public virtual DbSet<DepositoDeVehiculo> DEPOSITOVEHICULO { get; set; }
        public virtual DbSet<AlineacionHorizontal> ALINHORI { get; set; }
        public virtual DbSet<AlineacionVertical> ALINVERT { get; set; }
        public virtual DbSet<Canton> CANTON { get; set; }
        public virtual DbSet<CaracteristicasDeUbicacion> CARACUBI { get; set; }
        public virtual DbSet<Carril> CARRIL { get; set; }
        public virtual DbSet<Carroceria> CARROCERIA { get; set; }
        public virtual DbSet<Circulacion> CIRCULACION { get; set; }
        public virtual DbSet<Direccion> DIRECCION { get; set; }
        public virtual DbSet<Distrito> DISTRITO { get; set; }
        public virtual DbSet<Examen> EXAMEN { get; set; }
        public virtual DbSet<Inspector> INSPECTOR { get; set; }
        public virtual DbSet<Interseccion> INTERSECCION { get; set; }
        public virtual DbSet<NombreDeMenu> Nombre_Menu { get; set; }
        public virtual DbSet<UnidadesDeAlcohol> UNIDADES_ALCOHOL { get; set; }
        public virtual DbSet<VariablesParaBloqueo> VARIABLESBLOQUEO { get; set; }
        public virtual DbSet<DispositivosMoviles> DispositivosMoviles { get; set; }
        public virtual DbSet<HorasLaborales> HORASLABORALES { get; set; }
        public virtual DbSet<RolDePersonaPorTipoDeIdentificacionDeVehiculo> ROLPERSONAXTIPOIDEVEHICULO { get; set; }
        public virtual DbSet<TiposDeVehiculos> TIPOSVEHICULOS { get; set; }
        public virtual DbSet<Nacionalidad> NACIONALIDAD { get; set; }
        public virtual DbSet<Edad> EDAD { get; set; }
        public virtual DbSet<ArticulosPorDepositosDeBienes> ARTICULOSXDEPOSITOSBIENES { get; set; }
        public virtual DbSet<Autoridad> AUTORIDAD { get; set; }
        public virtual DbSet<CatalogoDeArticulos> CATARTICULO { get; set; }
        public virtual DbSet<DanioPorHospital> DAÑOXHOSPITAL { get; set; }
        public virtual DbSet<DetallePorTipoDanio> DETALLETIPODAÑO { get; set; }
        public virtual DbSet<DetallePorTipoSenial> DETALLETIPOSEÑAL { get; set; }
        public virtual DbSet<DispositivoPorRolPersona> DISPXROLPERSONA { get; set; }
        public virtual DbSet<Division> DIVISION { get; set; }
        public virtual DbSet<OpcionFormularioPorArticulo> OPCFORMULARIOXARTICULO { get; set; }
        public virtual DbSet<RutasPorDistritos> RUTASXDISTRITO { get; set; }
        public virtual DbSet<TipoDeIdentificacionPorTipoVehiculo> TIPOIDEVEHICULOXTIPOVEH { get; set; }
        public virtual DbSet<TiposPorDocumento> TIPOSXDOCUMENTO { get; set; }
        public virtual DbSet<ValidarCarroceria> VALIDARCARROCERIA { get; set; }
        public virtual DbSet<ConsecutivoNumeroMarco> CONSECUTIVONUMEROMARCO { get; set; }
        public virtual DbSet<RolPorPersonaOpcionFormulario> ROLPERSONA_OPCIONFORMULARIO { get; set; }
        public virtual DbSet<TipoVehiculoPorCodigoPorClase> TIPOVEHCODIGOCLASE { get; set; }
        public virtual DbSet<DepositosBienes> DEPOSITOBIENES { get; set; }
        public virtual DbSet<TIPODANO> TIPODANO { get; set; }
        public virtual DbSet<BitacoraSIBOAC> BitacoraSIBOAC { get; set; }
        public virtual DbSet<ParteOficial> PARTEOFICIAL { get; set; }
        public virtual DbSet<Boletas> BOLETA { get; set; }
        public virtual DbSet<PERSONA> PERSONA { get; set; }
        public virtual DbSet<GENERALES> GENERALES { get; set; }
        public virtual DbSet<ARTICULOXBOLETA> ARTICULOXBOLETA { get; set; }
        public virtual DbSet<VEHICULO> VEHICULO { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
        public virtual DbSet<LEYENDAPORAUTORIDAD> LEYENDAPORAUTORIDAD { get; set; }
        public virtual DbSet<ARTICULO_ESPECIFICO> ARTICULO_ESPECIFICO { get; set; }
        public virtual DbSet<OtrosAdjuntos> OtrosAdjuntos { get; set; }
        public virtual DbSet<TESTIGOXPARTE> TESTIGOXPARTE { get; set; }
        public virtual DbSet<TESTIGO> TESTIGO { get; set; }
    
        public virtual ObjectResult<BitacoraSIBOAC> GetBitacoraData(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string nombreTabla, string usuario, string operacion)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var nombreTablaParameter = nombreTabla != null ?
                new ObjectParameter("NombreTabla", nombreTabla) :
                new ObjectParameter("NombreTabla", typeof(string));
    
            var usuarioParameter = usuario != null ?
                new ObjectParameter("Usuario", usuario) :
                new ObjectParameter("Usuario", typeof(string));
    
            var operacionParameter = operacion != null ?
                new ObjectParameter("Operacion", operacion) :
                new ObjectParameter("Operacion", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BitacoraSIBOAC>("GetBitacoraData", fechaInicioParameter, fechaFinParameter, nombreTablaParameter, usuarioParameter, operacionParameter);
        }
    
        public virtual ObjectResult<BitacoraSIBOAC> GetBitacoraData(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string nombreTabla, string usuario, string operacion, MergeOption mergeOption)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var nombreTablaParameter = nombreTabla != null ?
                new ObjectParameter("NombreTabla", nombreTabla) :
                new ObjectParameter("NombreTabla", typeof(string));
    
            var usuarioParameter = usuario != null ?
                new ObjectParameter("Usuario", usuario) :
                new ObjectParameter("Usuario", typeof(string));
    
            var operacionParameter = operacion != null ?
                new ObjectParameter("Operacion", operacion) :
                new ObjectParameter("Operacion", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<BitacoraSIBOAC>("GetBitacoraData", mergeOption, fechaInicioParameter, fechaFinParameter, nombreTablaParameter, usuarioParameter, operacionParameter);
        }
    
        public virtual ObjectResult<GetDescargaBoletaData_Result> GetDescargaBoletaData(Nullable<int> tipoFecha, Nullable<System.DateTime> fechaInicial, Nullable<System.DateTime> fechaFinal, string usuarioSistema)
        {
            var tipoFechaParameter = tipoFecha.HasValue ?
                new ObjectParameter("TipoFecha", tipoFecha) :
                new ObjectParameter("TipoFecha", typeof(int));
    
            var fechaInicialParameter = fechaInicial.HasValue ?
                new ObjectParameter("FechaInicial", fechaInicial) :
                new ObjectParameter("FechaInicial", typeof(System.DateTime));
    
            var fechaFinalParameter = fechaFinal.HasValue ?
                new ObjectParameter("FechaFinal", fechaFinal) :
                new ObjectParameter("FechaFinal", typeof(System.DateTime));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDescargaBoletaData_Result>("GetDescargaBoletaData", tipoFechaParameter, fechaInicialParameter, fechaFinalParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetDescargaInspectorData_Result> GetDescargaInspectorData(Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string codigoOficial, string usuarioSistema)
        {
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var codigoOficialParameter = codigoOficial != null ?
                new ObjectParameter("CodigoOficial", codigoOficial) :
                new ObjectParameter("CodigoOficial", typeof(string));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDescargaInspectorData_Result>("GetDescargaInspectorData", fechaInicioParameter, fechaFinParameter, codigoOficialParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetConsultaeImpresionDeParteOficialData_Result> GetConsultaeImpresionDeParteOficialData(Nullable<int> tipoConsulta, string parametro1, string parametro2, string parametro3, string usuarioSistema)
        {
            var tipoConsultaParameter = tipoConsulta.HasValue ?
                new ObjectParameter("TipoConsulta", tipoConsulta) :
                new ObjectParameter("TipoConsulta", typeof(int));
    
            var parametro1Parameter = parametro1 != null ?
                new ObjectParameter("Parametro1", parametro1) :
                new ObjectParameter("Parametro1", typeof(string));
    
            var parametro2Parameter = parametro2 != null ?
                new ObjectParameter("Parametro2", parametro2) :
                new ObjectParameter("Parametro2", typeof(string));
    
            var parametro3Parameter = parametro3 != null ?
                new ObjectParameter("Parametro3", parametro3) :
                new ObjectParameter("Parametro3", typeof(string));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetConsultaeImpresionDeParteOficialData_Result>("GetConsultaeImpresionDeParteOficialData", tipoConsultaParameter, parametro1Parameter, parametro2Parameter, parametro3Parameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetConsultaeImpresionDeBoletasData_Result> GetConsultaeImpresionDeBoletasData(Nullable<System.DateTime> fechaDesde, Nullable<System.DateTime> fechaHasta, string idDelegaciones, string idInspectores, string usuarioSistema)
        {
            var fechaDesdeParameter = fechaDesde.HasValue ?
                new ObjectParameter("FechaDesde", fechaDesde) :
                new ObjectParameter("FechaDesde", typeof(System.DateTime));
    
            var fechaHastaParameter = fechaHasta.HasValue ?
                new ObjectParameter("FechaHasta", fechaHasta) :
                new ObjectParameter("FechaHasta", typeof(System.DateTime));
    
            var idDelegacionesParameter = idDelegaciones != null ?
                new ObjectParameter("idDelegaciones", idDelegaciones) :
                new ObjectParameter("idDelegaciones", typeof(string));
    
            var idInspectoresParameter = idInspectores != null ?
                new ObjectParameter("idInspectores", idInspectores) :
                new ObjectParameter("idInspectores", typeof(string));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetConsultaeImpresionDeBoletasData_Result>("GetConsultaeImpresionDeBoletasData", fechaDesdeParameter, fechaHastaParameter, idDelegacionesParameter, idInspectoresParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetReimpresionDeBoletasDeCampoData_Result> GetReimpresionDeBoletasDeCampoData(string serieBoleta, string numeroBoleta, string usuarioSistema)
        {
            var serieBoletaParameter = serieBoleta != null ?
                new ObjectParameter("serieBoleta", serieBoleta) :
                new ObjectParameter("serieBoleta", typeof(string));
    
            var numeroBoletaParameter = numeroBoleta != null ?
                new ObjectParameter("numeroBoleta", numeroBoleta) :
                new ObjectParameter("numeroBoleta", typeof(string));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReimpresionDeBoletasDeCampoData_Result>("GetReimpresionDeBoletasDeCampoData", serieBoletaParameter, numeroBoletaParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetReportePorUsuarioData_Result> GetReportePorUsuarioData(string idUsuario, Nullable<System.DateTime> fechaInicial, Nullable<System.DateTime> fechaFinal, string usuarioSistema)
        {
            var idUsuarioParameter = idUsuario != null ?
                new ObjectParameter("IdUsuario", idUsuario) :
                new ObjectParameter("IdUsuario", typeof(string));
    
            var fechaInicialParameter = fechaInicial.HasValue ?
                new ObjectParameter("FechaInicial", fechaInicial) :
                new ObjectParameter("FechaInicial", typeof(System.DateTime));
    
            var fechaFinalParameter = fechaFinal.HasValue ?
                new ObjectParameter("FechaFinal", fechaFinal) :
                new ObjectParameter("FechaFinal", typeof(System.DateTime));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReportePorUsuarioData_Result>("GetReportePorUsuarioData", idUsuarioParameter, fechaInicialParameter, fechaFinalParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetDescargaParteOficialData_Result> GetDescargaParteOficialData(Nullable<System.DateTime> fechaDesde, Nullable<System.DateTime> fechaHasta, Nullable<int> valor, string idAutoridades, string idDelegaciones)
        {
            var fechaDesdeParameter = fechaDesde.HasValue ?
                new ObjectParameter("FechaDesde", fechaDesde) :
                new ObjectParameter("FechaDesde", typeof(System.DateTime));
    
            var fechaHastaParameter = fechaHasta.HasValue ?
                new ObjectParameter("FechaHasta", fechaHasta) :
                new ObjectParameter("FechaHasta", typeof(System.DateTime));
    
            var valorParameter = valor.HasValue ?
                new ObjectParameter("Valor", valor) :
                new ObjectParameter("Valor", typeof(int));
    
            var idAutoridadesParameter = idAutoridades != null ?
                new ObjectParameter("idAutoridades", idAutoridades) :
                new ObjectParameter("idAutoridades", typeof(string));
    
            var idDelegacionesParameter = idDelegaciones != null ?
                new ObjectParameter("idDelegaciones", idDelegaciones) :
                new ObjectParameter("idDelegaciones", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDescargaParteOficialData_Result>("GetDescargaParteOficialData", fechaDesdeParameter, fechaHastaParameter, valorParameter, idAutoridadesParameter, idDelegacionesParameter);
        }
    
        public virtual ObjectResult<GetReporteStatusActualPlanoData_Result> GetReporteStatusActualPlanoData(Nullable<int> statusPlano, Nullable<System.DateTime> fechaInicial, Nullable<System.DateTime> fechaFinal, string delegacion, string autoridad, string usuarioSistema)
        {
            var statusPlanoParameter = statusPlano.HasValue ?
                new ObjectParameter("StatusPlano", statusPlano) :
                new ObjectParameter("StatusPlano", typeof(int));
    
            var fechaInicialParameter = fechaInicial.HasValue ?
                new ObjectParameter("FechaInicial", fechaInicial) :
                new ObjectParameter("FechaInicial", typeof(System.DateTime));
    
            var fechaFinalParameter = fechaFinal.HasValue ?
                new ObjectParameter("FechaFinal", fechaFinal) :
                new ObjectParameter("FechaFinal", typeof(System.DateTime));
    
            var delegacionParameter = delegacion != null ?
                new ObjectParameter("Delegacion", delegacion) :
                new ObjectParameter("Delegacion", typeof(string));
    
            var autoridadParameter = autoridad != null ?
                new ObjectParameter("Autoridad", autoridad) :
                new ObjectParameter("Autoridad", typeof(string));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReporteStatusActualPlanoData_Result>("GetReporteStatusActualPlanoData", statusPlanoParameter, fechaInicialParameter, fechaFinalParameter, delegacionParameter, autoridadParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetActividadOficialData_Result> GetActividadOficialData(string inspector, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string usuarioSistema)
        {
            var inspectorParameter = inspector != null ?
                new ObjectParameter("Inspector", inspector) :
                new ObjectParameter("Inspector", typeof(string));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetActividadOficialData_Result>("GetActividadOficialData", inspectorParameter, fechaInicioParameter, fechaFinParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetReportePorDelegacionAutoridadData_Result> GetReportePorDelegacionAutoridadData(Nullable<int> tipoConsulta, Nullable<System.DateTime> fechaInicio, Nullable<System.DateTime> fechaFin, string idAutoridad, string idDelegacion, string usuarioSistema)
        {
            var tipoConsultaParameter = tipoConsulta.HasValue ?
                new ObjectParameter("TipoConsulta", tipoConsulta) :
                new ObjectParameter("TipoConsulta", typeof(int));
    
            var fechaInicioParameter = fechaInicio.HasValue ?
                new ObjectParameter("FechaInicio", fechaInicio) :
                new ObjectParameter("FechaInicio", typeof(System.DateTime));
    
            var fechaFinParameter = fechaFin.HasValue ?
                new ObjectParameter("FechaFin", fechaFin) :
                new ObjectParameter("FechaFin", typeof(System.DateTime));
    
            var idAutoridadParameter = idAutoridad != null ?
                new ObjectParameter("IdAutoridad", idAutoridad) :
                new ObjectParameter("IdAutoridad", typeof(string));
    
            var idDelegacionParameter = idDelegacion != null ?
                new ObjectParameter("IdDelegacion", idDelegacion) :
                new ObjectParameter("IdDelegacion", typeof(string));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReportePorDelegacionAutoridadData_Result>("GetReportePorDelegacionAutoridadData", tipoConsultaParameter, fechaInicioParameter, fechaFinParameter, idAutoridadParameter, idDelegacionParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetReporteListadoParteOficialData_Result> GetReporteListadoParteOficialData(Nullable<System.DateTime> fechaDesde, Nullable<System.DateTime> fechaHasta, Nullable<int> valor, string idInspectores, string idDelegaciones, string usuarioSistema)
        {
            var fechaDesdeParameter = fechaDesde.HasValue ?
                new ObjectParameter("FechaDesde", fechaDesde) :
                new ObjectParameter("FechaDesde", typeof(System.DateTime));
    
            var fechaHastaParameter = fechaHasta.HasValue ?
                new ObjectParameter("FechaHasta", fechaHasta) :
                new ObjectParameter("FechaHasta", typeof(System.DateTime));
    
            var valorParameter = valor.HasValue ?
                new ObjectParameter("Valor", valor) :
                new ObjectParameter("Valor", typeof(int));
    
            var idInspectoresParameter = idInspectores != null ?
                new ObjectParameter("idInspectores", idInspectores) :
                new ObjectParameter("idInspectores", typeof(string));
    
            var idDelegacionesParameter = idDelegaciones != null ?
                new ObjectParameter("idDelegaciones", idDelegaciones) :
                new ObjectParameter("idDelegaciones", typeof(string));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReporteListadoParteOficialData_Result>("GetReporteListadoParteOficialData", fechaDesdeParameter, fechaHastaParameter, valorParameter, idInspectoresParameter, idDelegacionesParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetReporteListadoMultaFijaData_Result> GetReporteListadoMultaFijaData(Nullable<System.DateTime> fechaDesde, Nullable<System.DateTime> fechaHasta, Nullable<int> valor, string idInspectores, string idDelegaciones, string usuarioSistema)
        {
            var fechaDesdeParameter = fechaDesde.HasValue ?
                new ObjectParameter("FechaDesde", fechaDesde) :
                new ObjectParameter("FechaDesde", typeof(System.DateTime));
    
            var fechaHastaParameter = fechaHasta.HasValue ?
                new ObjectParameter("FechaHasta", fechaHasta) :
                new ObjectParameter("FechaHasta", typeof(System.DateTime));
    
            var valorParameter = valor.HasValue ?
                new ObjectParameter("Valor", valor) :
                new ObjectParameter("Valor", typeof(int));
    
            var idInspectoresParameter = idInspectores != null ?
                new ObjectParameter("idInspectores", idInspectores) :
                new ObjectParameter("idInspectores", typeof(string));
    
            var idDelegacionesParameter = idDelegaciones != null ?
                new ObjectParameter("idDelegaciones", idDelegaciones) :
                new ObjectParameter("idDelegaciones", typeof(string));
    
            var usuarioSistemaParameter = usuarioSistema != null ?
                new ObjectParameter("UsuarioSistema", usuarioSistema) :
                new ObjectParameter("UsuarioSistema", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetReporteListadoMultaFijaData_Result>("GetReporteListadoMultaFijaData", fechaDesdeParameter, fechaHastaParameter, valorParameter, idInspectoresParameter, idDelegacionesParameter, usuarioSistemaParameter);
        }
    
        public virtual ObjectResult<GetBitacoraDeAplicacion_Result> GetBitacoraDeAplicacion(string tipoConsulta1, string tipoConsulta2, Nullable<System.DateTime> fechaInicial, Nullable<System.DateTime> fechaFinal, string idUsuario)
        {
            var tipoConsulta1Parameter = tipoConsulta1 != null ?
                new ObjectParameter("TipoConsulta1", tipoConsulta1) :
                new ObjectParameter("TipoConsulta1", typeof(string));
    
            var tipoConsulta2Parameter = tipoConsulta2 != null ?
                new ObjectParameter("TipoConsulta2", tipoConsulta2) :
                new ObjectParameter("TipoConsulta2", typeof(string));
    
            var fechaInicialParameter = fechaInicial.HasValue ?
                new ObjectParameter("FechaInicial", fechaInicial) :
                new ObjectParameter("FechaInicial", typeof(System.DateTime));
    
            var fechaFinalParameter = fechaFinal.HasValue ?
                new ObjectParameter("FechaFinal", fechaFinal) :
                new ObjectParameter("FechaFinal", typeof(System.DateTime));
    
            var idUsuarioParameter = idUsuario != null ?
                new ObjectParameter("IdUsuario", idUsuario) :
                new ObjectParameter("IdUsuario", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetBitacoraDeAplicacion_Result>("GetBitacoraDeAplicacion", tipoConsulta1Parameter, tipoConsulta2Parameter, fechaInicialParameter, fechaFinalParameter, idUsuarioParameter);
        }
    }
}
