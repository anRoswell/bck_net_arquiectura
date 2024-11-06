using Core.DTOs;
using Core.DTOs.GDMovilDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IVersonDBService
    {
        Task<ResponseDto<ObtenerVersonDBDto>> ObtenerVersonDBAsync();
    }
}
