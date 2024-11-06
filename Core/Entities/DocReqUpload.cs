using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class DocReqUpload : BaseEntity
    {
        public DocReqUpload()
        {
            DocReqPolizaHistoricos = new HashSet<DocReqPolizaHistorico>();
            DocReqProveedorHistoricos = new HashSet<DocReqProveedorHistorico>();
            DocReqProveedorOtrosHistoricos = new HashSet<DocReqProveedorOtrosHistorico>();
            DocumentoContratoHistoricos = new HashSet<DocumentoContratoHistorico>();
            ContratoHistoricoCodArchivoActaLiquidacion = new HashSet<ContratoHistorico>();
            ContratoHistoricoCodArchivoNotificacionNoProrroga = new HashSet<ContratoHistorico>();
            ContratoHistoricoCodArchivoNotificacionTerminacion = new HashSet<ContratoHistorico>();
            ContratoHistoricoContArchivoActaInicio = new HashSet<ContratoHistorico>();
            ContratoHistoricoContArchivoCompraNoPresupuestada = new HashSet<ContratoHistorico>();
        }

        public string DrpuUrl { get; set; }
        public string DrcoNameDocument { get; set; }
        public string DrcoExtDocument { get; set; }
        public int? DrcoSizeDocument { get; set; }
        public string DrcoUrlDocument { get; set; }
        public string DrcoUrlRelDocument { get; set; }
        public string DrcoOriginalNameDocument { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }

        public virtual ICollection<ContratoHistorico> ContratoHistoricoCodArchivoActaLiquidacion { get; set; }
        public virtual ICollection<ContratoHistorico> ContratoHistoricoCodArchivoNotificacionNoProrroga { get; set; }
        public virtual ICollection<ContratoHistorico> ContratoHistoricoCodArchivoNotificacionTerminacion { get; set; }
        public virtual ICollection<ContratoHistorico> ContratoHistoricoContArchivoActaInicio { get; set; }
        public virtual ICollection<ContratoHistorico> ContratoHistoricoContArchivoCompraNoPresupuestada { get; set; }
        public virtual ICollection<DocReqPolizaHistorico> DocReqPolizaHistoricos { get; set; }
        public virtual ICollection<DocReqProveedorHistorico> DocReqProveedorHistoricos { get; set; }
        public virtual ICollection<DocReqProveedorOtrosHistorico> DocReqProveedorOtrosHistoricos { get; set; }
        public virtual ICollection<DocumentoContratoHistorico> DocumentoContratoHistoricos { get; set; }
    }
}