using Core.CustomEntities;
using Core.CustomEntities.Parametros;
using Core.DTOs;
using Core.Entities;
using Core.ModelResponse;
using Core.Options;
using Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRequerimientosService
    {
        Task<ParametrosRequerimientos> GetParametrosRequerimientos(QueryParametersRequerimientos parameters);
        Task<ParamsInicialesReqPrv> GetParametrosRequerimientosPrv(QueryParametersRequerimientos parameters);
        Task<List<Requerimientos>> GetRequerimientos();
        Task<List<Requerimientos>> GetRequerimientosAdjudicados(int user);
        Task<List<Requerimientos>> GetFiltroRequerimiento(int Id, int Estado, int Empresa, DateTime FechaInicio, DateTime FechaFin, int Categoria);
        Task<List<ResponseAction>> PostCrear(RequerimientoDto requerimientos, List<ReqArtSerRequeridosDoc> documentos);
        Task<List<ResponseAction>> PutActualizar(RequerimientoDto requerimientos, List<ReqArtSerRequeridosDoc> documentos);
        Task<List<ResponseAction>> PutActualizarEstado(QueryUpdateEstadoReq updateEstadoReq);
        Task<RequerimientoDetalle> GetRequerimientoDetallePorID(QuerySearchRequerimientos parametros);
        Task<List<Requerimientos>> GetRequerimientoPorID(int Id, int idProve);
        Task<List<ResponseAction>> PostNewComentario(ReqQuestionAnswer reqQuestionAnswer);
        Task<List<ResponseAction>> PostParticipar(ParamParticipacionReq parameters);
        Task<List<ResponseAction>> PutParticipar(ParamParticipacionReq parameters);
        Task<List<Proveedores>> GetParticipantesRequerimiento(int id);
        Task<int> GetCantidadParticipantesRequerimiento(int id);
        Task<List<RequerimientoComparativoDto>> GetRequerimientoComparativo(QuerySearchComparativaReq parametros, PathOptions pathOptions);
        Task<List<ResponseAction>> UpdateItemValidadoComparativa(QueryUpdateItemComparativa comparativa);
        Task<List<RequerimientoAdjudicacionDto>> GetRequerimientoAdjudicacion(QuerySearchComparativaReq parametros, PathOptions pathOptions);
        Task<List<PathArchivosArtSerReq>> GetDocumentosTipoArchivoReq(int IdReq);
        Task<List<ArticulosRequerimiento>> GetArticulosParaRequerimiento(QuerySearchSolicitudesApoteosys parametros);
        Task<List<AdjudicacionOrdenes>> GetAdjudicacionRequerimientoOrden(int idRequerimiento);
        Task<List<ResponseAction>> CreateAdjudicarRequerimiento(QueryAdjudicarReq parametersAdj);
        Task<List<ResponseAction>> UpdateAdjudicarRequerimiento(QueryAdjudicarReq parametersAdj);
        Task<List<ResponseAction>> AsociarOrdenesRequerimiento(QueryAsociarOrdenesReq parametros);
    }
}
