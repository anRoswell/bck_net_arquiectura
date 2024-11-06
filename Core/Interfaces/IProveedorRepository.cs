using Core.CustomEntities;
using Core.DTOs;
using Core.Entities;
using Core.HubConfig;
using Core.ModelResponse;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProveedorRepository
    {
        Task<List<Proveedores>> GetProveedores(int idPathFileServer);
        Task<List<Proveedores>> GetProveedorPorNit(string nit, int idPathFileServer);
        Task<List<Proveedores>> GetProveedorPorID(int id, int idPathFileServer);
        Task<List<ResponseAction>> SearchByCodValidation(QuerySearchByCodValidation data);
        Task<List<Proveedores>> GetFiltroProveedor(string idLocalidad, int idCategoriaServicio, int idEstado, string razonSocial, string nit, int idPathFileServer);
        Task<List<Requerimientos>> GetRequerimientosProveedor(int idProveedor);
        Task<List<ResponseAction>> ActualizarUrlPdf(int? id, string url);
        Task<List<ResponseActionUrl>> PostCrear(ProveedorDto proveedor, List<PrvDocumento> documentos, string Generatedpassword, SignalParams<ProveedorHub, DataProveedores> signalParam);
        Task<List<ResponseActionUrl>> PutActualizar(ProveedorDto proveedor, List<PrvDocumento> documentos, List<PrvDocumento> listDocumentsOthers, SignalParams<ProveedorHub, DataProveedores> signalParam, int idPathFileServer);
        Task<List<ResponseAction>> PutUpdateEstadoDocument(QueryUpdateEstadoDocument idDocument);
        Task<List<ResponseAction>> PutProveedorAprobar(QueryUpdateEstadoPrv updEstPrv);
        Task<List<ResponseAction>> PutUpdateTrazaInspect(QueryUpdateTrazaInspect data);
        Task<List<ProveedorInspektor>> ValidateBlackList(string numIdenti, string nombre);
        Task<List<Proveedores>> GetParticipantesRequerimiento(int id);
        Task<List<ResponseAction>> AprobarDesaprobarInspektor(QueryAprobarDesaprobarInspektor data);
        Task<List<ResponseAction>> ReenviarNotificacionCodigoValidacion(QueryReenvioNotificacionCodigoValidacion data);
        Task<List<ResponseAction>> CambiarCorreoProveedor(QueryCambiarCorreoProveedor data);
    }
}
