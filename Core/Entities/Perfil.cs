using System;

#nullable disable

namespace Core.Entities
{
    public partial class Perfil : BaseEntity
    {
        public string PrfNombrePerfil { get; set; }
        public bool PrfAdministrador { get; set; }
        public bool? PrfEstado { get; set; }
        public string CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
