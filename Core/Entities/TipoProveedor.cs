using System;

namespace Core.Entities
{
    public class TipoProveedor : BaseEntity
    {
        public string TipPrvNombreTipoProveedor { get; set; }
        public bool? TipPrvEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
