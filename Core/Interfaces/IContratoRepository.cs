
namespace Core.Interfaces
{
    using Core.CustomEntities;
    using Core.DTOs;
    using Core.Entities;
    using Core.ModelResponse;
    using Core.QueryFilters;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IContratoRepository : IRepository<Contrato>
    {
        Task<List<Contrato>> SearchAll();
        Task<List<Contrato>> SearchById(int id);
        Task<List<ResponseAction>> Post(ContratoDto contrato);
        Task<List<ResponseAction>> Update(ContratoDto contrato);
        Task<List<ResponseAction>> UpdateEstado(QueryContrato par);
        Task<List<DocumentoContrato>> GetDocumentoContratoById(int id);
        Task<List<PrvDocumentoDocReqProveedor>> GetDocReqProveedorById(int id);
        Task<List<DocReqPoliza>> GetDocReqPolizaById(int id);
        Task<List<ResponseAction>> EliminarDocContrato(int id);
        Task<ParametrosContrato> GetParametrosContrato();
        Task<List<AprobadoresContrato>> GetAbrobadoresContratoById(int id);
        Task<List<ResponseAction>> AprobacionContrato(QueryAprobacionContrato par);
        Task<List<TimelineDto>> GetTimelineContratoById(int id);
        Task<ContratoDetalle> GetContratoDetallePorID(int id);
        Task<List<ResponseAction>> AsociarDocumentosProveedor(DocumentosProveedorDto documentos);
        Task<List<ResponseAction>> UpdateUrlContrato(QueryUpdateUrlContrato par);
        Task<List<ResponseAction>> UpdateUrlContrato_CargadoManual(QueryUpdateUrlContrato par);
        Task<List<ResponseAction>> SolicitarDocumentosProveedor(QuerySolicitarDocumentosProveedor par);
        Task<ParametrosContrato> SearchByProveedor(int idUsuario);
        Task<List<ResponseAction>> RechazarDocumentosProveedor(QueryRechazarDocumentosProveedor par);
        Task<List<ResponseAction>> GuardarSeguimiento(SeguimientosContratoDto par);
        Task<List<ResponseAction>> GuardarNotificacionNoProrroga(ContratoNotificacionNoProrroga par);
        Task<List<ResponseAction>> GuardarNotificacionTerminacionAnticipada(ContratoNotificacionTerminacion par);
        Task<List<ResponseAction>> AprobarProrroga(QueryAprobarProrroga par);
        Task<List<ResponseAction>> AsociarPolizasRenovadas(QueryContratoPolizasRenovadas par);
        Task<List<ResponseAction>> GuardarActaLiquidacion(ContratoActaLiquidacion par);
        Task<List<ResponseAction>> SolicitarModificacionContrato(QuerySolicitudModificacion par);
        Task<ParametrosHistoricos> GetParametrosHistoricos();
        Task<List<ResponseAction>> GuardarComentario(ContQuestionAnswer entity);
        Task<List<ContratoComentarios>> ConsultarComentariosPorContratoId(int IdContrato);
        Task<List<ResponseAction>> CambiarEstadoContrato(CommandUpdateEstadoContrato command);
        Task<List<ContratoListado>> ListadoContratosReporte(QueryListadoContratosReporte query);
        Task<List<ContratoListado>> ListadoContratosHistoricosReporte(QueryListadoContratosReporte query);
        Task<List<ResponseAction>> AprobacionAdministrador(CommandUpdateEstadoContrato command);
    }
}
