using System;

#nullable disable

namespace Core.Entities
{
    public partial class ReqAdjudicacion : BaseEntity
    {
        public int RadjCodRequerimiento { get; set; }
        public string RadjTipoAdjudicacion { get; set; }
        public int RadjCodRequisitorContrato { get; set; }
        public int? RadjCodProveedorSelecTotal { get; set; }
        public int RadjCodGestorContrato { get; set; }
        public bool? RadjIsGuardadoTemporal { get; set; }
        public bool? RadjEstado { get; set; }
        public string CodUser { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string CodUserUpdate { get; set; }
        public DateTime FechaRegistroUpdate { get; set; }
        public string Info { get; set; }
        public string InfoUpdate { get; set; }
    }
}
