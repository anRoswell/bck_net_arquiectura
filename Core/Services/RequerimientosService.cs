using Core.CustomEntities;
using Core.CustomEntities.Parametros;
using Core.DTOs;
using Core.Entities;
using Core.Enumerations;
using Core.Interfaces;
using Core.ModelProcess;
using Core.ModelResponse;
using Core.Options;
using Core.QueryFilters;
using Core.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public class RequerimientosService : IRequerimientosService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationsService _notificationsService;

        public RequerimientosService(IUnitOfWork unitOfWork, INotificationsService notificationsService)
        {
            _unitOfWork = unitOfWork;
            _notificationsService = notificationsService;
        }

        public async Task<ParametrosRequerimientos> GetParametrosRequerimientos(QueryParametersRequerimientos parameters)
        {
            
            ParametrosRequerimientos paramsReq = new ParametrosRequerimientos
            {
                Empresas = await _unitOfWork.RequerimientosRepository.GetEmpresas(parameters.CodUser),
                ReqsListosParaAdjudicar = await GetRequerimientosListosParaAdjudicar(),
                ProductoServicios = await _unitOfWork.RequerimientosRepository.GetProductosServicios(),
                ListDocumentos = await _unitOfWork.RequerimientosRepository.GetDocumentos(),
                ListEstados = await _unitOfWork.RequerimientosRepository.GetEstadosRequerimiento(),
                ListUnidadMedida = await _unitOfWork.RequerimientosRepository.GetUnidadMedida(),
                ListTipoFicha = await _unitOfWork.RequerimientosRepository.GetReqTipoDocArtSerRequerido(),
                CantPrvValidacion = int.Parse(_unitOfWork.ParametrosInicialesRepository.GetCantidadProveedores_A_ValidarReq().Result?.FirstOrDefault().PgeValorParam),
                ListIva = await _unitOfWork.ParametrosInicialesRepository.GetSelectoresIva(),
                TipoCriterios = await _unitOfWork.ParametrosInicialesRepository.GetSelectorTipoCriterio(),
                TipoRequerimientos = await _unitOfWork.ParametrosInicialesRepository.GetTipoRequerimientos()
            };

            if (parameters.Action == "edit" || parameters.Action == "consultar")
            {
                QuerySearchRequerimientos parametros = new QuerySearchRequerimientos
                {
                    Id = parameters.Id,
                    CodProveedor = Convert.ToInt32(parameters.CodUser),
                    CodUser = parameters.CodUser
                };
                paramsReq.ListComentario = await _unitOfWork.RequerimientosRepository.GetComentarios(parameters.Id);
                paramsReq.Requerimientos = await GetRequerimientoDetallePorID(parametros);
            }

            return paramsReq;
        }

        private async Task<List<Requerimientos>> GetRequerimientosListosParaAdjudicar()
        {
            return await _unitOfWork.RequerimientosRepository.GetRequerimientosListosParaAdjudicar();
        }

        public async Task<ParamsInicialesReqPrv> GetParametrosRequerimientosPrv(QueryParametersRequerimientos parameters)
        {
            return _unitOfWork.ParametrosInicialesRepository.GetParametrosInicialesReqPrv().Result.FirstOrDefault();
        }

        public async Task<List<Requerimientos>> GetRequerimientos()
        {
            return await _unitOfWork.RequerimientosRepository.GetRequerimientos();
        }

        public async Task<List<Requerimientos>> GetRequerimientosAdjudicados(int user)
        {
            return await _unitOfWork.RequerimientosRepository.GetRequerimientosAdjudicados(user);
        }

        public async Task<List<Requerimientos>> GetFiltroRequerimiento(int Id, int Estado, int Empresa, DateTime FechaInicio, DateTime FechaFin, int Categoria)
        {
            return await _unitOfWork.RequerimientosRepository.GetFiltroRequerimiento(Id, Estado, Empresa, FechaInicio, FechaFin, Categoria);
        }
        
        public async Task<List<ResponseAction>> PostCrear(RequerimientoDto requerimientos, List<ReqArtSerRequeridosDoc> documentos)
        {
            var response = await _unitOfWork.RequerimientosRepository.PostCrear(requerimientos, documentos);

            if (response != null &&
                response.Any() &&
                response.FirstOrDefault() != null &&
                response.FirstOrDefault().estado)
            {
                await _notificationsService.PushSocket(
                    new DataNotifications
                    {
                        Method = HubConectionsMethods.Notification,
                        Action = HubConectionsActions.CreateRequerimiento
                    });
            }

            return response;
        }

        public async Task<List<ResponseAction>> PutActualizar(RequerimientoDto requerimientos, List<ReqArtSerRequeridosDoc> documentos)
        {
            var response = await _unitOfWork.RequerimientosRepository.PutActualizar(requerimientos, documentos);

            if (response != null &&
                response.Any() &&
                response.FirstOrDefault() != null &&
                response.FirstOrDefault().estado)
            {
                await _notificationsService.PushSocket(
                    new DataNotifications
                    {
                        Method = HubConectionsMethods.Notification,
                        Action = HubConectionsActions.UpdateRequerimiento
                    });
            }

            return response;
        }

        public async Task<List<ResponseAction>> PutActualizarEstado(QueryUpdateEstadoReq updateEstadoReq)
        {
            return await _unitOfWork.RequerimientosRepository.PutActualizarEstado(updateEstadoReq);
        }

        public async Task<RequerimientoDetalle> GetRequerimientoDetallePorID(QuerySearchRequerimientos parametros)
        {
            RequerimientoDetalle querySelect = new RequerimientoDetalle
            {
                Requerimientos = await GetRequerimientoPorID(parametros.Id, parametros.CodProveedor),       
                ReqArtSerRequeridos = await _unitOfWork.RequerimientosRepository.GetArticulosPorIdReq(parametros.Id, parametros.CodProveedor,parametros.IsNotIdUsuario),
                ReqCriteriosEvaluacion = await _unitOfWork.RequerimientosRepository.GetCriteriosPorIdReq(parametros.Id, parametros.CodProveedor),
                ReqPolizasSeguro = await _unitOfWork.RequerimientosRepository.GetPolizasPorIdReq(parametros.Id),
                ReqListDocumentos = await _unitOfWork.RequerimientosRepository.GetDocumentosPorIdReq(parametros),
                ReqRtaCriteriosEvaluacion = await _unitOfWork.RequerimientosRepository.GetRtaCriteriosPorIdReq(parametros.Id),
                ReqCriterosEvaluacionRangoRta = await _unitOfWork.RequerimientosRepository.GetRtaRangoCriteriosPorIdReq(parametros.Id),
                ReqListProveedoresParticipantes = await GetParticipantesRequerimiento(parametros.Id)
            };
            return querySelect;
        }

        public async Task<List<Requerimientos>> GetRequerimientoPorID(int Id,int idProve)
        {
            return await _unitOfWork.RequerimientosRepository.GetRequerimientoPorID(Id, idProve);
        }

        public async Task<List<ResponseAction>> PostNewComentario(ReqQuestionAnswer reqQuestionAnswer)
        {
            return await _unitOfWork.RequerimientosRepository.PostNewComentario(reqQuestionAnswer);
        }

        public async Task<List<ResponseAction>> PostParticipar(ParamParticipacionReq parameters)
        {
            var response = await _unitOfWork.RequerimientosRepository.PostParticipar(parameters);

            if (response[0].estado)
            {
                // Consultamos datos para reporte
                List<ParticipacionDataReporteDto> dataReporte = await _unitOfWork.RequerimientosRepository.GetDataParticipanteRequerimiento(
                    new QuerySearchDataParticipacionReq { IdRequerimiento = parameters.ReqParticipantesDto.CodRequerimento, IdProveedor = parameters.ReqParticipantesDto.CodProveedor }
                );

                // Armamos Paths
                string pathRel = Path.Combine(parameters.PathOptions.Folder_Archivos_ParticipacionReq, parameters.ReqParticipantesDto.CodRequerimento.ToString(), parameters.ReqParticipantesDto.CodUser);
                string pathFS = Path.Combine(parameters.PathOptions.Path_FileServer_root, pathRel);

                // Generamos documento PDF y lo guardamos en el File Server
                string html = RequerimientosProcess.MapearParticipacionPdf(dataReporte);
                string url = Funciones.PdfSharpConvertParticipacion(html, pathFS, parameters.ReqParticipantesDto.CodUser, pathRel);
                await ActualizarUrlPdf(response[0].Id, url);
            }

            return response;
        }

        public async Task<List<ResponseAction>> PutParticipar(ParamParticipacionReq parameters)
        {
            var response = await _unitOfWork.RequerimientosRepository.PutParticipar(parameters);

            if (response[0].estado)
            {
                // Consultamos datos para reporte
                List<ParticipacionDataReporteDto> dataReporte = await _unitOfWork.RequerimientosRepository.GetDataParticipanteRequerimiento(
                    new QuerySearchDataParticipacionReq { IdRequerimiento = parameters.ReqParticipantesDto.CodRequerimento, IdProveedor = parameters.ReqParticipantesDto.CodProveedor }
                );

                // Armamos Paths
                string pathRel = Path.Combine(parameters.PathOptions.Folder_Archivos_ParticipacionReq, parameters.ReqParticipantesDto.CodRequerimento.ToString(), parameters.ReqParticipantesDto.CodUser);
                string pathFS = Path.Combine(parameters.PathOptions.Path_FileServer_root, pathRel);

                // Eliminamos documento anterior
                string urlFileAnterior = dataReporte.FirstOrDefault().Participante.FirstOrDefault().UrlPdfParticipacion;

                if (urlFileAnterior != null) {
                    string pathFileAnterior = Path.Combine(parameters.PathOptions.Path_FileServer_root, urlFileAnterior);
                    //////parameters.FilesProcess.RemoveFile(pathFileAnterior);
                }

                // Generamos documento PDF y lo guardamos en el File Server
                string html = RequerimientosProcess.MapearParticipacionPdf(dataReporte);
                string url = Funciones.PdfSharpConvertParticipacion(html, pathFS, parameters.ReqParticipantesDto.CodUser, pathRel);
                await ActualizarUrlPdf(response[0].Id, url);
            }

            return response;
        }
        
        public async Task<List<Proveedores>> GetParticipantesRequerimiento(int id)
        {
            return await _unitOfWork.ProveedorRepository.GetParticipantesRequerimiento(id);
        }

        public async Task<int> GetCantidadParticipantesRequerimiento(int id)
        {
            return await _unitOfWork.RequerimientosRepository.GetCantidadParticipantesRequerimiento(id);
        }

        public async Task<List<RequerimientoComparativoDto>> GetRequerimientoComparativo(QuerySearchComparativaReq parametros, PathOptions pathOptions)
        {
            return await _unitOfWork.RequerimientosRepository.GetComparativoParticipantesRequerimiento(parametros, pathOptions);
        }

        public async Task<List<ResponseAction>> UpdateItemValidadoComparativa(QueryUpdateItemComparativa comparativa)
        {
            return await _unitOfWork.RequerimientosRepository.UpdateItemValidadoComparativa(comparativa);
        }

        public async Task<List<RequerimientoAdjudicacionDto>> GetRequerimientoAdjudicacion(QuerySearchComparativaReq parametros, PathOptions pathOptions)
        {
            return await _unitOfWork.RequerimientosRepository.GetAdjudicacionParticipantesRequerimiento(parametros, pathOptions);
        }

        public async Task<List<ResponseAction>> ActualizarUrlPdf(int? id, string url)
        {
            return await _unitOfWork.RequerimientosRepository.ActualizarUrlPdf(id, url);
        }

        public async Task<List<PathArchivosArtSerReq>> GetDocumentosTipoArchivoReq(int IdReq)
        {
            return await _unitOfWork.RequerimientosRepository.GetDocumentosTipoArchivoReq(IdReq);
        }

        public async Task<List<ArticulosRequerimiento>> GetArticulosParaRequerimiento(QuerySearchSolicitudesApoteosys parametros)
        {
            return await _unitOfWork.RequerimientosRepository.GetArticulosParaRequerimiento(parametros);
        }

        public async Task<List<AdjudicacionOrdenes>> GetAdjudicacionRequerimientoOrden(int idRequerimiento)
        {
            return await _unitOfWork.RequerimientosRepository.GetAdjudicacionRequerimientoOrden(idRequerimiento);
        }

        public async Task<List<ResponseAction>> CreateAdjudicarRequerimiento(QueryAdjudicarReq parametersAdj)
        {
            var response = await _unitOfWork.RequerimientosRepository.CreateAdjudicarRequerimiento(parametersAdj);

            if (response != null &&
                response.Any() &&
                response.FirstOrDefault() != null &&
                response.FirstOrDefault().estado)
            {
                await _notificationsService.PushSocket(
                    new DataNotifications
                    {
                        Method = HubConectionsMethods.Notification,
                        Action = HubConectionsActions.AdjudicacionRequerimiento
                    });
            }

            return response;
        }

        public async Task<List<ResponseAction>> UpdateAdjudicarRequerimiento(QueryAdjudicarReq parametersAdj)
        {
            var response = await _unitOfWork.RequerimientosRepository.UpdateAdjudicarRequerimiento(parametersAdj);

            if (response != null &&
                response.Any() &&
                response.FirstOrDefault() != null &&
                response.FirstOrDefault().estado)
            {
                await _notificationsService.PushSocket(
                    new DataNotifications
                    {
                        Method = HubConectionsMethods.Notification,
                        Action = HubConectionsActions.UpdateAdjudicacionRequerimiento
                    });
            }

            return response;
        }

        public async Task<List<ResponseAction>> AsociarOrdenesRequerimiento(QueryAsociarOrdenesReq parametros)
        {
            return await _unitOfWork.RequerimientosRepository.AsociarOrdenesRequerimiento(parametros);
        }
    }
}
