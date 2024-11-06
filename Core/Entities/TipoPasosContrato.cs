using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class TipoPasosContrato : BaseEntity
    {
        public int TpcIdTipoPasosContrato { get; set; }
        public int TpcCodFlujoContrato { get; set; }
        public string TpcDescripcion { get; set; }
        public bool? PcnEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
