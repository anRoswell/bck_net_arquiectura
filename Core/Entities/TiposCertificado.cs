using System;

#nullable disable

namespace Core.Entities
{
    public partial class TiposCertificado : BaseEntity
    {
        public string TcerDescripcion { get; set; }
        public bool? TcerEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
