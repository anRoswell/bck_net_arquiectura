using Core.DTOs;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPlantillaRepository
    {
        Task<List<RequerimientoComparativoDto>> ConsultarJson(int idReq);
        Task<DataTableCollection> ConsultarMultiplesTablas();
    }
}
