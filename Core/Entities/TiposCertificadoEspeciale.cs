using System;

#nullable disable

namespace Core.Entities
{
    public partial class TiposCertificadoEspeciale : BaseEntity
    {
        public string TcereDescripcion { get; set; }
        public bool? TcereEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
