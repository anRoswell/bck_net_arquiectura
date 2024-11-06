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
    public interface IRequerimientosRepository
    {
        Task<List<Requerimientos>> GetRequerimientos();
        Task<List<Requerimientos>> GetFiltroRequerimiento( int Id,int Estado,int Empresa, DateTime FechaInicio,DateTime FechaFin, int Categoria);        
        Task<List<Empresa>> GetEmpresas(string CodUsuario);
        Task<List<PrvProdServ>> GetProductosServicios();
        Task<List<ResponseAction>> PostCrear(RequerimientoDto requerimientos, List<ReqArtSerRequeridosDoc> documentos);
        Task<List<ResponseAction>> PutActualizar(RequerimientoDto requerimientos, List<ReqArtSerRequeridosDoc> documentos);
        Task<List<ResponseAction>> PutActualizarEstado(QueryUpdateEstadoReq updateEstadoReq);
        Task<List<Requerimientos>> GetRequerimientoPorID(int Id,int idProve);
        Task<List<ReqArtSerRequeridos>> GetArticulosPorIdReq(int Id, int idProve, bool IsNotIdUsuario);
        Task<List<ReqCriterosEvaluacion>> GetCriteriosPorIdReq(int Id, int idProve);
        Task<List<ReqPolizasSeguro>> GetPolizasPorIdReq(int Id);
        Task<List<ReqListDocumentos>> GetDocumentosPorIdReq(QuerySearchRequerimientos parametros);
        Task<List<ReqRtaCriteriosEvaluacion>> GetRtaCriteriosPorIdReq(int Id);
        Task<List<ReqCriterosEvaluacionRangoRta>> GetRtaRangoCriteriosPorIdReq(int Id);
        Task<List<Documento>> GetDocumentos();
        Task<List<Estados>> GetEstadosRequerimiento();
        Task<List<UnidadMedida>> GetUnidadMedida();     
        Task<List<ReqTipoDocArtSerRequerido>> GetReqTipoDocArtSerRequerido();
        Task<List<ReqQuestionAnswer>> GetComentarios(int reqIdRequerimientos);
        Task<List<ResponseAction>> PostNewComentario(ReqQuestionAnswer reqQuestionAnswer);
        Task<List<ResponseAction>> PostParticipar(ParamParticipacionReq parameters);
        Task<List<ResponseAction>> PutParticipar(ParamParticipacionReq parameters);
        Task<List<Requerimientos>> GetRequerimientosAdjudicados(int user);
        Task<int> GetCantidadParticipantesRequerimiento(int id);
        Task<List<RequerimientoComparativoDto>> GetComparativoParticipantesRequerimiento(QuerySearchComparativaReq parametros, PathOptions pathOptions);
        Task<List<ResponseAction>> UpdateItemValidadoComparativa(QueryUpdateItemComparativa comparativa);
        Task<List<Requerimientos>> GetRequerimientosListosParaAdjudicar();
        Task<List<RequerimientoAdjudicacionDto>> GetAdjudicacionParticipantesRequerimiento(QuerySearchComparativaReq parametros, PathOptions pathOptions);
        Task<List<ParticipacionDataReporteDto>> GetDataParticipanteRequerimiento(QuerySearchDataParticipacionReq parametros);
        Task<List<ResponseAction>> ActualizarUrlPdf(int? id, string url);
        Task<List<PathArchivosArtSerReq>> GetDocumentosTipoArchivoReq(int IdReq);
        Task<List<ArticulosRequerimiento>> GetArticulosParaRequerimiento(QuerySearchSolicitudesApoteosys parametros);
        Task<List<AdjudicacionOrdenes>> GetAdjudicacionRequerimientoOrden(int idRequerimiento);
        Task<List<ResponseAction>> CreateAdjudicarRequerimiento(QueryAdjudicarReq parametersAdj);
        Task<List<ResponseAction>> UpdateAdjudicarRequerimiento(QueryAdjudicarReq parametersAdj);
        Task<List<ResponseAction>> AsociarOrdenesRequerimiento(QueryAsociarOrdenesReq parametros);
    }
}
