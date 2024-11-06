using System;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QuerySearchSolicitudesApoteosys
    {
        public List<string> Empresa { get; set; }
        public string Estado { get; set; }
        public string CodigoArticulo { get; set; }
        public string TipoRequerimiento { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_Inicial { get; set; }
        public DateTime Fecha_Final { get; set; }
    }
}
