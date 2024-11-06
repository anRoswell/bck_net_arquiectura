using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class DocumentoContrato : BaseEntity
    {
        public int CodContrato { get; set; }
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
    }
}
