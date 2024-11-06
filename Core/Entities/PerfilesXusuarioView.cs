using System;

namespace Core.Entities
{
    public class PerfilesXusuarioView:BaseEntity
    {
        public int PxuUsrCodUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int PxuAplCodAplicacion { get; set; }
        public int PxuPrfCodPerfil { get; set; }
        public string NombrePerfil { get; set; }
        public bool PxuEstado { get; set; }
        public string CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }     
    }
}
