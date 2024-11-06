using Core.DTOs;
using System.Collections.Generic;

namespace Core.QueryFilters
{
    public class QueryAdjudicarReq : BaseQuery
    {
        public int IdReqAdjudicacion { get; set; }
        public int CodRequerimiento { get; set; }
        public string TipoAdjudicacion { get; set; }
        public int? CodRequisitor { get; set; }
        public int? CodProveedorSeleccionado { get; set; }
        public int? CodGestor { get; set; }
        public bool? RadjIsGuardadoTemporal { get; set; }
        public List<ReqAdjudicacionDetalleDto> ArticulosAdjudicar { get; set; }
    }
}
