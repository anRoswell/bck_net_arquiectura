using System;

#nullable disable

namespace Core.Entities
{
    public partial class DocReqProveedorOtro : BaseEntity
    {
        public int DrpoCodContrato { get; set; }
        public int DrpoCodDocumento { get; set; }
        public string DrpoNombreDocumento { get; set; }
        public int DrpoVigencia { get; set; }
        public bool? DrpoObligatorio { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public int? CodArchivo { get; set; }
    }
}
