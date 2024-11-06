using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.QueryFilters.QueryFiltersSCRWeb
{
    public class QueryOp360OrdenesV2
    {
        public int? id_contratista { get; set; }
        public int? id_contratista_persona { get; set; }
        public int? id_zona { get; set; }
        public string id_estado_orden { get; set; }
        public string codigo_estado { get; set; }
        public int? id_orden { get; set; }

        public string? ServerSide { get; set; }
        public QueryOp360ServerSideV2 ServerSideJson { get; set; }
    }

}
