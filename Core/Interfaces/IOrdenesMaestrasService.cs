using Core.Entities;
using Core.ModelResponse;
using Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOrdenesMaestrasService
    {
        Task<List<OrdenesMaestras>> GetListado();
        Task<List<OrdenesMaestras>> SearchByProveedor(QuerySearchOrdenes parametros);
        Task<List<ResponseAction>> PostOrdenReq(OrdenReq ordenReq);
    }
}
