using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class CertificadosRetencion
    {
        public List<CertificadoRetencionMaestro> Maestro { get; set; }
        public List<CertificadoRetencionFuenteDte> Detalle { get; set; }
    }
}
