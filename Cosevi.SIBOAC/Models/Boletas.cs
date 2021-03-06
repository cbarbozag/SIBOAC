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

    public partial class Boletas
    {
        public string fuente { get; set; }
        [DisplayName("Serie")]
        [Range(0, int.MaxValue, ErrorMessage = "Solo se permiten n�meros")]
        public int serie { get; set; }
        [DisplayName("N�mero Boleta")]
        public decimal numero_boleta { get; set; }
        public string codigo_delito { get; set; }
        public string tipo_documento { get; set; }
        public string tipo_ide { get; set; }
        public string identificacion { get; set; }
        public string tipo_lic { get; set; }
        public Nullable<System.DateTime> fecha_hora_boleta { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
        public string alcoholemia_realizada { get; set; }
        public string num_alcohosensor { get; set; }
        public string num_prueba_alcohol { get; set; }
        public string foto_tomada { get; set; }
        public string plano_generado { get; set; }
        public string licencia_confiscada { get; set; }
        public string placa_confiscada { get; set; }
        public string auto_detenido { get; set; }
        public string conductor_detenido { get; set; }
        public string conductor_ausente { get; set; }
        public string firmo { get; set; }
        public string parte_generado { get; set; }
        public string fuente_parteoficial { get; set; }
        public string serie_parteoficial { get; set; }
        public string numeroparte { get; set; }
        public string usuario_registra { get; set; }
        public string lugar_hechos { get; set; }
        public string observaciones { get; set; }
        public string consecutivo { get; set; }
        public string clase_placa { get; set; }
        public string codigo_placa { get; set; }
        public string numero_placa { get; set; }
        public string marca { get; set; }
        public Nullable<System.DateTime> fecha_recibido { get; set; }
        public string codigo_inspector { get; set; }
        public string codigo_delegacion { get; set; }
        public string codigo_autoridad_registra { get; set; }
        public string estado { get; set; }
        public Nullable<decimal> multa { get; set; }
        public string autoridad_admin { get; set; }
        public string codigo_colision { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string nivel_alcohol { get; set; }
        public int cod_provincia { get; set; }
        public int cod_canton { get; set; }
        public int cod_distrito { get; set; }
        public string fuente_libro { get; set; }
        public Nullable<int> periodo_libro { get; set; }
        public Nullable<short> codigo_libro { get; set; }
        public Nullable<int> codigo_ruta { get; set; }
        public Nullable<int> calle { get; set; }
        public Nullable<int> avenida { get; set; }
        public string nacionalidad { get; set; }
        public string tipo_notificacion { get; set; }
        public Nullable<short> cantp_metal { get; set; }
        public Nullable<short> cantp_papel { get; set; }
        public Nullable<short> cantpermiso { get; set; }
        public string tipoidevehiculo { get; set; }
        public string otrotipovehic { get; set; }
        public string codrol { get; set; }
        public string vehiculo_fuga { get; set; }
        public byte[] firma_inspector { get; set; }
        public byte[] firma_conductor { get; set; }
        public string humo { get; set; }
        public Nullable<int> velocidad { get; set; }
        public Nullable<System.DateTime> fecha_descarga { get; set; }
        public string numeroHH { get; set; }
        public Nullable<System.DateTime> fechaexportacionHH { get; set; }
        public string TempXY { get; set; }
        public Nullable<decimal> Relacionado_ParteOficial { get; set; }
        public string codOficinaImpugnacion { get; set; }
        public string codVehiculoDepositado { get; set; }
        public string codigoDepositoPlaca { get; set; }
        public string numero_placa_otrovehiculo { get; set; }
        public string numeroSerie { get; set; }
        public Nullable<int> Tipo_Boleta { get; set; }
        public string VersionAplicacion { get; set; }
        public Nullable<int> calle2 { get; set; }
        public Nullable<int> avenida2 { get; set; }
        public Nullable<int> kilometro { get; set; }
        public Nullable<bool> por_denuncia { get; set; }
        public Nullable<bool> NoGuardaCoordenadas { get; set; }
        public string articulos_sugeridos_justificacion { get; set; }
        public Nullable<int> Tipo_Examen { get; set; }
        public Nullable<int> Tipo_Conductor { get; set; }
    }
}
