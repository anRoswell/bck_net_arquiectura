using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class RefererServidore : BaseEntity
    {
        public string RequestHeadersReferer { get; set; }
        public bool PermitirAcceso { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
    }
}
