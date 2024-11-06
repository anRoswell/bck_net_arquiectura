using System;

#nullable disable

namespace Core.Entities
{
    public partial class TipoEstado : BaseEntity
    {
        public string TeDescripcion { get; set; }
        public bool? TeEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
