using Core.DTOs;
using Core.DTOs.GDWebDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGDWebService
    {
        Task<ResponseDto> CargaInicialAsync();
        Task<ResponseDto<Op360GD_ResumenGlobalOrdenesDto>> GetResumenGlobalOrdenesAsync();
    }
}
