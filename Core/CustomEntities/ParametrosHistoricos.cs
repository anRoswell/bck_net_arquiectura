using Core.DTOs;
using Core.Entities;
using System.Collections.Generic;

namespace Core.CustomEntities
{
    public class ParametrosHistoricos
    {
        public List<Contrato> Contratos { get; set; }
        public List<ClaseContrato> ClasesContrato { get; set; }
        public List<FormasPago> FormasPago { get; set; }
        public List<Sino> Si_No { get; set; }
        public List<UnidadNegocio> UnidadesNegocio { get; set; }
        public List<UsuarioDto> UsuariosSelect { get; set; }
        public List<DocumentoDto> DocsProveedor { get; set; }
        public List<TipoContrato> TiposContratos {  get; set; }
    }
}