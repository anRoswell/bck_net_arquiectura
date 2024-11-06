using System;
using System.Collections.Generic;
using System.Text;

namespace Core.QueryFilters
{
    public class CommandUpdateEstadoContrato : BaseQuery
    {
        public int IdContrato { get; set; }
        public int ContEstado { get; set; }
    }
}