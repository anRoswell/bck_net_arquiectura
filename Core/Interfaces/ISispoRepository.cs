using Core.Entities;
using Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISispoRepository
    {
        Task<List<EstadoCuentasXPorPagar>> EstadoCuentasXPorPagarApoteosys(QuerySearchEstadoCuentasXPagar parameters, List<Empresa> empresasActivas);
        Task<List<EstadoCuentasXPorPagar>> EstadoCuentasXPorPagarApoteosysDte(QuerySearchEstadoCuentasXPagarDetalle parameters);
    }
}
