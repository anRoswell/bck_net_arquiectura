using System;

#nullable disable

namespace Core.Entities
{
    public partial class PermisosMenuXperfil : BaseEntity
    {
        public int PmpPrfCodPerfil { get; set; }
        public int PmpAplCodAplicacion { get; set; }
        public int PmpMenCodMenu { get; set; }
        public bool? PmpEjecutar { get; set; }
        public bool? PmpLeer { get; set; }
        public bool? PmpEditar { get; set; }
        public bool? PmpGrabar { get; set; }
        public bool? PmpBorrar { get; set; }
        public bool? PmpConsultar { get; set; }
        public string pmpNombreMenu { get; set; }
        public string pmpNombrePerfil { get; set; }
        public string CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
