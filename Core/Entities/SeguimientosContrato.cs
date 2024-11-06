using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class SeguimientosContrato
    {
        public int ScoIdSeguimiento { get; set; }
        public int? ScoCodContrato { get; set; }
        public DateTime? ScoFecha { get; set; }
        public string ScoObservacion { get; set; }
        public decimal? ScoPagosEfectuados { get; set; }
        public int? CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public string ScoTipo { get; set; }
    }
}
