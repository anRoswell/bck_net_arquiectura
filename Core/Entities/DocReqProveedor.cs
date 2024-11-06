using System;

#nullable disable

namespace Core.Entities
{
    public class DocReqProveedor : BaseEntity
    {
        public int DrpCodContrato { get; set; }
        public int DrpCodDocumento { get; set; }
        public int? DrpCodPrvDocumento { get; set; }
        public bool? DrpAprobado { get; set; }
        public bool? DrpObligatorio { get; set; }
        public int? DrpTipoVersion { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public int? CodArchivo { get; set; }
    }
}
