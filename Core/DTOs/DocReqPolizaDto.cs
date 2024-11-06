using System;

namespace Core.DTOs
{
    public class DocReqPolizaDto
    {
        public int? Id { get; set; }
        public int DrpoCodContrato { get; set; }
        public int DrpoCodTipoDocumento { get; set; }
        public string DrpoTipoPoliza { get; set; }
        public string DrpoCobertura { get; set; }
        public string DrpoVigencia { get; set; }
        public int DrpoExpedida { get; set; }
        public int DrpoAprobada { get; set; }
        public int DrpoEstado { get; set; }
        public bool? DrpoEsRenovada { get; set; }
        public DateTime? DrpoFechaVencimiento { get; set; }
        public DateTime? DrpoFechaEmision { get; set; }
        public string UrlArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public int? CodArchivo { get; set; }
        public int KeyFile { get; set; }
    }
}
