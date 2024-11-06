using System;

#nullable disable

namespace Core.Entities
{
    public partial class ReqDocumentosRequerido : BaseEntity
    {
        public int RdrCodReqListDocumentos { get; set; }
        public int RdrCodProveedor { get; set; }
        public string RdrNameDocument { get; set; }
        public string RdrExtDocument { get; set; }
        public int? RdrSizeDocument { get; set; }
        public string RdrUrlDocument { get; set; }
        public string RdrUrlRelDocument { get; set; }
        public string RdrOriginalNameDocument { get; set; }
        public int RdrEstadoDocumento { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
