using Core.DTOs;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPlantillaService
    {
        Task<List<RequerimientoComparativoDto>> ConsultarJson(int idReq);
        Task<PruebaDataTable> ConsultarMultiplesTablas();
    }
}
