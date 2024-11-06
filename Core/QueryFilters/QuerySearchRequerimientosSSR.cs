using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QuerySearchRequerimientosSSR
    {
        public int First { get; set; }
   
        public int Rows { get; set; }
        public string SortField { get; set; }
        public int SortOrder { get; set; }
        public string Filter { get; set; }

    }
}
