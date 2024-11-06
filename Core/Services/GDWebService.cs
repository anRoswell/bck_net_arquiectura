using Core.DTOs;
using Core.DTOs.GDWebDto;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class GDWebService : IGDWebService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GDWebService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto> CargaInicialAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto>().ExecuteStoreProcedureAsync("aire.pkg_g_scr.prc_carga_inicial_scr_movil");  //sp de ejemplo
        }

        public Task<ResponseDto<Op360GD_ResumenGlobalOrdenesDto>> GetResumenGlobalOrdenesAsync()
        {
            return _unitOfWork.StoreProcedure<ResponseDto<Op360GD_ResumenGlobalOrdenesDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_ordenes.prc_resumen_global_ordenes");  //sp de ejemplo
        }
    }
}
