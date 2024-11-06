using System;

#nullable disable

namespace Core.Entities
{
    public partial class PermisosEmpresasxUsuario : BaseEntity
    {
        public int PeuUsrCodUsuario { get; set; }
        public string PeuUsrNombreCompleto { get; set; }
        public int PeuEmpCodEmpresa { get; set; }
        public string PeuEmpNombreEmpresa { get; set; }
        public bool PeuEstado { get; set; }
        public string CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
