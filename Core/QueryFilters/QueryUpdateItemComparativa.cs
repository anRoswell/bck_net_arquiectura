using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QueryUpdateItemComparativa : BaseQuery
    {
        public List<int> ListIdReqArtSerParticipante { get; set; }
        public bool ItemValido { get; set; }
        public int CodRequerimiento { get; set; }
    }
}
