using System;

#nullable disable

namespace Core.Entities
{
    public partial class EmpresasSelectedCertEsp : BaseEntity
    {
        public int EscCodEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public int EscCodCertificadosEspeciales { get; set; }
        public bool? EscEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }

        //public virtual CertificadosEspeciale EscCodCertificadosEspecialesNavigation { get; set; }
    }
}
