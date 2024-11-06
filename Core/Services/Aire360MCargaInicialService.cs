namespace Core.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.DTOs;
    using Core.DTOs.CargaInicialMovilDto;
    using Core.DTOs.CuestionarioInstanciasMovilDto;
    using Core.DTOs.EncuestaInicialMovilDto;
    using Core.DTOs.FilesDto;
    using Core.DTOs.OrdenesAsignadasTecnicoMovilDto;
    using Core.Entities;
    using Core.Interfaces;
    using Newtonsoft.Json;
    using Core.Enumerations;
    using System;
    using Core.DTOs.RegistrarGestionOrdenMovilDto;
    using Core.Dtos.RechazarOrdenDto;
    using Core.DTOs.ComprometerOrdenDto;
    using Core.Extensions;

    public class Aire360MCargaInicialService : IAire360MCargaInicialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileManagementService _fileManagementService;
        private readonly IFileAccessService _fileAccessService;

        public Aire360MCargaInicialService(IUnitOfWork unitOfWork, IFileManagementService fileManagementService, IFileAccessService fileAccessService)
        {
            _unitOfWork = unitOfWork;
            _fileManagementService = fileManagementService;
            _fileAccessService = fileAccessService;
        }

        public async Task<ResponseDto<DataCargaInicialMovilDto>> GetCargaInicialAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<DataCargaInicialMovilDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_scr.prc_carga_inicial_scr_movil");
        }

        public async Task<ResponseDto<EncuestaInicialMovilDto>> GetEncuestaInicialAsync(EncuestaInicialRequestMovilDto encuestaInicialRequestMovilDto)
        {
            var param = JsonConvert.SerializeObject(encuestaInicialRequestMovilDto);
            return await _unitOfWork.StoreProcedure<ResponseDto<EncuestaInicialMovilDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_scr.prc_consultar_encuensta_inicial_realizada", param);
        }

        public async Task<ResponseDto<IList<OrdenesAsignadasTecnicoMovilDto>>> GetOrdenesAsignadasTecnicoAsync(OrdenesAsignadasTecnicoMovilRequestDto ordenesAsignadasTecnicoMovilRequestDto)
        {
            var param = JsonConvert.SerializeObject(ordenesAsignadasTecnicoMovilRequestDto);
            var muestra = await _unitOfWork.StoreProcedure<ResponseDto<IList<OrdenesAsignadasTecnicoMovilDto>>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_consultar_ordenes_asignadas_tecnico", param);
            return muestra;
        }

        public async Task<ResponseDto<CuestionarioInstanciasResponseMovilDto>> CreateCuestionarioInstanciaAsync(CuestionarioInstanciasRequestMovilDto cuestionarioInstanciasRequestMovilDto)
        {
            Enum_RutasArchivos modulo = Enum_RutasArchivos.ScrOrdenesMovile;
            gnl_rutas_archivo_servidor rootFileServer = await _fileAccessService.GetRoot(modulo);
            List<SoporteMovilDto> soportes = await ProcessSoportes(cuestionarioInstanciasRequestMovilDto.soportes, modulo, rootFileServer, cuestionarioInstanciasRequestMovilDto.id_usuario);
            CuestionarioInstanciasMovilDto cuestionarioInstanciasMovilDto = new(cuestionarioInstanciasRequestMovilDto.id_cuestionario, cuestionarioInstanciasRequestMovilDto.id_orden, cuestionarioInstanciasRequestMovilDto.id_contratista_persona, cuestionarioInstanciasRequestMovilDto.id_usuario, cuestionarioInstanciasRequestMovilDto.respuestas, soportes);
            var param = JsonConvert.SerializeObject(cuestionarioInstanciasMovilDto);
            return await _unitOfWork.StoreProcedure<ResponseDto<CuestionarioInstanciasResponseMovilDto>>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_generales.prc_registrar_cuestionario_instancia", param);
        }

        public async Task<ResponseDto> RegisterGestionOrdenAsync(RegistrarGestionOrdenMovilRequestDto registrarGestionOrdenMovilRequestDto)
        {
            //--Se castea de 0 a null el dia 18/07/2024
            registrarGestionOrdenMovilRequestDto.info_servicio.tipo_no_visibilidad = registrarGestionOrdenMovilRequestDto.info_servicio.tipo_no_visibilidad == 0 ? null : registrarGestionOrdenMovilRequestDto.info_servicio.tipo_no_visibilidad;

            Enum_RutasArchivos modulo = Enum_RutasArchivos.ScrOrdenesMovile;
            gnl_rutas_archivo_servidor rootFileServer = await _fileAccessService.GetRoot(modulo);
            List<List<SoporteMovilDto>> cuestionarioSoporte = new();
            foreach (CuestionarioRequestDto cuestionario in registrarGestionOrdenMovilRequestDto.cuestionarios)
            {
                List<SoporteMovilDto> cuestionarios = await ProcessSoportes(cuestionario.soportes, modulo, rootFileServer, registrarGestionOrdenMovilRequestDto.id_usuario);
                cuestionarioSoporte.Add(cuestionarios);
            }
            List<IList<SoporteMovilDto>> accionesFotos = new()
            {
                await ProcessSoportes(registrarGestionOrdenMovilRequestDto.accion_vs.fotos, modulo, rootFileServer, registrarGestionOrdenMovilRequestDto.id_usuario),
                await ProcessSoportes(registrarGestionOrdenMovilRequestDto.accion_vm.fotos, modulo, rootFileServer, registrarGestionOrdenMovilRequestDto.id_usuario),
                await ProcessSoportes(registrarGestionOrdenMovilRequestDto.reconexion.fotos, modulo, rootFileServer, registrarGestionOrdenMovilRequestDto.id_usuario),
                await ProcessSoportes(registrarGestionOrdenMovilRequestDto.irregularidad.fotos, modulo, rootFileServer, registrarGestionOrdenMovilRequestDto.id_usuario),
                await ProcessSoportes(registrarGestionOrdenMovilRequestDto.material_retirado.fotos, modulo, rootFileServer, registrarGestionOrdenMovilRequestDto.id_usuario)
            };
            List<SoporteMovilDto> soporteAnomalia = await ProcessSoportes(registrarGestionOrdenMovilRequestDto.anomalias.fotos, modulo, rootFileServer, registrarGestionOrdenMovilRequestDto.id_usuario);
            AnomaliasMovilDto anomalias = ProccessAnomalia(registrarGestionOrdenMovilRequestDto.anomalias, soporteAnomalia);
            List<AccionesMovilDto> acciones = ProcessAccionesAsync(registrarGestionOrdenMovilRequestDto, accionesFotos);
            List<CuestionarioMovilDto> cuestionariosSoporte = ProcessCuestionariosAsync(registrarGestionOrdenMovilRequestDto.cuestionarios, cuestionarioSoporte, registrarGestionOrdenMovilRequestDto.id_usuario);
            RegistrarGestionOrdenMovilSpDto registrarGestionOrdenMovilSpDto = new(registrarGestionOrdenMovilRequestDto.fecha_inicio_ejecucion, registrarGestionOrdenMovilRequestDto.fecha_fin_ejecucion, registrarGestionOrdenMovilRequestDto.id_orden, registrarGestionOrdenMovilRequestDto.tiene_alguna_anomalia,
                                                                                    registrarGestionOrdenMovilRequestDto.info_acta, registrarGestionOrdenMovilRequestDto.info_servicio, registrarGestionOrdenMovilRequestDto.persona_atiende,
                                                                                    registrarGestionOrdenMovilRequestDto.testigo, acciones, registrarGestionOrdenMovilRequestDto.material_instalado, anomalias, cuestionariosSoporte, registrarGestionOrdenMovilRequestDto.georreferencia);
            var param = JsonConvert.SerializeObject(registrarGestionOrdenMovilSpDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_registrar_orden_gestion", param);
        }

        public async Task<ResponseDto> RechazarOrdenAsync(RechazarOrdenRequestDto rechazarOrdenRequestDto)
        {

            var param = JsonConvert.SerializeObject(rechazarOrdenRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_rechazar_orden_scr", param);
        }

        public async Task<ResponseDto> ComprometerOrdenAsync(ComprometerOrdenRequestDto comprometerOrdenRequestDto)
        {
            var param = JsonConvert.SerializeObject(comprometerOrdenRequestDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_ordenes.prc_comprometer_orden_scr", param);
        }

        #region Private Methods
        private async Task<List<SoporteMovilDto>> ProcessSoportes(IList<SoporteRequestMovilDto> soportes, Enum_RutasArchivos modulo, gnl_rutas_archivo_servidor rootFileServer, int userId)
        {
            List<SoporteMovilDto> processedSoportes = new();
            foreach (SoporteRequestMovilDto soporte in soportes)
            {
                FileResponse fileResponse = await CreateFileAndGetResponse(soporte.base64, modulo, rootFileServer);
                var idTipoSoporte = soporte.codigo_tipo_soporte.GetTipoSoporte();
                var tmpgnl_soportes = await _fileAccessService.SaveFileScrSoportes(fileResponse, userId, idTipoSoporte);
                Uri uri = new Uri($"{rootFileServer.RUTA_WEB}{tmpgnl_soportes.Id}");
                SoporteMovilDto soporteMovilDto = new(soporte, fileResponse, uri.ToString());
                processedSoportes.Add(soporteMovilDto);
            }

            return processedSoportes;
        }

        private async Task<FileResponse> CreateFileAndGetResponse(string base64, Enum_RutasArchivos modulo, gnl_rutas_archivo_servidor rootFileServer)
        {
            FileByBase64Dto fileByBase64Dto = new(base64, modulo, rootFileServer.RUTA_RED);
            return await _fileManagementService.CreateFileByBase64(fileByBase64Dto);
        }

        private static List<AccionesMovilDto> ProcessAccionesAsync(RegistrarGestionOrdenMovilRequestDto request, List<IList<SoporteMovilDto>> accionesFotos)
        {
            List<AccionesMovilDto> acciones = new();
            string[] accionesNames = { "accion_vs", "accion_vm", "reconexion", "irregularidad", "material_retirado" };
            for (int i = 0; i < accionesNames.Length; i++)
            {
                AccionesMovilDto accion = new(accionesNames[i], GetObservacionByIndex(request, i), accionesFotos[i], GetAccionIdByIndex(request, i), GetSubActividadByIndex(request,i), GetMaterialesByIndex(request, i));
                acciones.Add(accion);
            }

            return acciones;
        }

        private static string GetObservacionByIndex(RegistrarGestionOrdenMovilRequestDto request, int index)
        {
            return index switch
            {
                0 => request.accion_vs.observacion,
                1 => request.accion_vm.observacion,
                2 => request.reconexion.observaciones,
                3 => request.irregularidad.observaciones,
                4 => request.material_retirado.observaciones,
                _ => string.Empty,
            };
        }

        private static int? GetAccionIdByIndex(RegistrarGestionOrdenMovilRequestDto request, int index)
        {
            return index switch
            {
                0 => request.accion_vs.accion_vs,
                1 => request.accion_vm.accion_vm,
                2 => request.reconexion.accion_rcs,
                3 => request.irregularidad.accion_ri,
                4 => request.material_retirado.accion_mdv,
                _ => null,
            };
        }

        private static IList<MaterialesMovilDto> GetMaterialesByIndex(RegistrarGestionOrdenMovilRequestDto request, int index)
        {
            return index switch
            {
                4 => request.material_retirado.materiales,
                _ => new List<MaterialesMovilDto>(),
            };
        }

        private static int? GetSubActividadByIndex(RegistrarGestionOrdenMovilRequestDto request, int index)
        {
            return index switch
            {
                2 => request.reconexion.subactividad,
                _ => null
            };
        }

        private static List<CuestionarioMovilDto> ProcessCuestionariosAsync(IList<CuestionarioRequestDto> cuestionariosSoporte, List<List<SoporteMovilDto>> cuestionarioSoporte, int userId)
        {
            List<CuestionarioMovilDto> cuestionarios = new();
            for (int i = 0; i < cuestionarioSoporte.Count; i++)
            {
                CuestionarioMovilDto cuestionario = new(cuestionariosSoporte[i].id_cuestionario, cuestionariosSoporte[i].id_orden, cuestionariosSoporte[i].id_contratista_persona, userId, cuestionariosSoporte[i].respuestas, cuestionarioSoporte[i], cuestionariosSoporte[i].observaciones ?? string.Empty);
                cuestionarios.Add(cuestionario);
            }

            return cuestionarios;
        }

        private static AnomaliasMovilDto ProccessAnomalia(AnomaliasMovilRequestDto anomalia, IList<SoporteMovilDto> anomaliaFotos)
        {
            return new AnomaliasMovilDto()
            {
                id_anomalia = anomalia.id_anomalia,
                id_subanomalia = anomalia.id_subanomalia,
                observaciones_subanomalia = anomalia.observaciones_subanomalia,
                observaciones = anomalia.observaciones,
                fotos = anomaliaFotos
            };
        }


        #endregion
    }
}

