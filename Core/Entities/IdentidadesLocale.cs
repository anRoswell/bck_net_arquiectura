using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class IdentidadesLocale
    {
        public int IdIdentidad { get; set; }
        public string IdGrupo { get; set; }
        public int Consecutivo { get; set; }
        public string Nombre { get; set; }
        public string DescripcionOpcional { get; set; }
        public bool? Estado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
    }
}
