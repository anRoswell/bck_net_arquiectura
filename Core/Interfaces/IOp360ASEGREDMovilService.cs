using Core.DTOs;
using Core.DTOs.ASEGREDMovilDto;
using Core.DTOs.ASEGREDWebDto;
using Core.DTOs.GDMovilDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOp360ASEGREDMovilService
    {
        Task<ResponseDto<Op360ASEGRED_DataCargaInicialMovilDto>> CargaInicialAsync();
        Task<ResponseDto> ConsultaOrdenesAsync();
        Task<ResponseDto> OrdenXIdAsync(Op360ASEGRED_OrdenIdDto op360ASEGRED_OrdenIdDto);
        Task<ResponseDto<Op360ASEGRED_OrdenesAsignadasTecnicoMovilDto>> GetOrdenesAsignadasTecnicoAsync(Op360ASEGRED_OrdenesAsignadasTecnicoMovilRequestDto op360ASEGRED_OrdenesAsignadasTecnicoMovilRequest);
        Task<ResponseDto> RegistarGestionOrdenAsync(Op360ASEGRED_GestionOrdenDto op360ASEGRED_GestionOrdenDto);
        Task<ResponseDto> RechazarOrdenAsync(Op360ASEGRED_RechazarOrdenRequestDto op360ASEGRED_RechazarOrdenRequest);
        Task<ResponseDto> ComprometerOrdenAsync(Op360ASEGRED_ComprometerOrdenRequestDto op360ASEGRED_ComprometerOrdenRequest);
        //nuevo
        Task<ResponseDto> CierreDeProceso(Op360ASEGRED_CierreDeProcesoDto op360ASEGRED_CierreDeProceso);
        Task<ResponseDto<Op360ASEGRED_EjecucionDeObrasDto>> EjecucionDeObras();
        Task<ResponseDto<Op360ASEGRED_ListarObraXContratistaDto>> ListarObraXContratista();
        Task<ResponseDto<Op360ASEGRED_EjecucionDeEstructurasDto>> EjecucionDeEstructuras();
        Task<ResponseDto> EditarEstructura(Op360ASEGRED_EditarEstructuraMovilDto op360ASEGRED_EditarEstructuraMovil);
        Task<ResponseDto<Op360ASEGRED_ListarInspeccionesDto>> ListarInspecciones();
        Task<ResponseDto<Op360ASEGRED_ListarObrasMovilDto>> ListarObras();
    }
}
