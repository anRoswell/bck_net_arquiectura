using System;

namespace Core.QueryFilters
{
    public class QueryListadoContratosReporte
    {
        public string NumeroContrato { get; set; }
        public string CedulaProveedor { get; set; }
        public int TipoContrato { get; set; } 
        public DateTime? VigenciaDesde {  get; set; }
        public DateTime? VigenciaHasta {  get; set; }
        public int Estado {  get; set; }
    }
}
