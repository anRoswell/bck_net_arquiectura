using System;

#nullable disable

namespace Core.Entities
{
    public partial class PermisosUsuarioxMenu : BaseEntity
    {
        public int PumUsrCodUsuario { get; set; }
        public int PumAplCodAplicacion { get; set; }
        public int PumMenCodMenu { get; set; }
        public bool? PumEjecutar { get; set; }
        public bool? PumLeer { get; set; }
        public bool? PumEditar { get; set; }
        public bool? PumGrabar { get; set; }
        public bool? PumBorrar { get; set; }
        public bool? PumConsultar { get; set; }
        public string CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
