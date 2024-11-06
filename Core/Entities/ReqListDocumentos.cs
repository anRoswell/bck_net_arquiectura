using System;

namespace Core.Entities
{
    public partial class ReqListDocumentos : BaseEntity
    {
        public int RlCodRequerimiento { get; set; }
        public int CodDocumento { get; set; }
        public string RldocNombreDocumento { get; set; }
        public int RldocVigencia { get; set; }
        public bool RldocObligatorio { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }

    public partial class ReqListDocumentos
    {
        public string CocNombreDocumento { get; set; }
        public string DescripcionDocumento { get; set; }
        public int? VigenciaMaxDocumento { get; set; }
        public bool? RequiereVigencia { get; set; }
        public string UrlDocumento { get; set; }
        public string NombreOriginalDocumento { get; set; }
        public DateTime? FechaExpedicion { get; set; }
        public int? DiasVencidosDocumento { get; set; }
        public bool? SolicitarDocumento { get; set; }
    }
}
