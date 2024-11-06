using Core.DTOs;
using Core.DTOs.ManualDeUsoASEGREDMovil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOp360ASEGREDManualDeUsoMovilService
    {
        Task<ResponseDto<Op360ASEGRED_ManualDeUsoDto>> ManualDeUso();
    }
}
