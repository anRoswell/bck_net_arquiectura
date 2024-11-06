using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class ClaseContrato : BaseEntity
    {
        public ClaseContrato()
        {
            Contratos = new HashSet<Contrato>();
        }
        public string CcDescripcion { get; set; }
        public bool? CcEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public virtual ICollection<Contrato> Contratos { get; set; }
    }
}
