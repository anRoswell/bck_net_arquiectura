using System;
using System.Collections.Generic;

namespace Core.DTOs
{
    public class CertificadosEspecialesDto
    {
        public int Id { get; set; }
        public int CerCodProveedor { get; set; }
        public int CerCodTipoCertificado { get; set; }
        public string NombreTipoCertificado { get; set; }
        public int CerPeriodo { get; set; }
        public string CerDescripcion { get; set; }
        public string CerDestinatario { get; set; }
        public bool CerIncluirGarantia { get; set; }
        public string cerHtmlPdf { get; set; }
        public int CerEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
        public List<int> ListadoEmpresas { get; set; }
    }
}
