using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class Aplicacion : BaseEntity
    {
        public string AplNombre { get; set; }
        public string AplDescripcion { get; set; }
        public bool? AplEstado { get; set; }
        public string CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
