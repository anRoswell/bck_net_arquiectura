namespace Core.Services
{
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.DTOs.AsignarDesasignarOrdenesAGestoresDto;
    using Core.DTOs.ObtenerOrdenesTrabajoOficinaCentralDto;
    using Core.DTOs.CargaInicialGosDto;
    using Core.Interfaces;
    using Newtonsoft.Json;
    using Core.DTOs.Gos.GetOrdenById;
    using Core.DTOs.Gos.UpdateOrden;
    using Core.DTOs.Gos.CrearComentario;
    using Core.DTOs.Gos.CambiarEstadoOrden;
    using Core.DTOs.Gos.ObtenerArchivosInstancia;
    using Core.DTOs.Gos.ObtenerArchivosInstanciaDetalle;
    using Core.DTOs.Gos.ObtenerOrdenesTrabajoOficinaCentralDto;
    using Core.Enumerations;
    using Core.Entities;
    using Core.DTOs.FilesDto;
    using System;
    using System.Collections.Generic;
    using Core.DTOs.Gos.Web.UpdateOrden;
    using Core.DTOs.Gos.Web.ConsultaOrdenesFechaDto;
    using System.IO;
    using Core.DTOs.Gos.Web.GetSoporteById;
    using Core.Exceptions;
    using Core.DTOs.Gos.Web.ConsultarOrdenesGestion;
    using Core.DTOs.Gos.Web.Dashboard;
    using Core.QueryFilters;

    public class Op360GosWebService : IOp360GosWebService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileManagementService _fileManagementService;
        private readonly IFileAccessService _fileAccessService;

        public Op360GosWebService(IUnitOfWork unitOfWork, IFileManagementService fileManagementService, IFileAccessService fileAccessService)
        {
            _unitOfWork = unitOfWork;
            _fileManagementService = fileManagementService;
            _fileAccessService = fileAccessService;
        }

        public async Task<ResponseDto<ParametrosInicialesResponseDto>> GetParametrosInicialesAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<ParametrosInicialesResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_carga_inicial_gos");
        }

        public async Task<ResponseDto> UpdateOrdenAsync(UpdateOrdenRequestDto updateOrdenRequestDto, int myUser)
        {
            List<AdjuntosUpdateDto> soportes = await ProcessSoportesGestiones(updateOrdenRequestDto.datos_gestion.datos_desarrollo.adjuntos, myUser);
            UpdateOrdenSpDto updateOrdenSpDto = new UpdateOrdenSpDto
            {
                id_usuario = myUser,
                datos_basicos = updateOrdenRequestDto.datos_basicos
            };
            updateOrdenSpDto.datos_gestion = new DatosGestionUpdateDto();
            var datosProgramacion = updateOrdenRequestDto.datos_gestion.datos_programacion;
            updateOrdenSpDto.datos_gestion.datos_programacion = datosProgramacion;
            updateOrdenSpDto.datos_gestion.datos_desarrollo = new DatosDesarrolloUpdateDto
            {
                adjuntos = new List<AdjuntosUpdateDto>()
            };
            updateOrdenSpDto.datos_gestion.datos_desarrollo.adjuntos = soportes;
            updateOrdenSpDto.datos_gestion.datos_desarrollo.nombre_proyecto = updateOrdenRequestDto.datos_gestion.datos_desarrollo.nombre_proyecto;
            updateOrdenSpDto.datos_gestion.datos_desarrollo.nombre_contacto = updateOrdenRequestDto.datos_gestion.datos_desarrollo.nombre_contacto;
            updateOrdenSpDto.datos_gestion.datos_desarrollo.id_tema = updateOrdenRequestDto.datos_gestion.datos_desarrollo.id_tema;
            updateOrdenSpDto.datos_gestion.datos_desarrollo.id_subtema = updateOrdenRequestDto.datos_gestion.datos_desarrollo.id_subtema;
            updateOrdenSpDto.datos_gestion.datos_desarrollo.cantidad_asistentes = updateOrdenRequestDto.datos_gestion.datos_desarrollo.cantidad_asistentes;
            updateOrdenSpDto.datos_gestion.datos_desarrollo.observaciones = updateOrdenRequestDto.datos_gestion.datos_desarrollo.observaciones;
            var param = JsonConvert.SerializeObject(updateOrdenSpDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_actualizar_orden", param);
        }

        public async Task<ResponseDto> CrearComentarioAsync(CrearComentarioRequestDto crearComentarioRequestDto, int myUser)
        {
            CrearComentarioSpDto crearComentarioSpDto = new() {
                comentario = crearComentarioRequestDto.comentario,
                id_orden = crearComentarioRequestDto.id_orden,
                usuario_registra = myUser
            }; ;
            var param = JsonConvert.SerializeObject(crearComentarioSpDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_gos.prc_crear_comentario_orden", param);
        }

        public async Task<ResponseDto<GetOrdenByIdResponseDto>> GetOrdenByIdAsync(GetOrdenByIdRequest getOrdenByIdRequest)
        {
            var param = JsonConvert.SerializeObject(getOrdenByIdRequest);
            return await _unitOfWork.StoreProcedure<ResponseDto<GetOrdenByIdResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_consulta_datos_orden_id", param);
        }

        public async Task<ResponsePaginationDto<ObtenerOrdenesTrabajoOficinaCentralResponseDto>> ObtenerOrdenesTrabajoOficinaCentralAsync(ObtenerOrdenesTrabajoOficinaCentralRequestDto obtenerOrdenesTrabajoOficinaCentralRequestDto)
        {
            ObtenerOrdenesTrabajoOficinaCentralSpDto obtenerOrdenesTrabajoOficinaCentralSpDto = new() {
                filtros = obtenerOrdenesTrabajoOficinaCentralRequestDto.filtros,
                ServerSideJson = string.IsNullOrEmpty(obtenerOrdenesTrabajoOficinaCentralRequestDto.ServerSide) ? null : JsonConvert.DeserializeObject<QueryOp360ServerSide>(obtenerOrdenesTrabajoOficinaCentralRequestDto.ServerSide)
            };
            var param = JsonConvert.SerializeObject(obtenerOrdenesTrabajoOficinaCentralSpDto);
            return await _unitOfWork.StoreProcedure<ResponsePaginationDto<ObtenerOrdenesTrabajoOficinaCentralResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_consulta_datos_ordenes", param);
        }

        public async Task<ResponseDto> AsignarDesasignarOrdenesAGestoresAsync(AsignarDesasignarOrdenesAGestoresRequestDto asignarDesasignarOrdenesAGestoresRequestDto)
        {
            var param = JsonConvert.SerializeObject(asignarDesasignarOrdenesAGestoresRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_gos.prc_actualiza_asignacion_gestor_orden", param);
        }

        public async Task<ResponsePaginationDto<ObtenerArchivosInstanciaResponseDto>> ObtenerArchivosInstanciaAsync(ObtenerArchivosInstanciaRequestDto obtenerArchivosInstanciaRequestDto)
        {
            var param = JsonConvert.SerializeObject(obtenerArchivosInstanciaRequestDto);
            return await _unitOfWork.StoreProcedure<ResponsePaginationDto<ObtenerArchivosInstanciaResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_consultar_archivos_instancia", param);
        }

        public async Task<ResponsePaginationDto<ObtenerArchivosInstanciaDetalleResponseDto>> ObtenerArchivosInstanciaDetalleAsync(ObtenerArchivosInstanciaDetalleRequestDto obtenerArchivosInstanciaDetalleRequestDto)
        {
            var param = JsonConvert.SerializeObject(obtenerArchivosInstanciaDetalleRequestDto);
            return await _unitOfWork.StoreProcedure<ResponsePaginationDto<ObtenerArchivosInstanciaDetalleResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_consultar_archivos_instancia_detalle", param);
        }

        public async Task<ResponseDto> CambiarEstadoOrdenAsync(CambiarEstadoOrdenRequestDto cambiarEstadoOrdenRequestDto)
        {
            var param = JsonConvert.SerializeObject(cambiarEstadoOrdenRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_gos.prc_cambiar_estado_orden", param);
        }

        public async Task<ResponseDto<GetSoporteByIdResponseDto>> GetSoporteByIdAsync(GetSoporteByIdRequestDto getSoporteByIdRequestDto)
        {
            try
            {
                var data = await _unitOfWork.GosSoporteRepository.GetById(getSoporteByIdRequestDto.id_soporte);
                gnl_rutas_archivo_servidor rootFileServer = await _unitOfWork.RutasArchivoServidorOracleRepository.GetById(getSoporteByIdRequestDto.id_Ruta);

                string PathRedBase = Path.Combine(rootFileServer.RUTA_RED, data.nombre);
            
                if (System.IO.File.Exists(PathRedBase))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(PathRedBase);
                    string base64String = Convert.ToBase64String(fileBytes);
                    FileInfo fileInfo = new FileInfo(PathRedBase);
                    var fileData = new ResponseDto<GetSoporteByIdResponseDto>()
                    {
                        Codigo = 0,
                        Mensaje = "Registro Encontrado con Éxito",
                        Datos = new GetSoporteByIdResponseDto
                        {
                            Base64 = base64String
                        }
                    };
                    return fileData;
                }
                else
                {
                    return new ResponseDto<GetSoporteByIdResponseDto>()
                    {
                        Codigo = 1,
                        TotalRecords = 0,
                        Mensaje = "No se encontro el archivo.",
                        Datos = null
                    };
                }
            }
            catch (Exception e)
            {
                throw new BusinessException($"Error inesperado: {e.Message} - {e.StackTrace}");
            }
        }

        public async Task<ResponseDto<ReporteDto>> GetDashboardAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<ReporteDto>>().ExecuteStoreProcedureAsync("AIRE.pkg_g_gos.PRC_CONSULTAR_ORDENES_AGRUPADAS_AREA_CENTRAL_GOS");
        }

        #region Gos
        public async Task<ResponseDto<ConsultaOrdenesFechaResponseDto>> ConsultarOrdenesAreaCentralGosExcel(ConsultaOrdenesFechaRequestDto consultaOrdenesFechaRequestDto)
        {
            var param = JsonConvert.SerializeObject(consultaOrdenesFechaRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto<ConsultaOrdenesFechaResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_consulta_datos_ordenes_fechas", param);
        }

        public async Task<ResponseDto<ConsultaOrdenesGestionResponseDto>> ConsultaOrdenesGestionGosExcel(ConsultaOrdenesGestionSpDto consultaOrdenesGestionSpDto)
        {
            var param = JsonConvert.SerializeObject(consultaOrdenesGestionSpDto);
            return await _unitOfWork.StoreProcedure<ResponseDto<ConsultaOrdenesGestionResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_consulta_ordenes_gestion", param);
        }
        #endregion

        #region Private Method
        private async Task<List<AdjuntosUpdateDto>> ProcessSoportesGestiones(IList<SoporteUpdateDto> soportes, int? userId)
        {
            List<AdjuntosUpdateDto> processedSoportes = new();
            foreach (SoporteUpdateDto soporte in soportes)
            {
                if (soporte.id_adjunto.HasValue)
                {
                    AdjuntosUpdateDto soporteMovilDto = new(soporte.id_adjunto, soporte.accion, soporte.codigo_tipo_soporte, null, null, null, 0);
                    processedSoportes.Add(soporteMovilDto);
                }
                else
                {
                    var enumRuta = (Enum_RutasArchivos)soporte.id_ruta;
                    gnl_rutas_archivo_servidor rootFileServer = await _fileAccessService.GetRoot(enumRuta);
                    FileResponse fileResponse = await CreateFileAndGetResponse(soporte.base64, enumRuta, rootFileServer);
                    var idTipoSoporte = GetTipoSoporte(soporte.codigo_tipo_soporte);
                    var tmpgnl_soportes = await _fileAccessService.SaveFileGosSoportes(fileResponse, userId.Value, idTipoSoporte);
                    Uri uri = new($"{rootFileServer.RUTA_WEB}{tmpgnl_soportes.Id}");
                    AdjuntosUpdateDto soporteMovilDto = new(soporte.id_adjunto, soporte.accion, soporte.codigo_tipo_soporte, fileResponse.NombreInterno, uri.ToString(), fileResponse.Extension, fileResponse.Size);
                    processedSoportes.Add(soporteMovilDto);
                }
            }

            return processedSoportes;
        }

        private async Task<FileResponse> CreateFileAndGetResponse(string base64, Enum_RutasArchivos modulo, gnl_rutas_archivo_servidor rootFileServer)
        {
            FileByBase64Dto fileByBase64Dto = new(base64, modulo, rootFileServer.RUTA_RED);
            return await _fileManagementService.CreateFileByBase64(fileByBase64Dto);
        }

        private static TipoSoporteEnum GetTipoSoporte(string codigoTipoSoporte)
        {
            return codigoTipoSoporte switch
            {
                "GACTA" => TipoSoporteEnum.GACTA,
                "GASIS" => TipoSoporteEnum.GASIS,
                "GEVID" => TipoSoporteEnum.GEVID,
                "GORDN" => TipoSoporteEnum.GORDN,
                "GOCM" => TipoSoporteEnum.GOCM,
                "GANO" => TipoSoporteEnum.GANO,
                "GCAU" => TipoSoporteEnum.GCAU,
                "GPDF" => TipoSoporteEnum.GPDF,
                _ => throw new NotImplementedException()
            };
        }
        #endregion
    }
}