using Core.DTOs;
using System.Collections.Generic;

namespace Core.Entities
{
    public class ParametrosContrato
    {
        public List<Empresa> Empresas { get; set; }
        public List<Contrato> Contratos  { get; set; }
        public List<ClaseContrato> ClasesContrato { get; set; }
        public List<FormasPago> FormasPago { get; set; }
        public List<Sino> Si_No { get; set; }
        public List<UnidadNegocio> UnidadesNegocio { get; set; }
        public List<UsuarioDto> UsuariosSelect { get; set; }
        public List<DocumentoDto> DocsProveedor { get; set; }
        public List<TipoContrato> TiposContratos { get; set; }
    }
}
