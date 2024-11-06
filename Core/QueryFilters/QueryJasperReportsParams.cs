using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.QueryFilters
{
    public class QueryJasperReportsParams //: BaseQuery
    {
        public string _repName { get; set; }
        public string _repFormat { get; set; }
        /// <summary>
        /// Parametros del reporte en formato json:
        /// {"prIdImg":8,"prIdOrden":2177}
        /// </summary>
        public string _repParams { get; set; }
    }
}