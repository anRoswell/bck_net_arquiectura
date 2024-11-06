using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class Contrato : BaseEntity
    {
        public Contrato()
        {
            ContratoHistoricos = new HashSet<ContratoHistorico>();
        }

        public int? ContCodEmpresa { get; set; }
        public int? ContCodProveedor { get; set; }
        public int? ContCodRequerimiento { get; set; }
        public string ContNombreRteLegalCtante { get; set; }
        public string ContCcrepresentanteCtante { get; set; }
        public string ContEmailRteLegalCtante { get; set; }
        public string ContDireccionNotificacionCtante { get; set; }
        public string ContTelefonoCtante { get; set; }
        public int? ContCodGestorContrato { get; set; }
        public int? ContCodCoordinadorContrato { get; set; }
        public int? ContCodGestorRiesgo { get; set; }
        public int? ContCodUnidadNegocio { get; set; }
        public int? ContCodClaseContrato { get; set; }
        public int? ContAprobFinanciera { get; set; }
        public int? ContAprobCompras { get; set; }
        public int? ContAprobArea { get; set; }
        public string ContObjetoContrato { get; set; }
        public string ContCarateristicasEspecificas { get; set; }
        public string? ContDuracionContrato { get; set; }
        public DateTime? ContVigenciaDesde { get; set; }
        public DateTime? ContVigenciaHasta { get; set; }
        public decimal? ContValorContrato { get; set; }
        public string? ContCodFormaPago { get; set; }
        public bool? ContRequierenAnticipos { get; set; }
        public decimal? ContValorAnticipo { get; set; }
        public bool? ContRequiereIngresoPersonal { get; set; }
        public bool? ContPresupuestado { get; set; }
        public int? ContCodTipoProrroga { get; set; }
        public int? ContDuracionProrroga { get; set; }
        public int? ContPreavisoProrrogaDias { get; set; }
        public bool? ContRequiereActaInicio { get; set; }
        public bool? ContRequiereActaLiquidacion { get; set; }
        public DateTime? ContFechaLiquidacionEsperada { get; set; }
        public DateTime? ContFechaLiquidacionReal { get; set; }
        public int? ContTipoDocumento { get; set; }
        public int? ContCodEstado { get; set; }
        public string ContUrlPdf { get; set; }
        public string ContUrlAbsolutePdf { get; set; }
        public string CodUser { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime? FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public string ContObservacion { get; set; }
        public string ContContactoContratista { get; set; }
        public string ContEmailContactoContratista { get; set; }
        public string ContNitContratista { get; set; }
        public int? ContCodRequisitor { get; set; }
        public int? ContConsecutivoFlujo { get; set; }
        public string ContJustificacionRechazo { get; set; }
        public int? ContArchivoCompraNoPresupuestada { get; set; }
        public string ContEmailContratante { get; set; }
        public int? ContArchivoActaInicio { get; set; }
        public DateTime? ContFechaInicioProrroga { get; set; }
        public DateTime? ContFechaNotificacionNoProrroga { get; set; }
        public int? CodArchivoNotificacionNoProrroga { get; set; }
        public string ContObservacionNoProrroga { get; set; }
        public DateTime? ContFechaNotifTermAnticipada { get; set; }
        public int? CodArchivoNotificacionTerminacion { get; set; }
        public string ContObservacionTermAnticipada { get; set; }
        public bool? ContAprobacionProrroga { get; set; }
        public int? CodArchivoActaLiquidacion { get; set; }
        public int? ContPolizasRenovadas { get; set; }
        public string ContConsecutivoAlterno { get; set; }
        public int? ContOtroSiActual { get; set; }
        public int? ContCodContratoHistoricoActual { get; set; }
        public DateTime? ContFechaActaInicio { get; set; }
        public int? ContCodTipoContrato { get; set; }
        public int? contCodRepresentanteLegal { get; set; }
    }

    public partial class Contrato
    {
        public string ClaseContrato { get; set; }
        public string ProveedorContrato { get; set; }
        public string EmpresaContrato { get; set; }
        public bool? AprobadoPorCoordinador { get; set; }
        public bool? AprobadoPorFinanciero { get; set; }
        public bool? AprobadoPorCompras { get; set; }
        public bool? AprobadoPorArea { get; set; }
        public string UrlArchivoCompraNoPresupuestada { get; set; }
        public string NombreArchivoCompraNoPresupuestada { get; set; }
        public string UrlArchivoActaInicio { get; set; }
        public string NombreArchivoActaInicio { get; set; }
        public string UrlArchivoNotificacionNoProrroga { get; set; }
        public string NombreArchivoNotificacionNoProrroga { get; set; }
        public string UrlArchivoNotificacionTerminacion { get; set; }
        public string NombreArchivoNotificacionTerminacion { get; set; }
        public string UrlArchivoActaLiquidacion { get; set; }
        public string NombreArchivoActaLiquidacion { get; set; }
        public virtual ContratoHistorico ContratoHistoricoActual { get; set; }
        public virtual ICollection<ContratoHistorico> ContratoHistoricos { get; set; }
        public virtual ClaseContrato ContClaseContrato { get; set; }
        public virtual Proveedores ContProveedor { get; set; }
    }
}
