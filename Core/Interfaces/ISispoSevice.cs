using Core.Entities;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISispoSevice
    {
        Task<List<EstadoCuentasXPorPagar>> EstadoCuentasXPorPagarApoteosys(QuerySearchEstadoCuentasXPagar parameters);
        Task<List<EstadoCuentasXPorPagar>> EstadoCuentasXPorPagarApoteosysDte(QuerySearchEstadoCuentasXPagarDetalle parameters);
    }
}
