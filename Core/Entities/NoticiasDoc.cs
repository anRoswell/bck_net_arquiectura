using System;

#nullable disable

namespace Core.Entities
{
    public partial class NoticiasDoc : BaseEntity
    {
        public int NotdCodNoticias { get; set; }
        public string NotdNameDocument { get; set; }
        public string NotdExtDocument { get; set; }
        public int? NotdSizeDocument { get; set; }
        public string NotdUrlDocument { get; set; }
        public string NotdUrlRelDocument { get; set; }
        public string NotdOriginalNameDocument { get; set; }
        public int NotdEstadoDocumento { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
