using System;

#nullable disable

namespace Core.Entities
{
    public partial class PrvListaRestrictiva
    {
        public int PrvPlrIdPrvListaRestrictiva { get; set; }
        public int PrvPlrCodTraza { get; set; }
        public int PrvPlrCodPrvTipoUsListaRtva { get; set; }
        public int PrvPlrNoConsulta { get; set; }
        public string PrvPlrPrioridad { get; set; }
        public string PrvPlrTipoDocumento { get; set; }
        public string PrvPlrIdentificacion { get; set; }
        public string PrvPlrNombreProveedor { get; set; }
        public string PrvPlrNumeroTipoLista { get; set; }
        public string PrvPlrLista { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
