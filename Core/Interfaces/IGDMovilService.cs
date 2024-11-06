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

namespace Core.Interfaces
{
    public interface IGDMovilService
    {
        Task<ResponseDto> CargaInicialAsync();
        Task<ResponseDto> ConsultaOrdenesAsync();
        Task<ResponseDto> OrdenXIdAsync(Op360GD_OrdenIdDto gestionDanioOrdenId);
        Task<ResponseDto<IList<Op360GD_OrdenesAsignadasTecnicoMovilDto>>> GetOrdenesAsignadasTecnicoAsync(Op360GD_OrdenesAsignadasTecnicoMovilRequestDto op360GD_OrdenesAsignadasTecnicoMovilRequest);
        //RegistarGestionOrden
        Task<ResponseDto> RechazarOrdenAsync(Op360GD_RechazarOrdenRequestDto op360GD_RechazarOrdenRequest);
        Task<ResponseDto> ComprometerOrdenAsync(Op360GD_ComprometerOrdenRequestDto op360GD_ComprometerOrdenRequest);
        Task<ResponseDto> RegistarGestionOrdenAsync(Op360GD_GestionOrdenDto op360GD_GestionOrden);
    }
}
