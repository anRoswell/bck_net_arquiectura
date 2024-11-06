using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class DocReqProveedorHistorico
    {
        public int DrpIdDocRequeridosProveedorHistorico { get; set; }
        public int DrpCodContratoHistorico { get; set; }
        public int DrpCodDocumento { get; set; }
        public int? DrpCodPrvDocumento { get; set; }
        public bool? DrpAprobado { get; set; }
        public bool? DrpObligatorio { get; set; }
        public int? DrpTipoVersion { get; set; }
        public int? CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public virtual DocReqUpload Archivo { get; set; }
        public virtual ContratoHistorico DrpContratoHistorico { get; set; }
    }
}
