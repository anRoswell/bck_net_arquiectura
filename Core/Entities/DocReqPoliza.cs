using System;

#nullable disable

namespace Core.Entities
{
    public partial class DocReqPoliza : BaseEntity
    {
        public int DrpoCodContrato { get; set; }
        public string DrpoTipoPoliza { get; set; }
        public int? DrpoCodTipoDocumento { get; set; }
        public string DrpoCobertura { get; set; }
        public string DrpoVigencia { get; set; }
        public int DrpoExpedida { get; set; }
        public int DrpoAprobada { get; set; }
        public int DrpoEstado { get; set; }
        public bool? DrpoEsRenovada { get; set; }
        public DateTime? DrpoFechaVencimiento { get; set; }
        public DateTime? DrpoFechaEmision { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public int? CodArchivo { get; set; }
    }
}
