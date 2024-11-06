using Core.DTOs;
using Core.DTOs.ManualDeUsoASEGREDMovil;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class Op360ASEGREDManualDeUsoMovilService : IOp360ASEGREDManualDeUsoMovilService
    {
        private readonly IUnitOfWork _unitOfWork;

        public Op360ASEGREDManualDeUsoMovilService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<Op360ASEGRED_ManualDeUsoDto>> ManualDeUso()
        {
            return await _unitOfWork.StoreProcedure<ResponseDto<Op360ASEGRED_ManualDeUsoDto>>().ExecuteStoreProcedureAsync("aire.pkg_g_dummi.prc_dummi");  //sp de ejemplo
        }
    }
}
