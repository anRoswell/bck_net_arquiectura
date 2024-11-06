using System;

namespace Core.Entities
{
    public partial class Estados : BaseEntity
    {
        public string ParDescripcion { get; set; }
        public bool? ParvEstado { get; set; }
        public int CodTipoEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
