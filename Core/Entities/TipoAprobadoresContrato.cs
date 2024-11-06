using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class TipoAprobadoresContrato : BaseEntity
    {   
        public string TacDescripcion { get; set; }
        public bool? TacEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
