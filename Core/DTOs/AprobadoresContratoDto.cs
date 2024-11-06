using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs
{
    public class AprobadoresContratoDto
    {
        public int Id { get; set; }
        public int ApcCodTipoAprobadoresContrato { get; set; }
        public int ApcCodContrato { get; set; }
        public int ApcCodRequisitor { get; set; }
        public bool ApcAprobacion { get; set; }
        public string ApcJustificacion { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
