
namespace Core.Interfaces
{
    using Core.CustomEntities;
    using Core.DTOs;
    using Core.Entities;
    using Core.ModelResponse;
    using Core.QueryFilters;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IContratoService 
    {
        Task<List<Contrato>> SearchAll();
        Task<List<Contrato>> SearchById(QueryContrato par);
        Task<List<ResponseAction>> Post(ContratoDto contrato);
        Task<List<ResponseAction>> Update(ContratoDto contrato);
        Task<List<ResponseAction>> UpdateEstado(QueryContrato par);
        Task<ContratoDetalle> GetContratoDetallePorID(int id);
        Task<List<ResponseAction>> EliminarDocContrato(int id);
        Task<ParametrosContrato> GetParametrosContrato();
        Task<List<ResponseAction>> AprobacionContrato(QueryAprobacionContrato par);
        Task<List<TimelineDto>> GetTimelineContratoById(int id);
        Task<List<ResponseAction>> AsociarDocumentosProveedor(DocumentosProveedorDto documentos);
        Task<List<ResponseAction>> HabilitarContrato_FirmaElectronica(QueryHabilitarContrato parameters);
        Task<List<ResponseAction>> UpdateUrlContrato(QueryUpdateUrlContrato par);
        Task<List<ResponseAction>> UpdateUrlContrato_CargadoManual(QueryUpdateUrlContrato par);
        Task<string> ValidarArchivoContrato(int idContrato);
        Task<List<ResponseAction>> SolicitarDocumentosProveedor(QuerySolicitarDocumentosProveedor parameters);
        Task<ParametrosContrato> SearchByProveedor(int idUsuario);
        Task<List<ResponseAction>> RechazarDocumentosProveedor(QueryRechazarDocumentosProveedor parameters);
        Task<List<ResponseAction>> GuardarSeguimiento(SeguimientosContratoDto parameters);
        Task<List<ResponseAction>> GuardarNotificacionNoProrroga(ContratoNotificacionNoProrroga parameters);
        Task<List<ResponseAction>> GuardarNotificacionTerminacionAnticipada(ContratoNotificacionTerminacion parameters);
        Task<List<ResponseAction>> AprobarProrroga(QueryAprobarProrroga parameters);
        Task<List<ResponseAction>> AsociarPolizasRenovadas(QueryContratoPolizasRenovadas parameters);
        Task<List<ResponseAction>> GuardarActaLiquidacion(ContratoActaLiquidacion parameters);
        Task<List<ResponseAction>> SolicitarModificacionContrato(QuerySolicitudModificacion par);
        Task<ParametrosHistoricos> GetParametrosHistoricos();
        Task<List<ResponseAction>> GuardarComentarioContrato(CommandCreateComentarioContrato command);
        Task<List<ContratoComentarios>> ConsultarComentariosContrato(int IdContrato);
        Task<List<ResponseAction>> CambiarEstadoContrato(CommandUpdateEstadoContrato command);
        //Task<List<ContratoListado>> ListadoContratos(QueryListadoContratosReporte query);
        //Task<byte[]> ListadoContratosExcel(QueryListadoContratosReporte query);
        //Task<byte[]> ListadoContratosHistoricoExcel(QueryListadoContratosReporte query);
        Task<List<ResponseAction>> AprobacionAdministrador(CommandUpdateEstadoContrato command);
    }
}
