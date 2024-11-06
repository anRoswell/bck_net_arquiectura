using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class QueryCambiarCorreoProveedor : BaseQuery
    {
        public string Nit { get; set; }
        public string Correo { get; set; }
        public string CorreoAlterno { get; set; }
    }
}
