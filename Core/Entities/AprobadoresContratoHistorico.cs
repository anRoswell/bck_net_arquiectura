using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class AprobadoresContratoHistorico
    {
        public int ApcIdAprobadoresContratoHistorico { get; set; }
        public int ApcCodTipoAprobadoresContrato { get; set; }
        public int ApcCodContratoHistorico { get; set; }
        public int ApcCodRequisitor { get; set; }
        public bool ApcAprobacion { get; set; }
        public string ApcJustificacion { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public virtual ContratoHistorico ApcContratoHistorico { get; set; }
    }
}
