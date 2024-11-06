using Core.Entities;

namespace Core.CustomEntities
{
    public class AdjudicacionReq : BaseEntity
    {
        public int RadjCodRequerimiento { get; set; }
        public string RadjTipoAdjudicacion { get; set; }
        public int RadjCodRequisitorContrato { get; set; }
        public int? RadjCodProveedorSelecTotal { get; set; }
        public int RadjCodGestorContrato { get; set; }
        public bool? RadjIsGuardadoTemporal { get; set; }
        public bool? RadjEstado { get; set; }
        public int RadteCodProveedor { get; set; }
        public int RadteCodArtSerRequerido { get; set; }
        public int RadteCantidadAdjudicada { get; set; }
    }
}
