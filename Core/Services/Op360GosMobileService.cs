namespace Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.DTOs.FilesDto;
    using Core.DTOs.Gos.Mobile.ComprometerGestion;
    using Core.DTOs.Gos.Mobile.CrearGestion;
    using Core.DTOs.Gos.Mobile.ObtenerGestionesByGestorDto;
    using Core.DTOs.Gos.Mobile.ProcesarGestionDto;
    using Core.DTOs.ObtenerGestionesByGestorDto;
    using Core.DTOs.ProcesarGestionDto;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Interfaces;
    using Newtonsoft.Json;

    public class Op360GosMobileService : IOp360GosMobileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileManagementService _fileManagementService;
        private readonly IFileAccessService _fileAccessService;

        public Op360GosMobileService(IUnitOfWork unitOfWork, IFileManagementService fileManagementService, IFileAccessService fileAccessService)
        {
            _unitOfWork = unitOfWork;
            _fileManagementService = fileManagementService;
            _fileAccessService = fileAccessService;
        }

        public async Task<ResponseDto<IList<GestionesByGestorDto>>> ObtenerGestionesByGestorAsync(ObtenerGestionesByGestorRequestDto obtenerGestionesByGestorRequestDto)
        {
            var param = JsonConvert.SerializeObject(obtenerGestionesByGestorRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto<IList<GestionesByGestorDto>>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_consultar_gestiones_gestor", param);
        }

        public async Task<ResponseDto> ProcesarGestionAsync(ProcesarGestionRequestDto procesarGestionRequestDto)
        {
            List<SoporteSpDto> soportes = await ProcessSoportesGestiones(procesarGestionRequestDto.soportes, procesarGestionRequestDto.id_usuario_registra);
            ProcesarGestionSpDto procesarGestionSpDto = new(procesarGestionRequestDto.id_orden, procesarGestionRequestDto.numero_orden, procesarGestionRequestDto.ind_ambiente_registra,
                procesarGestionRequestDto.id_contratista_persona, procesarGestionRequestDto.id_usuario_registra, procesarGestionRequestDto.anomalias, procesarGestionRequestDto.causal,
                procesarGestionRequestDto.programacion, procesarGestionRequestDto.actividades, soportes);
            var param = JsonConvert.SerializeObject(procesarGestionSpDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_registrar_orden_gestion", param);
        }

        public async Task<ResponseDto<CrearGestionResponseDto>> CrearGestionAsync(CrearGestionRequestDto crearGestionRequestDto)
        {
            var param = JsonConvert.SerializeObject(crearGestionRequestDto);

            var resul = await _unitOfWork.StoreProcedure<ResponseDto<CrearGestionResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_crear_gestion", param);

            return resul;
        }

        public async Task<ResponseDto> ComprometerGestionAsync(ComprometerGestionRequestDto comprometerGestionRequestDto)
        {
            var param = JsonConvert.SerializeObject(comprometerGestionRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_gos.prc_comprometer_orden", param);
        }

        #region Private Methods
        private async Task<List<SoporteSpDto>> ProcessSoportesGestiones(IList<SoporteDTO> soportes, int? userId)
        {
            List<SoporteSpDto> processedSoportes = new();
            foreach (SoporteDTO soporte in soportes)
            {
                var rutaEnum = (Enum_RutasArchivos)soporte.id_ruta;
                gnl_rutas_archivo_servidor rootFileServer = await _fileAccessService.GetRoot(rutaEnum);
                FileResponse fileResponse = await CreateFileAndGetResponse(soporte.Base64, rutaEnum, rootFileServer);
                var idTipoSoporte = GetTipoSoporte(soporte.codigo_tipo_soporte);
                var tmpgnl_soportes = await _fileAccessService.SaveFileGosSoportes(fileResponse, userId.Value, idTipoSoporte);
                Uri uri = new($"{rootFileServer.RUTA_WEB}{tmpgnl_soportes.Id}");
                SoporteSpDto soporteMovilDto = new(fileResponse.NombreInterno, fileResponse.Size, fileResponse.Extension, uri.ToString(), soporte.codigo_tipo_soporte);
                processedSoportes.Add(soporteMovilDto);
            }

            return processedSoportes;
        }

        private async Task<FileResponse> CreateFileAndGetResponse(string base64, Enum_RutasArchivos tipoRuta, gnl_rutas_archivo_servidor rootFileServer)
        {
            FileByBase64Dto fileByBase64Dto = new(base64, tipoRuta, rootFileServer.RUTA_RED);
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