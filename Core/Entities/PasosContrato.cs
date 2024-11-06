using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class PasosContrato : BaseEntity
    {
        public int? PcnIdPasosContrato { get; set; }
        public int? PcnCodContrato { get; set; }
        public int? PcnCodEstadoContrato { get; set; }
        public int? PcnCodTipoPaso { get; set; }
        public int? PcnConsecutivoFlujo { get; set; }
        public string CodUser { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime? FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
