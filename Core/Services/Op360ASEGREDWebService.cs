using Core.DTOs;
using Core.DTOs.ASEGREDWebDto;
using Core.DTOs.GDWebDto;
using Core.Entities.ASEGREDWebEntities;
using Core.Interfaces;
using Core.QueryFilters.QueryFiltersASEGREDWeb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class Op360ASEGREDWebService : IOp360ASEGREDWebService
    {
        private readonly IUnitOfWork _unitOfWork;

        public Op360ASEGREDWebService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<Op360ASEGRED_DataCargaInicialWebDto>> CargaInicialAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_DataCargaInicialWebDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_asegred.prc_carga_inicial_web");  //sp creado
        }

        public async Task<ResponseDto> CrearObra(Op360ASEGRED_CrearObraDto op360ASEGRED_CrearObra)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_CrearObra);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_asegred.prc_registrar_obra", param);  //sp creado
        }

        public async Task<ResponseDto<Op360ASEGRED_ListarObrasDto>> ListarObras()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ListarObrasDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto> EditarObra(Op360ASEGRED_EditarObraDto op360ASEGRED_EditarObra)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EditarObra);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> EliminarObra(Op360ASEGRED_EliminarObraDto op360ASEGRED_EliminarObra)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EliminarObra);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public Task<ResponseDto> CargueMasivoExel(Op360ASEGRED_CargueMasivoExcelDto op360ASEGRED_CargueMasivoExcel)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_CargueMasivoExcel);
            return _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> EliminarCargueMasivoExcel(Op360ASEGRED_EliminarCargueMasivoExcelDto op360ASEGRED_EliminarCargueMasivoExcel)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EliminarCargueMasivoExcel);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> CargaSoportesObra(Op360ASEGRED_CargaSoportesObraDto op360ASEGRED_CargaSoportesObra)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_CargaSoportesObra);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> EliminarSoportesObra(Op360ASEGRED_EliminarSoportesObraDto op360ASEGRED_EliminarSoportesObra)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EliminarSoportesObra);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }
        
        public async Task<ResponseDto> AsignarObraAliadoContratista(Op360ASEGRED_AsignarObraAliadoContratistaDto op360ASEGRED_AsignarObraAliadoContratista)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_AsignarObraAliadoContratista);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ListarEstructurasDto>> ListarEstructuras()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ListarEstructurasDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto> EditarEstructura(Op360ASEGRED_EditarEstructuraDto op360ASEGRED_EditarEstructura)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EditarEstructura);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> AgregarEstructura(Op360ASEGRED_AgregarEstructuraDto op360ASEGRED_AgregarEstructura)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_AgregarEstructura);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> EliminarEstructura(Op360ASEGRED_EliminarEstructuraDto op360ASEGRED_EliminarEstructura)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EliminarEstructura);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_DescargarSoportesDto>> DescargarSoportes()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_DescargarSoportesDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto> ValidarSoporte(Op360ASEGRED_ValidarSoporteDto op360ASEGRED_ValidarSoporte)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_ValidarSoporte);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> RechazarSoporte(Op360ASEGRED_RechazarSoporteDto op360ASEGRED_RechazarSoporte)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_RechazarSoporte);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_DescargarDisenioObraDto>> DescargarDisenioObra()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_DescargarDisenioObraDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros");  //sp de ejemplo
        }

        public async Task<ResponseDto> CrearSoporte(Op360ASEGRED_CrearSoporteDto op360ASEGRED_CrearSoporte)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_CrearSoporte);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> EliminarSoporte(Op360ASEGRED_EliminarSoporteDto op360ASEGRED_EliminarSoporte)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EliminarSoporte);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ConsultarObrasConFiltrosResponseDto>> ConsultarObrasConFiltros(Op360ASEGRED_ConsultarObrasConFiltrosDto op360ASEGRED_ConsultarObrasConFiltros)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_ConsultarObrasConFiltros);
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ConsultarObrasConFiltrosResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> CierreUnidadesConstructivas(Op360ASEGRED_CierreUnidadesConstructivasDto op360ASEGRED_CierreUnidadesConstructivas)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_CierreUnidadesConstructivas);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ImprimirObraDto>> ImprimirObra()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ImprimirObraDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ListarDetalleEstructuraDto>> ListarDetalleEstructura()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ListarDetalleEstructuraDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ImprimirPosteDto>> ImprimirPoste()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ImprimirPosteDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_AuditoriaPosteDto>> AuditoriaPoste()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_AuditoriaPosteDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ListarEstructurasPorGeoreferenciaDto>> ListarEstructurasPorGeoreferencia()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ListarEstructurasPorGeoreferenciaDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto> PreLiquidarOrden(Op360ASEGRED_PreLiquidarOrdenDto op360ASEGRED_PreLiquidarOrden)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_PreLiquidarOrden);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ImprimirOrdenDto>> ImprimirOrden()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ImprimirOrdenDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto> NuevaPreFactura(Op360ASEGRED_NuevaPreFacturaDto op360ASEGRED_NuevaPreFactura)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_NuevaPreFactura);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ListarHistorialPreFacturasParcialesDto>> ListarHistorialPreFacturasParciales()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ListarHistorialPreFacturasParcialesDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto> EliminarPreFactura(Op360ASEGRED_EliminarPreFacturaDto op360ASEGRED_EliminarPreFactura)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EliminarPreFactura);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto> EditarPreFactura(Op360ASEGRED_EditarPreFacturaDto op360ASEGRED_EditarPreFactura)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_EditarPreFactura);
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureNonQueryAsync("aire.pkg_g_dummi.prc_dummi_parametros", param);  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_DescargarPreFacturaDto>> DescargarPreFactura()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_DescargarPreFacturaDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_ListadoUnidadesConstructivasCerradasResponseDto>> ListadoUnidadesConstructivasCerradas(Op360ASEGRED_ListadoUnidadesConstructivasCerradasDto op360ASEGRED_ListadoUnidadesConstructivasCerradas)
        {
            var param = JsonConvert.SerializeObject(op360ASEGRED_ListadoUnidadesConstructivasCerradas);
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ListadoUnidadesConstructivasCerradasResponseDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi_parametros");  //sp de ejemplo
        }

        public async Task<ResponseDto<Op360ASEGRED_DescargarPosiblePreFacturaDto>> DescargarPosiblePreFactura()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_DescargarPosiblePreFacturaDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }
    }
}
