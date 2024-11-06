using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class DocumentoContratoHistorico
    {
        public int DcIdDocumentoContratoHistorico { get; set; }
        public int CodContratoHistorico { get; set; }
        public int CodTipoDocumento { get; set; }
        public int? CodDocumento { get; set; }
        public string DcPermisos { get; set; }
        public int? CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public virtual DocReqUpload Archivo { get; set; }
        public virtual ContratoHistorico DcContratoHistorico { get; set; }
    }
}
