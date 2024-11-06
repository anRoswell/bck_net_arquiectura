using System;

#nullable disable

namespace Core.Entities
{
    public partial class PathsPortal : BaseEntity
    {
        public string Path { get; set; }
        public string Tipo { get; set; }
        public string Clasificar { get; set; }
        public string Observaciones { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
    }
}
