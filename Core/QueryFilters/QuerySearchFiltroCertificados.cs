using System;

namespace Core.QueryFilters
{
    public class QuerySearchFiltroCertificados : BaseQuery
    {
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public int EstadoCertificado { get; set; }
    }
}
