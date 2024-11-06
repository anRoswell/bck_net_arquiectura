using Core.CustomEntities;
using Core.Entities;
using Core.HubConfig;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITerceroRepository
    {
        Task<List<TerceroDto>> GetTerceros(int idPathFileServer);

        Task<List<TerceroDto>> GetTerceroPorNit(string nit, int idPathFileServer);

        Task<List<TerceroDto>> GetTerceroPorID(int id, int idPathFileServer);

        Task<List<ResponseAction>> SearchByCodValidation(QuerySearchByCodValidation data);

        Task<List<TerceroDto>> GetFiltroTercero(string idLocalidad, int idCategoriaServicio, int idEstado, string razonSocial, string prvNit, int idPathFileServer);

        Task<List<Requerimientos>> GetRequerimientosTercero(int idProveedor);

        Task<List<ResponseAction>> ActualizarUrlPdf(int? id, string url);

        Task<List<ResponseActionUrl>> PostCrear(TerceroCrearDto proveedor, List<PrvDocumento> documentos, string Generatedpassword, SignalParams<ProveedorHub, DataProveedores> signalParam);

        Task<List<ResponseActionUrl>> PutActualizar(TerceroActualizarDto proveedor, List<PrvDocumento> documentos, List<PrvDocumento> listDocumentsOthers, SignalParams<ProveedorHub, DataProveedores> signalParam, int idPathFileServer);

        Task<List<ResponseAction>> PutUpdateEstadoDocument(QueryUpdateEstadoDocument idDocument);

        Task<List<ResponseAction>> PutTerceroAprobar(QueryUpdateEstadoPrv updEstPrv);

        Task<List<ResponseAction>> PutUpdateTrazaInspect(QueryUpdateTrazaInspect data);

        Task<List<ProveedorInspektor>> ValidateBlackList(string numIdenti, string nombre);

        Task<List<Tercero>> GetParticipantesRequerimiento(int id);

        Task<List<ResponseAction>> AprobarDesaprobarInspektor(QueryAprobarDesaprobarInspektor data);

        Task<List<ResponseAction>> ReenviarNotificacionCodigoValidacion(QueryReenvioNotificacionCodigoValidacion data);

        Task<List<ResponseAction>> CambiarCorreoTercero(QueryCambiarCorreoProveedor data);
    }
}