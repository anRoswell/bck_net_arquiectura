using Core.CustomEntities;
using Core.Entities;
using System.Collections.Generic;

namespace Core.DTOs
{
    public class ParametrosRequerimientosDto
    {
        public List<Empresa> Empresas { get; set; }
        public List<Requerimientos> ReqsListosParaAdjudicar { get; set; }
        public List<PrvProdServ> ProductoServicios { get; set; }
        public List<Documento> ListDocumentos { get; set; }
        public List<Estados> ListEstados { get; set; }
        public List<UnidadMedida> ListUnidadMedida { get; set; }
        public List<ReqTipoDocArtSerRequerido> ListTipoFicha { get; set; }
        public List<ReqQuestionAnswer> ListComentario { get; set; }
        public RequerimientoDetalle Requerimientos { get; set; }
        public int CantPrvValidacion { get; set; }
        public List<IdentidadesLocale> ListIva { get; set; }
        public List<TipoCriterio> TipoCriterios { get; set; }
        public List<TipoRequerimiento> TipoRequerimientos { get; set; }
    }
}
