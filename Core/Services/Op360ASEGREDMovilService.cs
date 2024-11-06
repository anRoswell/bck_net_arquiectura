using Core.DTOs;
using Core.DTOs.ASEGREDMovilDto;
using Core.DTOs.ASEGREDWebDto;
using Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class Op360ASEGREDMovilService : IOp360ASEGREDMovilService
    {
        private readonly IUnitOfWork _unitOfWork;

        public Op360ASEGREDMovilService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<Op360ASEGRED_DataCargaInicialMovilDto>> CargaInicialAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_DataCargaInicialMovilDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto> ConsultaOrdenesAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto> OrdenXIdAsync(Op360ASEGRED_OrdenIdDto op360ASEGRED_OrdenIdDto)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_OrdenIdDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_OrdenesAsignadasTecnicoMovilDto>> GetOrdenesAsignadasTecnicoAsync(Op360ASEGRED_OrdenesAsignadasTecnicoMovilRequestDto op360ASEGRED_OrdenesAsignadasTecnicoMovilRequest)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_OrdenesAsignadasTecnicoMovilRequest);
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_OrdenesAsignadasTecnicoMovilDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> RegistarGestionOrdenAsync(Op360ASEGRED_GestionOrdenDto op360ASEGRED_GestionOrdenDto)
        {
            Op360ASEGRED_RegistarGestionOrdenDto registarGestionOrdenDto = new(op360ASEGRED_GestionOrdenDto.NUM_OS, op360ASEGRED_GestionOrdenDto.NUM_SOLICITUD, op360ASEGRED_GestionOrdenDto.TIPO_TRABAJO, op360ASEGRED_GestionOrdenDto.TIP_OS, op360ASEGRED_GestionOrdenDto.F_GEN, op360ASEGRED_GestionOrdenDto.F_ESTM_REST, op360ASEGRED_GestionOrdenDto.NIC,
                                                                op360ASEGRED_GestionOrdenDto.NUM_CAMP, op360ASEGRED_GestionOrdenDto.TIPO_SUSPENSION, op360ASEGRED_GestionOrdenDto.COMENT_OS, op360ASEGRED_GestionOrdenDto.COMENT_OS2, op360ASEGRED_GestionOrdenDto.DATOSUM, op360ASEGRED_GestionOrdenDto.ACTIVIDADES,
                                                                op360ASEGRED_GestionOrdenDto.PRECINTOS, op360ASEGRED_GestionOrdenDto.RECIBOS, op360ASEGRED_GestionOrdenDto.APACOEN);
            var param = JsonConvert.SerializeObject(registarGestionOrdenDto);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> RechazarOrdenAsync(Op360ASEGRED_RechazarOrdenRequestDto op360ASEGRED_RechazarOrdenRequest)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_RechazarOrdenRequest);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> ComprometerOrdenAsync(Op360ASEGRED_ComprometerOrdenRequestDto op360ASEGRED_ComprometerOrdenRequest)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_ComprometerOrdenRequest);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> CierreDeProceso(Op360ASEGRED_CierreDeProcesoDto op360ASEGRED_CierreDeProceso)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_CierreDeProceso);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_EjecucionDeObrasDto>> EjecucionDeObras()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_EjecucionDeObrasDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ListarObraXContratistaDto>> ListarObraXContratista()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ListarObraXContratistaDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_EjecucionDeEstructurasDto>> EjecucionDeEstructuras()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_EjecucionDeEstructurasDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto> EditarEstructura(Op360ASEGRED_EditarEstructuraMovilDto op360ASEGRED_EditarEstructuraMovil)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EditarEstructuraMovil);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ListarInspeccionesDto>> ListarInspecciones()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ListarInspeccionesDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ListarObrasMovilDto>> ListarObras()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ListarObrasMovilDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }
    }
}
