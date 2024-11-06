using Core.CustomEntities;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QueryAsociarOrdenesReq : BaseQuery
    {
        public int CodRequerimiento { get; set; }
        public List<AsociacionOrdenes> ListadoAsociacionOrdenes { get; set; }
    }
}
