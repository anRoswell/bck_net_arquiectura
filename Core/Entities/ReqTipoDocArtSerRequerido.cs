using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class ReqTipoDocArtSerRequerido: BaseEntity
    {
        public string ReqTdasrDescripcion { get; set; }
        public bool ReqTdasrEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }

        //public virtual ICollection<ReqArtSerRequeridosDoc> ReqArtSerRequeridosDocs { get; set; }
    }
}
