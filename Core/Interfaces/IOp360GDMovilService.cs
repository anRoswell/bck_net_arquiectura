using Core.DTOs.CargaInicialMovilDto;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.QueryFilters;
using Core.DTOs.GDMovilDto;
using Core.DTOs.ASEGREDWebDto;
using Core.DTOs.FilesDto;

namespace Core.Interfaces
{
    public interface IOp360GDMovilService
    {
        Task<ResponseDto<Op360GD_DataCargaIniciaMovilDto>> CargaInicialAsync();
        Task<ResponseDto<Op360GD_ConsultaOrdenesDto>> ConsultaOrdenesAsync();
        Task<ResponseDto<Op360GD_OrdenId_ResponseDto>> OrdenXIdAsync(Op360GD_OrdenIdDto gestionDanioOrdenId);
        Task<ResponseArrayDto<Op360GD_OrdenesAsignadasTecnicoMovilDto>> GetOrdenesAsignadasTecnicoAsync(Op360GD_OrdenesAsignadasTecnicoMovilRequestDto op360GD_OrdenesAsignadasTecnicoMovilRequest);
        Task<ResponseDto> RechazarOrdenAsync(Op360GD_RechazarOrdenRequestDto op360GD_RechazarOrdenRequest);
        Task<ResponseDto> ComprometerOrdenAsync(Op360GD_ComprometerOrdenRequestDto op360GD_ComprometerOrdenRequest);
        Task<ResponseDto> RegistarGestionOrdenAsync(Op360GD_GestionOrdenDto op360GD_GestionOrden);
    }
}
