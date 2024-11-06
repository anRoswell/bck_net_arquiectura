using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class ParametrosCertificados
    {
        public List<TiposCertificado> TiposCertificados { get; set; }
        public List<TiposCertificadoEspeciale> TiposCertificadoEspeciales { get; set; }
        public List<Empresa> Empresas { get; set; }
        public List<Empresa> EmpresasApoteosys { get; set; }
    }
}
