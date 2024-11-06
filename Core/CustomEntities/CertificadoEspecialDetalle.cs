using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class CertificadoEspecialDetalle
    {
        public List<CertificadosEspeciale> Certificado { get; set; }
        public List<EmpresasSelectedCertEsp> ListadoEmpresasSelected { get; set; }
        public List<RespuestasCertEspeciale> ListadoRespuestas { get; set; }
    }
}
