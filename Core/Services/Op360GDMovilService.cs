using Core.DTOs.CargaInicialMovilDto;
using Core.DTOs;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Newtonsoft.Json;
using Core.DTOs.GDMovilDto;
using Core.Enumerations;
using Core.DTOs.CuestionarioInstanciasMovilDto;
using static Core.DTOs.GDMovilDto.Op360GD_GestionOrdenDto;
using Core.DTOs.RegistrarGestionOrdenMovilDto;
using Core.DTOs.FilesDto;

namespace Core.Services
{
    public class Op360GDMovilService : IOp360GDMovilService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileManagementService _fileManagementService;
        private readonly IFileAccessService _fileAccessService;

        public Op360GDMovilService(IUnitOfWork unitOfWork, IFileManagementService fileManagementService, IFileAccessService fileAccessService)
        {
            _unitOfWork = unitOfWork;
            _fileManagementService = fileManagementService;
            _fileAccessService = fileAccessService;
        }

        public async Task<ResponseDto<Op360GD_DataCargaIniciaMovilDto>> CargaInicialAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360GD_DataCargaIniciaMovilDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_eventos_gd.prc_carga_inicial_movil");  //sp creado
        }

        public async Task<ResponseDto<Op360GD_ConsultaOrdenesDto>> ConsultaOrdenesAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ConsultaOrdenesDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_consulta_de_ordenes");  //sp cascaron
        }

        public async Task<ResponseDto<Op360GD_OrdenId_ResponseDto>> OrdenXIdAsync(Op360GD_OrdenIdDto gestionDanioDto)
        {
            var param = JsonConvert.SerializeObject(gestionDanioDto);
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360GD_OrdenId_ResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummigd.prc_consultar_orden_x_id", param);  //sp cascaron
        }

        public async Task<ResponseArrayDto<Op360GD_OrdenesAsignadasTecnicoMovilDto>> GetOrdenesAsignadasTecnicoAsync(Op360GD_OrdenesAsignadasTecnicoMovilRequestDto op360GD_OrdenesAsignadasTecnicoMovilRequest)
        {
            var param = JsonConvert.SerializeObject(op360GD_OrdenesAsignadasTecnicoMovilRequest);
            return await _unitOfWork.StoreProcedure<ResponseArrayDto<Op360GD_OrdenesAsignadasTecnicoMovilDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_eventos_gd.prc_lista_ordenes_asignadas_tecnico", param);  //sp creado
        }

        public async Task<ResponseDto> RegistarGestionOrdenAsync(Op360GD_GestionOrdenDto op360GD_GestionOrden)
        {
            var id_usuario = op360GD_GestionOrden.pregunta.id_usuario;
            Enum_RutasArchivos modulo = Enum_RutasArchivos.GDOrdenesMovileLocal;
            gnl_rutas_archivo_servidor rootFileServer = await _fileAccessService.GetRoot(modulo);

            List<Op360_SoportesMovilGDDto> cuestionarioSoporte = new ();
            foreach (Op360_SoporteDTO soporte in op360GD_GestionOrden.soportes)
            {
                Op360_SoportesMovilGDDto cuestionarios = await ProcessSoportes(soporte, modulo, rootFileServer, id_usuario);
                cuestionarioSoporte.Add(cuestionarios);
            }

            List<Op360_SoportesMovil2GDDto> reglasDeOroSoporte = new();
            foreach (Op360_Reglas_De_OroDto soporte in op360GD_GestionOrden.reglas_de_oro)
            {
                Op360_SoportesMovil2GDDto cuestionarios = await ProcessSoportes2(soporte, modulo, rootFileServer, id_usuario);
                reglasDeOroSoporte.Add(cuestionarios);
            }

            Op360GD_GestionOrdenResponseDto op360GD_GestionOrdenResponseDto = new(/*op360GD_GestionOrden.id_usuario,*/
                                                                                    op360GD_GestionOrden.pregunta,
                                                                                    op360GD_GestionOrden.causa,
                                                                                    reglasDeOroSoporte,
                                                                                    op360GD_GestionOrden.materiales_instalar,
                                                                                    op360GD_GestionOrden.materiales_retirar,
                                                                                    op360GD_GestionOrden.cierre_orden,
                                                                                    op360GD_GestionOrden.acciones,
                                                                                    cuestionarioSoporte);

            var param = JsonConvert.SerializeObject(op360GD_GestionOrdenResponseDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_eventos_gd.prc_registrar_orden_gestion", param);  //sp creado

        }

        public async Task<ResponseDto> RechazarOrdenAsync(Op360GD_RechazarOrdenRequestDto op360GD_RechazarOrdenRequest)
        {
            var param = JsonConvert.SerializeObject(op360GD_RechazarOrdenRequest);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_rechazar_orden", param);  //sp cascaron
        }

        public async Task<ResponseDto> ComprometerOrdenAsync(Op360GD_ComprometerOrdenRequestDto op360GD_ComprometerOrdenRequest)
        {
            var param = JsonConvert.SerializeObject(op360GD_ComprometerOrdenRequest);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummigd.prc_comprometer_orden", param);  //sp cascaron
        }

        #region Private Methods
        private async Task<Op360_SoportesMovilGDDto> ProcessSoportes(Op360_SoporteDTO soporte, Enum_RutasArchivos modulo, gnl_rutas_archivo_servidor rootFileServer, int userId)
        {
           // Op360_SoportesMovilGDDto processedSoportes = new();
            
                FileResponse fileResponse = await CreateFileAndGetResponse(soporte.base64, modulo, rootFileServer);
                var tmpgnl_soportes = await _fileAccessService.SaveFileSoportes(fileResponse, userId);
                Uri uri = new Uri($"{rootFileServer.RUTA_WEB}{tmpgnl_soportes.Id}");
                Op360_SoportesMovilGDDto soporteMovilDto = new(soporte, fileResponse, uri.ToString());
                //processedSoportes.Add(soporteMovilDto);
            

            return soporteMovilDto;
        }

        private async Task<Op360_SoportesMovil2GDDto> ProcessSoportes2(Op360_Reglas_De_OroDto ReglasDeOro, Enum_RutasArchivos modulo, gnl_rutas_archivo_servidor rootFileServer, int userId)
        {
            // Op360_SoportesMovilGDDto processedSoportes = new();

            FileResponse fileResponse = await CreateFileAndGetResponse(ReglasDeOro.base64, modulo, rootFileServer);
            var tmpgnl_soportes = await _fileAccessService.SaveFileSoportes(fileResponse, userId);
            Uri uri = new Uri($"{rootFileServer.RUTA_WEB}{tmpgnl_soportes.Id}");
            Op360_SoportesMovil2GDDto soporteMovilDto = new(ReglasDeOro, fileResponse, uri.ToString());
            //processedSoportes.Add(soporteMovilDto);


            return soporteMovilDto;
        }

        private async Task<FileResponse> CreateFileAndGetResponse(string base64, Enum_RutasArchivos modulo, gnl_rutas_archivo_servidor rootFileServer)
        {
            FileByBase64Dto fileByBase64Dto = new(base64, modulo, rootFileServer.RUTA_RED);
            return await _fileManagementService.CreateFileByBase64(fileByBase64Dto);
        }
        #endregion

    }
}
