using Core.DTOs;
using Core.DTOs.GDMovilDto;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class VersonDBService : IVersonDBService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VersonDBService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<ObtenerVersonDBDto>> ObtenerVersonDBAsync()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<ObtenerVersonDBDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_generales.prc_version_db");
        }
    }
}
