using System;

#nullable disable

namespace Core.Entities
{
    public partial class CertificadosEspeciale : BaseEntity
    {
        //public CertificadosEspeciale()
        //{
        //    EmpresasSelectedCertEsps = new HashSet<EmpresasSelectedCertEsp>();
        //    RespuestasCertEspeciales = new HashSet<RespuestasCertEspeciale>();
        //}

        public int CerCodProveedor { get; set; }
        public int CerCodTipoCertificado { get; set; }
        public int CerPeriodo { get; set; }
        public string CerDescripcion { get; set; }
        public string CerDestinatario { get; set; }
        public bool CerIncluirGarantia { get; set; }
        public string CerHtmlPdf { get; set; }
        public int CerEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }

        //public virtual ICollection<EmpresasSelectedCertEsp> EmpresasSelectedCertEsps { get; set; }
        //public virtual ICollection<RespuestasCertEspeciale> RespuestasCertEspeciales { get; set; }
    }

    public partial class CertificadosEspeciale
    {
        public string NombreTipoCertificado { get; set; }
    }
}
