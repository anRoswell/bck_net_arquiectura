using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QueryCreateOrdenReq
    {
        public int CodOrden { get; set; }
        public int CodRequerimiento { get; set; }
        public string CodProveedor { get; set; }
        public int CodUser { get; set; }
    }
}
