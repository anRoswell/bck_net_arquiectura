using Core.DTOs;
using Core.DTOs.ASEGREDWebDto;
using Core.DTOs.CargaInicialMovilDto;
using Core.DTOs.GDWebDto;
using Core.Entities.ASEGREDWebEntities;
using Core.Interfaces;
using Core.QueryFilters.QueryFiltersASEGREDWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOp360ASEGREDWebService
    {
        Task<ResponseDto<Op360ASEGRED_DataCargaInicialWebDto>> CargaInicialAsync();
        Task<ResponseDto> CrearObra(Op360ASEGRED_CrearObraDto op360ASEGRED_CrearObra);
        Task<ResponseDto<Op360ASEGRED_ListarObrasDto>> ListarObras();
        Task<ResponseDto> EditarObra(Op360ASEGRED_EditarObraDto op360ASEGRED_EditarObra);
        Task<ResponseDto> EliminarObra(Op360ASEGRED_EliminarObraDto op360ASEGRED_EliminarObra);
        Task<ResponseDto> CargueMasivoExel(Op360ASEGRED_CargueMasivoExcelDto op360ASEGRED_CargueMasivoExcel);
        Task<ResponseDto> EliminarCargueMasivoExcel(Op360ASEGRED_EliminarCargueMasivoExcelDto op360ASEGRED_EliminarCargueMasivoExcel);
        Task<ResponseDto> CargaSoportesObra(Op360ASEGRED_CargaSoportesObraDto op360ASEGRED_CargaSoportesObra);
        Task<ResponseDto> EliminarSoportesObra(Op360ASEGRED_EliminarSoportesObraDto op360ASEGRED_EliminarSoportesObra);
        Task<ResponseDto> AsignarObraAliadoContratista(Op360ASEGRED_AsignarObraAliadoContratistaDto op360ASEGRED_AsignarObraAliadoContratista);
        Task<ResponseDto<Op360ASEGRED_ListarEstructurasDto>> ListarEstructuras();
        Task<ResponseDto> EditarEstructura(Op360ASEGRED_EditarEstructuraDto op360ASEGRED_EditarEstructura);
        Task<ResponseDto> AgregarEstructura(Op360ASEGRED_AgregarEstructuraDto op360ASEGRED_AgregarEstructura);
        Task<ResponseDto> EliminarEstructura(Op360ASEGRED_EliminarEstructuraDto op360ASEGRED_EliminarEstructura);
        Task<ResponseDto<Op360ASEGRED_DescargarSoportesDto>> DescargarSoportes();
        Task<ResponseDto> ValidarSoporte(Op360ASEGRED_ValidarSoporteDto op360ASEGRED_ValidarSoporte);
        Task<ResponseDto> RechazarSoporte(Op360ASEGRED_RechazarSoporteDto op360ASEGRED_RechazarSoporte);
        Task<ResponseDto<Op360ASEGRED_DescargarDisenioObraDto>> DescargarDisenioObra();
        Task<ResponseDto> CrearSoporte(Op360ASEGRED_CrearSoporteDto op360ASEGRED_CrearSoporte);
        Task<ResponseDto> EliminarSoporte(Op360ASEGRED_EliminarSoporteDto op360ASEGRED_EliminarSoporte);
        Task<ResponseDto<Op360ASEGRED_ConsultarObrasConFiltrosResponseDto>> ConsultarObrasConFiltros(Op360ASEGRED_ConsultarObrasConFiltrosDto op360ASEGRED_ConsultarObrasConFiltros);
        Task<ResponseDto> CierreUnidadesConstructivas(Op360ASEGRED_CierreUnidadesConstructivasDto op360ASEGRED_CierreUnidadesConstructivas);
        Task<ResponseDto<Op360ASEGRED_ImprimirObraDto>> ImprimirObra();
        Task<ResponseDto<Op360ASEGRED_ListarDetalleEstructuraDto>> ListarDetalleEstructura();
        Task<ResponseDto<Op360ASEGRED_ImprimirPosteDto>> ImprimirPoste();
        Task<ResponseDto<Op360ASEGRED_AuditoriaPosteDto>> AuditoriaPoste();
        Task<ResponseDto<Op360ASEGRED_ListarEstructurasPorGeoreferenciaDto>> ListarEstructurasPorGeoreferencia();
        Task<ResponseDto> PreLiquidarOrden(Op360ASEGRED_PreLiquidarOrdenDto op360ASEGRED_PreLiquidarOrden);
        Task<ResponseDto<Op360ASEGRED_ImprimirOrdenDto>> ImprimirOrden();
        Task<ResponseDto> NuevaPreFactura(Op360ASEGRED_NuevaPreFacturaDto op360ASEGRED_NuevaPreFactura);
        Task<ResponseDto<Op360ASEGRED_ListarHistorialPreFacturasParcialesDto>> ListarHistorialPreFacturasParciales();
        Task<ResponseDto> EliminarPreFactura(Op360ASEGRED_EliminarPreFacturaDto op360ASEGRED_EliminarPreFactura);
        Task<ResponseDto> EditarPreFactura(Op360ASEGRED_EditarPreFacturaDto op360ASEGRED_EditarPreFactura);
        Task<ResponseDto<Op360ASEGRED_DescargarPreFacturaDto>> DescargarPreFactura();
        Task<ResponseDto<Op360ASEGRED_ListadoUnidadesConstructivasCerradasResponseDto>> ListadoUnidadesConstructivasCerradas(Op360ASEGRED_ListadoUnidadesConstructivasCerradasDto op360ASEGRED_ListadoUnidadesConstructivasCerradas);
        Task<ResponseDto<Op360ASEGRED_DescargarPosiblePreFacturaDto>> DescargarPosiblePreFactura();
    }
}
