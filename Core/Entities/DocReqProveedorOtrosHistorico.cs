using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public partial class DocReqProveedorOtrosHistorico
    {
        public int DrpoIdDocReqProveedorOtrosHistorico { get; set; }
        public int DrpoCodContratoHistorico { get; set; }
        public int DrpoCodDocumento { get; set; }
        public string DrpoNombreDocumento { get; set; }
        public int DrpoVigencia { get; set; }
        public bool? DrpoObligatorio { get; set; }
        public int? CodArchivo { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public virtual DocReqUpload Archivo { get; set; }

        public virtual ContratoHistorico DrpoContratoHistorico { get; set; }
    }
}
