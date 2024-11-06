using Core.DTOs;
using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class ContratoDetalle
    {
        public List<Contrato> Contratos { get; set; }
        public List<DocumentoContratoDto> DocumentosContrato { get; set; }
        public List<PrvDocumentoDocReqProveedor> DocsReqProveedor { get; set; }
        public List<DocReqProveedorOtroDto> DocsReqProveedorOtros { get; set; }
        public List<DocReqPolizaDto> DocsReqPoliza { get; set; }
        public List<AprobadoresContrato> Aprobadores { get; set; }
        public List<DocumentosRegistroProveedor> DocumentosRegistroProveedor { get; set; }
        public List<SeguimientosContratoDto> SeguimientosContrato { get; set; }
        public List<ContratoHistorico>? HistoricosContrato { get; set; }
        public List<HistoricosProrroga> HistoricosProrrogas { get; set; }
    }
}