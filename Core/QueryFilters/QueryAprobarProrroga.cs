using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QueryAprobarProrroga: BaseQuery
    {
        public int IdContrato { get; set; }
        public bool Aprobar { get; set; }
        public DateTime contReajusteVigencia { get; set; }
        public DateTime contVigenciaHasta { get; set; }
        
    }
}
