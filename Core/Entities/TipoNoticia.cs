using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class TipoNoticia
    {
        public int TnIdTipoNoticias { get; set; }
        public string TnDescripcion { get; set; }
        public bool TnEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
