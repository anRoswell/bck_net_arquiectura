using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.QueryFilters.QueryFiltersASEGREDWeb
{
    public class QueryOp360ASEGRED_OrdenesContratistas
    {
        public int? id_contratista { get; set; }
        public int? id_contratista_persona { get; set; }
        public int? id_zona { get; set; }
        public int? id_estado_orden { get; set; }
        public int? id_orden { get; set; }

        public int? pageSize { get; set; }
        public int? pageNumber { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
    }
}
